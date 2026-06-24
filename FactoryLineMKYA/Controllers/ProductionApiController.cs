using FactoryLineMKYA.Services;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("get-order/{id}")]
        public async Task<IActionResult> GetOrderData(string id)
        {
            // เรียกไปที่ฟังก์ชันเตรียมข้อมูลใน Service
            var result = await _apiMkyaservice.GetPreparedDataAsync(id);

            if (result == null)
            {
                // ถ้าไม่พบข้อมูล ส่ง HTTP 404กลับไป
                return NotFound(new { message = $"ไม่พบข้อมูลเลขที่ {id}" });
            }

            // 🚀 ถ้าเจอข้อมูล ส่ง HTTP 200 OK พร้อมก้อน JSON ที่เตรียมไว้ให้ฝั่งที่มายิงดึงไปได้เลย!
            return Ok(result);
        }
    }
}
