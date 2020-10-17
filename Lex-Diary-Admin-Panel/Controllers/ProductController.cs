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
using Lex_Diary_Admin_Panel.Models.ViewModels;

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
            using (var client = new HttpClientDemo())
            {
                List<Color> colorList = new List<Color>();
                var responseTask = client.GetAsync("product/readColors.php");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var resultTask = result.Content.ReadAsStringAsync().Result;
                    colorList = JsonConvert.DeserializeObject<List<Color>>(resultTask);
                    Session["ColorList"] = colorList;
                    ViewBag.ColorList = colorList.ToList();
                }
                else
                {
                    colorList = null;
                   
                }
            }
                return View();
        }
        [HttpPost]
        public ActionResult Add(Product product,string thumbnailFile1, List<string> colors, List<double> sizes)
        {

            
            try
            {
                using (var client = new HttpClientDemo())
                {
                    List<Color> colorList = new List<Color>();
                    var responseTask = client.GetAsync("product/readColors.php");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var resultTask = result.Content.ReadAsStringAsync().Result;
                        colorList = JsonConvert.DeserializeObject<List<Color>>(resultTask);
                        Session["ColorList"] = colorList;
                        ViewBag.ColorList = colorList.ToList();
                    }
                    else
                    {
                        colorList = null;

                    }
                }

                Product aProduct = new Product();
                using (var client = new HttpClientDemo())
                {



                    if (sizes != null)
                    {
                        var sizeString = String.Join(",", sizes);
                        aProduct.sizes = sizeString;
                    }
                    else aProduct.sizes = "NULL";
                    if (colors != null)
                    {
                        var colorString = String.Join(",", colors);
                        aProduct.colors = colorString;
                    }
                    else aProduct.colors = "NULL";

                    aProduct.productName = product.productName;
                    aProduct.productDescription = product.productDescription;
                    aProduct.productPrice = product.productPrice;
                    aProduct.discountPercentage = product.discountPercentage;
                    if (!string.IsNullOrEmpty(product.file1))
                    {
                        aProduct.file1 = product.file1;
                        aProduct.thumbnail = thumbnailFile1;
                    }
                    if (!string.IsNullOrEmpty(product.file2))
                    {
                        aProduct.file2 = product.file2;
                    }
                    else
                    {
                        aProduct.file2 = "NULL";
                    }
                    if (!string.IsNullOrEmpty(product.file3))
                    {
                        aProduct.file3 = product.file3;
                    }
                    else
                    {
                        aProduct.file3 = "NULL";
                    }
                    if (!string.IsNullOrEmpty(product.file4))
                    {
                        aProduct.file4 = product.file4;
                    }
                    else
                    {
                        aProduct.file4 = "NULL";
                    }
                    if (!string.IsNullOrEmpty(product.file5))
                    {
                        aProduct.file5 = product.file5;
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
                        TempData["class"] = MessageUtility.Success;
                        // Session["IsLogin"] = false;

                        return RedirectToAction("Add", "Product");
                    }

                    else
                    {
                        TempData["Message"] = "Sorry! Registration failed. Please Try again or contact with the administration.";
                        TempData["class"] = MessageUtility.Error;
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
                        foreach(var product in products)
                        {
                            List<string> listOfColors = new List<string>(product.colors.Split(',',':'));
                            listOfColors.RemoveAll(u => u.StartsWith("#"));
                            listOfColors.RemoveAll(u => u.Contains("NULL"));
                            var colors = String.Join(",", listOfColors);
                            product.colors = colors;
                        }
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
        public ActionResult Edit(int id)
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
            Product product = new Product();
            try
            {
                using (var client = new HttpClientDemo())
                {
                    List<Color> colorList = new List<Color>();
                    var responseTask = client.GetAsync("product/readColors.php");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var resultTask = result.Content.ReadAsStringAsync().Result;
                        colorList = JsonConvert.DeserializeObject<List<Color>>(resultTask);
                        Session["ColorList"] = colorList;
                        ViewBag.ColorList = colorList.ToList();
                    }
                    else
                    {
                        colorList = null;

                    }
                }
                    using (var client = new HttpClientDemo())
                {

                    var responseTask = client.GetAsync("product/readProduct.php?id="+id);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var resultTask = result.Content.ReadAsStringAsync().Result;
                        product = JsonConvert.DeserializeObject<Product>(resultTask);
                        Session["ProductDetails"] = product;

                        List<string> listOfColors = new List<string>(product.colors.Split(',', ':'));
                        listOfColors.RemoveAll(u => u.StartsWith("#"));
                        listOfColors.RemoveAll(u => u.Contains("NULL"));
                        var colors = String.Join(",", listOfColors);
                        product.colors = colors;

                    //    TempData["Message"] = "Product get Successfully";
                     //   TempData["class"] = MessageUtility.Success;
                    }
                    else
                    {
                        product = null;
                     //   TempData["Message"] = "Sorry! Something went wrong. Please Try Again";
                     //   TempData["class"] = MessageUtility.Error;
                    }
                    //return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {

            }
            return View(product);
        }

        public ActionResult EditProductDetails(Product product, List<string> colors, List<double> sizes)
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
            ProductVM aProduct = new ProductVM();
            aProduct.id = product.Id;
            aProduct.productName = product.productName;
            aProduct.productPrice = product.productPrice;
            aProduct.productDescription = product.productDescription;
            aProduct.discountPercentage = product.discountPercentage;
           
            Product productDetails = (Product) Session["ProductDetails"];
            if (colors == null)
            {
                aProduct.colors = productDetails.colors;
            }else
            {
                var colorString = String.Join(",", colors);
                aProduct.colors = colorString;
            }
            if (sizes == null)
            {
                aProduct.sizes = productDetails.sizes;
            }else
            {
                var sizeString = String.Join(",", sizes);
                aProduct.sizes = sizeString;
            }
            using (var client = new HttpClientDemo())
            {
                var postTask = client.PostAsJsonAsync("registration/updateProductDescription.php", aProduct);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    TempData["Message"] = "Product updated successfully";
                    TempData["class"] = MessageUtility.Success;
                    // Session["IsLogin"] = false;

                    return RedirectToAction("List", "Product");
                }

                else
                {
                    TempData["Message"] = "Sorry! product update failed. Please Try again or contact with the administration.";
                     TempData["class"] = MessageUtility.Error;
                    //Session["IsLogin"] = false;
                    return View();
                }
            }


            return RedirectToAction("List", "Product");
        }
        public ActionResult EditProductImage(int id)
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
            Product product = new Product();
            try
            {
             
                using (var client = new HttpClientDemo())
                {

                    var responseTask = client.GetAsync("product/readProduct.php?id=" + id);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var resultTask = result.Content.ReadAsStringAsync().Result;
                        product = JsonConvert.DeserializeObject<Product>(resultTask);
                       // TempData["Message"] = "Product get Successfully";
                       // TempData["class"] = MessageUtility.Success;
                    }
                    else
                    {
                        product = null;
                      //  TempData["Message"] = "Sorry! Something went wrong. Please Try Again";
                       // TempData["class"] = MessageUtility.Error;
                    }
                    //return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {

                throw;
            }


            return View(product);
        }
        [HttpPost]
        public ActionResult EditProductImage(Product product, string thumbnailFile1)
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
            ProductImageVM productImage1 = new ProductImageVM();
            ProductImageVM productImage2 = new ProductImageVM();
            ProductImageVM productImage3 = new ProductImageVM();
            ProductImageVM productImage4 = new ProductImageVM();
            ProductImageVM productImage5 = new ProductImageVM();
            if (product.file1 !=null)
            {
                productImage1.id = product.Id.ToString();
                productImage1.fileNo = "1";
                productImage1.image = product.file1;
                productImage1.thumbnail = thumbnailFile1;

                using (var client = new HttpClientDemo())
                {
                    var postTask = client.PostAsJsonAsync("registration/updateProductImage.php", productImage1);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                    }

                    else
                    {

                    }
                }
            }
            if (product.file2 != null)
            {
                productImage2.id = product.Id.ToString();
                productImage2.fileNo = "2";
                productImage2.image = product.file2;
                productImage2.thumbnail = "";

                using (var client = new HttpClientDemo())
                {
                    var postTask = client.PostAsJsonAsync("registration/updateProductImage.php", productImage2);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                    }

                    else
                    {

                    }
                }
            }
            if (product.file3 != null)
            {
                productImage3.id = product.Id.ToString();
                productImage3.fileNo = "3";
                productImage3.image = product.file3;
                productImage3.thumbnail= "";

                using (var client = new HttpClientDemo())
                {
                    var postTask = client.PostAsJsonAsync("registration/updateProductImage.php", productImage3);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                    }

                    else
                    {

                    }
                }
            }
            if (product.file4 != null)
            {
                productImage4.id = product.Id.ToString();
                productImage4.fileNo = "4";
                productImage4.image = product.file4;
                productImage4.thumbnail = "";

                using (var client = new HttpClientDemo())
                {
                    var postTask = client.PostAsJsonAsync("registration/updateProductImage.php", productImage4);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                    }

                    else
                    {

                    }
                }
            }
            if (product.file5 != null)
            {
                productImage5.id = product.Id.ToString();
                productImage5.fileNo = "5";
                productImage5.image = product.file5;
                productImage5. thumbnail = "";

                using (var client = new HttpClientDemo())
                {
                    var postTask = client.PostAsJsonAsync("registration/updateProductImage.php", productImage5);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                    }

                    else
                    {

                    }
                }
            }

            return RedirectToAction("List", "Product");
        }

        
        public ActionResult Delete(int id)
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
            try
            {
              
                using (var client = new HttpClientDemo())
                {
                    
                    int productid = id;
                    var responseTask = client.GetAsync("product/deleteProduct.php?id="+ id + "&isDeleted=1");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                        TempData["Message"] = "Product deleted Successfully";
                        TempData["class"] = MessageUtility.Success;
                    }
                    else
                    {

                        TempData["Message"] = "Sorry! Something went wrong. Please Try Again";
                        TempData["class"] = MessageUtility.Error;
                    }
                   
                }
            }
            catch (Exception e)
            {

            }
            return RedirectToAction("List","Product");
        }
    }
}