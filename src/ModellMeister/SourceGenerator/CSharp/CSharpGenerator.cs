using Microsoft.CSharp;
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
        /// Creates the source code for a specific scope by
        /// using the text writer
        /// </summary>
        /// <param name="model">Scope to be converted</param>
        /// <param name="writer">Writer to be used</param>
        public void CreateSource(CompositeType model, TextWriter writer)
        {
            this.compileUnit = new CodeCompileUnit();
            this.nameSpace= new CodeNamespace("ModelBased");
            this.compileUnit.Namespaces.Add(this.nameSpace);

            var sharpProvider = new CSharpCodeProvider();
            // Creates the models for each type

            this.CreateClassForCompositeType(model);

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
        private void CreateClassForCompositeType(CompositeType compositeType)
        {
            this.CreateClassForEntityWithPorts(compositeType);

            foreach (var type in compositeType.Types)
            {
                this.CreateClassForType(type);
            }
        }

        /// <summary>
        /// Creates the class for a simple type
        /// </summary>
        /// <param name="nameSpace">Namespace, where properties will be added</param>
        /// <param name="type">The type to be added</param>
        private CodeTypeDeclaration CreateClassForType(ModelType type)
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

            this.nameSpace.Types.Add(csharpType);

            return csharpType;
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
