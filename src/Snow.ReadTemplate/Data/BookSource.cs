using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Toolkit.Collections;
using Snow.ReadTemplate.Models;

namespace Snow.ReadTemplate.Data
{
   public  class BookSource : IIncrementalSource<Book>
    {
        public Task<IEnumerable<Book>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = new CancellationToken())
        {
            return BookManager.GetBooks(pageIndex, pageSize);
        }
    }
}
