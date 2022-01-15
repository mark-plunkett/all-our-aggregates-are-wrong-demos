using System;
using System.Net.Http;

namespace Marketing.ViewModelComposition
{
    public class MarketingApi : HttpClient
    {
        public MarketingApi(Uri baseAddress)
        {
            this.BaseAddress = baseAddress;
        }
    }
}