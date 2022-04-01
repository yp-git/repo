namespace TradesAPI.Models
{
    public class Position
    {
        public string SecurityCode { get; set; } = null!;

        public int Notional { get; set; }
    }
}
