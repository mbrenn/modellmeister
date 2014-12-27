using ModellMeister.FileParser;
using ModellMeister.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Tests
{
    [TestFixture]
    public class MbgiParserTests
    {
        [Test]
        public void TestSimpleFile()
        {
            var loadedFile = File.ReadAllText("../../../../examples/mbgi/onlytype.mbgi");

            var parser = new MbgiFileParser();
            var globalScope = parser.ParseFileFromText(loadedFile);

            Assert.That(globalScope.Types.Count, Is.EqualTo(1));
            
            var currentType = globalScope.Types[0];
            Assert.That(currentType.Name, Is.EqualTo("Summer"));

            Assert.That(currentType.Inputs.Count, Is.EqualTo(2));
            Assert.That(currentType.Outputs.Count, Is.EqualTo(1));

            Assert.That(currentType.Inputs[0].Name, Is.EqualTo("Summand1"));
            Assert.That(currentType.Inputs[0].DataType, Is.EqualTo(DataType.Double));
            Assert.That(currentType.Inputs[1].Name, Is.EqualTo("Summand2"));
            Assert.That(currentType.Inputs[1].DataType, Is.EqualTo(DataType.Double));

            Assert.That(currentType.Outputs[0].Name, Is.EqualTo("Sum"));
            Assert.That(currentType.Outputs[0].DataType, Is.EqualTo(DataType.Double));                
        }
    }
}
