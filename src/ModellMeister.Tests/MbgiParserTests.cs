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
    }
}
