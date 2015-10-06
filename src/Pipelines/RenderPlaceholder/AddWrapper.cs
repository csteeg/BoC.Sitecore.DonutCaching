using Sitecore.Diagnostics;
using Sitecore.Mvc.ExperienceEditor.Presentation;
using Sitecore.Mvc.Pipelines.Response.RenderPlaceholder;
using Sitecore.Mvc.Presentation;

namespace BoC.Sitecore.DonutCaching.Pipelines.RenderPlaceholder
{
    public class AddWrapper : RenderPlaceholderProcessor
    {
        public override void Process(RenderPlaceholderArgs args)
        {
            Assert.ArgumentNotNull((object)args, "args");
            var marker = this.GetMarker();
            if (marker == null)
                return;
            args.Disposables.Add(new Wrapper(args.Writer, marker));
        }

        protected virtual IMarker GetMarker()
        {
            PlaceholderContext currentOrNull = PlaceholderContext.CurrentOrNull;
            if (currentOrNull == null)
                return (IMarker)null;
            return new PlaceHolderMarker(currentOrNull, PageContext.Current);
        }
    }
}
