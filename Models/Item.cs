﻿using System;

namespace Semaforo.Models
{
    public class Item
    {
        public string ItemName { get; set; }
        public string LotNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int DaysUntilExpirationDate { get; set; }
        public double Value { get; set; }
        public double Quantity { get; set; }

        public Item(string itemName, string lotNumber, DateTime expirationDate, int daysUntilExpirationDate, double value, double quantity)
        {
            ItemName = itemName;
            LotNumber = lotNumber;
            ExpirationDate = expirationDate;
            DaysUntilExpirationDate = daysUntilExpirationDate;
            Value = value;
            Quantity = quantity;
        }
    }
}