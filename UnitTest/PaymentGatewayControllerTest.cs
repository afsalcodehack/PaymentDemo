using Entity.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using server.Service;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using server.Controllers;
using server.Models;

namespace UnitTest
{
    [TestClass]
public    class PaymentGatewayControllerTest
    {
        private MockRepository mockRepository;

        private Mock<IOptions<StripeOptions>> mockOptions;
        private Mock<IStripeClient> mockClient;
        private Mock<ILogger<PaymentGatewayController>> mockLogger;
        private string secretKey = "sk_test_51Iv3WDH4DR7BOnAWtWXeiWcg3Ztdl3xnPqLAkvQY9rg2orkMVwxEgPYSh2KKfI2WH5UORaVDbwlcJnZdPAwxkHDS00c06HuETL";
        private Mock<IAuthorizeCapturePaymentGateway> mockAuthorizeCapturePaymentGateway;
        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockOptions = this.mockRepository.Create<IOptions<StripeOptions>>();
            this.mockClient = this.mockRepository.Create<IStripeClient>();
            this.mockAuthorizeCapturePaymentGateway = this.mockRepository.Create<IAuthorizeCapturePaymentGateway>();
            this.mockLogger = this.mockRepository.Create<ILogger<PaymentGatewayController>>();

        }
        [TestMethod]
        private PaymentGatewayController CreatePaymentGatewayController()
        {
            return new PaymentGatewayController(
                this.mockOptions.Object,
                this.mockAuthorizeCapturePaymentGateway.Object,
                this.mockLogger.Object
               );
           
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Authorize_StateUnderTest_ReturnNull()
        {
            string expectedParam = "request";
            var paymentGatewayController = this.CreatePaymentGatewayController();
            PaymentIntentCreateRequest paymentIntentCreateRequest = null;
            // Act
            try
            {
                var result = paymentGatewayController.Authorize(paymentIntentCreateRequest);
            }
            // Assert
            catch (ArgumentException ex)
            {
                Assert.AreEqual(expectedParam, ex.ParamName);
            }
        }

        [TestMethod]
        public void Authorize_StateUnderTest_ReturnExpectedBehavior()
        {
            var paymentGatewayController = this.CreatePaymentGatewayController();
            var paymentMethodTypes = new List<string>();
            paymentMethodTypes.Add("card");

            PaymentIntentCreateRequest paymentIntentCreateRequest = new PaymentIntentCreateRequest
            {
                Amount = 2000,
                CaptureMethod = "capture",
                PaymentMethodTypes = paymentMethodTypes,
                Payment_id = "py88991122kju",
                Currency="usd"
            };
            AuthorizeCaptureResponse authorizeCaptureResponse = new AuthorizeCaptureResponse
            {
                clientSecret= "sk_test_51Iv3WDH4DR7BOnAWtWXeiWcg3Ztdl3xnPqLAkvQY9rg2orkMVwxEgPYSh2KKfI2WH5UORaVDbwlcJnZdPAwxkHDS00c06HuETL"
            };
            // Act
            mockAuthorizeCapturePaymentGateway.Setup(x => x.Authorize(paymentIntentCreateRequest))
              .Returns(authorizeCaptureResponse);
            var result = paymentGatewayController.Authorize(paymentIntentCreateRequest);

            // Assert
            Assert.AreEqual(result, authorizeCaptureResponse);
        }
        [TestCleanup]
        public void TearDown()
        {
            mockRepository.VerifyAll();
        }
    }
}
