using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        [MaxLength(12, ErrorMessage = "Kan inte boka fler än 12 biljetter")]
        public List<Ticket> Tickets { get; set; }
        [ForeignKey("User")]

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
