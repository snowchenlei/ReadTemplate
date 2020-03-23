using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snow.ReadTemplate.Common;

namespace Snow.ReadTemplate.ViewModels
{
    public class MainViewModel : BindableBase
    {
        /// <summary>
        /// Gets or sets the article that the user is currently viewing. 
        /// </summary>
        public ArticleViewModel CurrentArticle
        {
            get { return _currentArticle; }
            set
            {
                // CurrentArticle is a special case, so it doesn't use SetProperty 
                // to update the backing field, raising the PropertyChanged event
                // only when the field value changes. Instead, CurrentArticle raises
                // PropertyChanged every time the setter is called. This ensures
                // that the ListView selection is updated when changing feeds, even 
                // if the first article is the same in both feeds. It also ensures
                // that clicking an article in the narrow view will always navigate
                // to the details view, even if the article is already the current one.
                _currentArticle = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentArticleAsObject));
            }
        }
        private ArticleViewModel _currentArticle;

        /// <summary>
        /// Gets the current article as an instance of type Object. 
        /// </summary>
        public object CurrentArticleAsObject => CurrentArticle as object;

    }
}
