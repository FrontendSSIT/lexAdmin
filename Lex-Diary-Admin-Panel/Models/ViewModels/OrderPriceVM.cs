using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lex_Diary_Admin_Panel.Models.ViewModels
{
    public class OrderPriceVM
    {
        
        public string subTotal { get; set; }
        public string deliveryCharge { get; set; }
        public string total { get; set; }
    }
}