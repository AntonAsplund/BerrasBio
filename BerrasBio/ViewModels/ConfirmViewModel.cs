using BerrasBio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio.ViewModels
{
    public class ConfirmViewModel
    {
        public Viewing Viewing { get; set; }
        public List<int> SeatNumbers { get; set; }
    }
}
