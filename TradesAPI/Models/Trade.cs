namespace TradesAPI.Models
{
    public class Trade
    {       
            
            public long TradeId { get; set; }
            internal int Version { get; set; }
            public string SecurityCode { get; set; } = null!;
            public int Quantity { get; set; }
            public string Action { get; set; } 
            public bool Buy { get; set; }
        
    }
}
