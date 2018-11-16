using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Baskets.Common;

namespace Baskets.Repository
{
    public interface IBasketRepository
    {
        IList<Basket> GetBaskets();

        IList<Basket> GetBasketsByDomain(int domain);

        Basket GetBasketByTransactionNumber(Guid transactionNumber);

    }
}
