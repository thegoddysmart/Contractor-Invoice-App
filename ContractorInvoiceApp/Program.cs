using ContractorInvoiceApp.Models;
using ContractorInvoiceApp.Services;

namespace ContractorInvoiceApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new InvoiceService();
            service.Run();
        }
    }
}
