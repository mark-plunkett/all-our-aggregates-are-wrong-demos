using System;
using System.Net.Http;

namespace Warehouse.ViewModelComposition
{
    public class WarehouseApi : HttpClient
    {
        public WarehouseApi(Uri baseAddress)
        {
            this.BaseAddress = baseAddress;
        }
    }
}
