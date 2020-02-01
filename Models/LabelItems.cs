
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardCore.Models
{
    public class LabeItems

    {
        public Int64 LABELITEMID { get; set; }
        public string COLOR { get; set; }
        public Int64 CARDID { get; set; }
        public Int64 USERID { get; set; }

        public Int16 WIDTH {get; set;}
        public Int16 HEIGHT {get; set;}
    }
}