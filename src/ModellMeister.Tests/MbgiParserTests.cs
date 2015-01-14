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
            Assert.That(currentType.Name, Is.EqualTo("Adder"));

            Assert.That(currentType.Inputs.Count, Is.EqualTo(2));
            Assert.That(currentType.Outputs.Count, Is.EqualTo(1));

            Assert.That(currentType.Inputs[0].Name, Is.EqualTo("Summand1"));
            Assert.That(currentType.Inputs[0].DataType, Is.EqualTo(DataType.Double));
            Assert.That(currentType.Inputs[1].Name, Is.EqualTo("Summand2"));
            Assert.That(currentType.Inputs[1].DataType, Is.EqualTo(DataType.Double));

            Assert.That(currentType.Outputs[0].Name, Is.EqualTo("Sum"));
            Assert.That(currentType.Outputs[0].DataType, Is.EqualTo(DataType.Double));                
        }

        [Test]
        public void TestTwoFile()
        {
            var loadedFile = File.ReadAllText("../../../../examples/mbgi/twotypes.mbgi");

            var parser = new MbgiFileParser();
            var globalScope = parser.ParseFileFromText(loadedFile);

            Assert.That(globalScope.Types.Count, Is.EqualTo(2));

            var currentType = globalScope.Types[0];
            Assert.That(currentType.Name, Is.EqualTo("Adder"));

            Assert.That(currentType.Inputs.Count, Is.EqualTo(2));
            Assert.That(currentType.Outputs.Count, Is.EqualTo(1));

            Assert.That(currentType.Inputs[0].Name, Is.EqualTo("Summand1"));
            Assert.That(currentType.Inputs[0].DataType, Is.EqualTo(DataType.Double));
            Assert.That(currentType.Inputs[1].Name, Is.EqualTo("Summand2"));
            Assert.That(currentType.Inputs[1].DataType, Is.EqualTo(DataType.Double));

            Assert.That(currentType.Outputs[0].Name, Is.EqualTo("Sum"));
            Assert.That(currentType.Outputs[0].DataType, Is.EqualTo(DataType.Double));

            currentType = globalScope.Types[1];
            Assert.That(currentType.Name, Is.EqualTo("Multiplier"));

            Assert.That(currentType.Inputs.Count, Is.EqualTo(2));
            Assert.That(currentType.Outputs.Count, Is.EqualTo(1));

            Assert.That(currentType.Inputs[0].Name, Is.EqualTo("Factor1"));
            Assert.That(currentType.Inputs[0].DataType, Is.EqualTo(DataType.Double));
            Assert.That(currentType.Inputs[1].Name, Is.EqualTo("Factor2"));
            Assert.That(currentType.Inputs[1].DataType, Is.EqualTo(DataType.Double));

            Assert.That(currentType.Outputs[0].Name, Is.EqualTo("Product"));
            Assert.That(currentType.Outputs[0].DataType, Is.EqualTo(DataType.Double));
        }

        [Test]
        public void TestTwoBlocks()
        {
            var loadedFile = File.ReadAllText("../../../../examples/mbgi/twoblocks.mbgi");

            var parser = new MbgiFileParser();
            var globalScope = parser.ParseFileFromText(loadedFile);

            Assert.That(globalScope.Types.Count, Is.EqualTo(1));
            Assert.That(globalScope.Blocks.Count, Is.EqualTo(2));
            Assert.That(globalScope.Wires.Count, Is.EqualTo(1));
            
            // Checks the Type
            var currentType = globalScope.Types[0];
            Assert.That(currentType.Name, Is.EqualTo("Adder"));

            Assert.That(currentType.Inputs.Count, Is.EqualTo(2));
            Assert.That(currentType.Outputs.Count, Is.EqualTo(1));

            Assert.That(currentType.Inputs[0].Name, Is.EqualTo("Summand1"));
            Assert.That(currentType.Inputs[0].DataType, Is.EqualTo(DataType.Double));
            Assert.That(currentType.Inputs[1].Name, Is.EqualTo("Summand2"));
            Assert.That(currentType.Inputs[1].DataType, Is.EqualTo(DataType.Double));

            Assert.That(currentType.Outputs[0].Name, Is.EqualTo("Sum"));
            Assert.That(currentType.Outputs[0].DataType, Is.EqualTo(DataType.Double));

            /////////////////////////////////
            // Checks the block
            var firstSummer = globalScope.Blocks[0];
            var secondSummer = globalScope.Blocks[1];
            Assert.That(firstSummer.Name, Is.EqualTo("FirstSummer"));
            Assert.That(firstSummer.Type, Is.EqualTo(currentType));
            Assert.That(firstSummer.Inputs.Count, Is.EqualTo(2));
            Assert.That(firstSummer.Outputs.Count, Is.EqualTo(1));

            Assert.That(firstSummer.Inputs[0].Name, Is.EqualTo("Summand1"));
            Assert.That(firstSummer.Inputs[0].DataType, Is.EqualTo(DataType.Double));
            Assert.That(firstSummer.Inputs[1].Name, Is.EqualTo("Summand2"));
            Assert.That(firstSummer.Inputs[1].DataType, Is.EqualTo(DataType.Double));

            Assert.That(firstSummer.Outputs[0].Name, Is.EqualTo("Sum"));
            Assert.That(firstSummer.Outputs[0].DataType, Is.EqualTo(DataType.Double));

            // Second block
            Assert.That(secondSummer.Name, Is.EqualTo("SecondSummer"));
            Assert.That(secondSummer.Type, Is.EqualTo(currentType));

            Assert.That(secondSummer.Inputs[0].Name, Is.EqualTo("Summand1"));
            Assert.That(secondSummer.Inputs[0].DataType, Is.EqualTo(DataType.Double));
            Assert.That(secondSummer.Inputs[1].Name, Is.EqualTo("Summand2"));
            Assert.That(secondSummer.Inputs[1].DataType, Is.EqualTo(DataType.Double));

            Assert.That(secondSummer.Outputs[0].Name, Is.EqualTo("Sum"));
            Assert.That(secondSummer.Outputs[0].DataType, Is.EqualTo(DataType.Double));
        
            ///////////////////////////////
            // Checks the wire
            var currentWire = globalScope.Wires[0];
            Assert.That(currentWire.InputOfWire, Is.EqualTo(firstSummer.Outputs[0]));
            Assert.That(currentWire.OutputOfWire, Is.EqualTo(secondSummer.Inputs[0]));
        }

        [Test]
        public void TestDefaultValue()
        {
            var loadedFile = File.ReadAllText("../../../../examples/mbgi/defaultvalue.mbgi");

            var parser = new MbgiFileParser();
            var globalScope = parser.ParseFileFromText(loadedFile);

            var constantType = globalScope.Types.Where(x => x.Name == "Constant").FirstOrDefault();
            Assert.That(constantType, Is.Not.Null);

            var inputPort = constantType.Inputs.First();
            Assert.That(constantType, Is.Not.Null);

            Assert.That(inputPort.DataType, Is.EqualTo(DataType.Double));
            Assert.That(inputPort.DefaultValue, Is.EqualTo(1));


            var c1 = globalScope.Blocks.Where(x => x.Name == "C1").FirstOrDefault();
            Assert.That(c1, Is.Not.Null);

            inputPort = c1.Inputs.First();
            Assert.That(c1, Is.Not.Null);

            Assert.That(inputPort.DataType, Is.EqualTo(DataType.Double));
            Assert.That(inputPort.DefaultValue, Is.EqualTo(1));

            var c2 = globalScope.Blocks.Where(x => x.Name == "C2").FirstOrDefault();
            Assert.That(c2, Is.Not.Null);

            inputPort = c2.Inputs.First();
            Assert.That(c2, Is.Not.Null);

            Assert.That(inputPort.DataType, Is.EqualTo(DataType.Double));
            Assert.That(inputPort.DefaultValue, Is.EqualTo(3.0));
        }

        [Test]
        public void TestNoNameSpace()
        {
            var loadedFile = "T MyNameSpace";

            var parser = new MbgiFileParser();
            var globalScope = parser.ParseFileFromText(loadedFile);
            Assert.That(globalScope.NameSpace, Is.EqualTo("ModelBased"));
        }

        [Test]
        public void TestNameSpace()
        {
            var loadedFile = "N MyNameSpace";

            var parser = new MbgiFileParser();
            var globalScope = parser.ParseFileFromText(loadedFile);
            Assert.That(globalScope.NameSpace, Is.EqualTo("MyNameSpace"));
        }

        [Test]
        public void TestBoolean()
        {
            var loadedFile = @"
T And
TI Input1 : Boolean
TI Input2 : Boolean
TO Output : Boolean";

            var parser = new MbgiFileParser();
            var globalScope = parser.ParseFileFromText(loadedFile);
            var foundType = globalScope.Types.Where(x => x.Name == "And").FirstOrDefault();
            Assert.That(foundType , Is.Not.Null);

            var foundInput = foundType.Inputs.Where(x => x.Name == "Input1").FirstOrDefault();
            Assert.That(foundInput, Is.Not.Null);

            Assert.That(foundInput.DataType, Is.EqualTo(DataType.Boolean));
        }

        [Test]
        public void TestNameSpaceDefinedTwice()
        {
            var loadedFile = @"
N MyNameSpace
T And
N OtherNameSpace
T Or";

            var parser = new MbgiFileParser();
            var globalScope = parser.ParseFileFromText(loadedFile);

            var andFound = globalScope.Types.Where(x => x.Name == "And").FirstOrDefault();
            var orFound = globalScope.Types.Where(x => x.Name == "Or").FirstOrDefault();

            Assert.That(andFound, Is.Not.Null);
            Assert.That(orFound, Is.Not.Null);

            Assert.That(andFound.NameSpace, Is.EqualTo("MyNameSpace"));
            Assert.That(orFound.NameSpace, Is.EqualTo("OtherNameSpace"));
        }
    }
}
