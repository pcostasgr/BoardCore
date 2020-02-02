
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardCore.Models
{
    public class Cards

    {
        public Int64 CARDID { get; set; }
        public string CARDTITLE { get; set; }
        public string CARDDESCR { get; set; }
        public Int64 LISTID { get; set; }
        public DateTime? CARDDATE { get; set; }
        public Int64 USERID {get; set;}

         public override string ToString()
        {
            return "CARDID:" + this.CARDID + " CARDTITLE:" 
            + this.CARDTITLE + " CARDDESCR:" 
            + this.CARDDESCR + " LISTID:" + this.LISTID 
            + " USERID:" + this.USERID;
        }
    }
}