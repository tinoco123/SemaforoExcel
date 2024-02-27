using System;
using CsvHelper.Configuration.Attributes;

namespace Semaforo.Models
{
    public class Item
    {
        [Name("Producto")]
        public string ItemName { get; set; }
        [Name("Número de lote")]
        public string LotNumber { get; set; }
        [Ignore]
        public DateTime ExpirationDate { get; set; }
        [Name("Fecha de expiración")]
        public string ExpirationDateView { get; set; }
        [Name("Días hasta la fecha de expiración")]
        public int DaysUntilExpirationDate { get; set; }
        public string Color { get; set; }
        [Name("Valor")]
        public double Value { get; set; }
        [Name("Cantidad")]
        public double Quantity { get; set; }

        public Item(string itemName, string lotNumber, DateTime expirationDate, string expirationDateView, int daysUntilExpirationDate, string color, double value, double quantity)
        {
            ItemName = itemName;
            LotNumber = lotNumber;
            ExpirationDate = expirationDate;
            ExpirationDateView = expirationDateView;
            DaysUntilExpirationDate = daysUntilExpirationDate;
            Color = color;
            Value = value;
            Quantity = quantity;
        }

        public Item()
        {

        }
    }
}
