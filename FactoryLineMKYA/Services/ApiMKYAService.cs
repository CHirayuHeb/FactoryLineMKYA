using FactoryLineMKYA.Models;
using FactoryLineMKYA.Models.Modelsap;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FactoryLineMKYA.Services
{
    public class ApiMKYAService: IApiMKYAService // 🌟 สืบทอดมาจาก Interface 
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string BaseUrl = "http://xx.xx.xx.xx:[PORT]/api/v1"; // 🌟 ตั้ง URL หลักไว้ที่นี่ที่เดียว

        private readonly BapiClass _bapiClass; 


        public ApiMKYAService(IHttpClientFactory httpClientFactory, BapiClass bapiClass)
        {
            _httpClientFactory = httpClientFactory;
            _bapiClass = bapiClass; // 🌟 โยนค่าที่ระบบส่งมา ให้กับตัวแปรของคลาส

        }

        // ====== 🚀 ฟังก์ชันที่ 1: Production Order ======
        public async Task<string> PostPlanOrderStatusAsync(ProductionOrderResult result)
        {
            string url = $"{BaseUrl}/production-order"; // รวมร่าง URL ปลายทางเฉพาะฟังก์ชันนี้
            return await ExecutePostAsync(url, result);
        }

        // ====== 🌟 ตัวอย่างฟังก์ชันที่ 2: Sale Order / Print Label ======
        public async Task<string> PostSaleOrderPrintLabelAsync(List<soitem> saleOrders)
        {
            string url = $"{BaseUrl}/sale-order"; // รวมร่าง URL ปลายทางเฉพาะฟังก์ชันนี้
            return await ExecutePostAsync(url, saleOrders);
        }



        // ==========================================
        // 🛠️ ฟังก์ชันส่วนตัว (Private Helper) สำหรับลดการเขียนโค้ดซ้ำซ้อน
        // ทุกฟังก์ชันด้านบนจะวิ่งมาใช้ตัวนี้ร่วมกัน ทำให้แก้ไขโค้ดที่เดียวจบ
        // ==========================================
        private async Task<string> ExecutePostAsync(string url, object data)
        {
            try
            {
                string jsonBody = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var client = _httpClientFactory.CreateClient();
                client.Timeout = TimeSpan.FromSeconds(15);

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return "Success";
                }

                string errorContent = await response.Content.ReadAsStringAsync();
                return $"Fail: ICS Error {response.StatusCode} - {errorContent}";
            }
            catch (Exception ex)
            {
                return $"Fail: Connection Error ({ex.Message})";
            }
        }


        public async Task<ProductionOrderResult> GetPreparedDataAsync(string id)
        {

            var result = await _bapiClass.GetProductionOrderAsync(id);
            return await Task.FromResult(result); // ส่งก้อนข้อมูลที่เตรียมเสร็จแล้วกลับออกไป
        }

    }
}
