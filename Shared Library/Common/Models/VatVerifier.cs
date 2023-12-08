using CheckVatService;
using System.Text.RegularExpressions;
namespace Shared.Common.Models
{
    public enum VerificationStatus
    {
        Valid,
        Invalid,
        // Unable to get status (e.g. service unavailable)
        Unavailable
    }
    //In case we want to expand to different vat verifiers
    public interface IVatVerifier
    {
        VerificationStatus Verify(string countryCode, string vatId);
    }
    //This makes testing in isolation much easier, as the client suddenly isn't important.
    public interface ICheckVatServiceClient
    {
        checkVatResponse checkVatAsync(checkVatRequest request);
        void Dispose();
    }
    /// <summary>
    /// Implementation client of the ICheckVatServiceClient.
    /// </summary>
    public class CheckVatServiceClient : ICheckVatServiceClient, IDisposable
    {
        private readonly checkVatPortTypeClient _client;

        public CheckVatServiceClient()
        {
            _client = new checkVatPortTypeClient();
        }

        public checkVatResponse checkVatAsync(checkVatRequest request)
        {
            return _client.checkVatAsync(request).Result;
        }

        public void Dispose()
        {
            _client.Close();
        }
    }
    public class VatVerifier : IVatVerifier
    {
        private readonly ICheckVatServiceClient _client;
        public VatVerifier(ICheckVatServiceClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Verifies the given VAT ID for the given country using the EU VIES web service.
        /// </summary>
        /// <param name="countryCode"></param>
        /// <param name="vatId"></param>
        /// <returns>Verification status</returns>
        // TODO: Implement Verify method
        public VerificationStatus Verify(string countryCode, string vatId)
        {
            //first we verify the input adheres to the requirements.
            string countryCodePattern = @"[A-Z]{2}";
            string vatIDPattern = @"[0-9A-Za-z\+\*\.]{2,12}";
            Match countryCodeIsValid = Regex.Match(countryCode, countryCodePattern);
            if (!countryCodeIsValid.Success)
            {
                return VerificationStatus.Invalid;
            }
            Match vatIDIsValid = Regex.Match(vatId, vatIDPattern);
            if (!vatIDIsValid.Success)
            {
                return VerificationStatus.Invalid;
            }
            //We need to make a request and get a response. We have the objects in the SOAP configuration.
            checkVatRequest vatRequest = new checkVatRequest()
            {
                countryCode = countryCode,
                vatNumber = vatId,
            };
            try
            {
                var response = _client.checkVatAsync(vatRequest);
                if (!response.valid)
                {
                    return VerificationStatus.Invalid;
                }
            }
            catch (Exception)
            {
                //We end up here if there is an issue - typically MS exception is thrown.
                throw;
            }

            //dispose client to free resources.
            _client.Dispose();
            return VerificationStatus.Valid;
        }
    }
}
