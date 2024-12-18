﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeInABottle.Contracts.Services;
public interface ILocationService
{
    public Task<(double Latitude, double Longitude)> GetCoordinatesAsync();
}
