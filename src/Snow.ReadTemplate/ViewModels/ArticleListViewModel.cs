using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using Snow.ReadTemplate.Common;
using Snow.ReadTemplate.Models;

namespace Snow.ReadTemplate.ViewModels
{
    public class ArticleListViewModel : BindableBase
    {
        public ArticleListViewModel()
        {
            _pageIndex = 1;
            _pageSize = 10;
        }
        private bool _isLoading;

        /// <summary>
        /// Gets or sets a value that specifies whether orders are being loaded.
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        private int _pageIndex;

        public int PageIndex
        {
            get => _pageIndex;
            set => Set(ref _pageIndex, value);
        }

        private int _pageSize;

        public int PageSize
        {
            get => _pageSize;
            set => Set(ref _pageSize, value);
        }

        /// <summary>
        /// Gets the orders to display.
        /// </summary>
        public ObservableCollection<ArticleViewModel> Articles { get; private set; } = new ObservableCollection<ArticleViewModel>();


        public async void QueryArticles(string query)
        {
            IsLoading = true;
            Articles.Clear();
            if (!string.IsNullOrEmpty(query))
            {
                var results = await BookManager.GetBooks(PageIndex, PageSize, query);
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    foreach (ArticleViewModel o in results)
                    {
                        Articles.Add(o);
                    }
                    IsLoading = false;
                });
            }
        }
    }
}
