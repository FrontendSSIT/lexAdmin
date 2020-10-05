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
        public class ImageFileClass
        {
            public string file1 { get; set; }
        }
        [HttpPost]
        public ActionResult Add(/*string productName, string productDescription, string productPrice, */ string input, HttpPostedFileBase file1/*,HttpPostedFileBase file2, HttpPostedFileBase file3, HttpPostedFileBase file4, HttpPostedFileBase file5*/)
        {


            //if (file != null && file.ContentLength > 0)
            //{
            //    var fileName = Path.GetFileName(file.FileName);



            //    file.SaveAs(path);
            //}

            //var filename = Path.GetFileName(file.FileName);
            //var path = Path.Combine(Server.MapPath("http://sellinbd.com/Lawyer-Shopregistration/lawyer_products/"), filename);
            //file.SaveAs(path);

            try
            {
                ImageFileClass c1 = new ImageFileClass();
                c1.file1 = input;
                using (var client = new HttpClientDemo())
                {
                    Product product = new Product();
                    if (file1 != null)
                    {
                        string file1Name = Path.GetFileName(file1.FileName);
                        byte[] file1PictureAsByte = new byte[file1.ContentLength];
                        using (BinaryReader theReader = new BinaryReader(file1.InputStream))
                        {
                            file1PictureAsByte = theReader.ReadBytes(file1.ContentLength);
                        }
                        string file1DataAsString = Convert.ToBase64String(file1PictureAsByte);
                        product.file1 = file1DataAsString;
                    }
                    //if (file2 != null)
                    //{
                    //    string file2Name = Path.GetFileName(file2.FileName);
                    //    byte[] file2PictureAsByte = new byte[file2.ContentLength];
                    //    using (BinaryReader theReader = new BinaryReader(file1.InputStream))
                    //    {
                    //        file2PictureAsByte = theReader.ReadBytes(file2.ContentLength);
                    //    }
                    //    string file2DataAsString = Convert.ToBase64String(file2PictureAsByte);
                    //    product.file2 = file2DataAsString;
                    //}
                    //if (file3 != null)
                    //{
                    //    string file3Name = Path.GetFileName(file3.FileName);
                    //    byte[] file3PictureAsByte = new byte[file3.ContentLength];
                    //    using (BinaryReader theReader = new BinaryReader(file3.InputStream))
                    //    {
                    //        file3PictureAsByte = theReader.ReadBytes(file3.ContentLength);
                    //    }
                    //    string file3DataAsString = Convert.ToBase64String(file3PictureAsByte);
                    //    product.file3 = file3DataAsString;
                    //}
                    //if (file4 != null)
                    //{
                    //    string file4Name = Path.GetFileName(file4.FileName);
                    //    byte[] file4PictureAsByte = new byte[file4.ContentLength];
                    //    using (BinaryReader theReader = new BinaryReader(file4.InputStream))
                    //    {
                    //        file4PictureAsByte = theReader.ReadBytes(file4.ContentLength);
                    //    }
                    //    string file4DataAsString = Convert.ToBase64String(file4PictureAsByte);
                    //    product.file4 = file4DataAsString;
                    //}

                    //if (file5!= null)
                    //{
                    //    string file5Name = Path.GetFileName(file5.FileName);
                    //    byte[] file5PictureAsByte = new byte[file5.ContentLength];
                    //    using (BinaryReader theReader = new BinaryReader(file5.InputStream))
                    //    {
                    //        file5PictureAsByte = theReader.ReadBytes(file4.ContentLength);
                    //    }
                    //    string file5DataAsString = Convert.ToBase64String(file5PictureAsByte);
                    //    product.file5 = file5DataAsString;
                    //}
                    //product.productName = productName;
                    //product.productDescription = productDescription;
                    //product.productPrice = productPrice;
                    //product.userNumber = ConfigurationManager.AppSettings["userName"].ToString();


                    client.BaseAddress = new Uri("http://sellinbd.com/Lawyer-Shop/");
                    var postTask = client.PostAsJsonAsync("registration/test.php", c1);
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
                        //FlashMessage.Warning("Sorry! Registration failed. Please Try again or contact with the administration.");
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