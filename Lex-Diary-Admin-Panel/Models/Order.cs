using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lex_Diary_Admin_Panel.Models
{
    public class Order
    {
        public string o_id { get; set; }
        public string userNumber { get; set; }
        public string userName { get; set; }
        public string totalAmount { get; set; }
    }
}