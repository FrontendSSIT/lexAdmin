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
        public ActionResult Add(string productName, string productDescription, string productPrice, string file1,string file2, string file3, string file4, string file5, string thumbnailFile1)
        {
            
   
            try
            {
                using (var client = new HttpClientDemo())
                {
                    Product aProduct = new Product();
                    aProduct.productName = productName;
                    aProduct.productDescription = productDescription;
                    aProduct.productPrice = productPrice;
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





                    //if (file1 != null)
                    //{

                    //    if (file1 != null && file1.ContentLength > 0)
                    //    {
                    //        MemoryStream stream = new MemoryStream(file1.ContentLength);
                    //        stream.Position = 0;
                    //        //product.file1.FileName = file1.FileName;
                    //        //product.file1.ContentType = file1.ContentType;
                    //        //product.file1.InputStream = file1.InputStream;

                    //    }

                    //}
                    //var img = Path.GetFileName(file1.FileName);

                    //if (ModelState.IsValid)
                    //{
                    //    if (file1 != null && file1.ContentLength > 0)
                    //    {
                    //        var path = Path.Combine(Server.MapPath("http://sellinbd.com/Lawyer-Shopregistration/lawyer_products/"),
                    //                                System.IO.Path.GetFileName(file1.FileName));
                    //        file1.SaveAs(path);


                    //    }
                    //    string targetFolder = HttpContext.Current.Server.MapPath("http://sellinbd.com/Lawyer-Shopregistration/lawyer_products/");
                    //string targetPath = Path.Combine(targetFolder, file1.FileName);
                    //file1.SaveAs(targetPath);



                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //var postTask = client.PostAsJsonAsync("registration/addPostLawyerShop.php", product);
                    //postTask.Wait();
                    //var result = postTask.Result;
                    //if (result.IsSuccessStatusCode)
                    //{

                    //    TempData["Message"] = "Product saved successfully";
                    //    //TempData["class"] = MessageUtility.Success;
                    //    // Session["IsLogin"] = false;

                    //    return RedirectToAction("Add", "Product");
                    //}

                    //else
                    //{
                    //    //FlashMessage.Warning("Sorry! Registration failed. Please Try again or contact with the administration.");
                    //    TempData["Message"] = "Sorry! Registration failed. Please Try again or contact with the administration.";
                    //    // TempData["class"] = MessageUtility.Error;
                    //    Session["IsLogin"] = false;
                    //    return View();
                    //}
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
    }
}