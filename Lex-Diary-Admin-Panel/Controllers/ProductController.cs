using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using Lex_Diary_Admin_Panel.Models;
using System.Net;
using Lex_Diary_Admin_Panel.Utility;
using System.IO;
using System.Configuration;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Lex_Diary_Admin_Panel.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            bool isLogin = false;
            if (Session["isLogin"] != null)
            {
                isLogin = (bool)Session["isLogin"];
            }
            if (!isLogin)
            {
                Session["isLogin"] = false;
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Add(string productName, string productDescription, string productPrice,string discountPercentage, string file1,string file2, string file3, string file4, string file5, string thumbnailFile1)
        {
            
   
            try
            {
                using (var client = new HttpClientDemo())
                {
                    Product aProduct = new Product();
                    aProduct.productName = productName;
                    aProduct.productDescription = productDescription;
                    aProduct.productPrice = productPrice;
                    aProduct.discountPercentage = discountPercentage;
                    if (!string.IsNullOrEmpty(file1))
                    {
                        aProduct.file1 = file1;
                        aProduct.thumbnail = thumbnailFile1;
                    }
                    if (!string.IsNullOrEmpty(file2))
                    {
                        aProduct.file2 = file2;
                    }else
                    {
                        aProduct.file2 = "NULL";
                    }
                    if (!string.IsNullOrEmpty(file3))
                    {
                        aProduct.file3 = file3;
                    }
                    else
                    {
                        aProduct.file3 = "NULL";
                    }
                    if (!string.IsNullOrEmpty(file4))
                    {
                        aProduct.file4 = file4;
                    }
                    else
                    {
                        aProduct.file4 = "NULL";
                    }
                    if (!string.IsNullOrEmpty(file5))
                    {
                        aProduct.file5 = file5;
                    }
                    else
                    {
                        aProduct.file5 = "NULL";
                    }
                    aProduct.userNumber = ConfigurationManager.AppSettings["userName"].ToString();
                    var postTask = client.PostAsJsonAsync("registration/addPost.php", aProduct);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                        TempData["Message"] = "Product saved successfully";
                        //TempData["class"] = MessageUtility.Success;
                        // Session["IsLogin"] = false;

                        return RedirectToAction("Add", "Product");
                    }

                    else
                    {
                        TempData["Message"] = "Sorry! Registration failed. Please Try again or contact with the administration.";
                        // TempData["class"] = MessageUtility.Error;
                        Session["IsLogin"] = false;
                        return View();
                    }
                }


                return View();
               
            }
            catch (Exception ex)
            {

                return View();
            }
        }

        public ActionResult List()
        {
            List<Product> products = new List<Product>();
            bool isLogin = false;
            if (Session["isLogin"] != null)
            {
                isLogin = (bool)Session["isLogin"];
            }
            if (!isLogin)
            {
                Session["isLogin"] = false;
                return RedirectToAction("Login", "Home");
            }

            try
            {

                using (var client = new HttpClientDemo())
                {
                  
                    var responseTask = client.GetAsync("product/readProducts.php");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var resultTask = result.Content.ReadAsStringAsync().Result;
                        products = JsonConvert.DeserializeObject<List<Product>>(resultTask);
                        TempData["Message"] = "Product get Successfully";
                        TempData["class"] = MessageUtility.Success;
                    }
                    else
                    {
                        products = null;
                        TempData["Message"] = "Sorry! Something went wrong. Please Try Again";
                        TempData["class"] = MessageUtility.Error;
                    }
                    //return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
               // Session["isLogin"] = "NoAccount";
                //return RedirectToAction("Logout", "Login");
            }
            return View(products);
        }
    }
}