using System;
using System.Net.Http;

namespace Shipping.ViewModelComposition
{
    public class ShippingApi : HttpClient
    {
        public ShippingApi(Uri baseAddress)
        {
            this.BaseAddress = baseAddress;
        }
    }
}
