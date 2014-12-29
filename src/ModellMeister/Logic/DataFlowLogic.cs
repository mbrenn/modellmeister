using ModellMeister.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Logic
{
    /// <summary>
    /// Checks the dependency
    /// </summary>
    public class DataFlowLogic
    {
        /// <summary>
        /// Stores the type
        /// </summary>
        private CompositeType compositeType;

        /// <summary>
        /// Caches the association from ports to modelblocks, 
        /// since the wires do not contain information about the modelblocks being connected
        /// </summary>
        private Dictionary<Port, ModelBlock> cachePortToBlock 
            = new Dictionary<Port, ModelBlock>();

        /// <summary>
        /// Caches the information whether a block is dependent on another block
        /// The key is the block, receiving the data. 
        /// The values are sources for the of data
        /// </summary>
        private MultiValueDictionary<ModelBlock, ModelBlock> dependencies
             = new MultiValueDictionary<ModelBlock, ModelBlock>();

        public DataFlowLogic(CompositeType type)
        {
            this.compositeType = type;

            this.FillCaches();
        }

        /// <summary>
        /// Fill the caches for the ports and blocks
        /// </summary>
        private void FillCaches()
        {
            // First, fill the port cache
            foreach (var block in this.compositeType.Blocks)
            {
                foreach (var port in block.Inputs.Union(block.Outputs))
                {
                    this.cachePortToBlock[port] = block;
                }
            }

            // Second, the dependency cache
            foreach (var wire in this.compositeType.Wires)
            {
                var sendingBlock = this.cachePortToBlock[wire.InputOfWire];
                var receivingBlock = this.cachePortToBlock[wire.OutputOfWire];

                this.dependencies.Add(receivingBlock, sendingBlock);
            }
        }

        /// <summary>
        /// Gets the blocks as ordered by their dataflwo
        /// </summary>
        /// <returns>The enumeration of blocks within the database</returns>
        public IEnumerable<ModelBlock> GetBlocksByDataFlow()
        {
            var toBeOrdered = this.compositeType.Blocks.ToList();

            while (toBeOrdered.Count > 0)
            {
                var hasAdded = false;

                // Looks for the one, which has no dependency to an item, which is not in list
                foreach (var block in toBeOrdered.ToList())
                {
                    bool canBeAdded = true;

                    // Gets the inputs
                    if (this.dependencies.ContainsKey(block))
                    {
                        foreach (var dependentBlock in this.dependencies[block])
                        {
                            // And check, if we are still dependent on one on these blocks
                            if (toBeOrdered.Contains(dependentBlock))
                            {
                                // We have a dependency
                                canBeAdded = false;
                                break;
                            }
                        }
                    }

                    // The block does not have any open dependency, so it can be returned
                    if (canBeAdded)
                    {
                        toBeOrdered.Remove(block);
                        hasAdded = true;
                        yield return block;
                    }
                }

                // We had a complete round but could not return any value
                // We have a cyclic dependency
                if (hasAdded == false)
                {
                    var builder = new StringBuilder();
                    foreach (var block in toBeOrdered)
                    {
                        builder.Append(block + ", ");
                    }

                    throw new InvalidOperationException("Cyclic dependency with " + builder.ToString());
                }
            }
        }

        /// <summary>
        /// Checks whether the source model block sends data 
        /// to the target element, according to the wires in CompositeType
        /// </summary>
        /// <param name="source">Source element to be queried</param>
        /// <param name="target">Target element to be queried</param>
        /// <returns>true, if data is sent</returns>
        public bool IsDataSent(ModelBlock source, ModelBlock target)
        {
            foreach (var wire in this.compositeType.Wires)
            {
                var sourceBlock = this.cachePortToBlock[wire.InputOfWire];
                var targetBlock = this.cachePortToBlock[wire.OutputOfWire];

                if (sourceBlock == source
                    && targetBlock == target)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
