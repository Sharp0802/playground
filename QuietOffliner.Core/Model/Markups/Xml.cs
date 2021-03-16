using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QuietOffliner.Core.Model.Markups
{
    public class Xml
    {
        public static Task<Xml> New(string? xml = null)
        {
            return Task.FromResult(xml is null ? new Xml() : new Xml(xml));
        }
        
        private Xml()
            => Docs = new XDocument();
        
        private Xml(string xml)
            => Docs = XDocument.Parse(xml, LoadOptions.PreserveWhitespace);

        private XDocument Docs { get; }

        public Task Save(Stream stream)
        {
            Docs.Save(stream, SaveOptions.None);
            return Task.CompletedTask;
        }
    }
}