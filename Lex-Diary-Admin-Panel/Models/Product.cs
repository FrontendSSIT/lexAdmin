using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lex_Diary_Admin_Panel.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string productName { get; set; }
        [AllowHtml]
        public string productDescription { get; set; }
        public string productPrice { get; set; }
        public string file1 { get; set; }
        public string file2 { get; set; }
        public string file3 { get; set; }
        public string file4 { get; set; }
        public string file5 { get; set; }
        public string thumbnail { get; set; }
        public string rating { get; set; }
        public string date { get; set; }
        public string discountPercentage { get; set; }
        public string userNumber { get; set; }
       
    }
}