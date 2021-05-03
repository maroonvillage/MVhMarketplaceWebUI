using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webcoreapp.Enumerators
{
    public enum DynamicContentType
    {
        //TODO: This is probably going to be temporary
        Unknown = 0,
        None = 1,
        ClientImageFeed = 2,
        API = 3,
        VendorImageFeed = 4,
        InfoBlurb = 5,
        Menu = 6,
        PartialView = 7
    }
}
