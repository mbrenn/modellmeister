using BurnSystems.Logger;
using ModellMeister.Logic;
using ModellMeister.Model;
using ModellMeister.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.FileParser
{
    public class MbgiFileParser
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        private static ClassLogger logger = new ClassLogger(typeof(MbgiFileParser));

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

        /// <summary>
        /// Stores the name of the current namespace
        /// </summary>
        private string currentNameSpace = string.Empty;

        /// <summary>
        /// Stores the value whether the parser has been used. 
        /// It is not allowed to use the parser twice. 
        /// </summary>
        private bool alreadyUsed = false;

        /// <summary>
        /// Stores the path of the file, which is currently loaded into
        /// the context. 
        /// </summary>
        private string pathOfContext;

        public ModelCompositeType ParseFileFromText(string fileContent)
        {
            var reader = new StringReader(fileContent);
            return this.ParseFileFromReader(reader);
        }

        public ModelCompositeType ParseFileFromFile(string loadedFile)
        {
            var fullPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, loadedFile));
            this.pathOfContext = Path.GetDirectoryName(fullPath);

            if (!File.Exists(fullPath))
            {
                throw new InvalidOperationException("File not found: " + fullPath);
            }

            using (var reader = new StreamReader(fullPath))
            {
                return this.ParseFileFromReader(reader);
            }
        }

        /// <summary>
        /// Parses the file and includes the information. This method is not reentrable
        /// </summary>
        /// <param name="reader">The reader to be used</param>
        /// <returns>The created environment containing the complete information</returns>
        public ModelCompositeType ParseFileFromReader(string pathOfContext, TextReader reader)
        {
            this.pathOfContext = pathOfContext;
            return this.ParseFileFromReader(reader);
        }

        private ModelCompositeType ParseFileFromReader(TextReader reader)
        {
            if (this.alreadyUsed)
            {
                throw new InvalidOperationException("The parser was already used. It is not possible to reuse the parser");
            }

            this.alreadyUsed = true;
            this.root = new ModelCompositeType();
            this.root.Name = "_";
            this.root.NameSpace = "ModelBased";
            this.currentNameSpace = "ModelBased";

            this.currentScope = CurrentScope.Global;

            var lineParser = new LineParser();
            foreach (var line in lineParser.ParseFile(reader))
            {
                this.ParseLine(line);
            }

            return this.root;
        }

        private void ParseLine(ParsedLine line)
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
            else if (line.LineType == EntityType.NameSpace)
            {
                this.ReadNameSpace(line);
            }
            else if (line.LineType == EntityType.CommandImportFile)
            {
                this.ReadCommandImportFile(line);
            }
            else if (line.LineType == EntityType.CommandLoadLibrary)
            {
                this.ReadCommandLoadLibrary(line);
            }
            else
            {
                throw new InvalidOperationException("Unhandled type: " + line.LineType.ToString());
            }
        }

        private void ReadCommandImportFile(ParsedLine line)
        {
            var oldContext = this.pathOfContext;

            var fullPath = Path.GetFullPath(Path.Combine(this.pathOfContext, line.Arguments[0]));
            this.pathOfContext = Path.GetDirectoryName(fullPath);

            if (!File.Exists(fullPath))
            {
                throw new InvalidOperationException("File not found: " + fullPath);
            }

            using (var reader = new StreamReader(fullPath))
            {
                var lineParser = new LineParser();
                foreach (var innerLine in lineParser.ParseFile(reader))
                {
                    this.ParseLine(innerLine);
                }
            }

            this.pathOfContext = oldContext;
        }

        /// <summary>
        /// Reads an assembly from the command line
        /// </summary>
        /// <param name="line">Line to be parsed</param>
        private void ReadCommandLoadLibrary(ParsedLine line)
        {
            var fullPath = Path.GetFullPath(Path.Combine(this.pathOfContext, line.Arguments[0]));

            if (!File.Exists(fullPath))
            {
                throw new InvalidOperationException("Library not found: " + fullPath);
            }

            var assembly = Assembly.LoadFile(fullPath);

            // Now we get through the types and try to load them
            foreach (var type in assembly.GetTypes())
            {
                if (type.GetInterfaces().Any(x => x == typeof(IModelType)))
                {
                    // We got it, load it
                    var nativeType = new ModelNativeType();

                    var typeName = type.FullName;
                    var posDot = typeName.LastIndexOf('.');
                    if (posDot == -1)
                    {
                        nativeType.Name = typeName;
                        nativeType.NameSpace = string.Empty;
                    }
                    else
                    {
                        nativeType.Name = typeName.Substring(posDot + 1);
                        nativeType.NameSpace = typeName.Substring(0, posDot);
                    }

                    nativeType.Name = type.FullName;
                    nativeType.IsInLibrary = true;
                    nativeType.IsAutoGenerated = false;

                    // Read the ports of the type
                    foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                    {
                        var portAttribute =
                            property.GetCustomAttribute(typeof(PortAttribute)) as PortAttribute;

                        if (portAttribute != null)
                        {
                            var port = new ModelPort();
                            port.Name = property.Name;
                            port.DataType = Conversion.ConvertToDataType(property.PropertyType);

                            if (portAttribute.PortType == PortType.Input)
                            {
                                nativeType.Inputs.Add(port);
                            }
                            else if (portAttribute.PortType == PortType.Output)
                            {
                                nativeType.Outputs.Add(port);
                            }
                            else
                            {
                                throw new InvalidOperationException("Unknown port type: " + portAttribute.PortType.ToString());
                            }
                        }
                    }

                    this.root.Types.Add(nativeType);
                }
            }
        }

        /// <summary>
        /// Reads the namespace
        /// </summary>
        /// <param name="line">Line to be parsed</param>
        private void ReadNameSpace(ParsedLine line)
        {
            this.root.NameSpace = line.Name;
            this.currentNameSpace = line.Name;
        }

        private void ReadType(ParsedLine line)
        {
            this.currentBlock = null;
            this.currentScope = CurrentScope.InType;
            this.currentNativeType = new ModelNativeType();
            this.currentNativeType.Name = line.Name;
            this.currentNativeType.NameSpace = this.currentNameSpace;

            this.root.Types.Add(this.currentNativeType);

            logger.Verbose("Type is created: " + this.currentNativeType.Name);
        }

        private void ReadBlock(ParsedLine line)
        {
            var compositeBlock = this.root;
            this.currentNativeType = null;
            this.currentScope = CurrentScope.InBlock;
            this.currentBlock = this.ReadAndAddBlock(line, compositeBlock, true);
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

            logger.Verbose("- Input is created: " + port.Name);
        }

        private void ReadTypeOutput(ParsedLine line)
        {
            if (this.currentScope != CurrentScope.InType)
            {
                throw new InvalidOperationException("Unexpected scope: Expected InType");
            }

            var port = CreatePort(line);
            this.currentNativeType.Outputs.Add(port);

            logger.Verbose("- Output is created: " + port.Name);
        }

        private void ReadBlockInput(ParsedLine line)
        {
            if (this.currentScope != CurrentScope.InBlock)
            {
                throw new InvalidOperationException("Unexpected scope: Expected InBlock");
            }

                        
            // If the type is auto generated, the type also needs to get the ports
            if (this.currentBlock.Type.IsAutoGenerated)
            {
                var port = CreatePort(line);
                this.currentBlock.Inputs.Add(port);
                this.currentBlock.Type.Inputs.Add(port);

                logger.Verbose("- Input is created: " + port.Name);
            }
            else
            {
                // Find port with the name
                var portList = this.currentBlock.Inputs;
                UpdatePort(line, portList);
            }
        }

        private void ReadBlockOutput(ParsedLine line)
        {
            if (this.currentScope != CurrentScope.InBlock)
            {
                throw new InvalidOperationException("Unexpected scope: Expected InBlock");
            }

            // If the type is auto generated, the type also needs to get the ports
            if (this.currentBlock.Type.IsAutoGenerated)
            {
                var port = CreatePort(line);
                this.currentBlock.Outputs.Add(port);
                this.currentBlock.Type.Outputs.Add(port);

                logger.Verbose("- Output is created: " + port.Name);
            }
            else
            {
                // Find port with the name
                var portList = this.currentBlock.Inputs;
                UpdatePort(line, portList);
            }
        }

        private void ReadCompositeType(ParsedLine line)
        {
            this.currentScope = CurrentScope.InCompositeBlock;

            this.currentCompositeType = new ModelCompositeType();
            this.currentCompositeType.Name = line.Name;
            this.currentCompositeType.NameSpace = this.currentNameSpace;

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

            logger.Verbose("- Input for composite Block is created: " + port.Name);
        }

        private void ReadCompositeTypeOutput(ParsedLine line)
        {
            if (this.currentScope != CurrentScope.InCompositeBlock)
            {
                throw new InvalidOperationException("Unexpected scope: Expected InCompositeBlock");
            }

            var port = CreatePort(line);
            this.currentCompositeType.Inputs.Add(port);

            logger.Verbose("- Output for composite Block is created: " + port.Name);
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
        /// <param name="mayAutogenerate">Flag, whether autogeneration is allowed</param>
        /// <returns>The created block</returns>
        private ModelBlock ReadAndAddBlock(ParsedLine line, ModelCompositeType compositeBlock, bool mayAutogenerate = false)
        {
            var blockName = line.GetProperty(PropertyType.OfType);
            ModelType blockType;
            if (blockName == null && mayAutogenerate)
            {
                // Autocreate a type
                blockType = new ModelNativeType();
                blockType.Name = line.Name + "Type";
                blockType.NameSpace = this.currentNameSpace;
                blockType.IsAutoGenerated = true;
                this.root.Types.Add(blockType);
            }
            else
            {
                // Finds the type
                blockType = this.FindModelType(blockName);
            }

            // Creates the model and allocates the type
            var currentBlock = new ModelBlock();
            currentBlock.Name = line.Name;
            currentBlock.Type = blockType;
            this.PopulateBlock(currentBlock);

            logger.Verbose("Block is created: " + currentBlock.Name);
            compositeBlock.Blocks.Add(currentBlock);
            return currentBlock;
        }

        private void ReadAndAddWire(ParsedLine line, ModelCompositeType compositeType)
        {
            var wire = new ModelWire();
            compositeType.Wires.Add(wire);

            logger.Verbose("Wire is created between "
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

            if (line.GetProperty(PropertyType.OfType) == null)
            {
                throw new InvalidOperationException("No type for the port is given: " + port.Name);
            }

            port.DataType = LineParser.ConvertToDataType(line.GetProperty(PropertyType.OfType));

            var defaultValueAsString
                = line.GetProperty(PropertyType.DefaultValue);
            if (!string.IsNullOrEmpty(defaultValueAsString))
            {
                port.DefaultValue = Conversion.ToDataType(defaultValueAsString, port.DataType);
            }

            return port;
        }

        private static void UpdatePort(ParsedLine line, List<ModelPort> portList)
        {
            var foundPort = portList.Where(x => x.Name == line.Name).FirstOrDefault();
            if (foundPort == null)
            {
                throw new InvalidOperationException("Did not find the block: " + line.Name);
            }

            var defaultValue = line.Parameters[PropertyType.DefaultValue];
            if (defaultValue == null)
            {
                throw new InvalidOperationException("No default Value is given");
            }

            foundPort.DefaultValue = Conversion.ToDataType(
                defaultValue,
                foundPort.DataType);

            logger.Verbose("- Port is updated: " + foundPort.Name);
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

            var found = this.root.Types.Where(x => 
                {
                    if (x.Name == typeName)
                    {
                        return true;
                    }

                    if (x.Name.StartsWith("ModellMeister.")
                        && x.Name.Substring("ModellMeister.".Length) == typeName)
                    {
                        return true;
                    }

                    return false;
                }).FirstOrDefault();
            if (found == null)
            {
                throw new InvalidOperationException("Type: " + typeName + " was not found given");
            }

            if (found.IsAutoGenerated)
            {
                throw new InvalidOperationException(
                    "The model is '" 
                    + found.Name 
                    + "' auto-generated. It can not be reused. ");
            }

            return found;
        }

        /// <summary>
        /// Finds a port by name within the given composite type
        /// </summary>
        /// <param name="compositeType">Composite type to be used</param>
        /// <param name="portName">Name of the port to be used</param>
        /// <returns>The found port or an exception when not found</returns>
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
