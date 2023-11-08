using Markdig.Blazor.Components;
using Markdig.Helpers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace Markdig.Blazor.Renderers;

/// <summary>
/// An HTML renderer for a <see cref="CodeBlock"/> and <see cref="FencedCodeBlock"/>.
/// </summary>
/// <seealso cref="HtmlObjectRenderer{TObject}" />
public class CodeBlockRenderer : BlazorObjectRenderer<CodeBlock>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CodeBlockRenderer"/> class.
    /// </summary>
    public CodeBlockRenderer() { }
    
    protected override void Write(BlazorRenderer renderer, CodeBlock obj)
    {
        renderer.OpenComponent<MarkdownCodeComponent>()
            .AddAttributes(obj.TryGetAttributes())
            .AddComponentParam("AutoHighlight", renderer.EnableHljsHighlight)
            .AddComponentParam("Content", obj)
            .CloseComponent();
    }
}