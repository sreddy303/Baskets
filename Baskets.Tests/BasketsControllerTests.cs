using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Baskets.Controllers;
using Baskets.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Http;
using Baskets.Common;
using FluentAssertions;
using Newtonsoft.Json;

namespace Baskets.Tests
{
    [TestClass]
    public class BasketsControllerTests
    {
        private BasketsController _basketsController;
        private Mock<IBasketRepository> _mockBasketRepository;

        [TestInitialize]
        public void Setup()
        {
            _mockBasketRepository = new Mock<IBasketRepository>();
            _basketsController = new BasketsController(_mockBasketRepository.Object);
        }

        [TestMethod]
        public void GetBasketsControllerWithValidNoDomainReturnsOkResponse()
        {
            var response = _basketsController.Get(null);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [TestMethod]
        public void GetBasketsControllerWithNoDomainAndValidMockDataReturnsCorrectResult()
        {
            var expectedResult = GetBasketsTestData();

            _mockBasketRepository.Setup(x => x.GetBaskets()).Returns(expectedResult);

            var response = _basketsController.Get(null);
            var actualResult = ConvertResponseToListOfBaskets(response);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Assert.AreEqual(2, actualResult.Count);
            
            _mockBasketRepository.Verify(x => x.GetBaskets(), Times.Once);
        }

        [TestMethod]
        public void GetBasketsControllerWithDomainAndValidMockDataReturnsCorrectResult()
        {
            int domain = 1;
            var expectedResult = GetBasketsTestData();

            _mockBasketRepository.Setup(x => x.GetBasketsByDomain(It.IsAny<int>())).Returns(expectedResult);

            var response = _basketsController.Get(domain);
            var actualResult = ConvertResponseToListOfBaskets(response);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Assert.AreEqual(2, actualResult.Count);

            _mockBasketRepository.Verify(x => x.GetBasketsByDomain(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void GetBasketsControllerWithValidTransactionReturnsOkResponse()
        {
            Guid transactionNumber = new Guid("000840f3-e60c-4359-9ad5-b5de77c801c4");
            var response = _basketsController.GetTransaction(transactionNumber);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [TestMethod]
        public void GetBasketsControllerWithValidTransactionAndMockDataReturnsCorrectResult()
        {
            var expectedResult = GetBasketTestData();

            _mockBasketRepository.Setup(x => x.GetBasketByTransactionNumber(It.IsAny<Guid>())).Returns(expectedResult);

            var response = _basketsController.GetTransaction(expectedResult.TransactionNumber);
            var actualResult = ConvertResponseToSingleBasket(response);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult.TransactionNumber, actualResult.TransactionNumber);

            _mockBasketRepository.Verify(x => x.GetBasketByTransactionNumber(It.IsAny<Guid>()), Times.Once);
        }


        private List<Basket> ConvertResponseToListOfBaskets(HttpResponseMessage responseMessage)
        {
            return JsonConvert.DeserializeObject<List<Basket>>(responseMessage.Content.ReadAsStringAsync().Result);
        }

        private Basket ConvertResponseToSingleBasket(HttpResponseMessage responseMessage)
        {
            return JsonConvert.DeserializeObject<Basket>(responseMessage.Content.ReadAsStringAsync().Result);
        }

        private List<Basket> GetBasketsTestData()
        {
           return new List<Basket>()
            {
                new Basket()
                {
                    AgentId = 1,
                    CreatedDateTime = DateTime.Now,
                    Domain = 1,
                    NumberOfPassengers = null,
                    ReferrerUrl = "http://www.dummy.com",
                    ReservationSystem = "TestReservation",
                    SelectedCurrency = "TestCurrency",
                    TransactionNumber = new Guid(),
                    UserId = "TestUserId"
                },
                new Basket()
                {
                    AgentId = 1,
                    CreatedDateTime = DateTime.Now,
                    Domain = 10,
                    NumberOfPassengers = null,
                    ReferrerUrl = "http://www.dummy.com",
                    ReservationSystem = "TestReservation",
                    SelectedCurrency = "TestCurrency",
                    TransactionNumber = new Guid(),
                    UserId = "TestUserId"
                }
            };
        }

        private Basket GetBasketTestData()
        {
            return new Basket()
            {
                AgentId = 1,
                CreatedDateTime = DateTime.Now,
                Domain = 1,
                NumberOfPassengers = null,
                ReferrerUrl = "http://www.dummy.com",
                ReservationSystem = "TestReservation",
                SelectedCurrency = "TestCurrency",
                TransactionNumber = new Guid("000840f3-e60c-4359-9ad5-b5de77c801c4"),
                UserId = "TestUserId"

            };
        }
    }
}
