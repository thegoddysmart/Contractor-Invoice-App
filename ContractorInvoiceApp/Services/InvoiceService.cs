using ContractorInvoiceApp.Models;
using ContractorInvoiceApp.Utils;

namespace ContractorInvoiceApp.Services
{
    class InvoiceService
    {
        public void Run()
        {
            // Step 1: Enter contractor details
            var contractor = new Contractor();
            Console.Write("Enter contractor name: ");
            contractor.Name = Console.ReadLine();
            Console.Write("Enter project name: ");
            contractor.Project = Console.ReadLine();
            Console.Write("Enter client name: ");
            contractor.Client = Console.ReadLine();
            Console.Write("Enter contact info: ");
            contractor.ContactInfo = Console.ReadLine();

            // Step 2: Add materials
            var invoice = new Invoice { Contractor = contractor };
            Console.WriteLine("\nEnter materials (type 'done' to finish):");

            while (true)
            {
                Console.Write("Item name (or 'done'): ");
                string itemName = Console.ReadLine();
                if (itemName.ToLower() == "done") break;

                Console.Write("Quantity: ");
                int qty = int.Parse(Console.ReadLine());
                Console.Write("Unit price: ");
                decimal price = decimal.Parse(Console.ReadLine());

                invoice.Materials.Add(new Material
                {
                    ItemName = itemName,
                    Quantity = qty,
                    UnitPrice = price
                });
            }

            // Step 3: Enter labor cost
            Console.Write("Enter labor cost: ");
            invoice.LaborCost = decimal.Parse(Console.ReadLine());

            // Step 4: Generate & display invoice
            string invoiceText = invoice.GenerateInvoiceText();
            Console.WriteLine("\n" + Formatter.FormatBox(invoiceText));

            // Step 5: Save to file
            string dir = Path.Combine(Directory.GetCurrentDirectory(), "Invoices");
            Directory.CreateDirectory(dir);
            string fileName = Path.Combine(dir, $"Invoice_{DateTime.Now:yyyyMMdd_HHmmss}.txt");

            File.WriteAllText(fileName, invoiceText);
            Console.WriteLine($"\nInvoice saved as {fileName}");
        }
    }
}
