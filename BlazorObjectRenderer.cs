using Markdig.Renderers;
using Markdig.Syntax;

namespace Markdig.Blazor;

public abstract class BlazorObjectRenderer<TObject> : MarkdownObjectRenderer<BlazorRenderer, TObject>
where TObject : MarkdownObject
{
    
}