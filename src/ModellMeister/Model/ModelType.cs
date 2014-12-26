﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Model
{
    /// <summary>
    /// Enumeration of the possible model types
    /// </summary>
    public enum ModelType
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
        CompositeTypeOutput
    }
}
