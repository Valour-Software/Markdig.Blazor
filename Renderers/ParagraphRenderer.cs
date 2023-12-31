using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace Markdig.Blazor.Renderers;

public class ParagraphRenderer : BlazorObjectRenderer<ParagraphBlock>
{
    /// <inheritdoc/>
    protected override void Write(BlazorRenderer renderer, ParagraphBlock obj)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));
        if (obj == null) throw new ArgumentNullException(nameof(obj));
        
        renderer.OpenElement("p", 0)
            .AddAttributes(obj.TryGetAttributes(), 1)
            .WriteLeafInline(obj)
            .CloseElement();
    }
}