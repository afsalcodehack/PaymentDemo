using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Repositories.IRepositories;
using server.Models;
using server.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
   public class AuthorizeCapturePaymentGatewayTest
    {
        private Mock<IStripePaymentsRepository> mockStripePaymentsRepository;

        private MockRepository mockRepository;
        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockStripePaymentsRepository = this.mockRepository.Create<IStripePaymentsRepository>();

        }

        [TestMethod]
        private AuthorizeCapturePaymentGateway CreateAuthorizeCapturePaymentGateway()
        {
            return new AuthorizeCapturePaymentGateway(
               this.mockStripePaymentsRepository.Object
               );

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Authorize_StateUnderTest_ReturnNull()
        {
            string expectedParam = "request";
            var paymentGatewayController = this.CreateAuthorizeCapturePaymentGateway();
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
    }
}
