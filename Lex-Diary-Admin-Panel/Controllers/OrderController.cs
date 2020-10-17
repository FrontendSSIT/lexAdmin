using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Lex_Diary_Admin_Panel.Models;
using Lex_Diary_Admin_Panel.Models.ViewModels;
using Lex_Diary_Admin_Panel.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
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
                   // TempData["Message"] = "order list get Successfully";
                    TempData["class"] = MessageUtility.Success;
                }
                else
                {
                   
                    Session["TotalPendingOrders"] = 0;
                    orders = null;
                   // TempData["Message"] = "Sorry! Something went wrong. Please Try Again";
                    //TempData["class"] = MessageUtility.Error;
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

            Session["OrderStatus"] = "pending";

            using (var client = new HttpClientDemo())
            {

                var responseTask = client.GetAsync("product/readOrderList.php?status=pending");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var resultTask = result.Content.ReadAsStringAsync().Result;
                    orders = JsonConvert.DeserializeObject<List<Order>>(resultTask);
                    Session["TotalOrders"] = orders.Count;
                   // TempData["Message"] = "order list get Successfully";
                   // TempData["class"] = MessageUtility.Success;
                }
                else
                {
                    Session["TotalProducts"] = 0;
                    Session["TotalOrders"] = 0;
                    orders = null;
                   // TempData["Message"] = "Sorry! Something went wrong. Please Try Again";
                   // TempData["class"] = MessageUtility.Error;
                }
                //return RedirectToAction("Index", "Home");
            }
            return View(orders);
        }

        public ActionResult DeliveredOrderList()
        {
            Session["OrderStatus"] = "delivered";
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

                var responseTask = client.GetAsync("product/readOrderList.php?status=delivered");
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
                  //  TempData["Message"] = "Sorry! Something went wrong. Please Try Again";
                   // TempData["class"] = MessageUtility.Error;
                }
                //return RedirectToAction("Index", "Home");
            }
            return View(orders);
        }

        public ActionResult CancelledOrderList()
        {
            Session["OrderStatus"] = "cancelled";
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

                var responseTask = client.GetAsync("product/readOrderList.php?status=cancelled");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var resultTask = result.Content.ReadAsStringAsync().Result;
                    orders = JsonConvert.DeserializeObject<List<Order>>(resultTask);
                    Session["TotalOrders"] = orders.Count;
                   // TempData["Message"] = "order list get Successfully";
                    TempData["class"] = MessageUtility.Success;
                }
                else
                {
                    Session["TotalProducts"] = 0;
                    Session["TotalOrders"] = 0;
                    orders = null;
                   // TempData["Message"] = "Sorry! Something went wrong. Please Try Again";
                  //  TempData["class"] = MessageUtility.Error;
                }
                //return RedirectToAction("Index", "Home");
            }
            return View(orders);
        }

        public ActionResult OrderDetails(int id)
        {
            Session["OrderNo"] = id;
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

            
            UserDetails userDetails = new UserDetails();
            userDetails = ApiUtility.GetUserDetailsByOrderId(id);


            List<OrderDetails> orderDetailsList = new List<OrderDetails>();
            orderDetailsList = ApiUtility.GetOrderDetailsByOrderId(id);


            OrderPriceVM orderPrice = new OrderPriceVM();
            orderPrice = ApiUtility.GetOrderPriceCalculationByOrderId(id);

            Session["CustomerDetails"] = userDetails;
            Session["OrderProductDetailsList"] = orderDetailsList;
            Session["OrderPriceCalculation"] = orderPrice;
            Session["CustomerDetails"] = userDetails;
            return View();
        }
        [HttpPost]
        public ActionResult OrderDetails()
        {
            CreatePdf();
            return View();
        }
        
        public ActionResult UpdateOrderStatus(string orderId, string orderStatus)
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
                OrderStatus order = new OrderStatus();
                order.o_id = orderId;
                order.status = orderStatus;
                using (var client = new HttpClientDemo())
                {

                    var postTask = client.PostAsJsonAsync("product/updateOrderStatus.php", order);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                        TempData["Message"] = "Order status updated successfully";
                        //TempData["class"] = MessageUtility.Success;
                        // Session["IsLogin"] = false;
                        if (orderStatus == "delivered")
                        {
                            return RedirectToAction("DeliveredOrderList", "Order");
                        }
                        else if (orderStatus == "cancelled")
                        {
                            return RedirectToAction("CancelledOrderList", "Order");
                        }

                    }

                    else
                    {
                        TempData["Message"] = "Sorry! Something went wrong. Please Try again or contact with the administration.";
                        TempData["class"] = MessageUtility.Error;
                        Session["IsLogin"] = false;
                        return View();
                    }
                }
            }
            catch (Exception e)
            {

            }



            return RedirectToAction("PendingOrderList", "Order");
        }


        private void CreatePdf()
        {
            int orderId = Convert.ToInt32(Session["OrderNo"]);
            UserDetails userDetails = new UserDetails();
            userDetails = ApiUtility.GetUserDetailsByOrderId(orderId);


            List<OrderDetails> orderDetailsList = new List<OrderDetails>();
            orderDetailsList = ApiUtility.GetOrderDetailsByOrderId(orderId);


            OrderPriceVM orderPrice = new OrderPriceVM();
            orderPrice = ApiUtility.GetOrderPriceCalculationByOrderId(orderId);




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
            Chunk chunkTitle = new Chunk("Invoice", FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK));
            titlePara.Add(chunkTitle);
            pdfDoc.Add(titlePara);
           


            Paragraph customerInfoPara = new Paragraph();
            customerInfoPara.Alignment = 0;
            Chunk chunk = new Chunk("\n"+"Order#:  "+orderId+"\n"+ "Customer Name:  "+userDetails.userName + "\n" + "Address:  "+userDetails.address + "\n" + "Phone:  "+userDetails.userNumber+"\n"+"Contact Person Name:  "+userDetails.cpName+"\n"+"Contact Person Number:  "+userDetails.cpNumber+"\n");
            chunk.Font = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK);
            customerInfoPara.Add(chunk);
            pdfDoc.Add(customerInfoPara);




            Font normalFont = new Font();
            normalFont.Size = 10;
            //Table
            table = new PdfPTable(8);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;

            table.SpacingBefore = 20f;
            table.SpacingAfter = 20f;
            int[] columnSize = new[] {1,4,2,2,2,3,3,3};
            table.SetWidths(columnSize);
            table.DefaultCell.Phrase = new Phrase() { Font = normalFont };

            //Cell
            cell = new PdfPCell();
            //Chunk taleTitleChunk = new Chunk("Product List\n\n");
            Font font = new Font();
            font.Size = 10;
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
            cell.PaddingBottom = 10;
            cell.PaddingLeft = 3;
            table.AddCell(cell);

            cell = new PdfPCell();
            thChunk = new Chunk("Product Name");
            thChunk.Font = font;
            cell.AddElement(thChunk);
            cell.PaddingBottom = 10;
            cell.PaddingLeft = 3;
            table.AddCell(cell);

            cell = new PdfPCell();
            thChunk = new Chunk("Quantity");
            thChunk.Font = font;
            cell.AddElement(thChunk);
            cell.PaddingBottom = 10;
            cell.PaddingLeft = 3;
            table.AddCell(cell);


            cell = new PdfPCell();
            thChunk = new Chunk("Colour");
            thChunk.Font = font;
            cell.AddElement(thChunk);
            cell.PaddingBottom = 10;
            cell.PaddingLeft = 3;
            table.AddCell(cell);

            cell = new PdfPCell();
            Chunk dueChunk = new Chunk("Size", new Font() { Color = BaseColor.RED });
            dueChunk.Font = font;
            cell.AddElement(dueChunk);
            table.AddCell(cell);

            cell = new PdfPCell();
            thChunk = new Chunk("Unit Price");
            thChunk.Font = font;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.AddElement(thChunk);
            cell.PaddingBottom = 10;
            cell.PaddingLeft = 3;
            table.AddCell(cell);

            cell = new PdfPCell();
            thChunk = new Chunk("Discount Percentage");
            thChunk.Font = font;
            cell.AddElement(thChunk);
            table.AddCell(cell);

            cell = new PdfPCell();
            thChunk = new Chunk("Net Price");
            thChunk.Font = font;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.AddElement(thChunk);
            cell.PaddingBottom = 10;
            cell.PaddingLeft = 3;
            table.AddCell(cell);

           
            int serialNo = 1;
            Chunk text = new Chunk("Product List\n\n");
            Font fontSize = new Font();
            fontSize.Size = 9;
           

            foreach (var orderDetails in orderDetailsList)
            {
                var color = "";
                if (orderDetails.colors == "NULL")
                {
                     orderDetails.colors = "";
                }
                else
                {
                   List<string> colorName = new List<string>(orderDetails.colors.Split(':'));
                     color = colorName[0];
                }

                if (orderDetails.sizes == "NULL")
                {
                    orderDetails.sizes = "";
                }
                else
                {

                }


                cell = new PdfPCell();
                text = new Chunk(serialNo++.ToString());
                text.Font = fontSize;
                cell.AddElement(text);
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = 1;
                cell.PaddingBottom = 10;
                cell.PaddingLeft = 3;
                table.AddCell(cell);

                cell = new PdfPCell();
                text = new Chunk(orderDetails.productName);
                text.Font = fontSize;
                cell.AddElement(text);
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = 1;
                cell.PaddingBottom = 10;
                cell.PaddingLeft = 3;
                table.AddCell(cell);

                cell = new PdfPCell();
                text = new Chunk(orderDetails.quantity);
                text.Font = fontSize;
                cell.AddElement(text);
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = 1;
                cell.PaddingBottom = 10;
                cell.PaddingLeft = 3;
                table.AddCell(cell);



                cell = new PdfPCell();
                text = new Chunk(color);
                text.Font = fontSize;
                cell.AddElement(text);
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = 1;
                cell.PaddingBottom = 10;
                cell.PaddingLeft = 3;
                table.AddCell(cell);

                cell = new PdfPCell();
                text = new Chunk(orderDetails.sizes);
                text.Font = fontSize;
                cell.AddElement(text);
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = 1;
                cell.PaddingBottom = 10;
                cell.PaddingLeft = 3;
                table.AddCell(cell);


                var productPrice = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(orderDetails.productPrice));
                cell = new PdfPCell();
                text = new Chunk(productPrice + " BDT\n");
                text.Font = fontSize;
                cell.AddElement(text);
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.VerticalAlignment = 1;
                cell.PaddingBottom = 10;
                cell.PaddingLeft = 3;
                table.AddCell(cell);

                //var date = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(rqst.startServiceTime.ToString()));
             

                var discountPrice = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(orderDetails.discountPrice));
                cell = new PdfPCell();
                text = new Chunk(orderDetails.discountPercentage + "%");
                text.Font = fontSize;
                cell.AddElement(text);
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.VerticalAlignment = 1;
                cell.PaddingLeft = 3;
                table.AddCell(cell);

                cell = new PdfPCell();
                dueChunk = new Chunk(discountPrice+ " BDT", new Font() { Color = BaseColor.BLACK, Size = 10 });
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                cell.AddElement(dueChunk);
                table.AddCell(cell);
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
            //ph1.Add(new Chunk(Environment.NewLine));
            string title1 = "                                                                                                                                  Total";
            string subTotal = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(orderPrice.subTotal));
            Paragraph main = new Paragraph();
            ph1.Add(new Chunk(title1, FontFactory.GetFont("Arial",10, Font.NORMAL, BaseColor.BLACK))); // Here I add projectname as a chunk into Phrase.    
            ph1.Add(glue); // Here I add special chunk to the same phrase.    
            ph1.Add(new Chunk(subTotal + "   BDT", FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK))); // Here I add date as a chunk into same phrase.    
            main.Add(ph1);
            subTotalPara.Add(main);
            pdfDoc.Add(subTotalPara);

            Paragraph deliveryChargePara = new Paragraph();

            Chunk glue1 = new Chunk(new VerticalPositionMark());
            glue1.Font = FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.BLACK);
            Phrase ph2 = new Phrase();
           // ph2.Add(new Chunk(Environment.NewLine));
            string title2 = "                                                                                                                                  Delivery Charge";
            string deliveryCharge = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(orderPrice.deliveryCharge));
            Paragraph main1 = new Paragraph();
            ph2.Add(new Chunk(title2, FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK))); // Here I add projectname as a chunk into Phrase.    
            ph2.Add(glue1); // Here I add special chunk to the same phrase.    
            ph2.Add(new Chunk(deliveryCharge + "   BDT", FontFactory.GetFont("Arial",10, Font.NORMAL, BaseColor.BLACK))); // Here I add date as a chunk into same phrase.    
            main1.Add(ph2);
            deliveryChargePara.Add(main1);
            pdfDoc.Add(deliveryChargePara);

            line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0F, 35F, BaseColor.BLACK, Element.ALIGN_RIGHT, 1)));
            pdfDoc.Add(line);

            Paragraph totalPricePara = new Paragraph();

            Chunk glue3 = new Chunk(new VerticalPositionMark());
            
            Phrase ph3 = new Phrase();
            //ph3.Add(new Chunk(Environment.NewLine));
            string title3 = "                                                                                                                                  Total";
            string totalPrice = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(orderPrice.total));

            Paragraph main3 = new Paragraph();
            ph3.Add(new Chunk(title3, FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK))); // Here I add projectname as a chunk into Phrase.    
            ph3.Add(glue3); // Here I add special chunk to the same phrase.    
            ph3.Add(new Chunk(totalPrice + "   BDT", FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK))); // Here I add date as a chunk into same phrase.    
            main3.Add(ph3);
            totalPricePara.Add(main3);
            totalPricePara.SpacingAfter = 80f;
            pdfDoc.Add(totalPricePara);





            //line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 3)));
            //pdfDoc.Add(line);

            Paragraph signatureDashLine = new Paragraph();

            Chunk glue4 = new Chunk(new VerticalPositionMark());
           
            Phrase ph4 = new Phrase();
            //ph4.Add(new Chunk(Environment.NewLine));
            string title4 = "___________________";
            string receiver = "____________________";
            Paragraph main4 = new Paragraph();
            ph4.Add(new Chunk(title4)); // Here I add projectname as a chunk into Phrase.    
            ph4.Add(glue4); // Here I add special chunk to the same phrase.    
            ph4.Add(new Chunk(receiver)); // Here I add date as a chunk into same phrase.    
            main4.Add(ph4);
            signatureDashLine.Add(main4);
            pdfDoc.Add(signatureDashLine);




            Paragraph signature = new Paragraph();

            Chunk glue5 = new Chunk(new VerticalPositionMark());
            
            Phrase ph5 = new Phrase();
            //ph5.Add(new Chunk(Environment.NewLine));
            string title5 = "For Lex Mall";
            string receiverSignature = "Receiver Signature";
            Paragraph main5 = new Paragraph();
            ph5.Add(new Chunk(title5, FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK))); // Here I add projectname as a chunk into Phrase.    
            ph5.Add(glue5); // Here I add special chunk to the same phrase.    
            ph5.Add(new Chunk(receiverSignature, FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK))); // Here I add date as a chunk into same phrase.    
            main5.Add(ph5);
            signature.Add(main5);
            pdfDoc.Add(signature);




            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;" + orderId + "_Order_History.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();
        }
    }
}