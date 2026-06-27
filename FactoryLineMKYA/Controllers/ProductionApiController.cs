using FactoryLineMKYA.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryLineMKYA.Controllers
{
    [ApiController] // บ่งบอกว่าเป็น Web API (ส่งคืนข้อมูลเป็น JSON ทันที)
    [Route("api/[controller]")] // ตั้ง URL หลักเป็น /api/productionapi
    public class ProductionApiController : ControllerBase
    {
        private readonly IApiMKYAService _apiMkyaservice;

        // ดึง Service ที่เราเตรียมข้อมูลไว้เข้ามาใช้งาน
        public ProductionApiController(IApiMKYAService apiMkyaservice)
        {
            _apiMkyaservice = apiMkyaservice;
        }

        // 🌟 สร้างช่องทาง HTTP GET
        // URL สำเร็จจะเป็น: http://[IP_ของคุณ]:[PORT]/api/productionapi/get-order/10002345
        [HttpGet("getplanorder/{id}")]
        public async Task<IActionResult> GetOrderData(string id)
        {
            var result = await _apiMkyaservice.GetPreparedDataAsync(id);

            if (result == null || result.t1.ProductionOrder == null)
            {
                return NotFound(new { message = $"ไม่พบข้อมูลเลขที่ {id}" });
            }

            // 🌟 จัดรูปแบบ JSON ให้เคาะย่อหน้าและเว้นบรรทัดสวยงาม (Formatting.Indented)
            string prettyJson = JsonConvert.SerializeObject(result, Formatting.Indented);

            // 🚀 ส่งกลับออกไปเป็น Content ประเภท JSON ตรง ๆ ไม่ผ่านการครอบ string ซ้ำ
            return Content(prettyJson, "application/json");
        }
    }
}
