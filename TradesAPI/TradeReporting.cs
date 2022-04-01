using TradesAPI.Models;

namespace TradesAPI
{
    public class TradeReporting : ITradeReporting
    {
        public IEnumerable<Position> GetPositions(List<Transaction> alltrades)
        {           
            var updatedTrades = alltrades.Where(t => t.Action == Models.Action.UPDATE.ToString());
            var canceledTrades = alltrades.Where(t => t.Action == Models.Action.CANCEL.ToString());

            var onlyValidTrades = alltrades.Where(t => !updatedTrades.Any(u => u.TradeId == t.TradeId && u.Version - 1 == t.Version) && !canceledTrades.Any(c => c.TradeId == t.TradeId));


            var positions = from t in onlyValidTrades
                            group t by t.SecurityCode
                        into g
                            select new Position()
                            {
                                SecurityCode = g.ToList().OrderByDescending(k => k.Version).First().SecurityCode,
                                Notional = g.Sum(s => s.Buy ? s.Quantity : -s.Quantity)
                            };

            foreach (var c in canceledTrades)
            {
                positions = positions?.Append(new Position { SecurityCode = c.SecurityCode, Notional = 0 });
            }

            return positions;
        }
    }
}
