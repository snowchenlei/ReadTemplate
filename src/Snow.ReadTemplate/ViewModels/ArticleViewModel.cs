using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snow.ReadTemplate.Common;

namespace Snow.ReadTemplate.ViewModels
{
    public class ArticleViewModel : BindableBase
    {
        public int Id { get; set; }
        public string CreationTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        public string Time { get; set; } = DateTime.Now.ToShortTimeString();
        public string Title { get; set; }
        public string Author { get; set; }
        public string CoverImage { get; set; }
    }
}
