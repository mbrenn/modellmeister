using ModellMeister.FileParser;
using ModellMeister.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Tests
{
    [TestFixture]
    public class LineParseTests
    {
        [Test]
        public void TestLineParser()
        {
            var lineParser = new LineParser();
            var line1 = lineParser.ParseLine("B name");
            Assert.That(line1.LineType, Is.EqualTo(ModelType.Block));
            Assert.That(line1.Name, Is.EqualTo("name"));
            
            var line2 = lineParser.ParseLine("T othertype");
            Assert.That(line2.LineType, Is.EqualTo(ModelType.Type));
            Assert.That(line2.Name, Is.EqualTo("othertype"));

            var line3 = lineParser.ParseLine("B othername : type");
            Assert.That(line3.LineType, Is.EqualTo(ModelType.Block));
            Assert.That(line3.Name, Is.EqualTo("othername"));
            Assert.That(line3.Parameters.Count, Is.EqualTo(1));
            Assert.That(line3.Parameters[PropertyType.OfType], Is.EqualTo("type"));

            var line4 = lineParser.ParseLine("B othername : type defaultvalue 3");
            Assert.That(line4.LineType, Is.EqualTo(ModelType.Block));
            Assert.That(line4.Name, Is.EqualTo("othername"));
            Assert.That(line4.Parameters.Count, Is.EqualTo(2));
            Assert.That(line4.Parameters[PropertyType.DefaultValue], Is.EqualTo("3"));
            Assert.That(line4.Parameters[PropertyType.OfType], Is.EqualTo("type"));
        }

        [Test]
        public void TestRobustness()
        {
            var lineParser = new LineParser();
            var line3 = lineParser.ParseLine("B\tothername   :   \t type");
            Assert.That(line3.LineType, Is.EqualTo(ModelType.Block));
            Assert.That(line3.Name, Is.EqualTo("othername"));
            Assert.That(line3.Parameters.Count, Is.EqualTo(1));
            Assert.That(line3.Parameters[PropertyType.OfType], Is.EqualTo("type"));
        }
    }
}
