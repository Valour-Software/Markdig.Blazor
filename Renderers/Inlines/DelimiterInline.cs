using Markdig.Syntax.Inlines;

namespace Markdig.Blazor.Renderers.Inlines;

/// <summary>
/// A Blazor renderer for a <see cref="DelimiterInline"/>.
/// </summary>
public class DelimiterInlineRenderer : BlazorObjectRenderer<DelimiterInline>
{
    /// <inheritdoc/>
    protected override void Write(BlazorRenderer renderer, DelimiterInline obj)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        renderer.WriteText(obj.ToLiteral());
        renderer.WriteChildren(obj);
    }
}