using ContractorInvoiceApp.Models;
using ContractorInvoiceApp.Utils;

namespace ContractorInvoiceApp.Services
{
    class InvoiceService
    {
        private readonly string invoicesDir = Path.Combine(Directory.GetCurrentDirectory(), "Invoices");
        private readonly string trackerFile;

        public InvoiceService()
        {
            Directory.CreateDirectory(invoicesDir);
            trackerFile = Path.Combine(invoicesDir, "lastInvoiceNumber.txt");
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to Contractor Invoice App");
                Console.WriteLine("---------------------------------");
                Console.WriteLine("1. Create new invoice");
                Console.WriteLine("2. View saved invoices");
                Console.WriteLine("3. Exit");
                Console.Write("\nSelect an option (1-3): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateInvoice();
                        break;
                    case "2":
                        ViewInvoices();
                        break;
                    case "3":
                        Console.WriteLine("Exiting... Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice, press any key to try again...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private string GetNextInvoiceNumber()
        {
            int lastNumber = 0;

            if (File.Exists(trackerFile))
            {
                string lastNumStr = File.ReadAllText(trackerFile);
                int.TryParse(lastNumStr, out lastNumber);
            }

            lastNumber++;
            File.WriteAllText(trackerFile, lastNumber.ToString());

            return $"INV{lastNumber:D3}";
        }

        private void CreateInvoice()
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

            // Step 4: Assign invoice number
            invoice.InvoiceNumber = GetNextInvoiceNumber();

            // Step 5: Generate & display invoice
            string invoiceText = invoice.GenerateInvoiceText();
            invoiceText = $"Invoice No: {invoice.InvoiceNumber}\n" + invoiceText; // prepend number

            Console.WriteLine("\n" + Formatter.FormatBox(invoiceText));

            // Step 6: Save to file
            string fileName = Path.Combine(invoicesDir, $"{invoice.InvoiceNumber}_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
            File.WriteAllText(fileName, invoiceText);
            Console.WriteLine($"\nInvoice saved as {fileName}");

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        private void ViewInvoices()
        {
            var files = Directory.GetFiles(invoicesDir, "*.txt");

            if (files.Length == 0)
            {
                Console.WriteLine("\nNo invoices found.");
            }
            else
            {
                Console.WriteLine("\nSaved Invoices:");
                for (int i = 0; i < files.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
                }

                Console.Write("\nEnter invoice number to view (or press Enter to go back): ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int index) && index > 0 && index <= files.Length)
                {
                    string selectedFile = files[index - 1];
                    string content = File.ReadAllText(selectedFile);
                    Console.WriteLine("\n" + Formatter.FormatBox(content));
                }
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }
    }
}
