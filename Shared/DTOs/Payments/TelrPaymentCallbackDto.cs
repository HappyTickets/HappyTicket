namespace Shared.DTOs.Payments
{
    public class TelrPaymentCallbackDto
    {
        public string Tran_Type { get; set; }
        public char Tran_Status { get; set; }
        public string Tran_Check { get; set; }
        public int Tran_Store { get; set; }
        public string Tran_Class { get; set; }
        public string Tran_Test { get; set; }
        public string Tran_Ref { get; set; }
        public string Tran_PrevRef { get; set; }
        public string Tran_FirstRef { get; set; }
        public string Tran_Currency { get; set; }
        public string Tran_Amount { get; set; }
        public long Tran_CartId { get; set; }
        public string Tran_Desc { get; set; }
        public string Tran_AuthCode { get; set; }
        public string Tran_AuthMessage { get; set; }
    }
}
