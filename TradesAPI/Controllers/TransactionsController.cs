#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradesAPI.Models;

namespace TradesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TradeDBContext _context;
        private readonly ITradeReporting tradeReporting;

        public ILogger<TransactionsController> Logger { get; }

        public TransactionsController(TradeDBContext context, ILogger<TransactionsController> logger, ITradeReporting tradeReporting)
        {
            _context = context;
            Logger = logger;
            this.tradeReporting = tradeReporting;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return await _context.Transactions.ToListAsync();
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(long id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        [HttpGet("Positions")]
        public async Task<ActionResult<IEnumerable<Position>>> GetPositions()
        {
            return tradeReporting.GetPositions(await _context.Transactions.ToListAsync()).ToArray();
        }
              

       
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransaction", new { id = transaction.TransactionId }, transaction);
        }


        [HttpPost("PostTrade")]
        public async Task<ActionResult<Transaction>> PostTrade(Trade trade)
        {           
            var k = _context.Transactions.Where(t => t.TradeId == trade.TradeId);            
            int maxVer = k.Count() > 0 ? k.Max(t => t.Version): 0 ;

            var trans = new Transaction();

            if (maxVer == 0)
            {
                //new trade
                trans = new Transaction();
                trans.SetTrade(trade);
                trans.Version = 1;
                _context.Transactions.Add(trans);
            }
            else
            {
                trans.Version = maxVer + 1;
                trans.SetTrade(trade);                
                _context.Transactions.Add(trans);
            }
            await _context.SaveChangesAsync();

            trans = _context.Transactions.Where(t => t.TradeId == trade.TradeId).OrderByDescending(k => k.Version).First();
            return CreatedAtAction("GetTransaction", new { id = trans.TransactionId }, trans);
        }

        [HttpPost("PostTradeinBulk")]
        public async Task<ActionResult<string>> PostBulkTransaction(Trade[] trades)
        {
            //todo : transaction and exception handling
            try
            {
                trades?.ToList().ForEach(t => PostTrade(t).Wait());
            }
            catch(Exception ex)
            {
                Logger.LogError("Error occured",ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return StatusCode(StatusCodes.Status200OK);
        }
                

        private bool TransactionExists(long id)
        {
            return _context.Transactions.Any(e => e.TransactionId == id);
        }
    }
}
