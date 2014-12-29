using ModellMeister.Logic;
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
    public class LogicTests
    {
        [Test]
        public void TestBlockOrderEstimationWithEmptyList()
        {
            var compositeType = new CompositeType();
            var blockOrder = new DataFlowLogic(compositeType);

            var list = blockOrder.GetBlocksByDataFlow().ToList();
            Assert.That(list.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestBlockOrderEstimationWithOneItem()
        {
            var compositeType = new CompositeType();
            var block1 = new ModelBlock();
            compositeType.Blocks.Add(block1);
            var blockOrder = new DataFlowLogic(compositeType);

            var list = blockOrder.GetBlocksByDataFlow().ToList();
            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(list[0], Is.EqualTo(block1));
        }

        [Test]
        public void TestBlockOrderEstimationWithTwoItems()
        {
            var compositeType = new CompositeType();
            var block1 = new ModelBlock();
            var block2 = new ModelBlock();
            compositeType.Blocks.Add(block1);
            compositeType.Blocks.Add(block2);
            var blockOrder = new DataFlowLogic(compositeType);

            var list = blockOrder.GetBlocksByDataFlow().ToList();
            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0], Is.EqualTo(block1));
            Assert.That(list[1], Is.EqualTo(block2));
        }

        [Test]
        public void TestBlockOrderEstimationWithTwoItemsWithDependency()
        {
            var compositeType = new CompositeType();
            var block1 = new ModelBlock();
            var port1 = new Port();
            block1.Inputs.Add(port1);

            var block2 = new ModelBlock();
            var port2 = new Port();
            block2.Outputs.Add(port2);
            compositeType.Blocks.Add(block1);
            compositeType.Blocks.Add(block2);
            compositeType.Wires.Add(new Wire(port2, port1));

            var blockOrder = new DataFlowLogic(compositeType);

            var list = blockOrder.GetBlocksByDataFlow().ToList();
            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0], Is.EqualTo(block2));
            Assert.That(list[1], Is.EqualTo(block1));
        }

        [Test]
        public void TestBlockOrderEstimationWithFourItems()
        {
            var compositeType = new CompositeType();

            var block1 = new ModelBlock();
            var block2 = new ModelBlock();
            var block3 = new ModelBlock();
            var block4 = new ModelBlock();

            var port1 = new Port();
            var port2 = new Port();
            var port3 = new Port();
            var port4 = new Port();
            var port5 = new Port();
            var port6 = new Port();

            block3.Outputs.Add(port1);
            block4.Outputs.Add(port2);
            block2.Inputs.Add(port3);
            block2.Inputs.Add(port4);
            block2.Outputs.Add(port5);
            block1.Inputs.Add(port6);

            compositeType.Blocks.Add(block1);
            compositeType.Blocks.Add(block2);
            compositeType.Blocks.Add(block3);
            compositeType.Blocks.Add(block4);
            compositeType.Wires.Add(new Wire(port1, port3));
            compositeType.Wires.Add(new Wire(port2, port4));
            compositeType.Wires.Add(new Wire(port5, port6));           

            var blockOrder = new DataFlowLogic(compositeType);
            var list = blockOrder.GetBlocksByDataFlow().ToList();

            Assert.That(list.Count, Is.EqualTo(4));
            Assert.That(list[0], Is.EqualTo(block3));
            Assert.That(list[1], Is.EqualTo(block4));
            Assert.That(list[2], Is.EqualTo(block2));
            Assert.That(list[3], Is.EqualTo(block1));
        }

        [Test]
        public void TestBlockOrderEstimationCircular()
        {
            var compositeType = new CompositeType();

            var block1 = new ModelBlock();
            var block2 = new ModelBlock();

            var port1 = new Port();
            var port2 = new Port();
            var port3 = new Port();
            var port4 = new Port();

            block1.Inputs.Add(port1);
            block1.Outputs.Add(port2);
            block2.Inputs.Add(port3);
            block2.Outputs.Add(port4);

            compositeType.Blocks.Add(block1);
            compositeType.Blocks.Add(block2);
            compositeType.Wires.Add(new Wire(port2, port3));
            compositeType.Wires.Add(new Wire(port4, port1));

            var blockOrder = new DataFlowLogic(compositeType);
            Assert.Throws<InvalidOperationException>(() => blockOrder.GetBlocksByDataFlow().ToList());
        }
    }
}
