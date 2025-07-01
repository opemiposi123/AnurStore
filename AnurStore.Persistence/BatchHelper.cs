using AnurStore.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnurStore.Persistence
{
    public class BatchHelper
    {
       private readonly ApplicationContext _context;

        public BatchHelper(ApplicationContext context)
        {
            _context = context;

        }
        public  async Task<string> GenerateBatchNumberAsync()
        {
            var now = DateTime.Now;
            string month = now.ToString("MMM").ToUpper(); 
            string day = now.Day.ToString("D2");           
            string year = now.ToString("yy");              

            DateTime today = now.Date;
            int dailyCount = await _context.ProductPurchases
                .CountAsync(p => p.PurchaseDate.Date == today);
            int nextCount = dailyCount + 1;
            string counter = nextCount.ToString("D3");
            return $"{month}-{year}-BATCH-{day}-{counter}";
        }
    }
}
