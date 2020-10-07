﻿using Lex_Diary_Admin_Panel.Models;
using Lex_Diary_Admin_Panel.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lex_Diary_Admin_Panel.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            bool isLogin = false;
            if (Session["isLogin"] != null)
            {
                isLogin = (bool)Session["isLogin"];
            }
            if (isLogin)
            {
                return RedirectToAction("Add", "Product");
            }
            else
            {
                Session["isLogin"] = false;
                return RedirectToAction("Login", "Home");
            }
            
        }
        public ActionResult Dashboard()
        {
            Session["TotalProducts"] = 0;
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
                List<Product> products = new List<Product>();
                using (var client = new HttpClientDemo())
                {

                    var responseTask = client.GetAsync("product/readProducts.php");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var resultTask = result.Content.ReadAsStringAsync().Result;
                        products = JsonConvert.DeserializeObject<List<Product>>(resultTask);
                        Session["TotalProducts"] = products.Count;
                        TempData["Message"] = "Product get Successfully";
                        TempData["class"] = MessageUtility.Success;
                    }
                    else
                    {
                        Session["TotalProducts"] = 0;
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

            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            string UserName = ConfigurationManager.AppSettings["userName"].ToString();
            string Password = ConfigurationManager.AppSettings["password"].ToString();

            if(username==UserName && password == Password)
            {
                Session["isLogin"] = true;
                return RedirectToAction("Add", "Product");
            }
            Session["isLogin"] = false;
            return View();
        }
        public ActionResult LogOut()
        {
            Session["isLogin"] = false;

            return RedirectToAction("Login","Home");
        }
        public ActionResult genratePDF()
        {
            return View();
        }

        public ActionResult genrateInvoice()
        {
            return View();
        }
    }
}