using BerrasBio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio.ViewModels
{
    public class DetailsViewModel
    {
        public Movie Movie { get; set; }
        public List<Viewing> Viewings { get; set; }
    }
}
