using Microsoft.CSharp;
using ModellMeister.Logic;
using ModellMeister.Model;
using ModellMeister.Runtime;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.SourceGenerator.CSharp
{
    public class CSharpGenerator
    {
        private CodeCompileUnit compileUnit;

        private CodeNamespace nameSpace;

        /// <summary>
        /// Stores the type mapping
        /// </summary>
        private Dictionary<EntityWithPorts, CodeTypeDeclaration> typeMapping =
            new Dictionary<EntityWithPorts, CodeTypeDeclaration>();

        /// <summary>
        /// Creates the source code for a specific scope by
        /// using the text writer
        /// </summary>
        /// <param name="model">Scope to be converted</param>
        /// <param name="writer">Writer to be used</param>
        public void CreateSource(ModelCompositeType model, TextWriter writer)
        {
            var nameSpace = model.NameSpace;
            if (string.IsNullOrEmpty(nameSpace))
            {
                nameSpace = "ModelBased";
            }

            this.compileUnit = new CodeCompileUnit();
            this.nameSpace = new CodeNamespace(nameSpace);
            this.compileUnit.Namespaces.Add(this.nameSpace);

            var sharpProvider = new CSharpCodeProvider();

            // Creates the models for each type
            var typeDeclaration = this.CreateClassForType(model);
            
            // Adds the attribute for the root element
            typeDeclaration.CustomAttributes.Add(
                new CodeAttributeDeclaration("ModellMeister.Runtime.RootModelAttribute"));

            var generator = sharpProvider.CreateGenerator(writer);
            generator.GenerateCodeFromCompileUnit(
                this.compileUnit,
                writer,
                new System.CodeDom.Compiler.CodeGeneratorOptions());
        }

        /// <summary>
        /// Creates the class for a simple type
        /// </summary>
        /// <param name="nameSpace">Namespace, where properties will be added</param>
        /// <param name="type">The type to be added</param>
        private CodeTypeDeclaration CreateClassForType(EntityWithPorts type)
        {
            return this.CreateClassForEntityWithPorts(type);
        }

        /// <summary>
        /// Creates the type for every entity type with ports
        /// </summary>
        /// <param name="nameSpace">C# namespace</param>
        /// <param name="type">Entity type with ports</param>
        /// <returns></returns>
        private CodeTypeDeclaration CreateClassForEntityWithPorts(EntityWithPorts type)
        {            
            var csharpType = new CodeTypeDeclaration(type.Name);
            csharpType.Attributes = MemberAttributes.Public;
            csharpType.IsPartial = true;
            csharpType.BaseTypes.Add(new CodeTypeReference("ModellMeister.Runtime.IModelType"));

            this.typeMapping[type] = csharpType;

            this.CreatePorts(type, csharpType);

            this.nameSpace.Types.Add(csharpType);

            if (type.GetType() == typeof(ModelNativeType))
            {
                // Returns an empty execution method
                var executeMethod = new CodeMemberMethod();
                executeMethod.Name = "Execute";
                executeMethod.Parameters.Add(
                    new CodeParameterDeclarationExpression("ModellMeister.Runtime.StepInfo", "info"));
                executeMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                executeMethod.Statements.Add(
                    new CodeMethodInvokeExpression(
                        new CodeThisReferenceExpression(),
                        "DoExecute",
                        new CodeArgumentReferenceExpression("info")));

                csharpType.Members.Add(executeMethod);

                // Returns an empty init method
                var initMethod = new CodeMemberMethod();
                initMethod.Name = "Init";
                initMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                initMethod.Statements.Add(
                    new CodeMethodInvokeExpression(
                        new CodeThisReferenceExpression(),
                        "DoInit"));

                csharpType.Members.Add(initMethod);

                // Returns an partial declaration for the init method
                // Partial methods are not supported by CodeDom...
                // This will be called by the Init Method
                var initImplMethod = new CodeMemberField();
                initImplMethod.Name = "DoInit()";
                initImplMethod.Type = new CodeTypeReference("partial void");
                initImplMethod.Attributes = MemberAttributes.Final;

                csharpType.Members.Add(initImplMethod);

                var execImplMethod = new CodeMemberField();
                execImplMethod.Name = "DoExecute(ModellMeister.Runtime.StepInfo info)";
                execImplMethod.Type = new CodeTypeReference("partial void");
                execImplMethod.Attributes = MemberAttributes.Final;

                csharpType.Members.Add(execImplMethod);
            }
            else if (type.GetType() == typeof(ModelCompositeType))
            {
                this.FillClassForCompositeType(type as ModelCompositeType, csharpType);
            }

            return csharpType;
        }

        /// <summary>
        /// Creates a class for the composite type
        /// </summary>
        /// <param name="compositeType">The composite type</param>
        /// <param name="typeDeclaration">Creates the class</param>
        private void FillClassForCompositeType(
            ModelCompositeType compositeType, 
            CodeTypeDeclaration typeDeclaration)
        {
            // Creates the types within the composite type
            foreach (var type in compositeType.Types)
            {
                this.CreateClassForType(type);
            }

            // Returns an empty init method
            var initMethod = new CodeMemberMethod();
            initMethod.Name = "Init";
            initMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;

            typeDeclaration.Members.Add(initMethod);
            var initMethodStatements = initMethod.Statements;

            // Returns an empty execution method
            var executeMethod = new CodeMemberMethod();
            executeMethod.Name = "Execute";
            executeMethod.Parameters.Add(new CodeParameterDeclarationExpression("ModellMeister.Runtime.StepInfo", "info"));
            executeMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;

            typeDeclaration.Members.Add(executeMethod);

            // Creates the block properties
            var flowLogic = new DataFlowLogic(compositeType);
            foreach (var block in flowLogic.GetBlocksByDataFlow())
            {
                // Creates the block properties themselves
                var dotNetTypeOfBlock = this.typeMapping[block.Type];
                var fieldType = new CodeTypeReference(dotNetTypeOfBlock.Name);
                var fieldName = "_" + block.Name;

                var blockField = new CodeMemberField();
                blockField.Name = fieldName;
                blockField.Type = fieldType;
                blockField.Attributes = MemberAttributes.Private | MemberAttributes.Final;

                typeDeclaration.Members.Add(blockField);

                // Stores the expression to retrieve the field
                var fieldExpression = new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    fieldName);

                // Creates the property
                var property = new CodeMemberProperty();
                property.Name = block.Name;
                property.Type = fieldType;
                property.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                property.HasGet = true;
                property.HasSet = true;
                property.GetStatements.Add(
                    new CodeMethodReturnStatement(
                        fieldExpression));
                property.SetStatements.Add(
                    new CodeAssignStatement(
                        fieldExpression,
                        new CodePropertySetValueReferenceExpression()));

                typeDeclaration.Members.Add(property);

                // Adds some statements to the initialization
                initMethodStatements.Add(
                    new CodeAssignStatement(
                        fieldExpression,
                        new CodeObjectCreateExpression(
                            fieldType)));

                // Goes through the ports and adds the initialization, if the 
                // default value of the port is different to the one of the type
                foreach (var blockPort in block.Inputs.Union(block.Outputs))
                {
                    var typePort = 
                        block.Type.Inputs
                            .Union(block.Type.Outputs)
                            .Where(x => x.Name == blockPort.Name)
                            .FirstOrDefault();
                    if (typePort == null)
                    {
                        throw new InvalidOperationException("Some mismatch in ports");
                    }

                    if (blockPort.DefaultValue != typePort.DefaultValue)
                    {
                        // Ok, we need to create an assignment
                        initMethodStatements.Add(
                            new CodeAssignStatement(
                                new CodePropertyReferenceExpression(
                                    fieldExpression,
                                    blockPort.Name),
                                new CodePrimitiveExpression(
                                    ConvertToDotNetValue(blockPort.DataType, blockPort.DefaultValue))));
                    }
                }

                // Invokes the init Method of the block
                initMethodStatements.Add(
                    new CodeMethodInvokeExpression(
                        fieldExpression,
                        "Init"));

                // Adds some statements to the execution method
                // First, populate the input values
                foreach (var tuple in flowLogic.GetInputWiresForBlock(block))
                {
                    var targetBlockExpression =
                            new CodeFieldReferenceExpression(
                                    new CodeThisReferenceExpression(),
                                    block.Name);

                    var targetPortName =
                        new CodeFieldReferenceExpression(
                            targetBlockExpression,
                            tuple.Item2.OutputOfWire.Name);

                    CodeExpression sourceBlockExpression;

                    if (tuple.Item1 != compositeType)
                    {
                        sourceBlockExpression =
                            new CodeFieldReferenceExpression(
                                new CodeThisReferenceExpression(),
                                tuple.Item1.Name);
                    }
                    else
                    {
                        sourceBlockExpression = new CodeThisReferenceExpression();
                    }

                    var sourcePortName =
                        new CodeFieldReferenceExpression(
                            sourceBlockExpression,
                            tuple.Item2.InputOfWire.Name);
                    executeMethod.Statements.Add(new CodeAssignStatement(
                        targetPortName,
                        sourcePortName));
                }

                // Second, Execute it
                executeMethod.Statements.Add(
                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(
                            fieldExpression,
                            "Execute"),
                        new CodeArgumentReferenceExpression("info")));
            }

            // Populate the input of the wires
            foreach (var tuple in flowLogic.GetInputWiresForBlock(compositeType))
            {
                var targetPortName =
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        tuple.Item2.OutputOfWire.Name);

                CodeExpression sourceBlockExpression;

                if (tuple.Item1 != compositeType)
                {
                    sourceBlockExpression =
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(),
                            tuple.Item1.Name);
                }
                else
                {
                    sourceBlockExpression = new CodeThisReferenceExpression();
                }

                var sourcePortName =
                    new CodeFieldReferenceExpression(
                        sourceBlockExpression,
                        tuple.Item2.InputOfWire.Name);
                executeMethod.Statements.Add(new CodeAssignStatement(
                    targetPortName,
                    sourcePortName));
            }
        }

        /// <summary>
        /// Creates the members and properties of for the ports
        /// </summary>
        /// <param name="type">Type, containing the ports</param>
        /// <param name="csharpType">The C# Type, which will host the ports</param>
        private void CreatePorts(EntityWithPorts type, CodeTypeDeclaration csharpType)
        {
            // Creates the properties for the input and output ports
            foreach (var inputPort in type.Inputs)
            {
                this.CreatePort(csharpType, inputPort, PortType.Input);
            }

            foreach (var inputPort in type.Outputs)
            {
                this.CreatePort(csharpType, inputPort, PortType.Input);
            }
        }

        private void CreatePort(CodeTypeDeclaration csharpType, ModelPort port, PortType portType)
        {
            var fieldName = "_" + port.Name;
            var fieldType = new CodeTypeReference(ConvertToDotNetType(port.DataType));

            var field = new CodeMemberField();
            field.Name = fieldName;
            field.Type = fieldType;
            field.Attributes = MemberAttributes.Private | MemberAttributes.Final;
            if (port.DefaultValue != null)
            {
                field.InitExpression =
                    new CodePrimitiveExpression(
                        ConvertToDotNetValue(port.DataType, port.DefaultValue));
            }

            csharpType.Members.Add(field);

            var property = new CodeMemberProperty();
            property.Name = port.Name;
            property.Type = fieldType;
            property.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            property.HasGet = true;
            property.HasSet = true;
            property.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        fieldName)));
            property.SetStatements.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        fieldName),
                    new CodePropertySetValueReferenceExpression()));

            // Creates the attribute for the property
            // [Port(PortType.Input)]
            property.CustomAttributes.Add(
                new CodeAttributeDeclaration(
                    "ModellMeister.Runtime.Port",
                    new CodeAttributeArgument(
                        new CodeFieldReferenceExpression(
                            new CodeSnippetExpression(typeof(PortType).FullName), portType.ToString()))));
            csharpType.Members.Add(property);
        }

        private string ConvertToDotNetType(DataType dataType)
        {
            switch (dataType)
            {
                case DataType.Double:
                    return "System.Double";
                case DataType.Integer:
                    return "System.Int32";
                case DataType.String:
                    return "System.String";
                case DataType.Boolean:
                    return "System.Boolean";
                default:
                    throw new InvalidOperationException("Unknown Type: " + dataType.ToString());
            }
        }


        private object ConvertToDotNetValue(DataType dataType, object value)
        {
            switch (dataType)
            {
                case DataType.Double:
                    return Convert.ToDouble(value, CultureInfo.InvariantCulture);
                case DataType.Integer:
                    return Convert.ToInt32(value, CultureInfo.InvariantCulture);
                case DataType.String:
                    return Convert.ToString(value, CultureInfo.InvariantCulture);
                default: 
                    throw new InvalidOperationException("Unknown Type: " + dataType.ToString());
            }
        }
    }
}
