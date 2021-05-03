using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webcoreapp.Models;

namespace webcoreapp.Services
{
    public interface IMarketplaceService
    {
        Marketplace GetMarketplaceByDomain(string domain);

        Marketplace GetMarketplaceById(int marketPlaceId);


    }
}
