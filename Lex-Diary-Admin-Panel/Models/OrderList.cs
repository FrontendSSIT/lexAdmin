using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lex_Diary_Admin_Panel.Models
{
    public class OrderList
    {
        public string pending { get; set; }
        public string delivered { get; set; }
        public string cancelled { get; set; }
        public string total { get; set; }
    }
}