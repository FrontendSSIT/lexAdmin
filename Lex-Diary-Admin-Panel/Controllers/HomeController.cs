using iTextSharp.text;
using iTextSharp.text.pdf;
using Lex_Diary_Admin_Panel.Models;
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
                List<Order> orders = new List<Order>();
                using (var client = new HttpClientDemo())
                {

                    var responseTask = client.GetAsync("product/readOrderList.php");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var resultTask = result.Content.ReadAsStringAsync().Result;
                        orders = JsonConvert.DeserializeObject<List<Order>>(resultTask);
                        Session["TotalPendingOrders"] = orders.Count;
                        TempData["Message"] = "order list get Successfully";
                        TempData["class"] = MessageUtility.Success;
                    }
                    else
                    {

                        Session["TotalPendingOrders"] = 0;
                        orders = null;
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
        //private void CreatePdf(List<RequestList> requestList, LawyerList lawyerInfo)
        //{
        //    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 25, 25, 25, 15);
        //    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //    pdfDoc.Open();
        //    Chunk printDate = new Chunk("Date: " + DateTime.Today.ToShortDateString(), FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK));
        //    Paragraph para = new Paragraph();
        //    para.Add(printDate);
        //    para.Alignment = 2;
        //    pdfDoc.Add(para);


        //    Image image = Image.GetInstance(Server.MapPath("~/Content/SmartAdmin/img/uqeel-logo.png"));
        //    image.ScaleAbsolute(100, 75);
        //    image.Alignment = 1;
        //    pdfDoc.Add(image);


        //    PdfPTable table = new PdfPTable(1);
        //    PdfPCell cell = new PdfPCell();
        //    Paragraph titlePara = new Paragraph();
        //    titlePara.Alignment = 1;
        //    Chunk chunkTitle = new Chunk("Lawyer Transaction History", FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK));
        //    titlePara.Add(chunkTitle);
        //    pdfDoc.Add(titlePara);
        //    var line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
        //    pdfDoc.Add(line);


        //    Paragraph lawyerInfoPara = new Paragraph();
        //    lawyerInfoPara.Alignment = 1;
        //    Chunk chunk = new Chunk("\n" + lawyerInfo.GeneralInfo.name + "\n" + lawyerInfo.GeneralInfo.address + "\n" + lawyerInfo.GeneralInfo.mobileNumber);
        //    chunk.Font = FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.BLACK);
        //    lawyerInfoPara.Add(chunk);
        //    pdfDoc.Add(lawyerInfoPara);




        //    Font normalFont = new Font();
        //    normalFont.Size = 10;
        //    //Table
        //    table = new PdfPTable(7);
        //    table.WidthPercentage = 100;
        //    table.HorizontalAlignment = 0;
        //    table.SpacingBefore = 20f;
        //    table.SpacingAfter = 30f;
        //    int[] columnSize = new[] { 1, 7, 4, 4, 3, 2, 2 };
        //    table.SetWidths(columnSize);
        //    table.DefaultCell.Phrase = new Phrase() { Font = normalFont };

        //    //Cell
        //    cell = new PdfPCell();
        //    Chunk taleTitleChunk = new Chunk("Transaction History");
        //    Font font = new Font();
        //    font.Size = 11;
        //    taleTitleChunk.Font = font;
        //    cell.AddElement(taleTitleChunk);
        //    cell.Colspan = 7;
        //    cell.HorizontalAlignment = 0;
        //    cell.VerticalAlignment = 1;
        //    table.AddCell(cell);
        //    cell = new PdfPCell();
        //    Chunk thChunk = new Chunk("S#");
        //    thChunk.Font = font;
        //    cell.AddElement(thChunk);
        //    table.AddCell(cell);
        //    cell = new PdfPCell();
        //    thChunk = new Chunk("Service");
        //    thChunk.Font = font;
        //    cell.AddElement(thChunk);
        //    table.AddCell(cell);
        //    cell = new PdfPCell();
        //    thChunk = new Chunk("Lawyer");
        //    thChunk.Font = font;
        //    cell.AddElement(thChunk);
        //    table.AddCell(cell);
        //    cell = new PdfPCell();
        //    thChunk = new Chunk("Customer");
        //    thChunk.Font = font;
        //    cell.AddElement(thChunk);
        //    table.AddCell(cell);
        //    cell = new PdfPCell();
        //    thChunk = new Chunk("Started");
        //    thChunk.Font = font;
        //    cell.AddElement(thChunk);
        //    table.AddCell(cell);
        //    cell = new PdfPCell();
        //    thChunk = new Chunk("Fee");
        //    thChunk.Font = font;
        //    cell.AddElement(thChunk);
        //    table.AddCell(cell);

        //    cell = new PdfPCell();
        //    Chunk dueChunk = new Chunk("Due", new Font() { Color = BaseColor.RED });
        //    dueChunk.Font = font;
        //    cell.AddElement(dueChunk);
        //    table.AddCell(cell);
        //    int serialNo = 1;
        //    Chunk text = new Chunk("Transaction History");
        //    Font fontSize = new Font();
        //    fontSize.Size = 10;
        //    foreach (var rqst in requestList)
        //    {
        //        cell = new PdfPCell();
        //        text = new Chunk(serialNo++.ToString());
        //        text.Font = fontSize;
        //        cell.AddElement(text);
        //        cell.HorizontalAlignment = 0;
        //        cell.VerticalAlignment = 1;
        //        table.AddCell(cell);
        //        cell = new PdfPCell();
        //        text = new Chunk(rqst.service.serviceName + "\n" + rqst.service.category);
        //        text.Font = fontSize;
        //        cell.AddElement(text);
        //        cell.HorizontalAlignment = 0;
        //        cell.VerticalAlignment = 1;
        //        table.AddCell(cell);
        //        cell = new PdfPCell();
        //        text = new Chunk(lawyerInfo.GeneralInfo.name + "\n" + lawyerInfo.GeneralInfo.mobileNumber);
        //        text.Font = fontSize;
        //        cell.AddElement(text);
        //        cell.HorizontalAlignment = 0;
        //        cell.VerticalAlignment = 1;
        //        table.AddCell(cell);
        //        cell = new PdfPCell();
        //        text = new Chunk(rqst.customer.name + "\n" + rqst.customer.mobileNumber);
        //        text.Font = fontSize;
        //        cell.AddElement(text);
        //        cell.HorizontalAlignment = 0;
        //        cell.VerticalAlignment = 1;
        //        table.AddCell(cell);
        //        var date = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(rqst.startServiceTime.ToString()));
        //        cell = new PdfPCell();
        //        text = new Chunk(date.ToString("dd/MM/yyyy"));
        //        text.Font = fontSize;
        //        cell.AddElement(text);
        //        cell.HorizontalAlignment = 0;
        //        cell.VerticalAlignment = 1;
        //        table.AddCell(cell);


        //        cell = new PdfPCell();
        //        text = new Chunk(rqst.service.charge.ToString());
        //        text.Font = fontSize;
        //        cell.AddElement(text);
        //        cell.HorizontalAlignment = 0;
        //        cell.VerticalAlignment = 1;
        //        table.AddCell(cell);
        //        cell = new PdfPCell();
        //        dueChunk = new Chunk(rqst.service.charge.ToString(), new Font() { Color = BaseColor.RED, Size = 10 });
        //        cell.HorizontalAlignment = 1;
        //        cell.VerticalAlignment = 1;
        //        cell.AddElement(dueChunk);
        //        table.AddCell(cell);
        //    }



        //    pdfDoc.Add(table);
        //    //line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
        //    //pdfDoc.Add(line);
        //    pdfWriter.CloseStream = false;
        //    pdfDoc.Close();
        //    Response.Buffer = true;
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("content-disposition", "attachment;" + lawyerInfo.GeneralInfo.name + "_Transaction_History.pdf");
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.Write(pdfDoc);
        //    Response.End();
        //}

    }
}