﻿using System;

namespace Welt.API.Logging
{
    public interface ILogProvider
    {
        void Log(LogCategory category, string text, params object[] parameters);
    }
}