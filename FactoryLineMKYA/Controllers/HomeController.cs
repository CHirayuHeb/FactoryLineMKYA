using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FactoryLineMKYA.Models;
using FactoryLineMKYA.Models.Modelsap;
using System.Globalization;

using Newtonsoft.Json;
using FactoryLineMKYA.Services;

namespace FactoryLineMKYA.Controllers
{
    public class HomeController : Controller
    {
        // private readonly ApiMKYAService _ApiMKYAService;
        //private readonly BapiClass _BapiClass;
        private readonly IApiMKYAService _apiMkyaservice; // 🌟 เรียกใช้ผ่าน Interface
        public HomeController( IApiMKYAService apiMkyaservice)
        {
            _apiMkyaservice = apiMkyaservice;
           // _BapiClass = BapiClass;
        }

       BapiClass _BapiClass = new BapiClass();
        public IActionResult Index()
        {

            ViewFormmain _viewFormmain = new ViewFormmain();
            _viewFormmain.prodDate = DateTime.Now.ToString("yyyy/MM/dd");
            _viewFormmain.plant = "6338";
            _viewFormmain.prodLine = "A47";
            _viewFormmain.prodOrder = "";
            _viewFormmain.Material = "";
            _viewFormmain.delDate = "";

            return View(_viewFormmain);
        }

        [HttpPost]
        public async Task<JsonResult> GetProductionOrder(ViewFormmain viewFormmain)
        {
            var msgheader = "Success";
            var msg = "";
            var config = "S";
            try
            {
                var prodOrder = viewFormmain.prodOrder.IndexOf("|") >= 0
                              ? viewFormmain.prodOrder.Split('|')[1]
                              : viewFormmain.prodOrder;

                //var deldate = viewFormmain.delDate;
                var deldate = DateTime.ParseExact(viewFormmain.delDate.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                //"110000529748"

                var result = await _BapiClass.GetProductionOrderAsync(prodOrder);
                if (result.success == false)
                {
                    config = "error";
                    msgheader = "Production Order";
                    msg = "Production Order  " + prodOrder + "  is not found";
                    return Json(new { c1 = config, c2 = msgheader, c3 = msg });
                }
                var material = viewFormmain.Material;
                // 2. เรียกใช้งานคำสั่งแปลงเป็น JSON String ได้เลย!
                string prodOrderJson = JsonConvert.SerializeObject(result);
                string apiResultPln = await _apiMkyaservice.PostPlanOrderStatusAsync(result);

                if (apiResultPln == "Success")
                {
                    TempData["SuccessMessage"] = "ส่งข้อมูลเข้าสู่ระบบ ICS เรียบร้อยแล้ว!";
                }
                else
                {
                    TempData["ErrorMessage"] = apiResultPln;
                }









                SaleOrderGetListRequest request = CreateSaleOrderRequest(material, deldate);
                List<soitem> saleOrders = await _BapiClass.GetSaleOrderListAsync(request);
                if (saleOrders.Count == 0)
                {
                    config = "error";
                    msgheader = "Print Label";
                    msg = "Generate Label (Result 0 records)";
                    return Json(new { c1 = config, c2 = msgheader, c3 = msg });
                }
                // 2. เรียกใช้งานคำสั่งแปลงเป็น JSON String ได้เลย!
                // string saleOrdersJson = JsonConvert.SerializeObject(saleOrders);
                string apiResultsaleOrders = await _apiMkyaservice.PostSaleOrderPrintLabelAsync(saleOrders);
                
                config = "success";
                msgheader = "success";
                msg = "Success";
            }
            catch (Exception ex)
            {
                config = "error";
                msgheader = "error";
                msg = ex.Message;
            }

            return Json(new { c1 = config, c2 = msgheader, c3 = msg });

        }

        private SaleOrderGetListRequest CreateSaleOrderRequest(string material, string delDate)
        {
            return new SaleOrderGetListRequest
            {
                iplant = new List<RSDSSELOPT> { CreateSelectOption("I", "EQ", "6332") },

                isoldto = new List<RSDSSELOPT> { CreateSelectOption("I", "BT", "") },

                imaterial = new List<RSDSSELOPT> { CreateSelectOption("I", "EQ", material) },

                icustmat = new List<RSDSSELOPT> { CreateSelectOption("I", "EQ", "") },

                idelvdate = new List<RSDSSELOPT> { CreateSelectOption("I", "EQ", delDate) },

                isono = new List<RSDSSELOPT> { CreateSelectOption("I", "BT", "") },

                idist = new List<RSDSSELOPT> { CreateSelectOption("I", "BT", "") },

                idiv = new List<RSDSSELOPT> { CreateSelectOption("I", "BT", "") },

                ishipto = new List<RSDSSELOPT> { CreateSelectOption("I", "BT", "") },

                isaleorg = new List<RSDSSELOPT> { CreateSelectOption("I", "BT", "") },

                iitemno = new List<RSDSSELOPT> { CreateSelectOption("I", "BT", "") },

                ipodetail = new List<RSDSSELOPT> { CreateSelectOption("I", "BT", "") }
            };
        }

        private RSDSSELOPT CreateSelectOption(string sign, string option, string low, string high = "")
        {
            return new RSDSSELOPT
            {
                sign = sign,
                option = option,
                low = low,
                high = high
            };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
