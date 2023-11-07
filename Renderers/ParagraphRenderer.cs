using Markdig.Syntax;

namespace Markdig.Blazor.Renderers;

public class ParagraphRenderer : BlazorObjectRenderer<ParagraphBlock>
{
    /// <inheritdoc/>
    protected override void Write(BlazorRenderer renderer, ParagraphBlock obj)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        renderer.OpenElement("p")
            .WriteLeafInline(obj)
            .CloseElement();
    }
}