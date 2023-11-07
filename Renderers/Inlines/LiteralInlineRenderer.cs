using Markdig.Syntax.Inlines;

namespace Markdig.Blazor.Renderers.Inlines;

/// <summary>
/// A Blazor renderer for a <see cref="LiteralInline"/>.
/// </summary>
public class LiteralInlineRenderer : BlazorObjectRenderer<LiteralInline>
{
    /// <inheritdoc/>
    protected override void Write(BlazorRenderer renderer, LiteralInline obj)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        if (obj.Content.IsEmpty)
            return;

        renderer.WriteText(ref obj.Content);
    }
}