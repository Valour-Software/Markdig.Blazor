using Markdig.Blazor.Components;
using Markdig.Renderers.Html;
using Markdig.Syntax.Inlines;

namespace Markdig.Blazor.Renderers.Inlines;

/// <summary>
/// A HTML renderer for a <see cref="CodeInline"/>.
/// </summary>
/// <seealso cref="HtmlObjectRenderer{TObject}" />
public class CodeInlineRenderer : BlazorObjectRenderer<CodeInline>
{
    protected override void Write(BlazorRenderer renderer, CodeInline obj)
    {
        renderer.OpenComponent<MarkdownCodeComponent>()
            .AddAttributes(obj.TryGetAttributes())
            .AddComponentParam("AutoHighlight", renderer.EnableHljsHighlight)
            .AddComponentParam("InlineContent", obj)
            .CloseComponent();
    }
}