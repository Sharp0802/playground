using System;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace QuietOffliner.Core.Model.Markups
{
    public record Html : IDisposable
    {
        private bool Disposed { get; set; }

        public IDocument Docs { get; }

        private Html(IDocument docs)
        {
            Docs = docs;
        }

        public static async Task<Html> New(string html)
        {
            IHtmlParser parser = new HtmlParser();
            
            return new Html(await parser.ParseDocumentAsync(html));
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                Docs.Dispose();
            }

            Disposed = true;

            GC.SuppressFinalize(this);
        }
    }
}