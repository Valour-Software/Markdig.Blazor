using System.Net.Mime;
using Markdig.Syntax.Inlines;

namespace Markdig.Blazor.Renderers.Inlines;

    /// <summary>
    /// A Blazor renderer for a <see cref="LinkInline"/>.
    /// </summary>
    public class LinkInlineRenderer : BlazorObjectRenderer<LinkInline>
    {
        /// <inheritdoc/>
        protected override void Write(BlazorRenderer renderer, LinkInline link)
        {
            if (renderer == null) throw new ArgumentNullException(nameof(renderer));
            if (link == null) throw new ArgumentNullException(nameof(link));

            var url = link.GetDynamicUrl != null ? link.GetDynamicUrl() ?? link.Url : link.Url;

            if (!Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                url = "#";
            }
            
            renderer.OpenElement("a", 0)
                .AddAttribute("href", url, 1)
                .AddAttribute("rel", "noopener noreferrer nofollow", 2)
                .AddAttribute("target", "_blank", 3)
                .WriteText(url, 4)
                .CloseElement();
        }
    }