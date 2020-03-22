using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Toolkit.Collections;
using Snow.NewsTemplate.Models;

namespace Snow.NewsTemplate.Data
{
   public  class BookSource : IIncrementalSource<Book>
    {
        public Task<IEnumerable<Book>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = new CancellationToken())
        {
            return BookManager.GetBooks(pageIndex, pageSize);
        }
    }
}
