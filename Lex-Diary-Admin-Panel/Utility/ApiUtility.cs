using Lex_Diary_Admin_Panel.Models;
using Lex_Diary_Admin_Panel.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lex_Diary_Admin_Panel.Utility
{
    public class ApiUtility
    {
        public static UserDetails GetUserDetailsByOrderId(int orderId)
        {
            UserDetails userDetails = new UserDetails();


            //get using id

            using (var client = new HttpClientDemo())
            {

                var responseTask = client.GetAsync("product/readUserDetailsfromOrder.php?o_id=" + orderId);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var resultTask = result.Content.ReadAsStringAsync().Result;
                    userDetails = JsonConvert.DeserializeObject<UserDetails>(resultTask);
                   
                }
                else
                {
                    userDetails = null;
                   
                }
                
            }


            return userDetails;
        }

        public static List<OrderDetails> GetOrderDetailsByOrderId(int orderId)
        {
            List<OrderDetails> orderDetailsList = new List<OrderDetails>();

            using (var client = new HttpClientDemo())
            {

                var responseTask = client.GetAsync("product/readProductOrderDetails.php?o_id=" + orderId);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var resultTask = result.Content.ReadAsStringAsync().Result;
                    orderDetailsList = JsonConvert.DeserializeObject<List<OrderDetails>>(resultTask);

                }
                else
                {
                    orderDetailsList = null;

                }

            }

            return orderDetailsList;
           
        }


        public static OrderPriceVM GetOrderPriceCalculationByOrderId(int orderId)
        {
            OrderPriceVM orderPrice = new OrderPriceVM();


            //get using id

            using (var client = new HttpClientDemo())
            {

                var responseTask = client.GetAsync("product/readPriceDetails.php?o_id=" + orderId);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var resultTask = result.Content.ReadAsStringAsync().Result;
                    orderPrice = JsonConvert.DeserializeObject<OrderPriceVM>(resultTask);

                }
                else
                {
                    orderPrice = null;

                }

            }


            return orderPrice;
        }
    }
}