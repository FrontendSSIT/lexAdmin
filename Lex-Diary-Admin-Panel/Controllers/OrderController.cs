using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Lex_Diary_Admin_Panel.Models;
using Lex_Diary_Admin_Panel.Models.ViewModels;
using Lex_Diary_Admin_Panel.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lex_Diary_Admin_Panel.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
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
            List<Order> orders = new List<Order>();
            //List of products for an orders


           
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
            return View(orders);
        }

        public ActionResult PendingOrderList()
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
            List<Order> orders = new List<Order>();
            //List of products for an orders



            using (var client = new HttpClientDemo())
            {

                var responseTask = client.GetAsync("product/readOrderList.php");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var resultTask = result.Content.ReadAsStringAsync().Result;
                    orders = JsonConvert.DeserializeObject<List<Order>>(resultTask);
                    Session["TotalOrders"] = orders.Count;
                    TempData["Message"] = "order list get Successfully";
                    TempData["class"] = MessageUtility.Success;
                }
                else
                {
                    Session["TotalProducts"] = 0;
                    Session["TotalOrders"] = 0;
                    orders = null;
                    TempData["Message"] = "Sorry! Something went wrong. Please Try Again";
                    TempData["class"] = MessageUtility.Error;
                }
                //return RedirectToAction("Index", "Home");
            }
            return View(orders);
        }
        public ActionResult OrderDetails(int id)
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

            //get using id

            //using (var client = new HttpClientDemo())
            //{

            //    var responseTask = client.GetAsync("product/readProduct.php?id=" + id);
            //    responseTask.Wait();

            //    var result = responseTask.Result;
            //    if (result.IsSuccessStatusCode)
            //    {
            //        var resultTask = result.Content.ReadAsStringAsync().Result;
            //        product = JsonConvert.DeserializeObject<Product>(resultTask);
            //        TempData["Message"] = "Product get Successfully";
            //        TempData["class"] = MessageUtility.Success;
            //    }
            //    else
            //    {
            //        product = null;
            //        TempData["Message"] = "Sorry! Something went wrong. Please Try Again";
            //        TempData["class"] = MessageUtility.Error;
            //    }
            //    //return RedirectToAction("Index", "Home");
            //}


            return View();
        }
        [HttpPost]
        public ActionResult OrderDetails()
        {
            CreatePdf();
            return View();
        }
        
        private void CreatePdf()
        {
            Customer customer = new Customer();
            customer.Name = "Sabrina Rahman Mim";
            customer.MobileNo = "+880 1745 972 199";
            customer.Address = "Road# 12/3, Farmgate, bijoy shoroni, Dhaka- 1206";
            OrderDetails aOderDetails1 = new OrderDetails();
            OrderDetails aOderDetails2 = new OrderDetails();
            OrderDetails aOderDetails3 = new OrderDetails();
            OrderDetails aOderDetails4 = new OrderDetails();

            aOderDetails1.productName = "Epson Expression Home XP-4100 Printer ";
            aOderDetails1.price = "3300";
            aOderDetails1.size = "22 inch";
            aOderDetails1.color = "black";
            aOderDetails2.productName = "Printer";
            aOderDetails2.price = "3300";
            aOderDetails2.size = "22 inch";
            aOderDetails2.color = "White";
            aOderDetails3.productName = "Printer";
            aOderDetails3.price = "3300";
            aOderDetails3.size = "22 inch";
            aOderDetails3.color = "Grey";




            List<OrderDetails> orderDetailsList = new List<OrderDetails>();
            orderDetailsList.Add(aOderDetails1);
            orderDetailsList.Add(aOderDetails2);
            orderDetailsList.Add(aOderDetails3);

            OrderPriceVM orderPrice = new OrderPriceVM();
            var orderSubtotal = orderPrice.subTotal;
            var orderDeliveryCharge = orderPrice.deliveryCharge;
            var orderTotalPrice = orderPrice.total;


            iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 25, 25, 25, 15);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
           

            Image image = Image.GetInstance(Server.MapPath("~/Content/template-content/img/logo/lex-diary-logo.jpg"));
            image.ScaleAbsolute(80, 55);
            image.Alignment = 2;
            pdfDoc.Add(image);

            Chunk address = new Chunk("123/2 Middle Paikpara, Mirpur -1, Dhaka-1216", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK));
            Paragraph addressPara = new Paragraph();
            addressPara.Add(address);
            addressPara.Alignment = 2;
            pdfDoc.Add(addressPara);

            Chunk printDate = new Chunk("Date: " + DateTime.Today.ToShortDateString(), FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK));
            Paragraph para = new Paragraph();
            para.Add(printDate);
            para.Alignment = 0;
            pdfDoc.Add(para);

            var line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            //pdfDoc.Add(line);

            PdfPTable table = new PdfPTable(1);
            PdfPCell cell = new PdfPCell();
            Paragraph titlePara = new Paragraph();
            titlePara.Alignment = 1;
            Chunk chunkTitle = new Chunk("Customer Order History", FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK));
            titlePara.Add(chunkTitle);
            pdfDoc.Add(titlePara);
           


            Paragraph customerInfoPara = new Paragraph();
            customerInfoPara.Alignment = 0;
            Chunk chunk = new Chunk("\n"+"Order#:  26\n"+ "Customer Name:  "+customer.Name + "\n" + "Address:  "+customer.Address + "\n" + "Phone:  "+customer.MobileNo);
            chunk.Font = FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.BLACK);
            customerInfoPara.Add(chunk);
            pdfDoc.Add(customerInfoPara);




            Font normalFont = new Font();
            normalFont.Size = 10;
            //Table
            table = new PdfPTable(5);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;

            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;
            int[] columnSize = new[] { 2, 6, 4,4, 4 };
            table.SetWidths(columnSize);
            table.DefaultCell.Phrase = new Phrase() { Font = normalFont };

            //Cell
            cell = new PdfPCell();
            //Chunk taleTitleChunk = new Chunk("Product List\n\n");
            Font font = new Font();
            font.Size = 11;
            //taleTitleChunk.Font = font;
           // cell.AddElement(taleTitleChunk);
            //cell.Colspan = 7;
           // cell.HorizontalAlignment = 1;
            //cell.VerticalAlignment = 1;
            //table.AddCell(cell);

            cell = new PdfPCell();
            Chunk thChunk = new Chunk("SL");
            thChunk.Font = font;
            cell.AddElement(thChunk);
            table.AddCell(cell);

            cell = new PdfPCell();
            thChunk = new Chunk("Product Name");
            thChunk.Font = font;
            cell.AddElement(thChunk);
            table.AddCell(cell);

            cell = new PdfPCell();
            thChunk = new Chunk("Color and Sizes");
            thChunk.Font = font;
            cell.AddElement(thChunk);
            table.AddCell(cell);

            cell = new PdfPCell();
            thChunk = new Chunk("Price");
            thChunk.Font = font;
            cell.AddElement(thChunk);
            table.AddCell(cell);

            //cell = new PdfPCell();
            //thChunk = new Chunk("Started");
            //thChunk.Font = font;
            //cell.AddElement(thChunk);
            //table.AddCell(cell);

            cell = new PdfPCell();
            thChunk = new Chunk("Discount Price");
            thChunk.Font = font;
            cell.HorizontalAlignment = 1;
            cell.AddElement(thChunk);
            table.AddCell(cell);

            //cell = new PdfPCell();
            //Chunk dueChunk = new Chunk("Due", new Font() { Color = BaseColor.RED });
            //dueChunk.Font = font;
            //cell.AddElement(dueChunk);
            //table.AddCell(cell);
            int serialNo = 1;
            Chunk text = new Chunk("Product List\n\n");
            Font fontSize = new Font();
            fontSize.Size = 10;
           

            foreach (var orderDetails in orderDetailsList)
            {
                cell = new PdfPCell();
                text = new Chunk(serialNo++.ToString());
                text.Font = fontSize;
                cell.AddElement(text);
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = 1;
                table.AddCell(cell);

                cell = new PdfPCell();
                text = new Chunk(orderDetails.productName + "\n");
                text.Font = fontSize;
                cell.AddElement(text);
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = 1;
                table.AddCell(cell);

                cell = new PdfPCell();
                text = new Chunk("Size:  "+orderDetails.size + "\n" + "Color:  "+orderDetails.color);
                text.Font = fontSize;
                cell.AddElement(text);
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = 1;
                table.AddCell(cell);

                cell = new PdfPCell();
                text = new Chunk(orderDetails.price + " BDT\n");
                text.Font = fontSize;
                cell.AddElement(text);
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                table.AddCell(cell);

                //var date = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(rqst.startServiceTime.ToString()));
                //var date =  DateTime.Now;
                //cell = new PdfPCell();
                //text = new Chunk(date.ToString("dd/MM/yyyy"));
                //text.Font = fontSize;
                //cell.AddElement(text);
                //cell.HorizontalAlignment = 0;
                //cell.VerticalAlignment = 1;
                //table.AddCell(cell);


                cell = new PdfPCell();
                text = new Chunk("3000 BDT");
                text.Font = fontSize;
                cell.AddElement(text);
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                table.AddCell(cell);

                //cell = new PdfPCell();
                //dueChunk = new Chunk("50 BDT", new Font() { Color = BaseColor.RED, Size = 10 });
                //cell.HorizontalAlignment = 1;
                //cell.VerticalAlignment = 1;
                //cell.AddElement(dueChunk);
                //table.AddCell(cell);
            }

            pdfDoc.Add(table);


            //Paragraph totalPricePara = new Paragraph();
            //totalPricePara.Alignment = 2;
            //Chunk chunk1 = new Chunk("Subtotal:"+"\t"+"2500"+ "BDT"+"\n"+"Delivery Charge:"+"\t"+"25 BDT"+"\n"+"Total: "+"\t"+"2550 BDT");
            //chunk1.Font = FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.BLACK);
            //totalPricePara.Add(chunk1);
            //pdfDoc.Add(totalPricePara);

           
            Paragraph subTotalPara = new Paragraph();
           
            Chunk glue = new Chunk(new VerticalPositionMark());
            Phrase ph1 = new Phrase();
            ph1.Add(new Chunk(Environment.NewLine));
            string title1 = "                    Subtotal";
            string subTotal = "25000";
            Paragraph main = new Paragraph();
            ph1.Add(new Chunk(title1)); // Here I add projectname as a chunk into Phrase.    
            ph1.Add(glue); // Here I add special chunk to the same phrase.    
            ph1.Add(new Chunk(subTotal + "   BDT")); // Here I add date as a chunk into same phrase.    
            main.Add(ph1);
            subTotalPara.Add(main);
            pdfDoc.Add(subTotalPara);

            Paragraph deliveryChargePara = new Paragraph();

            Chunk glue1 = new Chunk(new VerticalPositionMark());
            Phrase ph2 = new Phrase();
            ph2.Add(new Chunk(Environment.NewLine));
            string title2 = "                    Delivery Charge";
            string deliveryCharge = "25000";
            Paragraph main1 = new Paragraph();
            ph2.Add(new Chunk(title2)); // Here I add projectname as a chunk into Phrase.    
            ph2.Add(glue1); // Here I add special chunk to the same phrase.    
            ph2.Add(new Chunk(deliveryCharge + "   BDT")); // Here I add date as a chunk into same phrase.    
            main1.Add(ph2);
            deliveryChargePara.Add(main1);
            pdfDoc.Add(deliveryChargePara);

            line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line);

            Paragraph totalPricePara = new Paragraph();

            Chunk glue3 = new Chunk(new VerticalPositionMark());
            Phrase ph3 = new Phrase();
            ph3.Add(new Chunk(Environment.NewLine));
            string title3 = "                    Total";
            string totalPrice = "25000";
            Paragraph main3 = new Paragraph();
            ph3.Add(new Chunk(title3)); // Here I add projectname as a chunk into Phrase.    
            ph3.Add(glue3); // Here I add special chunk to the same phrase.    
            ph3.Add(new Chunk(totalPrice + "   BDT")); // Here I add date as a chunk into same phrase.    
            main3.Add(ph3);
            totalPricePara.Add(main3);
            pdfDoc.Add(totalPricePara);



           
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;" + customer.Name + "_Order_History.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();
        }
    }
}