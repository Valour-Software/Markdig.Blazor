using System.Collections.Specialized;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace Markdig.Blazor.Renderers;

public class QuoteBlockRenderer : BlazorObjectRenderer<QuoteBlock>
{
    /// <inheritdoc/>
    protected override void Write(BlazorRenderer renderer, QuoteBlock obj)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        var r = renderer.OpenElement("blockquote", 0)
            .AddAttributes(obj.TryGetAttributes(), 1);
        
        r.WriteChildren(obj);
        r.CloseElement();
    }
}