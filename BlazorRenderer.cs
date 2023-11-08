using System.Runtime.CompilerServices;
using System.Text.Json;
using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Markdig.Blazor;

public class BlazorRenderer : RendererBase
{
    private RenderTreeBuilder _builder;
    private char[] buffer;

    public bool EnableHljsHighlight { get; set; }
    
    public BlazorRenderer(RenderTreeBuilder builder, bool enableHljsHighlight = true)
    {
        _builder = builder;
        EnableHljsHighlight = enableHljsHighlight;
        buffer = new char[1024];
        
        ObjectRenderers.Add(new Markdig.Blazor.Renderers.ParagraphRenderer());
        ObjectRenderers.Add(new Markdig.Blazor.Renderers.CodeBlockRenderer());
        ObjectRenderers.Add(new Markdig.Blazor.Renderers.HeadingRenderer());
        ObjectRenderers.Add(new Markdig.Blazor.Renderers.QuoteBlockRenderer());
        ObjectRenderers.Add(new Markdig.Blazor.Renderers.ThematicBreakRenderer());
        ObjectRenderers.Add(new Markdig.Blazor.Renderers.ListRenderer());
        
        ObjectRenderers.Add(new Markdig.Blazor.Renderers.Inlines.LinkInlineRenderer());
        ObjectRenderers.Add(new Markdig.Blazor.Renderers.Inlines.AutolinkInlineRenderer());
        ObjectRenderers.Add(new Markdig.Blazor.Renderers.Inlines.LiteralInlineRenderer());
        ObjectRenderers.Add(new Markdig.Blazor.Renderers.Inlines.EmphasisInlineRenderer());
        ObjectRenderers.Add(new Markdig.Blazor.Renderers.Inlines.CodeInlineRenderer());
        ObjectRenderers.Add(new Markdig.Blazor.Renderers.Inlines.DelimiterInlineRenderer());
        ObjectRenderers.Add(new Markdig.Blazor.Renderers.Inlines.LineBreakInlineRenderer());
    }
    
    public void SetBuilder(RenderTreeBuilder builder)
    {
        _builder = builder;
    }

    public override object Render(MarkdownObject markdownObject)
    {
        // Console.WriteLine("Render called...");
        Write(markdownObject);
        return _builder;
    }

    public BlazorRenderer AddMarkupContent(string content, int seq = 0)
    {
        _builder.AddMarkupContent(seq, content);
        return this;
    }
    
    public BlazorRenderer OpenElement(string tag, int seq = 0)
    {
        // Console.WriteLine("Opening element: " + tag);
        _builder.OpenElement(seq, tag);
        return this;
    }

    public BlazorRenderer CloseElement()
    {
        _builder.CloseElement();
        return this;
    }
    
    public BlazorRenderer OpenComponent<T>() where T : ComponentBase
    {
        _builder.OpenComponent<T>(0);
        return this;
    }

    public BlazorRenderer CloseComponent()
    {
        _builder.CloseComponent();
        return this;
    }

    public BlazorRenderer AddAttribute(string name, string content, int seq = 0)
    {
        _builder.AddAttribute(seq, name, content);
        return this;
    }

    public BlazorRenderer AddAttributes(HtmlAttributes attributes, int seq = 0)
    {
        if (attributes is null)
            return this;

        if (attributes.Id is not null)
            _builder.AddAttribute(0, "id", attributes.Id);

        if (attributes.Classes is not null && attributes.Classes.Count > 0)
        {
            var classes = "";
            for (int i = 0; i < attributes.Classes.Count; i++)
            {
                if (i > 0)
                    classes += " ";

                classes += attributes.Classes[i];
            }

            _builder.AddAttribute(seq, "class", classes);
            seq++;
        }

        if (attributes.Properties is not null && attributes.Properties.Count > 0)
        {
            foreach (var property in attributes.Properties)
            {
                _builder.AddAttribute(seq, property.Key, property.Value);
                seq++;
            }
        }

        return this;
    }

    public BlazorRenderer AddComponentParam(string name, object value)
    {
        _builder.AddComponentParameter(0, name, value);
        return this;
    }

    public BlazorRenderer WriteLeafInline(LeafBlock leafBlock)
    {
        if (leafBlock is null) throw new ArgumentNullException(nameof(leafBlock));
        Inline inline = leafBlock.Inline;
        
        while (inline != null)
        {
            Write(inline);
            inline = inline.NextSibling;
        }

        return this;
    }
    
    public BlazorRenderer WriteLeafRawLines(LeafBlock leafBlock)
    {
        if (leafBlock == null) throw new ArgumentNullException(nameof(leafBlock));
        if (leafBlock.Lines.Lines != null)
        {
            var lines = leafBlock.Lines;
            var slices = lines.Lines;
            for (var i = 0; i < lines.Count; i++)
            {
                if (i != 0)
                {
                    _builder.OpenElement(0, "br");
                    _builder.CloseElement();
                }

                WriteText(ref slices[i].Slice);
            }
        }

        return this;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteText(ref StringSlice slice)
    {
        if (slice.Start > slice.End)
            return;

        WriteText(slice.Text, slice.Start, slice.Length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlazorRenderer WriteText(string text, int seq = 0)
    {
        _builder.AddContent(seq, text);
        //_builder.OpenElement(1, "br");
        //_builder.CloseElement();

        return this;
    }
    
    public void WriteInline(Inline inline)
    {
        _builder.AddContent(0, inline.Span.ToString());
    }
    
    public void WriteText(string text, int offset, int length)
    {
        if (text == null)
            return;

        if (offset == 0 && text.Length == length)
        {
            WriteText(text);
        }
        else
        {
            if (length > buffer.Length)
            {
                buffer = text.ToCharArray();
                WriteText(new string(buffer, offset, length));
            }
            else
            {
                text.CopyTo(offset, buffer, 0, length);
                WriteText(new string(buffer, 0, length));
            }
        }
    }
}