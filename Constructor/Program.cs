using System;
using Starcounter;
using Constructor.ViewModels;

namespace Constructor
{
    class Program
    {
        static void Main()
        {
            Application.Current.Use(new HtmlFromJsonProvider());
            Application.Current.Use(new CustomPartialToStandaloneHtmlProvider());

            Handle.GET("/constructor", () =>
            {
                var page = new IndexPage();
                page.Init();
                return page;
            });

            Handle.GET("/constructor/product/{?}", (ulong no) =>
            {
                var page = new ProductPage();
                page.Init(no);
                return page;
            });
        }
    }
}