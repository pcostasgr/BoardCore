
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardCore.Models
{
    public class Lists

    {
        public Int64 LISTID { get; set; }
        public string ListTitle { get; set; }
        public Int64 USERID { get; set; }

        public List<Cards> cardData { get; set;}
    }
}