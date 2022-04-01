using System;
using System.Collections.Generic;

namespace TradesAPI.Models
{
    public partial class Transaction
    {
        internal long TransactionId { get; set; }
        public long TradeId { get; set; }
        public int Version { get; set; }
        public string SecurityCode { get; set; } = null!;
        public int Quantity { get; set; }
        public string Action { get; set; }
        public bool Buy { get; set; }
        
        public void SetTrade(Trade trade)
        {
            TradeId = trade.TradeId;
            SecurityCode = trade.SecurityCode;
            Quantity = trade.Quantity;
            Action = trade.Action;
            Buy = trade.Buy;
        }
            
    }

    public enum Action
    {
        INSERT,
        UPDATE,
        CANCEL
    }   

}
