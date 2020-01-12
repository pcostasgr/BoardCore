
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardCore.Models
{
    public class CheckLists

    {
        public Int64 CHECKLISTID { get; set; }
        public string TITLE { get; set; }
        public Int64 USERID { get; set; }
        public Int64 CARDID { get; set; }

        public List<CheckListItems> ITEMS  { get; set; }

 public override string ToString()
        {
            return "CHECKLISTID:" + this.CHECKLISTID + " TITLE:" 
            + this.TITLE + " CARDID:" + this.CARDID + " USERID:" + this.USERID;
        }
    }
}