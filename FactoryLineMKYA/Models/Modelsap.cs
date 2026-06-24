using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SapNwRfc;

namespace FactoryLineMKYA.Models.Modelsap
{
    public class ViewFormmain
    {
        public string prodDate { get; set; }
        public string plant { get; set; }
        public string prodLine { get; set; }
        public string prodOrder { get; set; }
        public string Material { get; set; }
        public string delDate { get; set; }
    }

    public class t1Model
    {
        public string ProductionOrder { get; set; }
        public string ConfirmNumber { get; set; }
        public string OrderType { get; set; }
        public string orderstatus { get; set; }
        public string orderversion { get; set; }
        public string icsno { get; set; }
        public string icsname { get; set; }
        public decimal planqty { get; set; }
        public int prodqty { get; set; }
        public int goodqty { get; set; }
        public int compoundqty { get; set; }
        public int defectqty { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string unit { get; set; }
        public decimal grqty { get; set; }
        public string planplant { get; set; }
        public string lgort { get; set; }
        public string prodplant { get; set; }
        public string orderstart { get; set; }
        public string orderend { get; set; }
        public bool Select { get; set; }
        public decimal acum_prodqty { get; set; }
        public int stdtt { get; set; }
        public decimal noemp { get; set; }
        public int cavity { get; set; }
        public int losstime { get; set; }
        public string ActualTime { get; set; }
        public int ActualSeconds { get; set; }
        public int actual_min { get; set; }
        public string actual_timeformat { get; set; }
        public int stdactual { get; set; }
        public int stdactual_percent { get; set; }
        public string prodline { get; set; }
        public int assqty { get; set; }
        public int AcumProdQty { get; set; }
        public string backflushno { get; set; }
        public string vicsname { get; set; }
        public string vSplitNo { get; set; }
        public string vSplitIcs { get; set; }
        public int STDTime { get; set; }
        public int Cavity { get; set; }

        public ZSMPP019 activity { get; set; } = new ZSMPP019();
    }
    public class ProductionOrderResult
    {
        public t1Model t1 { get; set; }
        public List<t1matModel> t1mat { get; set; }
        public bool success { get; set; }
        public string message { get; set; } = string.Empty;
    }
    public class t1matModel
    {
        public string ProductionOrder { get; set; }
        public string icsno { get; set; }
        public string icsname { get; set; }
        public string component { get; set; }
        public string comname { get; set; }
        public string comnamees { get; set; }
        public decimal qty { get; set; }
        public string unit { get; set; }
        public int assqty { get; set; }
        public int wfaqty { get; set; }
        public bool select { get; set; }
        public string plant { get; set; }
        public string lgort { get; set; }
        public int lossqty { get; set; }
        public decimal usageqty { get; set; }
        public string nodecimal { get; set; }
        public string matgroup { get; set; }
        public string mvtype { get; set; }
        public string rsnum { get; set; }
        public string rspos { get; set; }
    }
    public class ZBAPI_GETCURRENT_DATETIMEOutput
    {
        [SapName("DATE")]
        public DateTime date { get; set; }
        [SapName("TIME")]
        public TimeSpan time { get; set; }
        [SapName("DAY")]
        public string day { get; set; }
        [SapName("MONTH")]
        public string month { get; set; }
    }

