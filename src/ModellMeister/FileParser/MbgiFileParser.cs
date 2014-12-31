using ModellMeister.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.FileParser
{
    public class MbgiFileParser
    {
        /// <summary>
        /// Defines the current parsing scope
        /// </summary>
        private CurrentScope currentScope = CurrentScope.Global;

        /// <summary>
        /// Stores the current tpye
        /// </summary>
        private ModelNativeType currentNativeType;

        /// <summary>
        /// Stores the root
        /// </summary>
        private ModelCompositeType root;

        /// <summary>
        /// Stores the current tpye
        /// </summary>
        private ModelBlock currentBlock;

        /// <summary>
        /// Stores the composite type
        /// </summary>
        private ModelCompositeType currentCompositeType;

        public ModelCompositeType ParseFileFromText(string loadedFile)
        {
            var reader = new StringReader(loadedFile);
            return this.ParseFile(reader);
        }

        /// <summary>
        /// Parses the file and includes the information. This method is not reentrable
        /// </summary>
        /// <param name="reader">The reader to be used</param>
        /// <returns>The created environment containing the complete information</returns>
        public ModelCompositeType ParseFile(TextReader reader)
        {
            this.root = new ModelCompositeType();
            this.root.Name = "_";

            this.currentScope = CurrentScope.Global;

            var lineParser = new LineParser();
            foreach (var line in lineParser.ParseFile(reader))
            {
                if (line.LineType == Model.EntityType.Type)
                {
                    this.ReadType(line);
                }
                else if (line.LineType == EntityType.TypeInput)
                {
                    this.ReadTypeInput(line);
                }
                else if (line.LineType == EntityType.TypeOutput)
                {
                    this.ReadTypeOutput(line);
                }
                else if (line.LineType == EntityType.Block)
                {
                    this.ReadBlock(line);
                }
                else if (line.LineType == EntityType.BlockInput)
                {
                    this.ReadBlockInput(line);
                }
                else if (line.LineType == EntityType.BlockOutput)
                {
                    this.ReadBlockOutput(line);
                }
                else if (line.LineType == EntityType.Wire)
                {
                    this.ReadWire(line);
                }
                else if (line.LineType == EntityType.CompositeType)
                {
                    this.ReadCompositeType(line);
                }
                else if (line.LineType == EntityType.CompositeBlock)
                {
                    this.ReadCompositeBlock(line);
                }
                else if (line.LineType == EntityType.CompositeTypeInput)
                {
                    this.ReadCompositeTypeInput(line);
                }
                else if (line.LineType == EntityType.CompositeTypeOutput)
                {
                    this.ReadCompositeTypeOutput(line);
                }
                else if (line.LineType == EntityType.CompositeWire)
                {
                    this.ReadCompositeWire(line);
                }
                else
                {
                    throw new InvalidOperationException("Unhandled type: " + line.LineType.ToString());
                }
            }

            return this.root;
        }

        private void ReadType(ParsedLine line)
        {
            this.currentBlock = null;
            this.currentScope = CurrentScope.InType;
            this.currentNativeType = new ModelNativeType();
            this.currentNativeType.Name = line.Name;

            this.root.Types.Add(this.currentNativeType);

            Console.WriteLine("Type is created: " + this.currentNativeType.Name);
        }

        private void ReadBlock(ParsedLine line)
        {
            var compositeBlock = this.root;
            this.currentNativeType = null;
            this.currentScope = CurrentScope.InBlock;
            this.currentBlock = ReadAndAddBlock(line, compositeBlock);
        }

        private void ReadWire(ParsedLine line)
        {
            if (line.Arguments.Count != 2)
            {
                throw new InvalidOperationException("Line for Wire (W) does not have two attributes");
            }

            this.currentScope = CurrentScope.Global;
            var compositeType = this.root;
            this.ReadAndAddWire(line, compositeType);
        }

        private void ReadTypeInput(ParsedLine line)
        {
            if (this.currentScope != CurrentScope.InType)
            {
                throw new InvalidOperationException("Unexpected scope: Expected InType");
            }

            var port = CreatePort(line);
            this.currentNativeType.Inputs.Add(port);

            Console.WriteLine("- Input is created: " + port.Name);
        }

        private void ReadTypeOutput(ParsedLine line)
        {
            if (this.currentScope != CurrentScope.InType)
            {
                throw new InvalidOperationException("Unexpected scope: Expected InType");
            }

            var port = CreatePort(line);
            this.currentNativeType.Outputs.Add(port);

            Console.WriteLine("- Output is created: " + port.Name);
        }

        private void ReadBlockInput(ParsedLine line)
        {
            if (this.currentScope != CurrentScope.InBlock)
            {
                throw new InvalidOperationException("Unexpected scope: Expected InBlock");
            }

            var port = CreatePort(line);
            this.currentBlock.Inputs.Add(port);

            Console.WriteLine("- Input is created: " + port.Name);
        }

        private void ReadBlockOutput(ParsedLine line)
        {
            if (this.currentScope != CurrentScope.InBlock)
            {
                throw new InvalidOperationException("Unexpected scope: Expected InBlock");
            }

            var port = CreatePort(line);
            this.currentBlock.Outputs.Add(port);

            Console.WriteLine("- Output is created: " + port.Name);
        }

        private void ReadCompositeType(ParsedLine line)
        {
            this.currentScope = CurrentScope.InCompositeBlock;

            this.currentCompositeType = new ModelCompositeType();
            this.currentCompositeType.Name = line.Name;

            this.root.Types.Add(this.currentCompositeType);
        }

        private void ReadCompositeBlock(ParsedLine line)
        {
            if (this.currentScope != CurrentScope.InCompositeBlock)
            {
                throw new InvalidOperationException("Unexpected scope: Expected InCompositeBlock");
            }

            this.ReadAndAddBlock(line, this.currentCompositeType);
        }

        private void ReadCompositeTypeInput(ParsedLine line)
        {
            if (this.currentScope != CurrentScope.InCompositeBlock)
            {
                throw new InvalidOperationException("Unexpected scope: Expected InCompositeBlock");
            }

            var port = CreatePort(line);
            this.currentCompositeType.Inputs.Add(port);

            Console.WriteLine("- Input for composite Block is created: " + port.Name);
        }

        private void ReadCompositeTypeOutput(ParsedLine line)
        {
            if (this.currentScope != CurrentScope.InCompositeBlock)
            {
                throw new InvalidOperationException("Unexpected scope: Expected InCompositeBlock");
            }

            var port = CreatePort(line);
            this.currentCompositeType.Inputs.Add(port);

            Console.WriteLine("- Output for composite Block is created: " + port.Name);
        }

        private void ReadCompositeWire(ParsedLine line)
        {
            if (this.currentScope != CurrentScope.InCompositeBlock)
            {
                throw new InvalidOperationException("Unexpected scope: Expected InCompositeBlock");
            }

            this.ReadAndAddWire(line, this.currentCompositeType);
        }

        /// <summary>
        /// Reads a block and adds it to the composite type
        /// </summary>
        /// <param name="line">Line to be parsed</param>
        /// <param name="compositeBlock">Composite type where the block will be added</param>
        /// <returns>The created block</returns>
        private ModelBlock ReadAndAddBlock(ParsedLine line, ModelCompositeType compositeBlock)
        {
            var currentBlock = new ModelBlock();
            currentBlock.Name = line.Name;
            currentBlock.Type =
                this.FindModelType(line.GetProperty(PropertyType.OfType));
            this.PopulateBlock(currentBlock);

            Console.WriteLine("Block is created: " + currentBlock.Name);
            compositeBlock.Blocks.Add(currentBlock);
            return currentBlock;
        }

        private void ReadAndAddWire(ParsedLine line, ModelCompositeType compositeType)
        {
            var wire = new ModelWire();
            compositeType.Wires.Add(wire);

            Console.WriteLine("Wire is created between "
                + line.Arguments[0]
                + " and "
                + line.Arguments[1]);

            var inputPort = this.FindPort(compositeType, line.Arguments[0]);
            var outputPort = this.FindPort(compositeType, line.Arguments[1]);
            wire.InputOfWire = inputPort;
            wire.OutputOfWire = outputPort;
        }

        /// <summary>
        /// Populates the ports of a block by looking at the input 
        /// and output ports of the associated type
        /// </summary>
        /// <param name="modelBlock">The modelblock to be populated</param>
        private void PopulateBlock(ModelBlock modelBlock)
        {
            var otherType = modelBlock.Type;
            if (otherType == null)
            {
                throw new InvalidOperationException("Block '" + modelBlock.Name + "' has no type");
            }

            foreach (var inputPort in otherType.Inputs)
            {
                modelBlock.Inputs.Add(inputPort.Clone());
            }

            foreach (var outputPort in otherType.Outputs)
            {
                modelBlock.Outputs.Add(outputPort.Clone());
            }
        }

        /// <summary>
        /// Creates the port by a parsed line
        /// </summary>
        /// <param name="line">ine to be parsed</param>
        /// <returns>The created port</returns>
        public static ModelPort CreatePort(ParsedLine line)
        {
            var port = new ModelPort();
            port.Name = line.Name;
            port.DataType = LineParser.ConvertToDataType(line.GetProperty(PropertyType.OfType));
            return port;
        }

        /// <summary>
        /// Finds the modeltype by modelname
        /// </summary>
        /// <param name="typeName">Name of the type to be looked to </param>
        private ModelType FindModelType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                throw new InvalidOperationException("No type is given");
            }

            var found = this.root.Types.Where(x => x.Name == typeName).FirstOrDefault();
            if (found == null)
            {
                throw new InvalidOperationException("Type: " + typeName + " was not found given");
            }

            return found;
        }

        /// <summary>
        /// Finds a port by name within the given composite type
        /// </summary>
        /// <param name="compositeType">Composite type to be used</param>
        /// <param name="portName">Name of the port to be used</param>
        /// <returns></returns>
        private ModelPort FindPort(ModelCompositeType compositeType, string portName)
        {
            var portParts = portName.Split(new[] { '.' });
            if (portParts.Length == 1)
            {
                // Look into the composite type directly
                var found = compositeType.Inputs.Union(compositeType.Outputs)
                    .Where(x => x.Name == portName)
                    .FirstOrDefault();

                if (found == null)
                {
                    throw new InvalidOperationException("Port '" + portName + "' was not found");
                }

                return found;
            }
            else if (portParts.Length == 2)
            {
                // Find the block
                var foundBlock = compositeType.Blocks
                    .Where(x => x.Name == portParts[0])
                    .FirstOrDefault();

                if (foundBlock == null)
                {
                    throw new InvalidOperationException("Block for '" + portName + "' was not found");
                }

                // Find the port
                // Look into the composite type directly
                var found = foundBlock.Inputs.Union(foundBlock.Outputs)
                    .Where(x => x.Name == portParts[1])
                    .FirstOrDefault();

                if (found == null)
                {
                    throw new InvalidOperationException("Port '" + portName + "' was not found");
                }

                return found;
            }
            else
            {
                throw new InvalidOperationException("Portname '" + portName + "' is not understood");
            }
        }

        private enum CurrentScope
        {
            Global,
            InType,
            InBlock,
            InCompositeBlock
        }
    }
}
