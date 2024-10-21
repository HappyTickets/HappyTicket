using System.Text.Json.Serialization;

namespace Shared.DTOs.PaymentDTOs
{
    public class PaymentCalllbackDto
    {
        [JsonPropertyName("tran_store")]
        public int TranStore { get; set; }

        [JsonPropertyName("tran_type")]
        public string TranType { get; set; }

        [JsonPropertyName("tran_class")]
        public string TranClass { get; set; }

        [JsonPropertyName("tran_test")]
        public int TranTest { get; set; }

        [JsonPropertyName("tran_ref")]
        public string TranRef { get; set; }

        [JsonPropertyName("tran_prevref")]
        public string TranPrevRef { get; set; }

        [JsonPropertyName("tran_firstref")]
        public string TranFirstRef { get; set; }

        [JsonPropertyName("tran_order")]
        public string TranOrder { get; set; }

        [JsonPropertyName("tran_currency")]
        public string TranCurrency { get; set; }

        [JsonPropertyName("tran_amount")]
        public decimal TranAmount { get; set; }

        [JsonPropertyName("tran_cartid")]
        public string TranCartId { get; set; }

        [JsonPropertyName("tran_desc")]
        public string TranDesc { get; set; }

        [JsonPropertyName("tran_status")]
        public char TranStatus { get; set; }

        [JsonPropertyName("tran_authcode")]
        public string TranAuthCode { get; set; }

        [JsonPropertyName("tran_authmessage")]
        public string TranAuthMessage { get; set; }

        [JsonPropertyName("tran_check")]
        public string TranCheck { get; set; }

        [JsonPropertyName("card_code")]
        public string CardCode { get; set; }

        [JsonPropertyName("card_payment")]
        public string CardPayment { get; set; }

        [JsonPropertyName("bin_number")]
        public string BinNumber { get; set; }

        [JsonPropertyName("card_issuer")]
        public string CardIssuer { get; set; }

        [JsonPropertyName("card_country")]
        public string CardCountry { get; set; }

        [JsonPropertyName("card_last4")]
        public string CardLast4 { get; set; }

        [JsonPropertyName("cart_lang")]
        public string CartLang { get; set; }

        [JsonPropertyName("integration_id")]
        public string IntegrationId { get; set; }

        [JsonPropertyName("actual_payment_date")]
        public DateTime ActualPaymentDate { get; set; }

        [JsonPropertyName("bill_title")]
        public string BillTitle { get; set; }

        [JsonPropertyName("bill_fname")]
        public string BillFname { get; set; }

        [JsonPropertyName("bill_sname")]
        public string BillSname { get; set; }

        [JsonPropertyName("bill_addr1")]
        public string BillAddr1 { get; set; }

        [JsonPropertyName("bill_addr2")]
        public string BillAddr2 { get; set; }

        [JsonPropertyName("bill_addr3")]
        public string BillAddr3 { get; set; }

        [JsonPropertyName("bill_city")]
        public string BillCity { get; set; }

        [JsonPropertyName("bill_region")]
        public string BillRegion { get; set; }

        [JsonPropertyName("bill_country")]
        public string BillCountry { get; set; }

        [JsonPropertyName("bill_zip")]
        public string BillZip { get; set; }

        [JsonPropertyName("bill_phone1")]
        public string BillPhone1 { get; set; }

        [JsonPropertyName("bill_email")]
        public string BillEmail { get; set; }

        [JsonPropertyName("xtra_")]
        public string Xtra { get; set; }
    }

}