    public class ZFMMPP009_INF_PRD_ORDERParameters
    {
        [SapName("IV_AUFNR")]
        public string ProductionOrder { get; set; }
    }
    public class ZFMMPP009_INF_PRD_ORDEROutput
    {
        [SapName("ES_RETURN")]
        public ZSMPP020 es_return { get; set; }
        [SapName("ES_HEADER_DATA")]
        public ZSMPP017 HeaderData { get; set; }
        [SapName("ET_COMPONENT_LIST_DATA")]
        public ZSMPP018[] Components { get; set; }
        [SapName("ET_ACTIVITY_LIST_DATA")]
        public ZSMPP019[] activity { get; set; }
    }
    public class ZSMPP020
    {
        [SapName("MSGTY")]
        public string type { get; set; }
        [SapName("MESSAGE")]
        public string message { get; set; }
    }
    public class ZSMPP017
    {
        [SapName("AUFNR")]
        public string ProductionOrder { get; set; }
        [SapName("AUART")]
        public string OrderType { get; set; }
        [SapName("PLNBEZ")]
        public string icsno { get; set; }
        [SapName("MAKTX")]
        public string icsname { get; set; }
        [SapName("MAKTX_ES")]
        public string vIcsName { get; set; }
        [SapName("GAMNG")]
        public decimal planqty { get; set; }
        [SapName("GMEIN")]
        public string unit { get; set; }
        [SapName("WERKS")]
        public string planplant { get; set; }
        [SapName("ALORT")]
        public string lgort { get; set; }
        [SapName("WERKS")]
        public string prodplant { get; set; }
        [SapName("GSTRP")]
        public string orderstart { get; set; }
        [SapName("GLTRP")]
        public string orderend { get; set; }
        [SapName("WEMNG")]
        public decimal ordergr { get; set; }
        [SapName("TXT04")]
        public string orderstatus { get; set; }
        [SapName("VERID")]
        public string orderversion { get; set; }

        [SapName("STDTIME")]
        public int stdtime { get; set; }

        [SapName("CAVITY")]
        public int cavity { get; set; }
    }
    public class ZSMPP018
    {
        [SapName("AUFNR")]
        public string ProductionOrder { get; set; }
        [SapName("MATNR")]
        public string component { get; set; }
        [SapName("MAKTX")]
        public string comname { get; set; }
        [SapName("MAKTX_ES")]
        public string comnamees { get; set; }
        [SapName("GAMNG")]
        public decimal qty { get; set; }
        [SapName("GMEIN")]
        public string unit { get; set; }
        [SapName("WERKS")]
        public string plant { get; set; }
        [SapName("LGPRO")]
        public string lgort { get; set; }
        [SapName("BWART")]
        public string mvtype { get; set; }
        [SapName("RSNUM")]
        public string rsnum { get; set; }
        [SapName("RSPOS")]
        public string rspos { get; set; }
        [SapName("MATKL")]
        public string matgroup { get; set; }
    }
    public class ZSMPP019
    {
        [SapName("AUFNR")]
        public string ProductionOrder { get; set; }
        [SapName("RUECK")]
        public string ConfirmNumber { get; set; }
        [SapName("VORNR")]
        public string actnumber { get; set; }
        [SapName("ARBPL")]
        public string workcenter { get; set; }
        [SapName("KTEXT")]
        public string workcentername { get; set; }
        [SapName("LSTAR01")]
        public string acttype1 { get; set; }
        [SapName("LSTAR_TXT01")]
        public string acttype1name { get; set; }
        [SapName("ISM01")]
        public decimal acttype1qty { get; set; }
        [SapName("ILE01")]
        public string acttype1unit { get; set; }
        [SapName("LSTAR02")]
        public string acttype2 { get; set; }
        [SapName("LSTAR_TXT02")]
        public string acttype2name { get; set; }
        [SapName("ISM02")]
        public decimal acttype2qty { get; set; }
        [SapName("ILE02")]
        public string acttype2unit { get; set; }
        [SapName("LSTAR03")]
        public string acttype3 { get; set; }
        [SapName("LSTAR_TXT03")]
        public string acttype3name { get; set; }
        [SapName("ISM03")]
        public decimal acttype3qty { get; set; }
        [SapName("ILE03")]
        public string acttype3unit { get; set; }
        [SapName("LSTAR04")]
        public string acttype4 { get; set; }
        [SapName("LSTAR_TXT04")]
        public string acttype4name { get; set; }
        [SapName("ISM04")]
        public decimal acttype4qty { get; set; }
        [SapName("ILE04")]
        public string acttype4unit { get; set; }
        [SapName("LSTAR05")]
        public string acttype5 { get; set; }
        [SapName("LSTAR_TXT05")]
        public string acttype5name { get; set; }
        [SapName("ISM05")]
        public decimal acttype5qty { get; set; }
        [SapName("ILE05")]
        public string acttype5unit { get; set; }
        [SapName("LSTAR06")]
        public string acttype6 { get; set; }
        [SapName("LSTAR_TXT06")]
        public string acttype6name { get; set; }
        [SapName("ISM06")]
        public decimal acttype6qty { get; set; }
        [SapName("ILE06")]
        public string acttype6unit { get; set; }
        [SapName("STEUS")]
        public string controlkey { get; set; }
    }

