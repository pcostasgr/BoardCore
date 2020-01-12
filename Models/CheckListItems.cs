
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardCore.Models
{
    public class CheckListItems

    {
        public Int64 CLITEMID { get; set; }
        public string ITEMTITLE { get; set; }
        public Int64 USERID { get; set; }
        public Int64 CHECKLISTID { get; set; }

        public Int16 ISCHECKED { get; set ;}


    }
}