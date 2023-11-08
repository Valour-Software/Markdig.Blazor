using Markdig.Syntax.Inlines;

namespace Markdig.Blazor.Renderers.Inlines;

/// <summary>
/// A Blazor renderer for a <see cref="LineBreakInline"/>.
/// </summary>
public class LineBreakInlineRenderer : BlazorObjectRenderer<LineBreakInline>
{
    /// <inheritdoc/>
    protected override void Write(BlazorRenderer renderer, LineBreakInline obj)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        if (obj.IsHard)
        {
            renderer.OpenElement("br", 0)
                .CloseElement();
            renderer.OpenElement("br", 0)
                .CloseElement();
        }
        else
        {
            // Soft line break.
            renderer.OpenElement("br", 0)
                .CloseElement();
        }
    }
}