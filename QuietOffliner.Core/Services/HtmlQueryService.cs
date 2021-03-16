using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using QuietOffliner.Core.Model.Markups;

namespace QuietOffliner.Core.Services
{
    public static class HtmlQueryService
    {
        public static Task<string?> MetaData(this Html html, string name)
        {
            var metadata = html.Docs.QuerySelector($"meta[name=\"{name}'\"]");
            return Task.FromResult(metadata?.GetAttribute("content"));
        }

        public static Task<IElement> FindById(this Html html, string id)
            => Task.FromResult((html.Docs ?? throw new NullReferenceException()).QuerySelector($"#{id}"));
        
        public static Task<IElement> FindByClass(this Html html, string @class)
            => Task.FromResult((html.Docs ?? throw new NullReferenceException()).QuerySelector($".{@class}"));
        
        public static Task<IElement> FindBySelector(this Html html, string selector)
            => Task.FromResult((html.Docs ?? throw new NullReferenceException()).QuerySelector($"{selector}"));
        
        public static Task<IEnumerable<IElement>> FindAllById(this Html html, string id)
            => Task.FromResult((html.Docs ?? throw new NullReferenceException()).QuerySelectorAll($"#{id}").AsEnumerable());
        
        public static Task<IEnumerable<IElement>> FindAllByClass(this Html html, string @class)
            => Task.FromResult((html.Docs ?? throw new NullReferenceException()).QuerySelectorAll($".{@class}").AsEnumerable());
        
        public static Task<IEnumerable<IElement>> FindAllBySelector(this Html html, string selector)
            => Task.FromResult((html.Docs ?? throw new NullReferenceException()).QuerySelectorAll($"{selector}").AsEnumerable());
        
        

        public static Task<IElement> FindById(this IElement elem, string id)
            => Task.FromResult(elem.QuerySelector($"#{id}"));
        
        public static Task<IElement> FindByClass(this IElement elem, string @class)
            => Task.FromResult(elem.QuerySelector($".{@class}"));
        
        public static Task<IElement> FindBySelector(this IElement elem, string selector)
            => Task.FromResult(elem.QuerySelector($"{selector}"));
        
        public static Task<IEnumerable<IElement>> FindAllById(this IElement elem, string id)
            => Task.FromResult(elem.QuerySelectorAll($"#{id}").AsEnumerable());
        
        public static Task<IEnumerable<IElement>> FindAllByClass(this IElement elem, string @class)
            => Task.FromResult(elem.QuerySelectorAll($".{@class}").AsEnumerable());
        
        public static Task<IEnumerable<IElement>> FindAllBySelector(this IElement elem, string selector)
            => Task.FromResult(elem.QuerySelectorAll($"{selector}").AsEnumerable());
    }
}