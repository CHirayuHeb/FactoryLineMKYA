using FactoryLineMKYA.Models.Modelsap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryLineMKYA.Services
{
    public interface IApiMKYAService
    {

        Task<string> PostPlanOrderStatusAsync(ProductionOrderResult orderId); //plan order
        Task<string> PostSaleOrderPrintLabelAsync(List<soitem> saleOrders); //plan order


        // 🌟 ฟังก์ชันใหม่สำหรับ GET: ดึงข้อมูลที่เตรียมไว้ (สมมติส่งค่ากลับเป็นก้อนข้อมูล ProductionOrderResult)
        Task<OrderModel> GetPreparedDataAsync(string orderId);

    }
}
