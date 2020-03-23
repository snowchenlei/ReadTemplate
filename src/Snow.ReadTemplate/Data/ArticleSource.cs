using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Toolkit.Collections;
using Snow.ReadTemplate.Models;
using Snow.ReadTemplate.ViewModels;

namespace Snow.ReadTemplate.Data
{
   public  class ArticleSource : IIncrementalSource<ArticleViewModel>
    {
        public Task<IEnumerable<ArticleViewModel>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = new CancellationToken())
        {
            return BookManager.GetBooks(pageIndex, pageSize);
        }
    }
}
