using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.PaymentDTOs
{
    public class DeserializedPaymentError
    {
        public string? Note { get; set; }

        public string? Message { get; set; }
    }
}
