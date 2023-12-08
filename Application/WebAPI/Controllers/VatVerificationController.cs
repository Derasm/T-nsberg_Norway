using Common.Models.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Models;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VatVerificationController : ControllerBase
    {
        private readonly IVatVerifier _vatVerifier;
        //Typical DI pattern, in case other VatVerifier is needed later. 
        public VatVerificationController(IVatVerifier vatVerifier)
        {
            _vatVerifier = vatVerifier;
        }
        // GET api/<VatVerificationController>/5
        [ProducesResponseType(typeof(VatVerificationStatus), 200)]
        [ProducesResponseType(400)]

        [HttpGet]
        public VatIdentifierResponse GetValidityOfVatCode(VatIdentifierRequest request)
        {
            VatIdentifierResponse response = new VatIdentifierResponse()
            {
                Status = _vatVerifier.Verify(request)
            };
            return response;
        }

    }
}
