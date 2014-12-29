using Microsoft.CSharp;
using ModellMeister.Logic;
using ModellMeister.Model;
using System;
using System.CodeDom;
using System.Collections.Generic;
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
        public void CreateSource(CompositeType model, TextWriter writer)
        {
            this.compileUnit = new CodeCompileUnit();
            this.nameSpace = new CodeNamespace("ModelBased");
            this.compileUnit.Namespaces.Add(this.nameSpace);

            var sharpProvider = new CSharpCodeProvider();

            // Creates the models for each type
            this.CreateClassForType(model);

            var generator = sharpProvider.CreateGenerator(writer);
            generator.GenerateCodeFromCompileUnit(
                this.compileUnit,
                writer,
                new System.CodeDom.Compiler.CodeGeneratorOptions()
                {
                    BlankLinesBetweenMembers = true,
                    BracingStyle = "C"                    
                });
        }

        /// <summary>
        /// Creates a class for the composite type
        /// </summary>
        /// <param name="compositeType">The composite type</param>
        /// <param name="typeDeclaration">Creates the class</param>
        private void FillClassForCompositeType(CompositeType compositeType, CodeTypeDeclaration typeDeclaration)
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

                // Adds some statements to the execution method
                // First, populate the input values
                foreach (var tuple in flowLogic.GetInputWiresForBlock(block))
                {
                    var targetPortName =
                        new CodeFieldReferenceExpression(
                            new CodeFieldReferenceExpression(
                                new CodeThisReferenceExpression(),
                                block.Name), tuple.Item2.OutputOfWire.Name);
                    var sourcePortName =
                        new CodeFieldReferenceExpression(
                            new CodeFieldReferenceExpression(
                                new CodeThisReferenceExpression(),
                                tuple.Item1.Name), tuple.Item2.InputOfWire.Name);
                    executeMethod.Statements.Add(new CodeAssignStatement(
                        targetPortName,
                        sourcePortName));
                }

                // Second, Execute it
                executeMethod.Statements.Add(
                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(
                            fieldExpression,
                            "Execute")));
            }
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

            this.typeMapping[type] = csharpType;

            this.CreatePorts(type, csharpType);

            this.nameSpace.Types.Add(csharpType);

            if (type.GetType() == typeof(ModelType))
            {
                // Returns an empty execution method
                var executeMethod = new CodeMemberMethod();
                executeMethod.Name = "Execute";
                executeMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;

                csharpType.Members.Add(executeMethod);
            }
            else if (type.GetType() == typeof(CompositeType))
            {
                this.FillClassForCompositeType(type as CompositeType, csharpType);
            }

            return csharpType;
        }

        /// <summary>
        /// Creates the members and properties of for the ports
        /// </summary>
        /// <param name="type">Type, containing the ports</param>
        /// <param name="csharpType">The C# Type, which will host the ports</param>
        private void CreatePorts(EntityWithPorts type, CodeTypeDeclaration csharpType)
        {
            // Creates the properties for the input and output ports
            foreach (var inputPort in type.Inputs.Union(type.Outputs))
            {
                var fieldName = "_" + inputPort.Name;
                var fieldType = new CodeTypeReference(ConvertToDotNetType(inputPort.DataType));

                var field = new CodeMemberField();
                field.Name = fieldName;
                field.Type = fieldType;
                field.Attributes = MemberAttributes.Private;
                csharpType.Members.Add(field);

                var property = new CodeMemberProperty();
                property.Name = inputPort.Name;
                property.Type = fieldType;
                property.Attributes = MemberAttributes.Public;
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

                csharpType.Members.Add(property);
            }
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
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
