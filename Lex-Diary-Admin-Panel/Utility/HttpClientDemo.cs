using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Lex_Diary_Admin_Panel.Utility
{
    public class HttpClientDemo : HttpClient
    {
        public HttpClientDemo()
        {
            DefaultRequestHeaders.Accept.Clear();
            DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
            DefaultRequestHeaders.Add("ccept-Encoding", "gzip, deflate, br");
            DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
            DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            //string url = ConfigurationManager.AppSettings["url"].ToString();
            //BaseAddress = new Uri(url);
            string UserName = ConfigurationManager.AppSettings["userName"].ToString();
            string Password = ConfigurationManager.AppSettings["password"].ToString();
            BaseAddress = new Uri("http://sellinbd.com/Lawyer-Shop/");
        }
    }
}