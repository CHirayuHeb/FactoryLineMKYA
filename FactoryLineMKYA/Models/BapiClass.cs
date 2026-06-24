using FactoryLineMKYA.Models.Modelsap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SapNwRfc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryLineMKYA.Models
{
    public class BapiClass
    {
        private readonly SapConnectionConfig _sapConfig;
        private readonly IConfiguration _configuration;
        private string _ImageDirectory => _configuration["AppSettings:ImageDirectory"];

        //string _dbSap = "AppServerHost=10.216.2.91;SystemNumber=00;User=THSRFC001;Password=THSRFC001@012345a;Client=410;Language=EN;PoolSize=5;SYSID=AQA";
        // string _dbSap = "AppServerHost=10.216.2.95;SystemNumber=00;User=THSRFC001;Password=THSRFC001@012345a;Client=100;Language=EN;PoolSize=5;SYSID=APA";
        string _dbSap = "AppServerHost=10.216.2.91;SystemNumber=00;User=THS001748;Password=Abcd1234@567890;Client=400;Language=EN;PoolSize=5;SYSID=AQA";

  
        private SapConnectionParameters GetConnectionParameters()
        {
            var sapSettings = _configuration.GetSection("SapConfiguration").Get<SapConnectionConfig>();

            // ดักเผื่อไม่มีการตั้งค่าในแอป จะได้แก้ปัญหาได้ตรงจุด
            if (sapSettings == null)
            {
                throw new InvalidOperationException("ไม่พบข้อมูล 'SapConfiguration' ในไฟล์คอนฟิกกรุณาตรวจสอบ appsettings.json");
            }

            return new SapConnectionParameters
            {
                AppServerHost = sapSettings.AppServerHost,
                SystemNumber = sapSettings.SystemNumber,
                SystemId = sapSettings.SystemID,
                Client = sapSettings.Client,
                User = sapSettings.Username,
                Password = sapSettings.Password,
                Language = sapSettings.Lang,
                // ป้องกัน Null เผื่อ PoolSize ไม่ได้ระบุมาใน JSON
                PoolSize = sapSettings.PoolSize.ToString()
            };
        }


        public async Task<ProductionOrderResult> GetProductionOrderAsync(string productionOrderId)
        {
            var result = new ProductionOrderResult();
            //var parameters = GetConnectionParameters(); // ดึง Config จากฟังก์ชันที่เราแยกไว้
            try
            {
                using (var connection = new SapConnection(_dbSap))
                {
                    connection.Connect(); // เปิดท่อเชื่อมต่อ SAP
                    var _func = connection.CreateFunction("ZFMMPP009_INF_PRD_ORDER");
                    ZFMMPP009_INF_PRD_ORDERParameters parameter = new ZFMMPP009_INF_PRD_ORDERParameters();
                    parameter.ProductionOrder = productionOrderId.PadLeft(12, '0');

                    // 2. สั่งรันฟังก์ชันภายใน SAP
                    var output = _func.Invoke<ZFMMPP009_INF_PRD_ORDEROutput>(parameter);

                    var last_activity = output.activity.OrderBy(f => f.actnumber).FirstOrDefault();

                    // 3. จัดกลุ่ม Map ข้อมูลเข้า t1Model
                    t1Model t1 = new t1Model();
                    t1.ProductionOrder = output.HeaderData.ProductionOrder;
                    t1.ConfirmNumber = last_activity?.ConfirmNumber;
                    t1.OrderType = output.HeaderData.OrderType;
                    t1.icsno = output.HeaderData.icsno;
                    t1.icsname = output.HeaderData.vIcsName;
                    t1.vicsname = output.HeaderData.icsname;
                    t1.planqty = output.HeaderData.planqty;
                    t1.prodline = last_activity?.workcenter;
                    t1.unit = output.HeaderData.unit;
                    t1.planplant = t1.prodplant = output.HeaderData.planplant;
                    t1.lgort = output.HeaderData.lgort;
                    t1.orderstart = output.HeaderData.orderstart;
                    t1.orderend = output.HeaderData.orderend;
                    t1.grqty = output.HeaderData.ordergr;
                    t1.orderstatus = output.HeaderData.orderstatus;
                    t1.orderversion = output.HeaderData.orderversion;
                    t1.StartTime = DateTime.Now;
                    t1.prodqty = 0;
                    t1.acum_prodqty = output.HeaderData.ordergr;
                    t1.activity = last_activity;
                    t1.STDTime = output.HeaderData.stdtime;
                    t1.Cavity = output.HeaderData.cavity;

                    // 4. วนลูปจับคู่ข้อมูลวัตถุดิบ t1matModel
                    List<t1matModel> t1mats = new List<t1matModel>();
                    foreach (var item in output.Components)
                    {
                        var t1mat = new t1matModel();
                        t1mat.ProductionOrder = t1.ProductionOrder;
                        t1mat.icsno = t1.icsno;
                        t1mat.icsname = t1.icsname;
                        t1mat.component = item.component;
                        t1mat.comname = item.comname;
                        t1mat.comnamees = item.comnamees;
                        t1mat.qty = item.qty;
                        t1mat.usageqty = Math.Round(item.qty / output.HeaderData.planqty, 3);
                        t1mat.nodecimal = "";
                        t1mat.unit = item.unit;
                        t1mat.lgort = item.lgort;
                        t1mat.matgroup = item.matgroup;
                        t1mat.assqty = 0;
                        t1mat.wfaqty = 0;
                        t1mat.lossqty = 0;
                        t1mat.plant = item.plant;
                        t1mat.mvtype = item.mvtype;
                        t1mat.rsnum = item.rsnum;
                        t1mat.rspos = item.rspos;
                        t1mat.select = true;
                        t1mats.Add(t1mat);

                    }

                }

                
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
            }

            return  await Task.FromResult(result);
        }


        public async Task<List<soitem>> GetSaleOrderListAsync(SaleOrderGetListRequest request)
        {
            List<soitem> result = new List<soitem>();
          //  var parameters = GetConnectionParameters(); // เรียกใช้ตัว Config ชุดกลาง

            using (var connection = new SapConnection(_dbSap))
            {
                connection.Connect();
                using (var _func = connection.CreateFunction("ZBF_SALEORDER_GETLIST"))
                {
                    // 1. แมปค่า Parameter ขาเข้า
                    ZBF_SALEORDER_GETLISTParameter parameter = new ZBF_SALEORDER_GETLISTParameter();
                    parameter.iplant = request.iplant.ToArray();
                    // parameter.isoldto = request.isoldto.ToArray();
                    parameter.imaterial = request.imaterial.ToArray();
                    // parameter.icustmat = request.icustmat.ToArray();
                    parameter.idelvdate = request.idelvdate.ToArray();
                    // parameter.isono = request.isono.ToArray();
                    // parameter.idist = request.idist.ToArray();
                    // parameter.idiv = request.idiv.ToArray();
                    // parameter.ishipto = request.ishipto.ToArray();
                    // parameter.isaleorg = request.isaleorg.ToArray();
                    // parameter.iitemno = request.iitemno.ToArray();
                    // parameter.ipodetail = request.ipodetail.ToArray();

                    // 2. เรียกฟังก์ชันภายใน SAP
                    var output = _func.Invoke<ZBF_SALEORDER_GETLISTOutput>(parameter);

                    // 3. วนลูปจัดการข้อมูลผลลัพธ์
                    foreach (var row in output.tsolabel)
                    {
                        soitem so = new soitem();
                        so.SELECT = false;
                        so.VBELN = row.VBELN;
                        so.POSNR = row.POSNR;
                        so.MATNR = row.MATNR;
                        so.KDMAT = row.KDMAT;
                        so.MAKTX_ES = row.MAKTX_ES;
                        so.MAKTX_EN = row.MAKTX_EN;
                        so.ZKDNM = row.ZKDNM;
                        so.BSTKD = row.BSTKD;
                        so.BSTKD_E = row.BSTKD_E;
                        so.EMPST = row.EMPST;
                        so.EDATU = row.EDATU.ToString("dd/MM/yyyy");
                        so.EZEIT = row.EZEIT.ToString();
                        so.KWMENG = row.KWMENG;
                        so.VRKME = row.VRKME;

                        if (int.TryParse(row.PACKSIZE, out var _packsize))
                            so.PACKSIZE = _packsize;
                        else
                            so.PACKSIZE = 0;

                        so.MFGDATE = row.MFGDATE.ToString("dd/MM/yyyy");
                        so.MFGOPEN = false;
                        so.WERKS = row.WERKS;
                        so.LGORT = "";
                        so.LGPBE = row.LGPBE;
                        so.VKORG = row.VKORG;
                        so.VTWEG = row.VTWEG;
                        so.SPART = row.SPART;
                        so.SOLDTO = row.SOLDTO.TrimStart('0');
                        so.SOLDTO_NAME = row.SOLDTO_NAME;
                        so.SHIPTO = row.SHIPTO.TrimStart('0');
                        so.SHIPTO_NAME = row.SHIPTO_NAME;
                        so.ZMODC = row.ZMODC;
                        so.ZCOLOR = row.ZCOLOR;
                        so.MDV01 = row.MDV01;
                        so.AUART = row.AUART;
                        so.Planorder = request.iplanorder;

                        if (so.MATNR != "7414669400")
                        {
                            if (so.MAKTX_EN.Length > 5)
                                so.SIDE = so.MAKTX_EN.Substring(5, 1);
                            if (so.SIDE != "R" || so.SIDE != "L")
                                so.SIDE = "";
                        }
                        else
                        {
                            so.SIDE = "";
                        }

                        // ====== ส่วนตรวจสอบข้อมูลประจำวัน 25/02/2026 ======
                        string result_type =  GET_NISSAN_TYPE(so.MATNR); // ⚠️ เมธอดนี้ต้องย้ายมาอยู่ในคลาสนี้ หรือเรียกผ่าน Helper ด้วยนะครับ

                        if (string.IsNullOrEmpty(result_type))
                        {
                            so.NISSAN_TYPE = "";
                        }
                        else
                        {
                            string materialNo = request.imaterial.FirstOrDefault()?.high ?? "";
                            if (string.IsNullOrEmpty(materialNo))
                                throw new Exception("Material No. is empty"); // เปลี่ยนจาก return Ok เป็นโยน Exception แทน

                            if (string.IsNullOrEmpty(so.Planorder))
                                throw new Exception("Production Order is required");

                            so.NISSAN_TYPE = "(" + result_type + ")";
                        }

                        // ====== ส่วนคำนวณ Path รูปภาพและ Config ======
                        string _ImageDirectory = await GET_PATH_CONFIG_1(request.rfcname, "A203", "ZBF_SALEORDER_GETLIST", "PATH", "KUNNR", so.SOLDTO);
                        if (string.IsNullOrEmpty(_ImageDirectory))
                            _ImageDirectory = "\\\\10.200.128.22\\Database\\pic_no_zTable";

                        so.IMAGEURL = GetImageUrl(so.WERKS, so.KDMAT);

                        string str_soldto = await GET_PATH_CONFIG(request.rfcname, "A203", "ZBF_SALEORDER_GETLIST", "INIT", "KUNNR");
                        string[] soldtos = str_soldto.Split(',');
                        if (Array.Exists(soldtos, s => s == so.SOLDTO.TrimStart('0')))
                            so.ISYAMAHA = "X";
                        else
                            so.ISYAMAHA = "";

                        // ====== ส่วนคำนวณ QR Format สำหรับ Yamaha ======
                        string str_qrformat = await GET_PROG_CONFIG_TAG_THAIYAMAHA(request.rfcname, "A203", "ZBF_SALEORDER_GETLIST", "QRCODE", "KUNNR");
                        string[] qrformat = str_qrformat.Split(',');
                        if (Array.Exists(qrformat, s => s.Contains(so.SOLDTO.TrimStart('0'))))
                        {
                            string v_text_format_qr = string.Empty;
                            string[] v_A_format_qr = new string[] { };
                            string[] v_text_sap = new string[] { };
                            string v_KDMAT = string.Empty;
                            string v_sCode = string.Empty;
                            string v_DShop = string.Empty;
                            string v_BCTYDATA = string.Empty;

                            for (int i = 0; i < qrformat.Length; i++)
                            {
                                if (qrformat[i].ToString().Contains(so.SOLDTO.TrimStart('0')))
                                {
                                    if (so.KDMAT.Contains("80") && so.KDMAT.Length > 25)
                                    {
                                        try
                                        {
                                            v_text_sap = qrformat[i].ToString().Split(' ');
                                            v_text_format_qr = v_text_sap[1].ToString();
                                            v_A_format_qr = v_text_format_qr.Split('-');
                                            v_KDMAT = so.KDMAT.Substring(0, 18).Replace("-", "");
                                            v_sCode = so.KDMAT.Substring(19, 4);
                                            v_DShop = so.KDMAT.Substring(24, 4);
                                            v_BCTYDATA = $"{v_A_format_qr[0]}{v_KDMAT}{v_A_format_qr[1]}{v_sCode}{v_A_format_qr[2]}{v_DShop}{v_A_format_qr[3]}{so.PACKSIZE.ToString("D6")}{v_A_format_qr[4]}";
                                        }
                                        catch (Exception)
                                        {
                                            v_BCTYDATA = string.Empty;
                                        }
                                    }
                                }
                            }
                            so.BCTYDATA = v_BCTYDATA;
                        }
                        else
                        {
                            so.BCTYDATA = string.Empty;
                        }

                        // ====== จัดการขีดเส้นใต้ Underline ======
                        string iUnder = await GET_PATH_CONFIG_1(request.rfcname, "A203", "ZBF_SALEORDER_GETLIST", "UNDERLINE", "KUNNR", so.SOLDTO);
                        if (string.IsNullOrEmpty(iUnder))
                            so.IUNDERL = "0";
                        else
                        {
                            so.IUNDERL = iUnder;
                            so.KDMAT_1 = so.KDMAT.Substring(0, (Convert.ToInt16(iUnder) - 1));
                            so.KDMAT_2 = so.KDMAT.Substring(Convert.ToInt16(iUnder) - 1);
                        }

                        result.Add(so);
                    }
                }
            }

            return result;
        }

        // 🛠️ ฟังก์ชันที่ 1: ดึงโปรแกรมคอนฟิกและแปลงผลลัพธ์เป็น String คั่นด้วยเครื่องหมาย Comma (,)
        private async Task<string> GET_PATH_CONFIG(string company, string program, string routine, string field, string v)
        {
            try
            {
               // var parameters = GetConnectionParameters(); // 🌟 ดึงค่าจากตัวแปรส่วนกลางที่เราทำฟังก์ชันแยกไว้

                using (var connection = new SapConnection(_dbSap))
                {
                    connection.Connect();
                    using (var _func = connection.CreateFunction("ZCA002_GET_PROG_CONFIG_RFC"))
                    {
                        ZCA002_GET_PROG_CONFIG_RFCParameter parameter = new ZCA002_GET_PROG_CONFIG_RFCParameter();
                        parameter.company = company;
                        parameter.program = program;
                        parameter.routine = routine;
                        parameter.field = field;

                        var output = _func.Invoke<ZCA002_GET_PROG_CONFIG_RFCOutput>(parameter);

                        string result = string.Join(",", output.t_result.Select(s => s.low.Trim().TrimStart('0')));
                        return await Task.FromResult(result);
                    }
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        // 🛠️ ฟังก์ชันที่ 2: เหมือนฟังก์ชันแรก แต่เพิ่มเงื่อนไขการกรองหาแมตช์ของ 'soldto'
        async Task<string> GET_PATH_CONFIG_1(string company, string program, string routine, string field, string soldto, string sOLDTO)
        {
            try
            {
               // var parameters = GetConnectionParameters(); // 🌟 ดึงค่าส่วนกลางโดยตรง

                using (var connection = new SapConnection(_dbSap))
                {
                    connection.Connect();
                    using (var _func = connection.CreateFunction("ZCA002_GET_PROG_CONFIG_RFC"))
                    {
                        ZCA002_GET_PROG_CONFIG_RFCParameter parameter = new ZCA002_GET_PROG_CONFIG_RFCParameter();
                        parameter.company = company;
                        parameter.program = program;
                        parameter.routine = routine;
                        parameter.field = field;

                        var output = _func.Invoke<ZCA002_GET_PROG_CONFIG_RFCOutput>(parameter);

                        List<string> resultlist = new List<string>();
                        string targetSoldTo = soldto.Trim().TrimStart('0');

                        foreach (var item in output.t_result)
                        {
                            if (item.low.Trim().TrimStart('0') == targetSoldTo)
                            {
                                resultlist.Add(item.high);
                            }
                        }

                        string result = string.Join(",", resultlist);
                        return await Task.FromResult(result);
                    }
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        // 🛠️ ฟังก์ชันสำหรับดึง QR Format ของ Thai Yamaha โดยเฉพาะ
        private async Task<string> GET_PROG_CONFIG_TAG_THAIYAMAHA(string company, string program, string routine, string field, string v)
        {
            try
            {
                // 🌟 เรียกใช้ชุด Config กลาง ไม่ต้องส่ง rfcname มาแล้ว
                var parameters = GetConnectionParameters();

                using (var connection = new SapConnection(parameters))
                {
                    connection.Connect();
                    using (var _func = connection.CreateFunction("ZCA002_GET_PROG_CONFIG_RFC"))
                    {
                        ZCA002_GET_PROG_CONFIG_RFCParameter parameter = new ZCA002_GET_PROG_CONFIG_RFCParameter();
                        parameter.company = company;
                        parameter.program = program;
                        parameter.routine = routine;
                        parameter.field = field;

                        var output = _func.Invoke<ZCA002_GET_PROG_CONFIG_RFCOutput>(parameter);

                        List<string> resultlist = new List<string>();
                        foreach (var item in output.t_result)
                        {
                            // จับ low กับ high มาต่อกันด้วยช่องว่างตาม Logic เดิมของคุณ
                            resultlist.Add($"{item.low.Trim().TrimStart('0')} {item.high.Trim().TrimStart('0')}");
                        }

                        string result = string.Join(",", resultlist);
                        return await Task.FromResult(result);
                    }
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        // 🛠️ ฟังก์ชันสำหรับดึงข้อมูลชนิดสแตมป์ของ Nissan จากฐานข้อมูลระบบเว็บ
        private string GET_NISSAN_TYPE(string icsno)
        {
            try
            {
                // เรียกใช้ _db ที่ผูกผ่าน .NET Core Service มาให้แล้ว
                bf_nissan_type result = new bf_nissan_type();// _db.bf_nissan_type.FirstOrDefault(x => x.icsno == icsno);

                // คืนค่ารูปแบบสแตมป์กลับไป ถ้าหาไม่เจอ (เป็น null) ให้ส่ง string.Empty แทน
                return result != null ? result.stamp_type : string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        string GetImageUrl(string plant, string customer_part)
        {
            string filepath = string.Empty;
            string filenotfound = string.Empty;

            filepath = string.Format("{0}\\{1}\\{2}.jpg", _ImageDirectory, plant, customer_part);
            filenotfound = string.Format("{0}\\NoPic.JPG", _ImageDirectory);

            try
            {
                if (File.Exists(filepath))
                    return Convert.ToBase64String(File.ReadAllBytes(filepath));
                else
                    return Convert.ToBase64String(File.ReadAllBytes(filenotfound));
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
