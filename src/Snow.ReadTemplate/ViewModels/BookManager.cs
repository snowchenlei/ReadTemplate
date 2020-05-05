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
                new ArticleViewModel {Id = 1,Title = "云日历安卓版1.15重磅更新：新增5个桌面小部件/图标菜单等", Author = "Futurum", CoverImage = "/Assets/1.png", Description="<p>[toc]</p>\n<p>#1.应用背景\n底端设备有大量网络报文（字节数组）：心跳报文，数据采集报文，告警报文上报。需要有对应的报文结构去解析这些字节流数据。</p>\n<p>#2.结构体解析\n由此，我第一点就想到了用结构体去解析。原因有以下两点：\n##2.1.结构体存在栈中\n类属于引用类型，存在堆中；结构体属于值类型，存在栈中，在一个对象的主要成员为数据且数据量不大的情况下，使用结构会带来更好的性能。\n##2.2.结构体不需要手动释放\n属于非托管资源，系统自动管理生命周期，局部方法调用完会自动释放，全局方法会一直存在。</p>\n<p>#3.封装心跳包结构体\n心跳协议报文如下：</p>\n<p><img src=\"https://img2020.cnblogs.com/blog/1606616/202003/1606616-20200331174259949-754271350.jpg\" alt=\"\" /></p>\n<p>对应结构体封装如下：</p>\n<pre><div>    [StructLayout(LayoutKind.Sequential, Pack = 1)] // 按1字节对齐\n    public struct TcpHeartPacket\n    {\n\n      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]   //结构体内定长数组\n      public byte[] head;\n\n      public byte type;\n\n      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]\n      public byte[] length;\n     \n      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]\n      public byte[] Mac;\n     \n      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 104)]\n      public byte[] data;//数据体\n     \n      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]\n      public byte[] tail;\n    }\n</div></pre>\n<p>#4.结构体静态帮助类\n主要实现了字节数组向结构体转换方法，以及结构体向字节数组的转换方法。</p>\n<pre><code>    public class StructHelper\n    {\n        //// &lt;summary&gt;\n        /// 结构体转byte数组\n        /// &lt;/summary&gt;\n        /// &lt;param name=&quot;structObj&quot;&gt;要转换的结构体&lt;/param&gt;\n        /// &lt;returns&gt;转换后的byte数组&lt;/returns&gt;\n        public static byte[] StructToBytes(Object structObj)\n        {\n            //得到结构体的大小\n            int size = Marshal.SizeOf(structObj);\n            //创建byte数组\n            byte[] bytes = new byte[size];\n            //分配结构体大小的内存空间\n            IntPtr structPtr = Marshal.AllocHGlobal(size);\n            //将结构体拷到分配好的内存空间\n            Marshal.StructureToPtr(structObj, structPtr, false);\n            //从内存空间拷到byte数组\n            Marshal.Copy(structPtr, bytes, 0, size);\n            //释放内存空间\n            Marshal.FreeHGlobal(structPtr);\n            //返回byte数组\n            return bytes;\n        }\n\n        /// &lt;summary&gt;\n        /// byte数组转结构体\n        /// &lt;/summary&gt;\n        /// &lt;param name=&quot;bytes&quot;&gt;byte数组&lt;/param&gt;\n        /// &lt;param name=&quot;type&quot;&gt;结构体类型&lt;/param&gt;\n        /// &lt;returns&gt;转换后的结构体&lt;/returns&gt;\n        public static object BytesToStuct(byte[] bytes, Type type)\n        {\n            //得到结构体的大小\n            int size = Marshal.SizeOf(type);\n            //byte数组长度小于结构体的大小\n            if (size &gt; bytes.Length)\n            {\n                //返回空\n                return null;\n            }\n            //分配结构体大小的内存空间\n            IntPtr structPtr = Marshal.AllocHGlobal(size);\n            try\n            {\n                //将byte数组拷到分配好的内存空间\n                Marshal.Copy(bytes, 0, structPtr, size);\n                //将内存空间转换为目标结构体\n                return Marshal.PtrToStructure(structPtr, type);\n            }\n            finally \n            {\n                //释放内存空间\n                Marshal.FreeHGlobal(structPtr);\n            }\n\n        }\n\n    }\n</code></pre>\n<p>#5.New出来的结构体是存在堆中还是栈中？\n有同事说new出来的都会放在堆里，我半信半疑。怎么去确定，new出来的结构体到底放在哪里有两种方式，一种是使用Visual Studio的调试工具查看，这种方法找了好久没找到怎么去查看，路过的高手烦请指点下；第二种方法就是查看反编译dll的IL（Intermediate Language）语言。查看最终是以怎样的方式去实现的。不懂IL想了解IL的可以看<a href=\"https://www.cnblogs.com/zery/p/3366175.html\">此篇</a>文章\n##5.1.不带形参的结构体构造</p>\n<ul>\n<li>调用代码</li>\n</ul>\n<pre><code>  //初始化结构体\n  TcpHeartPacket tcpHeartPacket = new TcpHeartPacket();\n  //将上报的心跳报文ReceviveBuff利用结构体静态帮助类StructHelper的BytesToStuct方法将字节流转化成结构体\n  tcpHeartPacket = (TcpHeartPacket)StructHelper.BytesToStuct(ReceviveBuff, tcpHeartPacket.GetType());\n</code></pre>\n<p><img src=\"https://img2020.cnblogs.com/blog/1606616/202003/1606616-20200331174322867-1027096871.jpg\" alt=\"\" />\n从对应的IL代码可以看出只是initobj，并没有newobj，其中newobj表示分配内存，完成对象初始化；而initobj表示对值类型的初始化。</p>\n<ul>\n<li><p>newobj用于分配和初始化对象；而initobj用于初始化值类型。因此，可以说，newobj在堆中分配内存，并完成初始化；而initobj则是对栈上已经分配好的内存，进行初始化即可，因此值类型在编译期已经在栈上分配好了内存。</p>\n</li>\n<li><p>newobj在初始化过程中会调用构造函数；而initobj不会调用构造函数，而是直接对实例置空。</p>\n</li>\n<li><p>newobj有内存分配的过程；而initobj则只完成数据初始化操作。</p>\n</li>\n</ul>\n<p>initobj 的执行结果是，将tcpHeartPacket中的引用类型初时化为null，而基元类型则置为0。\n综上，new 结构体（无参情况）是放在栈中的，只是做了null/0初始化。</p>\n<p>##5.2.带形参的结构体构造\n接下来看下带形参的结构体存放位置。\n简化版带形参的结构体如下：</p>\n<pre><code>    public struct TcpHeartPacket\n    {\n\n        public TcpHeartPacket(byte _type)\n        {\n            type = _type;\n         }\n        public byte type;\n\n    }\n</code></pre>\n<p>调用如下：</p>\n<pre><code>//带形参结构体new初始化\n  TcpHeartPacket tcpHeartPacket = new TcpHeartPacket(0x1);\n//类的new做对比\n  IWorkThread __workThread = new WorkThread();\n</code></pre>\n<p>IL代码如下：\n<img src=\"https://img2020.cnblogs.com/blog/1606616/202003/1606616-20200331174339954-306972164.jpg\" alt=\"\" /></p>\n<blockquote>\n<p>形成了鲜明的对比，new带参的结构体。IL只是去call（调用）ctor（结构体的构造函数），而下面的new类则直接就是newobj，实例化了一个对象存到堆空间去了。</p>\n</blockquote>\n<p>综合5.1,5.2表明结构体的new确实是存在栈里的，而类的new是存在堆里的。</p>\n<p>#6.性能测试\n测试结果如下：</p>\n<p><img src=\"https://img2020.cnblogs.com/blog/1606616/202003/1606616-20200331174354774-566947801.png\" alt=\"\" />\n<img src=\"https://img2020.cnblogs.com/blog/1606616/202003/1606616-20200331175428556-295266237.jpg\" alt=\"\" /></p>\n<p>使用结构体解析包需要几十个微妙，其实效率还是很差的。我用类封装成包，解析了，只需要几个微妙，性能差5到10倍。</p>\n<p>#7.原因分析</p>\n<p>主要时间消耗在了BytesToStuct方法，代码详见4</p>\n<ul>\n<li>心跳包里面用了很多byte[]字节数组，而字节数组本身需要在堆里开辟空间；</li>\n<li>该方法进行了装箱拆箱操作；</li>\n<li>分配内存在堆上，还是在堆上进行了copy操作；\n拆装箱的IL代码如下：</li>\n</ul>\n<p><img src=\"https://img2020.cnblogs.com/blog/1606616/202003/1606616-20200331174410872-781976551.jpg\" alt=\"\" /></p>\n<blockquote>\n<p>装箱使用的box指令，取消装箱是 unbox.any 指令</p>\n</blockquote>\n<p>#8.下一期:结构体与类封装的心跳包性能对比测试\n当数据比较大的时候，结构体这种数据复制机制会带来较大的开销。也难怪微软给出的准则中有一条：“当类型定义大于16字节时不要选用struct”。最终我也选择了类来封装以太网包的解析，性能可以达到微妙级，会在下一篇文章《结构体与类封装的心跳包性能对比测试》中作详细描述。</p>\n<p>#9.IL工具使用分享</p>\n<ul>\n<li>使用ildasm工具\n<a href=\"https://blog.csdn.net/jackson0714/article/details/44627161\">VS2013外部工具中添加ildasm.exe</a></li>\n<li>使用dnSpy工具\n<a href=\"https://github.com/cnxy/dnSpy/\">dnSpy的github地址</a></li>\n</ul>\n<hr>\n版权声明：本文为博主原创文章，遵循 CC 4.0 BY-SA 版权协议，转载请附上原文出处链接和本声明。 \n<p>本文链接：https://www.cnblogs.com/JerryMouseLi/p/12606920.html</p>\n<img src=\"http://counter.cnblogs.com//blog/post/12606920\" width=\"1\" height=\"1\" style=\"border:0px;visibility:hidden\"/>"},
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
        public static async Task<IEnumerable<ArticleViewModel>> GetBooks(int pageIndex, int pageSize, string query = "")
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
