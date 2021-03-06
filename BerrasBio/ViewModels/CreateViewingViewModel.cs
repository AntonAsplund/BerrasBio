﻿using BerrasBio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio.ViewModels
{
    public class CreateViewingViewModel
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public List<Salon> Salons { get; set; }
    }
}
