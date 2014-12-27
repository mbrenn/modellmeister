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
        /// <summary>
        /// Creates the source code for a specific scope by
        /// using the text writer
        /// </summary>
        /// <param name="model">Scope to be converted</param>
        /// <param name="writer">Writer to be used</param>
        public void CreateSource(CompositeType model, TextWriter writer)
        {
            var codeCompileUnit = new CodeCompileUnit();
            var mainScope = new CodeNamespace("ModelBased");
            codeCompileUnit.Namespaces.Add ( mainScope );

            var sharpProvider = new CSharpCodeProvider();
            // Creates the models for each type

            foreach (var type in model.Types)
            {
                var csharpType = new CodeTypeDeclaration(type.Name);
                csharpType.Attributes = MemberAttributes.Public;

                foreach (var inputPort in type.Inputs.Union(type.Outputs))
                {
                    var property = new CodeMemberProperty();
                    property.Name = inputPort.Name;
                    property.Type = new CodeTypeReference(ConvertToDotNetType(inputPort.DataType));
                    csharpType.Members.Add(property);
                }

                mainScope.Types.Add(csharpType);
            }

            var generator = sharpProvider.CreateGenerator(writer);
            generator.GenerateCodeFromCompileUnit(
                codeCompileUnit,
                writer,
                new System.CodeDom.Compiler.CodeGeneratorOptions());
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
