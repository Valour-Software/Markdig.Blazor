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
    private HashSet<string>? _blocksAsDiv;

    /// <summary>
    /// Initializes a new instance of the <see cref="CodeBlockRenderer"/> class.
    /// </summary>
    public CodeBlockRenderer() { }

    public class CodeBlockComponent : ComponentBase
    {
        [Inject]
        public IJSRuntime JsRuntime { get; set; }
        
        [Parameter]
        public CodeBlock Content { get; set; }
        
        [Parameter]
        public bool AutoHighlight { get; set; }

        private ElementReference _codeBlock;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "pre");
            builder.OpenElement(1, "code");
            builder.AddElementReferenceCapture(2, el =>
            {
                _codeBlock = el;
            });
            
            WriteLeafRawLines(builder, Content);
            
            builder.CloseElement();
            builder.CloseElement();
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
    
    protected override void Write(BlazorRenderer renderer, CodeBlock obj)
    {
        if (_blocksAsDiv is not null && (obj as FencedCodeBlock)?.Info is string info && _blocksAsDiv.Contains(info))
        {
            //var infoPrefix = (obj.Parser as FencedCodeBlockParser)?.InfoPrefix ??
            //                 FencedCodeBlockParser.DefaultInfoPrefix;

            renderer.OpenElement("div")
                .AddAttributes(obj.TryGetAttributes())
                .AddComponentParam("Content", obj)
                // .WriteLeafRawLines(obj, true, true, true)
                .CloseElement();
        }
        else
        {
            renderer.OpenComponent<CodeBlockComponent>()
                .AddComponentParam("AutoHighlight", renderer.EnableHljsHighlight)
                .AddComponentParam("Content", obj)
                .CloseComponent();
        }
    }
}