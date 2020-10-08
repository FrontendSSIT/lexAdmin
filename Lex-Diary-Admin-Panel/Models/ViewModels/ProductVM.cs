using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lex_Diary_Admin_Panel.Models.ViewModels
{
    public class ProductVM
    {
        public int id { get; set; }
            public string productName { get; set; }
            public string productDescription { get; set; }
            public string productPrice { get; set; }
            public string file1 { get; set; }
            public string file2 { get; set; }
            public string file3 { get; set; }
            public string file4 { get; set; }
            public string file5 { get; set; }
            public string thumbnail { get; set; }
            public string rating { get; set; }
            public string discountPercentage { get; set; }
            public string colors { get; set; }
            public string sizes { get; set; }
            public string discountPrice { get; set; }

    }
}