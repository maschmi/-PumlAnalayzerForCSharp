﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public interface IDoLog
    {        
        void Debug(string logmessage);
        void Error(string logmessage);
        void Info(string logmessage);
        void Warning(string logmessage);
        void Verbose(string logmessage);
    }
}
