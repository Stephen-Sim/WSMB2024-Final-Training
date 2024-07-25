using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Open_Session4.Models
{
    public class Team
    {
        public int rank { get; set; }
        public string name { get; set; }
        public string WDL { get; set; }
        public int point { get; set; }
        public int aggregatePoint { get; set; }
    }
}