    public class SaleOrderGetListRequest
    {
        public string rfcname { get; set; }
        public List<RSDSSELOPT> iplant { get; set; }
        public List<RSDSSELOPT> isoldto { get; set; }
        public List<RSDSSELOPT> imaterial { get; set; }
        public List<RSDSSELOPT> icustmat { get; set; }
        public List<RSDSSELOPT> idelvdate { get; set; }
        public List<RSDSSELOPT> isono { get; set; }
        public List<RSDSSELOPT> idist { get; set; }
        public List<RSDSSELOPT> idiv { get; set; }
        public List<RSDSSELOPT> ishipto { get; set; }
        public List<RSDSSELOPT> isaleorg { get; set; }
        public List<RSDSSELOPT> iitemno { get; set; }
        public List<RSDSSELOPT> ipodetail { get; set; }
        public string iplanorder { get; set; }
    }
    public class soitem
    {

        public string VBELN { get; set; }

        public string POSNR { get; set; }

        public string MATNR { get; set; }

        public string KDMAT { get; set; }

        public string MAKTX_ES { get; set; }

        public string MAKTX_EN { get; set; }

        public string BSTKD { get; set; }

        public string BSTKD_E { get; set; }

        public string EMPST { get; set; }

        public string EDATU { get; set; }

        public string EZEIT { get; set; }

        public decimal KWMENG { get; set; }

        public string VRKME { get; set; }

        public int PACKSIZE { get; set; }

        public string MFGDATE { get; set; }

        public string WERKS { get; set; }

        public string VKORG { get; set; }

        public string VTWEG { get; set; }

        public string SPART { get; set; }

        public string SOLDTO { get; set; }

        public string SOLDTO_NAME { get; set; }

        public string SHIPTO { get; set; }

        public string SHIPTO_NAME { get; set; }

        public string ZMODC { get; set; }

        public string ZCOLOR { get; set; }

        public string MDV01 { get; set; }

        public bool SELECT { get; set; }

        public string BASE64 { get; set; }

        public int LABELQTY { get; set; }

        public string LABELPAGE { get; set; }

        public string LGORT { get; set; }

        public string BCDATA { get; set; }

        public string SIDE { get; set; }

        public string LGPBE { get; set; }

        public short PAGE { get; set; }

        public bool MFGOPEN { get; set; }

        public string ZKDNM { get; set; }

        public string KDMAT_1 { get; set; }

        public string KDMAT_2 { get; set; }

        public string IUNDERL { get; set; }

        public string ISYAMAHA { get; set; }

        public string IMAGEURL { get; set; }

        public string CODE128 { get; set; }

        public string AUART { get; set; }

        public string Planorder { get; set; }

        public string BCTYDATA { get; set; }
        public string BASE64TY { get; set; }
        public string NISSAN_TYPE { get; set; }

    }
    public class ZBF_SALEORDER_GETLISTParameter
    {
        [SapName("I_PLANT")]
        public RSDSSELOPT[] iplant { get; set; }
        [SapName("I_SOLDTO")]
        public RSDSSELOPT[] isoldto { get; set; }
        [SapName("I_MATERIAL")]
        public RSDSSELOPT[] imaterial { get; set; }
        [SapName("I_CUSTPART")]
        public RSDSSELOPT[] icustmat { get; set; }
        [SapName("I_DELVDATE")]
        public RSDSSELOPT[] idelvdate { get; set; }
        [SapName("I_SALEORDER")]
        public RSDSSELOPT[] isono { get; set; }
        [SapName("I_DISTCHN")]
        public RSDSSELOPT[] idist { get; set; }
        [SapName("I_DIVISION")]
        public RSDSSELOPT[] idiv { get; set; }
        [SapName("I_SHIPTO")]
        public RSDSSELOPT[] ishipto { get; set; }
        [SapName("I_SALEORG")]
        public RSDSSELOPT[] isaleorg { get; set; }
        [SapName("I_SALEITEM")]
        public RSDSSELOPT[] iitemno { get; set; }
        [SapName("I_PONUMBER")]
        public RSDSSELOPT[] ipodetail { get; set; }
    }
    public class ZBF_SALEORDER_GETLISTOutput
    {
        [SapName("SO_LABEL")]
        public ZSSO_LABEL[] tsolabel { get; set; }
    }
    public class ZCA002_GET_PROG_CONFIG_RFCParameter
    {
        [SapName("I_BUKRS")]
        public string company { get; set; }
        [SapName("I_PROGRAM")]
        public string program { get; set; }
        [SapName("I_ROUTINE")]
        public string routine { get; set; }
        [SapName("I_FIELD")]
        public string field { get; set; }
    }

