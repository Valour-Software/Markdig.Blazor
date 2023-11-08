using System.Globalization;
using Markdig.Extensions.Tables;
using Markdig.Renderers.Html;

namespace Markdig.Blazor.Renderers.Extensions;

/// <summary>
/// A HTML renderer for a <see cref="Table"/>
/// </summary>
public class TableRenderer : BlazorObjectRenderer<Table>
{
    protected override void Write(BlazorRenderer renderer, Table table)
    {
        renderer.OpenElement("table", 0);

        bool hasBody = false;
        bool hasAlreadyHeader = false;
        bool isHeaderOpen = false;

        bool hasColumnWidth = false;
        foreach (var tableColumnDefinition in table.ColumnDefinitions)
        {
            if (tableColumnDefinition.Width != 0.0f && tableColumnDefinition.Width != 1.0f)
            {
                hasColumnWidth = true;
                break;
            }
        }

        if (hasColumnWidth)
        {
            foreach (var tableColumnDefinition in table.ColumnDefinitions)
            {
                var width = Math.Round(tableColumnDefinition.Width * 100) / 100;
                var widthValue = string.Format(CultureInfo.InvariantCulture, "{0:0.##}", width);

                renderer.OpenElement("col", 0);
                renderer.AddAttribute("style", $"width:{widthValue}%");
                renderer.CloseElement();
            }
        }

        foreach (var rowObj in table)
        {
            var row = (TableRow)rowObj;
            if (row.IsHeader)
            {
                // Allow a single thead
                if (!hasAlreadyHeader)
                {
                    renderer.OpenElement("thead", 0);
                    isHeaderOpen = true;
                }

                hasAlreadyHeader = true;
            }
            else if (!hasBody)
            {
                if (isHeaderOpen)
                {
                    renderer.CloseElement();
                    isHeaderOpen = false;
                }

                renderer.OpenElement("tbody", 0);
                hasBody = true;
            }

            renderer.OpenElement("tr", 0);
            renderer.AddAttributes(row.TryGetAttributes(), 1);
            
            for (int i = 0; i < row.Count; i++)
            {
                var cellObj = row[i];
                var cell = (TableCell)cellObj;

                renderer.OpenElement(row.IsHeader ? "th" : "td", 0);
                if (cell.ColumnSpan != 1)
                {
                    renderer.AddAttribute("colspan", cell.ColumnSpan.ToString(), 1);
                }

                if (cell.RowSpan != 1)
                {
                    renderer.AddAttribute("rowspan", cell.RowSpan.ToString(), 2);
                }

                if (table.ColumnDefinitions.Count > 0)
                {
                    var columnIndex = cell.ColumnIndex < 0 || cell.ColumnIndex >= table.ColumnDefinitions.Count
                        ? i
                        : cell.ColumnIndex;
                    columnIndex = columnIndex >= table.ColumnDefinitions.Count ? table.ColumnDefinitions.Count - 1 : columnIndex;
                    var alignment = table.ColumnDefinitions[columnIndex].Alignment;
                    if (alignment.HasValue)
                    {
                        switch (alignment)
                        {
                            case TableColumnAlign.Center:
                                renderer.AddAttribute("style", "text-align: center;", 3);
                                break;
                            case TableColumnAlign.Right:
                                renderer.AddAttribute("style", "text-align: right;", 3);
                                break;
                            case TableColumnAlign.Left:
                                renderer.AddAttribute("style", "text-align: left;", 3);
                                break;
                        }
                    }
                }

                renderer.AddAttributes(cell.TryGetAttributes(), 4);
                renderer.Write(cell);
                renderer.CloseElement();
            }

            renderer.CloseElement();
        }

        renderer.CloseElement();
        renderer.CloseElement();
    }
}