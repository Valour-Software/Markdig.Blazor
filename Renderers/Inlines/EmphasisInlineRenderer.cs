using Markdig.Syntax.Inlines;

namespace Markdig.Blazor.Renderers.Inlines;

public class EmphasisInlineRenderer: BlazorObjectRenderer<EmphasisInline>
{
    protected override void Write(BlazorRenderer renderer, EmphasisInline obj)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        string tag = null;

        switch (obj.DelimiterChar)
        {
            case '*':
            case '_':
                tag = obj.DelimiterCount == 2 ? "strong" : "em";
                break;
            case '~':
                tag = obj.DelimiterCount == 2 ? "s" : "sub";
                break;
            case '^':
                tag = "sup";
                break;
            case '+':
                tag = "ins";
                break;
            case '=':
                tag = "mark";
                break;
        }

        if (tag is not null)
        {
            renderer.OpenElement(tag)
                .WriteChildren(obj);
            renderer.CloseElement();
        }
        else
        {
            renderer.WriteChildren(obj);
        }
    }
}