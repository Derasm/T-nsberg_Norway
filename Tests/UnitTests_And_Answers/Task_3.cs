using CheckVatService;
using Moq;
using Shared.Common.Models;


namespace UnitTests_And_Answers
{
    internal class Task_3
    {
        Mock<ICheckVatServiceClient> _client;
        VatVerifier verifier;
        [SetUp]
        public void SetUp()
        {
            _client = new Mock<ICheckVatServiceClient>();
            //optimiistic approach: We are assuming the API call is always succesful here as we aren't testing the connection.
            _client.Setup(client => client.checkVatAsync(It.IsAny<checkVatRequest>()))
                     .Returns(new checkVatResponse { valid = true });
            //Setup the VatVerifier
            verifier = new VatVerifier(_client.Object);

        }
        /// <summary>
        /// This will fail, as the service endpoint cannot be called currently.
        /// </summary>
        [Test]
        public void VatVerifier_Should_Return_Valid_On_Valid_Request()
        {
            string countryCode = "DE";
            string vatID = "118856456";
            //mocking the client with moq, to keep this as unit-test instead of integration.

            Assert.That(verifier.Verify(countryCode, vatID), Is.EqualTo(VerificationStatus.Valid));
        }
        //Test that the countryCode is incorrect in amount of digits - first 1 char, then 3.
        [Test]
        public void VatVerifier_Wrong_Country_Code_Too_Short()
        {
            string countryCode = "D";
            string vatID = "118856456";
            //mocking the client with moq, to keep this as unit-test instead of integration

            var verifier = new VatVerifier(_client.Object);

            Assert.That(verifier.Verify(countryCode, vatID), Is.EqualTo(VerificationStatus.Invalid));
        }
        [Test]
        public void VatVerifier_Country_Code_Too_Long_Is_Correctly_Trimmed()
        {
            string countryCode = "DKK";
            string vatID = "DK118856456";


            Assert.That(verifier.Verify(countryCode, vatID), Is.EqualTo(VerificationStatus.Valid));
        }
        [Test]
        public void VatVerifier_Wrong_Country_ID_Format()
        {
            string countryCode = "DK";
            string vatID = "_____D___";
            Assert.That(verifier.Verify(countryCode, vatID), Is.EqualTo(VerificationStatus.Invalid));
        }
        [Test]
        public void VatVerifier_Country_ID_Too_Long_Is_Correctly_Trimmed()
        {
            string countryCode = "DK";
            string vatID = "DK11885645611111111";
            Assert.That(verifier.Verify(countryCode, vatID), Is.EqualTo(VerificationStatus.Valid));
        }
    }
}
