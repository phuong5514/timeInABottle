﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Core.Contracts.Services;
internal interface IBufferService
{
    public int BufferSize { get; }
    public void LoadBuffer();

}