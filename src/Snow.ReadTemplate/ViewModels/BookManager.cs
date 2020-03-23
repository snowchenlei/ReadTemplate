using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Snow.ReadTemplate.ViewModels;

namespace Snow.ReadTemplate.Models
{
    public class BookManager
    {
        private static List<ArticleViewModel> _books;

        static BookManager()
        {
            _books = new List<ArticleViewModel>
            {
                new ArticleViewModel {Id = 1,Title = "云日历安卓版1.15重磅更新：新增5个桌面小部件/图标菜单等", Author = "Futurum", CoverImage = "/Assets/1.png"},
                new ArticleViewModel {Id = 2,Title = "Mazim", Author = "Sequiter Que", CoverImage = "/Assets/2.png"},
                new ArticleViewModel {Id = 3,Title = "Elit", Author = "Tempor", CoverImage = "/Assets/3.png"},
                new ArticleViewModel {Id = 4,Title = "Etiam", Author = "Option", CoverImage = "/Assets/4.png"},
                new ArticleViewModel{ Id = 5,Title = "Feugait Eros Libex", Author = "Accumsan", CoverImage = "/Assets/5.png"},
                new ArticleViewModel{ Id = 6,Title = "Nonummy Erat", Author = "Legunt Xaepius", CoverImage = "/Assets/6.png"},
                new ArticleViewModel {Id = 7,Title = "Nostrud", Author = "Eleifend", CoverImage = "/Assets/7.png"},
                new ArticleViewModel {Id = 8,Title = "Per Modo", Author = "Vero Tation", CoverImage = "/Assets/8.png"},
                new ArticleViewModel {Id = 9,Title = "Suscipit Ad", Author = "Jack Tibbles", CoverImage = "/Assets/9.png"},
                new ArticleViewModel {Id = 10, Title = "Decima", Author = "Tuffy Tibbles", CoverImage = "/Assets/10.png"},
                new ArticleViewModel {Id = 11, Title = "Erat", Author = "Volupat", CoverImage = "/Assets/11.png"},
                new ArticleViewModel {Id = 12, Title = "Consequat", Author = "Est Possim", CoverImage = "/Assets/12.png"},
                new ArticleViewModel {Id = 13, Title = "Aliquip", Author = "Magna", CoverImage = "/Assets/13.png"},
                new ArticleViewModel {Id = 14, Title = "Vulpate", Author = "Futurum", CoverImage = "/Assets/1.png"},
                new ArticleViewModel {Id = 15, Title = "Mazim", Author = "Sequiter Que", CoverImage = "/Assets/2.png"},
                new ArticleViewModel {Id = 16, Title = "Elit", Author = "Tempor", CoverImage = "/Assets/3.png"},
                new ArticleViewModel {Id = 17, Title = "Etiam", Author = "Option", CoverImage = "/Assets/4.png"},
                new ArticleViewModel {Id = 18, Title = "Feugait Eros Libex", Author = "Accumsan", CoverImage = "/Assets/5.png"},
                new ArticleViewModel {Id = 19, Title = "Nonummy Erat", Author = "Legunt Xaepius", CoverImage = "/Assets/6.png"},
                new ArticleViewModel {Id = 20, Title = "Nostrud", Author = "Eleifend", CoverImage = "/Assets/7.png"},
                new ArticleViewModel {Id = 21, Title = "Per Modo", Author = "Vero Tation", CoverImage = "/Assets/8.png"},
                new ArticleViewModel {Id = 22, Title = "Suscipit Ad", Author = "Jack Tibbles", CoverImage = "/Assets/9.png"},
                new ArticleViewModel {Id = 23, Title = "Decima", Author = "Tuffy Tibbles", CoverImage = "/Assets/10.png"},
                new ArticleViewModel {Id = 24, Title = "Erat", Author = "Volupat", CoverImage = "/Assets/11.png"},
                new ArticleViewModel {Id = 25, Title = "Consequat", Author = "Est Possim", CoverImage = "/Assets/12.png"},
                new ArticleViewModel {Id = 26, Title = "Aliquip", Author = "Magna", CoverImage = "/Assets/13.png"},
                new ArticleViewModel {Id = 7, Title = "Nostrud", Author = "Eleifend", CoverImage = "/Assets/7.png"},
                new ArticleViewModel {Id = 8, Title = "Per Modo", Author = "Vero Tation", CoverImage = "/Assets/8.png"},
                new ArticleViewModel {Id = 9, Title = "Suscipit Ad", Author = "Jack Tibbles", CoverImage = "/Assets/9.png"},
                new ArticleViewModel {Id = 10, Title = "Decima", Author = "Tuffy Tibbles", CoverImage = "/Assets/10.png"},
                new ArticleViewModel {Id = 11, Title = "Erat", Author = "Volupat", CoverImage = "/Assets/11.png"},
                new ArticleViewModel {Id = 12, Title = "Consequat", Author = "Est Possim", CoverImage = "/Assets/12.png"},
                new ArticleViewModel {Id = 13, Title = "Aliquip", Author = "Magna", CoverImage = "/Assets/13.png"},
                new ArticleViewModel {Id = 14, Title = "Vulpate", Author = "Futurum", CoverImage = "/Assets/1.png"},
                new ArticleViewModel {Id = 15, Title = "Mazim", Author = "Sequiter Que", CoverImage = "/Assets/2.png"},
                new ArticleViewModel {Id = 16, Title = "Elit", Author = "Tempor", CoverImage = "/Assets/3.png"},
                new ArticleViewModel {Id = 17, Title = "Etiam", Author = "Option", CoverImage = "/Assets/4.png"},
                new ArticleViewModel {Id = 18, Title = "Feugait Eros Libex", Author = "Accumsan", CoverImage = "/Assets/5.png"},
                new ArticleViewModel {Id = 19, Title = "Nonummy Erat", Author = "Legunt Xaepius", CoverImage = "/Assets/6.png"},
                new ArticleViewModel {Id = 20, Title = "Nostrud", Author = "Eleifend", CoverImage = "/Assets/7.png"},
                new ArticleViewModel {Id = 21, Title = "Per Modo", Author = "Vero Tation", CoverImage = "/Assets/8.png"},
                new ArticleViewModel {Id = 22, Title = "Suscipit Ad", Author = "Jack Tibbles", CoverImage = "/Assets/9.png"},
                new ArticleViewModel {Id = 23, Title = "Decima", Author = "Tuffy Tibbles", CoverImage = "/Assets/10.png"},
                new ArticleViewModel {Id = 24, Title = "Erat", Author = "Volupat", CoverImage = "/Assets/11.png"},
                new ArticleViewModel {Id = 25, Title = "Consequat", Author = "Est Possim", CoverImage = "/Assets/12.png"},
                new ArticleViewModel {Id = 26, Title = "Aliquip", Author = "Magna", CoverImage = "/Assets/13.png"},
                new ArticleViewModel {Id = 1, Title = "Vulpate", Author = "Futurum", CoverImage = "/Assets/1.png"},
                new ArticleViewModel {Id = 2, Title = "Mazim", Author = "Sequiter Que", CoverImage = "/Assets/2.png"},
                new ArticleViewModel {Id = 3, Title = "Elit", Author = "Tempor", CoverImage = "/Assets/3.png"},
                new ArticleViewModel {Id = 4, Title = "Etiam", Author = "Option", CoverImage = "/Assets/4.png"},
                new ArticleViewModel {Id = 5, Title = "Feugait Eros Libex", Author = "Accumsan", CoverImage = "/Assets/5.png"},
                new ArticleViewModel {Id = 6, Title = "Nonummy Erat", Author = "Legunt Xaepius", CoverImage = "/Assets/6.png"},
                new ArticleViewModel {Id = 7, Title = "Nostrud", Author = "Eleifend", CoverImage = "/Assets/7.png"},
                new ArticleViewModel {Id = 8, Title = "Per Modo", Author = "Vero Tation", CoverImage = "/Assets/8.png"},
                new ArticleViewModel {Id = 9, Title = "Suscipit Ad", Author = "Jack Tibbles", CoverImage = "/Assets/9.png"},
                new ArticleViewModel {Id = 10, Title = "Decima", Author = "Tuffy Tibbles", CoverImage = "/Assets/10.png"},
                new ArticleViewModel {Id = 11, Title = "Erat", Author = "Volupat", CoverImage = "/Assets/11.png"},
                new ArticleViewModel {Id = 12, Title = "Consequat", Author = "Est Possim", CoverImage = "/Assets/12.png"},
                new ArticleViewModel {Id = 13, Title = "Aliquip", Author = "Magna", CoverImage = "/Assets/13.png"},
                new ArticleViewModel {Id = 14, Title = "Vulpate", Author = "Futurum", CoverImage = "/Assets/1.png"},
                new ArticleViewModel {Id = 15, Title = "Mazim", Author = "Sequiter Que", CoverImage = "/Assets/2.png"},
                new ArticleViewModel {Id = 16, Title = "Elit", Author = "Tempor", CoverImage = "/Assets/3.png"},
                new ArticleViewModel {Id = 17, Title = "Etiam", Author = "Option", CoverImage = "/Assets/4.png"},
                new ArticleViewModel {Id = 18, Title = "Feugait Eros Libex", Author = "Accumsan", CoverImage = "/Assets/5.png"},
                new ArticleViewModel {Id = 19, Title = "Nonummy Erat", Author = "Legunt Xaepius", CoverImage = "Assets/6.png"},
                new ArticleViewModel {Id = 20, Title = "Nostrud", Author = "Eleifend", CoverImage = "Assets/7.png"},
                new ArticleViewModel {Id = 21, Title = "Per Modo", Author = "Vero Tation", CoverImage = "Assets/8.png"},
                new ArticleViewModel {Id = 22, Title = "Suscipit Ad", Author = "Jack Tibbles", CoverImage = "Assets/9.png"},
                new ArticleViewModel {Id = 23, Title = "Decima", Author = "Tuffy Tibbles", CoverImage = "Assets/10.png"},
                new ArticleViewModel {Id = 24, Title = "Erat", Author = "Volupat", CoverImage = "Assets/11.png"},
                new ArticleViewModel {Id = 25, Title = "Consequat", Author = "Est Possim", CoverImage = "Assets/12.png"},
                new ArticleViewModel {Id = 26, Title = "Aliquip", Author = "Magna", CoverImage = "Assets/13.png"},
                new ArticleViewModel {Id = 7, Title = "Nostrud", Author = "Eleifend", CoverImage = "Assets/7.png"},
                new ArticleViewModel {Id = 8, Title = "Per Modo", Author = "Vero Tation", CoverImage = "Assets/8.png"},
                new ArticleViewModel {Id = 9, Title = "Suscipit Ad", Author = "Jack Tibbles", CoverImage = "Assets/9.png"},
                new ArticleViewModel {Id = 10, Title = "Decima", Author = "Tuffy Tibbles", CoverImage = "Assets/10.png"},
                new ArticleViewModel {Id = 11, Title = "Erat", Author = "Volupat", CoverImage = "Assets/11.png"},
                new ArticleViewModel {Id = 12, Title = "Consequat", Author = "Est Possim", CoverImage = "Assets/12.png"},
                new ArticleViewModel {Id = 13, Title = "Aliquip", Author = "Magna", CoverImage = "Assets/13.png"},
                new ArticleViewModel {Id = 14, Title = "Vulpate", Author = "Futurum", CoverImage = "Assets/1.png"},
                new ArticleViewModel {Id = 15, Title = "Mazim", Author = "Sequiter Que", CoverImage = "Assets/2.png"},
                new ArticleViewModel {Id = 16, Title = "Elit", Author = "Tempor", CoverImage = "Assets/3.png"},
                new ArticleViewModel {Id = 17, Title = "Etiam", Author = "Option", CoverImage = "Assets/4.png"},
                new ArticleViewModel {Id = 18, Title = "Feugait Eros Libex", Author = "Accumsan", CoverImage = "Assets/5.png"},
                new ArticleViewModel {Id = 19, Title = "Nonummy Erat", Author = "Legunt Xaepius", CoverImage = "Assets/6.png"},
                new ArticleViewModel {Id = 20, Title = "Nostrud", Author = "Eleifend", CoverImage = "Assets/7.png"},
                new ArticleViewModel {Id = 21, Title = "Per Modo", Author = "Vero Tation", CoverImage = "Assets/8.png"},
                new ArticleViewModel {Id = 22, Title = "Suscipit Ad", Author = "Jack Tibbles", CoverImage = "Assets/9.png"},
                new ArticleViewModel {Id = 23, Title = "Decima", Author = "Tuffy Tibbles", CoverImage = "Assets/10.png"},
                new ArticleViewModel {Id = 24, Title = "Erat", Author = "Volupat", CoverImage = "Assets/11.png"},
                new ArticleViewModel {Id = 25, Title = "Consequat", Author = "Est Possim", CoverImage = "Assets/12.png"},
                new ArticleViewModel {Id = 26, Title = "Aliquip", Author = "Magna", CoverImage = "Assets/13.png"}
            };
        }
        public static async Task<IEnumerable<ArticleViewModel>> GetBooks(int pageIndex, int pageSize)
        {
            await Task.Delay(1000);
            int offset = pageIndex * pageSize;
            var books =  _books.Skip(offset).Take(pageSize);
            return await Task.FromResult(books);
        }

        public static async Task<ArticleViewModel> GetBook(int id)
        {
            await Task.Delay(1000);

            var book = _books.Find(a => a.Id == id);
            return await Task.FromResult(book);
        }
    }
}
