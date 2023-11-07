using Microsoft.AspNetCore.Components.Rendering;

namespace Markdig.Blazor;

public static partial class Markdown
{
    /// <summary>
    /// Converts the specified markdown to HTML and renders it to the specified <see cref="RenderTreeBuilder"/>.
    /// </summary>
    /// <param name="markdown">Markdown text</param>
    /// <param name="builder">The render tree to render to</param>
    /// <param name="pipeline">The pipeline used for the conversion</param>
    /// <param name="renderer">The renderer used</param>
    public static void RenderToFragment(
        string markdown, 
        RenderTreeBuilder builder, 
        MarkdownPipeline pipeline = null, 
        BlazorRenderer renderer = null)
    {
        if (markdown is null) 
            throw new ArgumentNullException(nameof(markdown));
        
        pipeline ??= new MarkdownPipelineBuilder().Build();

        if (renderer is null)
            renderer = new BlazorRenderer(builder);
        else
            renderer.SetBuilder(builder);
        
        pipeline.Setup(renderer);
        
        var document = Markdig.Markdown.Parse(markdown, pipeline);
        renderer.Render(document);
    }
}