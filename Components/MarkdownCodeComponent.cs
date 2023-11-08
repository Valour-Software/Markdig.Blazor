using Markdig.Helpers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace Markdig.Blazor.Components;

public class MarkdownCodeComponent : ComponentBase
{
    [Inject]
    public IJSRuntime JsRuntime { get; set; }
        
    [Parameter]
    public CodeBlock Content { get; set; }
    
    [Parameter]
    public CodeInline InlineContent { get; set; }
        
    [Parameter]
    public bool AutoHighlight { get; set; }

    private ElementReference _codeBlock;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (InlineContent is null)
        {
            builder.OpenElement(0, "pre");
            builder.OpenElement(1, "code");
            builder.AddElementReferenceCapture(2, el => { _codeBlock = el; });
            WriteLeafRawLines(builder, Content);
            builder.CloseElement();
            builder.CloseElement();
        }
        else
        {
            builder.OpenElement(0, "span");
            builder.OpenElement(1, "code");
            builder.AddElementReferenceCapture(2, el => { _codeBlock = el; });
            builder.AddContent(3, InlineContent.ContentSpan.ToString());
            builder.CloseElement();
            builder.CloseElement();
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (AutoHighlight)
        {
            await JsRuntime.InvokeVoidAsync("hljsHighlight", _codeBlock);
        }
    }

    public void WriteLeafRawLines(RenderTreeBuilder builder, LeafBlock leafBlock)
    {
        if (leafBlock is null) throw new ArgumentNullException(nameof(leafBlock));

        string content = "";
            
        var slices = leafBlock.Lines.Lines;
        if (slices is not null)
        {
            for (int i = 0; i < slices.Length; i++)
            {
                ref StringSlice slice = ref slices[i].Slice;
                if (slice.Text is null)
                {
                    break;
                }
                    
                ReadOnlySpan<char> span = slice.AsSpan().TrimEnd();
                content += span.ToString();
                content += "\n";
            }
        }
            
        builder.AddContent(3, content);
    }
}