using Markdig.Syntax;

namespace Markdig.Blazor.Renderers;


public class ThematicBreakRenderer : BlazorObjectRenderer<ThematicBreakBlock>
{
    protected override void Write(BlazorRenderer renderer, ThematicBreakBlock obj)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        renderer.OpenElement("hr", 0)
            .CloseElement();
    }
}