using System;
using System.Net.Http;

namespace Sales.ViewModelComposition
{
    public class SalesApi : HttpClient
    {
        public SalesApi(Uri baseAddress)
        {
            this.BaseAddress = baseAddress;
        }
    }
}
