using TradesAPI.Models;

namespace TradesAPI
{
    public interface ITradeReporting
    {
        IEnumerable<Position> GetPositions(List<Transaction> transactions);
    }
}
