﻿#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion
namespace Welt.API.Forge
{
    public interface IWorldSystem
    {
        string Name { get; }
        IWorld[] Worlds { get; }
         
    }
}