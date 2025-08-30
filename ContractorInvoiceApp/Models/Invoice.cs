using System.Text;

namespace ContractorInvoiceApp.Models
{
    class Invoice
    {
        public Contractor Contractor { get; set; }
        public List<Material> Materials { get; set; } = new List<Material>();
        public decimal LaborCost { get; set; }
        public decimal VatRate { get; set; } = 0.15m; // 15% VAT

        public decimal MaterialsTotal => Materials.Sum(m => m.Total);
        public decimal Subtotal => MaterialsTotal + LaborCost;
        public decimal VatAmount => Subtotal * VatRate;
        public decimal Total => Subtotal + VatAmount;

        public string GenerateInvoiceText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("========== CONTRACTOR INVOICE ==========");
            sb.AppendLine($"Contractor: {Contractor.Name}");
            sb.AppendLine($"Project: {Contractor.Project}");
            sb.AppendLine($"Client: {Contractor.Client}");
            sb.AppendLine($"Contact: {Contractor.ContactInfo}");
            sb.AppendLine("---------------------------------------");

            // Materials header (table format)
            sb.AppendLine("Materials:");
            sb.AppendLine(string.Format("{0,-15} {1,-8} {2,-15} {3,-15}",
                "Item", "Qty", "Unit Price", "Total"));
            sb.AppendLine(new string('-', 55));

            foreach (var m in Materials)
            {
                sb.AppendLine(string.Format("{0,-15} {1,-8} ₵{2,-13:N2} ₵{3,-13:N2}",
                    m.ItemName, m.Quantity, m.UnitPrice, m.Total));
            }

            sb.AppendLine("---------------------------------------");
            sb.AppendLine($"Labor Cost:       {"₵" + LaborCost.ToString("N2"),15}");
            sb.AppendLine($"Materials Total:  {"₵" + MaterialsTotal.ToString("N2"),15}");
            sb.AppendLine($"Subtotal:         {"₵" + Subtotal.ToString("N2"),15}");
            sb.AppendLine($"VAT ({VatRate:P0}):      {"₵" + VatAmount.ToString("N2"),15}");
            sb.AppendLine($"TOTAL:            {"₵" + Total.ToString("N2"),15}");
            sb.AppendLine("=======================================");
            sb.AppendLine($"Generated on {DateTime.Now}");
            return sb.ToString();
        }
    }
}
