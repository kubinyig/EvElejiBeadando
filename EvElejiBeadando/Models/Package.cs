using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvElejiBeadando.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime PostedAt { get; set; } = DateTime.Now;
        public string FromCity { get; set; } = "";
        public string ToCity { get; set; } = "";
        public PackageStatus Status { get; set; }
        public decimal Price { get; set; }
        public int DaysRemaining { get; set; }
    }


}
