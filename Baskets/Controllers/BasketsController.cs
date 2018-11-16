using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Baskets.Repository;
using Newtonsoft.Json;

namespace Baskets.Controllers
{
    [RoutePrefix("api/baskets")]
    public class BasketsController : ApiController
    {
        private readonly IBasketRepository _basketRepository;

        public BasketsController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        /// <summary>
        /// Get all the baskets (If domain is specified it filters by domain otherwise retreives all the baskets)
        /// </summary>
        /// <param name="domain"></param>
        /// <returns>A json list of baskets. </returns>
        [Route("{domain?}")]
        [HttpGet]
        public HttpResponseMessage Get(int? domain = null)
        {
            var data = domain.HasValue ? _basketRepository.GetBasketsByDomain(domain.Value) : _basketRepository.GetBaskets();

            return CreateJsonResponse(data);
        }

        /// <summary>
        /// Gets the specified basket by transaction number
        /// </summary>
        /// <param name="transactionNumber"></param>
        /// <returns>Specified basket</returns>
        [Route("transactions")]
        [HttpGet]
        public HttpResponseMessage GetTransaction(Guid transactionNumber)
        {
            var data = _basketRepository.GetBasketByTransactionNumber(transactionNumber);

            return CreateJsonResponse(data);
        }

        private HttpResponseMessage CreateJsonResponse(object data)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            };

            return response;
        }
    }
}
