using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Diagnostics;
using Sitecore.Mvc.ExperienceEditor.Extensions;
using Sitecore.Mvc.ExperienceEditor.Presentation;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls;

namespace BoC.Sitecore.DonutCaching.Pipelines.RenderPlaceholder
{
    public class PlaceHolderMarker: IMarker
    {
        private readonly PlaceholderContext _placeholderContext;
        private readonly PageContext _pageContext;

        public PlaceHolderMarker(PlaceholderContext placeholderContext, PageContext pageContext)
        {
            _placeholderContext = placeholderContext;
            _pageContext = pageContext;
            Assert.IsNotNull((object)placeholderContext, "placeholderContext");
            Assert.ArgumentNotNull((object)pageContext, "pageContext");
        }
        public string GetStart()
        {
            return string.Format("<!-- placeholder: {0} -->", GetPlaceholderKey(_placeholderContext));
        }

        public string GetEnd()
        {
            return string.Format("<!-- /place_holder: {0} -->", GetPlaceholderKey(_placeholderContext));
        }

        static string GetPlaceholderKey(PlaceholderContext placeholderContext)
        {
            Assert.ArgumentNotNull((object)placeholderContext, "placeholderContext");
            string placeholderPath = placeholderContext.PlaceholderPath;
            if (placeholderPath.LastIndexOf("/") > 0)
                return placeholderPath;
            return placeholderPath.Replace("/", string.Empty);
        }

    }
}
