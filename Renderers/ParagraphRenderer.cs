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
        
        if (!renderer.IsFirstInContainer)
        {
            renderer.OpenElement("br", 0)
                .CloseElement();
        }
        
        renderer.OpenElement("p", 1)
            .AddAttributes(obj.TryGetAttributes())
            .WriteLeafInline(obj)
            .CloseElement();
        
        renderer.OpenElement("br", 2)
            .CloseElement();
    }
}