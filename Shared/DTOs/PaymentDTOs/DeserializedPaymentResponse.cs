using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.PaymentDTOs
{
    public class DeserializedPaymentResponse
    {
        public string? Method { get; set; }

        public string? Trace { get; set; }

        public DeserializedPaymentOrder? Order { get; set; }

        public DeserializedPaymentError? Error { get; set; }

    }
}
