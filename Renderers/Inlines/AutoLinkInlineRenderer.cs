using Markdig.Syntax.Inlines;

namespace Markdig.Blazor.Renderers.Inlines;

/// <summary>
/// A Blazor renderer for a <see cref="AutolinkInline"/>.
/// </summary>
/// <seealso cref="AutolinkInline" />
public class AutolinkInlineRenderer : BlazorObjectRenderer<AutolinkInline>
{
    /// <inheritdoc/>
    protected override void Write(BlazorRenderer renderer, AutolinkInline link)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));
        if (link == null) throw new ArgumentNullException(nameof(link));

        var url = link.Url;
        if (link.IsEmail)
        {
            url = "mailto:" + url;
        }

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