using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Baskets.Common;

namespace Baskets.Repository
{
    public class BasketRepository: IBasketRepository
    {
        public IList<Basket> GetBaskets()
        {
            var baskets = GetBasketsFromDataSource();

            return baskets.OrderByDescending(x => x.CreatedDateTime).ToList();
        }

        public IList<Basket> GetBasketsByDomain(int domain)
        {
            var baskets = GetBasketsFromDataSource();

            return baskets.Where( x=> x.Domain == domain).OrderByDescending(x => x.CreatedDateTime).ToList();
        }

        public Basket GetBasketByTransactionNumber(Guid transactionNumber)
        {
            var baskets = GetBasketsFromDataSource();

            return baskets.FirstOrDefault(x => x.TransactionNumber == transactionNumber);
        }

        private static IEnumerable<Basket> GetBasketsFromDataSource()
        {
            var solutionDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\"));
            var csvFilePath = Path.Combine(solutionDirectory, Constants.DataSourceDirectory, Constants.FileName);

            // Convert the .csv into basket object
            var baskets = File.ReadAllLines(csvFilePath)
                .Skip(1)
                .Select(x => x.Split(','))
                .Select(x => new Basket()
                {
                    TransactionNumber = Guid.Parse(x[0]),
                    NumberOfPassengers = x[1].ToInt(),
                    Domain = x[2].ToInt(),
                    AgentId = x[3].ToInt(),
                    ReferrerUrl = x[4],
                    CreatedDateTime = DateTime.Parse(x[5]),
                    UserId = x[6],
                    SelectedCurrency = x[7],
                    ReservationSystem = x[8]
                });

            return baskets;
        }
    }
}
