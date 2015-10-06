# BoC.Sitecore.DonutCaching
When you are using components with nested placeholders (beit normal placeholders or dynamic placeholders), most times you don’t want to cache the output of those placeholders. You want each component in that placeholder to decide for itself if it is cacheable. That’s why I created a nuget package that disables the caching of the nested placeholders, but caches the rest of the component’s output, aka “Donut caching”

You can simply install the package BoC.Sitecore.DonutCaching and you’re good to go. All nested placeholders won’t be cached by default once you have this package installed.

If you DO want a nested placeholder to be cached, you can either add a parameter to your specific rendering, or add an extra checkbox field to your Rendering Item. The parameter or field should be named “CachePlaceholders” and it’s value should be set to 1 to enable it.

The source code can be downloaded at Github. As you can see there, it works by adding a wrapper for each placeholder, so we can mark it’s start and end point in the html-ouput. This is done by surrounding the placeholder with a comment tag. When a component is rendered from cache, it’s handled by our custom RenderFromCache pipeline step. This step looks for comments containing the correct syntax, and replaces everything between those comment blocks with the output rendered by the RenderPlaceholder pipeline.

This module is currently only available for MVC, but since the code is on Github, feel free to extend it to support Webforms components and placeholders!
