using System;

using System;
using System.Collections.Generic;
using System.Text;

namespace App1.Models
{
    public class Attempt
    {
        public string username { get; set; }
        public string role { get; set; }
        public int attempt { get; set; }
        public string completion { get; set; }
        public string grade { get; set; }
        public DateTime datetime { get; set; }
    }
}
