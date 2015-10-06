using System;
using System.IO;
using Sitecore;
using Sitecore.Caching;
using Sitecore.Mvc.Pipelines;
using Sitecore.Mvc.Pipelines.Response.RenderPlaceholder;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;
using Sitecore.Mvc.Presentation;

namespace BoC.Sitecore.DonutCaching.Pipelines.RenderRendering
{
    public class RenderFromCache: global::Sitecore.Mvc.Pipelines.Response.RenderRendering.RenderFromCache
    {
        private const string placeHolderEndSearch = "<!-- /place_holder: {0} -->";
        private const string placeholderStartSearch = "<!-- placeholder: ";

        protected override bool Render(string cacheKey, TextWriter writer, RenderRenderingArgs args)
        {
            var htmlCache = Context.Site == null ? null : CacheManager.GetHtmlCache(Context.Site);
            var html = htmlCache?.GetHtml(cacheKey);
            if (html == null)
                return false;

            //it's possible to tell the component to cache placeholders too, if the rendering item has a field named "CachePlaceholders" set to "1"
            //or by setting a parameter with this name
            var hasParam = args.Rendering != null && args.Rendering.Parameters != null &&
                           args.Rendering.Parameters.Contains("CachePlaceholders");
            if ((hasParam && args.Rendering.Parameters["CachePlaceholders"] == "1") ||
                (!hasParam && args.Rendering != null && args.Rendering.Item != null && args.Rendering.Item["CachePlaceholders"] == "1"))
            {
                writer.Write(html);
            }
            else
            {
                ReplacePlaceholders(html, writer, args.Rendering);
            }
            return true;
        }

        private void ReplacePlaceholders(string html, TextWriter writer, Rendering rendering)
        {
            var index = html.IndexOf(placeholderStartSearch, StringComparison.Ordinal);
            if (index < 0)
            {
                writer.Write(html);
                return;
            }

            var endOfStartTag = html.IndexOf(" -->", index, StringComparison.Ordinal);
            var startOfKey = index + placeholderStartSearch.Length;
            var placeHolderKey = html.Substring(startOfKey, endOfStartTag - startOfKey);
            var endTag = string.Format(placeHolderEndSearch, placeHolderKey);
            var endOfPlaceHolder = html.IndexOf(endTag, endOfStartTag, StringComparison.Ordinal);
            if (endOfPlaceHolder < 0)
                throw new Exception("Could not find end of placeholder " + placeHolderKey);
            if (placeHolderKey.IndexOf("_cacheable", StringComparison.Ordinal) > placeHolderKey.LastIndexOf('/'))
                //another way to cache placeholders is to have the name contain _cacheable
            {
                writer.Write(html.Substring(0, endOfPlaceHolder + endTag.Length));
            }
            else
            {
                writer.Write(html.Substring(0, index));
                PipelineService.Get().RunPipeline<RenderPlaceholderArgs>("mvc.renderPlaceholder",
                    new RenderPlaceholderArgs(placeHolderKey, writer, rendering));
            }
            ReplacePlaceholders(html.Substring(endOfPlaceHolder + endTag.Length), writer, rendering);
        }
    }
}
