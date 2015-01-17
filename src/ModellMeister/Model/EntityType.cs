using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Model
{
    /// <summary>
    /// Enumeration of the possible model types
    /// </summary>
    public enum EntityType
    {
        Type, 
        Block, 
        Setting,
        Wire,
        BlockInput,
        BlockOutput, 
        TypeInput,
        TypeOutput,
        CompositeType,
        CompositeTypeInput,
        CompositeTypeOutput,
        CompositeBlock,
        CompositeWire,
        NameSpace,

        CommandImportFile,
        CommandLoadLibrary
    }
}