    public class ZCA002_GET_PROG_CONFIG_RFCOutput
    {
        [SapName("T_E_RESULT")]
        public EFG_RANGES[] t_result { get; set; }
    }

    public class EFG_RANGES
    {
        [SapName("SIGN")]
        public string sign { get; set; }
        [SapName("OPTION")]
        public string option { get; set; }
        [SapName("LOW")]
        public string low { get; set; }
        [SapName("HIGH")]
        public string high { get; set; }
    }
    public class ZSSO_LABEL
    {
        [SapName("VBELN")]
        public string VBELN { get; set; }
        [SapName("POSNR")]
        public string POSNR { get; set; }
        [SapName("MATNR")]
        public string MATNR { get; set; }
        [SapName("KDMAT")]
        public string KDMAT { get; set; }
        [SapName("MAKTX_ES")]
        public string MAKTX_ES { get; set; }
        [SapName("MAKTX_EN")]
        public string MAKTX_EN { get; set; }
        [SapName("ZKDNM")]
        public string ZKDNM { get; set; }
        [SapName("BSTKD")]
        public string BSTKD { get; set; }
        [SapName("BSTKD_E")]
        public string BSTKD_E { get; set; }
        [SapName("EMPST")]
        public string EMPST { get; set; }
        [SapName("EDATU")]
        public DateTime EDATU { get; set; }
        [SapName("EZEIT")]
        public TimeSpan EZEIT { get; set; }
        [SapName("KWMENG")]
        public decimal KWMENG { get; set; }
        [SapName("VRKME")]
        public string VRKME { get; set; }
        [SapName("PACKSIZE")]
        public string PACKSIZE { get; set; }
        [SapName("MFGDATE")]
        public DateTime MFGDATE { get; set; }
        [SapName("WERKS")]
        public string WERKS { get; set; }
        [SapName("LGORT")]
        public string LGORT { get; set; }
        [SapName("LGPBE")]
        public string LGPBE { get; set; }
        [SapName("VKORG")]
        public string VKORG { get; set; }
        [SapName("VTWEG")]
        public string VTWEG { get; set; }
        [SapName("SPART")]
        public string SPART { get; set; }
        [SapName("SOLDTO")]
        public string SOLDTO { get; set; }
        [SapName("SOLDTO_NAME")]
        public string SOLDTO_NAME { get; set; }
        [SapName("SHIPTO")]
        public string SHIPTO { get; set; }
        [SapName("SHIPTO_NAME")]
        public string SHIPTO_NAME { get; set; }
        [SapName("ZMODC")]
        public string ZMODC { get; set; }
        [SapName("ZCOLOR")]
        public string ZCOLOR { get; set; }
        [SapName("MDV01")]
        public string MDV01 { get; set; }
        [SapName("AUART")]
        public string AUART { get; set; }
    }
    public class RSDSSELOPT
    {
        [SapName("SIGN")]
        public string sign { get; set; }
        [SapName("OPTION")]
        public string option { get; set; }
        [SapName("LOW")]
        public string low { get; set; }
        [SapName("HIGH")]
        public string high { get; set; }
    }
    public partial class bf_nissan_type
    {
        public string icsno { get; set; }
        public string stamp_type { get; set; }
    }

    public  class OrderModel
    {
        public string OrderId { get; set; }
        public string Status { get; set; }
        public string UpdatedAt { get; set; }
    }
}
