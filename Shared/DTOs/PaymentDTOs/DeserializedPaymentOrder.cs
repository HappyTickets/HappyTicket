using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.PaymentDTOs
{
    public class DeserializedPaymentOrder
    {
        public string? Ref { get; set; }
        public string? Url { get; set; }
    }
}
