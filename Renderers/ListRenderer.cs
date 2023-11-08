using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace Markdig.Blazor.Renderers;

public class ListRenderer : BlazorObjectRenderer<ListBlock>
{
    protected override void Write(BlazorRenderer renderer, ListBlock listBlock)
    {
        if (listBlock.IsOrdered)
        {
            renderer.OpenElement("ol", 0);
            if (listBlock.BulletType != '1')
            {
                renderer.AddAttribute("type", listBlock.BulletType.ToString(), 1);
            }
            if (listBlock.OrderedStart is not null && listBlock.OrderedStart != "1")
            {
                renderer.AddAttribute("start", listBlock.OrderedStart, 2);
            }
        }
        else
        {
            renderer.OpenElement("ul", 0);
        }
        
        renderer.AddAttributes(listBlock.TryGetAttributes(), 3);

        foreach (var item in listBlock)
        {
            var listItem = (ListItemBlock)item;
            renderer.OpenElement("li", 0);
            renderer.AddAttributes(listItem.TryGetAttributes(), 1);
            renderer.WriteChildren(listItem);
            renderer.CloseElement();
        }
        
        renderer.CloseElement();
    }
}