using ModellMeister.Compiler;
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
    public class GeneratorTests
    {
        [Test]
        public void TestGeneratorForOnlyType()
        {
            var converter = new Mbgi2CsConverter();
            converter.ConvertFile(
                "../../../../examples/mbgi/onlytype.mbgi",
                "../../../../examples/cs/onlytype.cs");

            var content = File.ReadAllText("../../../../examples/cs/onlytype.cs");
            Assert.That(content.Length, Is.GreaterThan(0));
        }

        [Test]
        public void TestGeneratorForTwoTypes()
        {
            var converter = new Mbgi2CsConverter();
            converter.ConvertFile(
                "../../../../examples/mbgi/twotypes.mbgi",
                "../../../../examples/cs/twotypes.cs");

            var content = File.ReadAllText("../../../../examples/cs/onlytype.cs");
            Assert.That(content.Length, Is.GreaterThan(0));
        }

        [Test]
        public void TestGeneratorForCompositeBlock()
        {
            var converter = new Mbgi2CsConverter();
            converter.ConvertFile(
                "../../../../examples/mbgi/compositeblock.mbgi",
                "../../../../examples/cs/compositeblock.cs");

            var content = File.ReadAllText("../../../../examples/cs/compositeblock.cs");
            Assert.That(content.Length, Is.GreaterThan(0));
        }

        [Test]
        public void TestGeneratorForAutogeneration()
        {
            var converter = new Mbgi2CsConverter();
            converter.ConvertFile(
                "../../../../examples/mbgi/autogenerationblock.mbgi",
                "../../../../examples/cs/autogenerationblock.cs");

            var content = File.ReadAllText("../../../../examples/cs/autogenerationblock.cs");
            Assert.That(content.Length, Is.GreaterThan(0));
        }

        [Test]
        public void TestGeneratorDefaultValue()
        {
            var converter = new Mbgi2CsConverter();
            converter.ConvertFile(
                "../../../../examples/mbgi/defaultvalue.mbgi",
                "../../../../examples/cs/defaultvalue.cs");

            var content = File.ReadAllText("../../../../examples/cs/defaultvalue.cs");
            Assert.That(content.Length, Is.GreaterThan(0));
        }
    }
}
