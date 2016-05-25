#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Welt.Entities.EntityData;

namespace Welt.Entities
{
    /// <summary>
    ///     Used to create a byte array map of an entity's metadata.
    /// </summary>
    public class DataMap : Dictionary<string, EntityDataType>
    {
        private const int MAX_PROPERTIES = 32;
        
        public DataMap() : base(MAX_PROPERTIES)
        {
            
        }
    }
}