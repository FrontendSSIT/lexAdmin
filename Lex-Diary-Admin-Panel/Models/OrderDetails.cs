using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lex_Diary_Admin_Panel.Models
{
    public class OrderDetails
    {
        public string thumbnail { get; set; }
        public string productName { get; set; }
        public string quantity { get; set; }
        public string colors { get; set; }
        public string sizes { get; set; }
        public string productPrice { get; set; }
        public string totalPrice { get; set; }
        public string discountPrice { get; set; }
        public string discountPercentage { get; set; }
    }
}