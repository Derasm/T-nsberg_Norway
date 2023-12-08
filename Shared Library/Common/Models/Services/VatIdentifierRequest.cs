using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models.Services
{
    /// <summary>
    /// Exists to avoid passing two strings around, and not exposing the service.
    /// </summary>
    public class VatIdentifierRequest
    {
        public string CountryCode { get; set; }
        public string VatID { get; set; }
    }
}
