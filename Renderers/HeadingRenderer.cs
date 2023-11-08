using Markdig.Syntax;

namespace Markdig.Blazor.Renderers;

public class HeadingRenderer : BlazorObjectRenderer<HeadingBlock>
{
    private static readonly string[] Tags = new string[]
    {
        "h1",
        "h2",
        "h3",
        "h4",
        "h5",
        "h6"
    };
    
    protected override void Write(BlazorRenderer renderer, HeadingBlock obj)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        renderer.OpenElement(Tags[obj.Level - 1])
            .WriteLeafInline(obj)
            .CloseElement();
    }
}