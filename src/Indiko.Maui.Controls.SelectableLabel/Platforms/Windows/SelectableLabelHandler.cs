using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Windows.UI.Text;

using TextDeco = Windows.UI.Text.TextDecorations;

namespace Indiko.Maui.Controls.SelectableLabel.Platforms.Windows;


public class SelectableLabelHandler : ViewHandler<SelectableLabel, RichTextBlock>
{
    public static readonly IPropertyMapper<SelectableLabel, SelectableLabelHandler> Mapper =
     new PropertyMapper<SelectableLabel, SelectableLabelHandler>(ViewHandler.ViewMapper)
     {
         [nameof(SelectableLabel.Text)] = MapText,
         [nameof(SelectableLabel.TextColor)] = MapTextColor,
         [nameof(SelectableLabel.FontAttributes)] = MapFontAttributes,
         [nameof(SelectableLabel.FontSize)] = MapFontSize,
         [nameof(SelectableLabel.FontFamily)] = MapFontFamily,
         [nameof(SelectableLabel.LineBreakMode)] = MapLineBreakMode,
         [nameof(SelectableLabel.TextDecorations)] = MapTextDecorations,
         [nameof(SelectableLabel.TextTransform)] = MapTextTransform,
         [nameof(SelectableLabel.LineHeight)] = MapLineHeight,
         [nameof(SelectableLabel.MaxLines)] = MapMaxLines,
         [nameof(SelectableLabel.HorizontalTextAlignment)] = MapHorizontalTextAlignment,
         [nameof(SelectableLabel.VerticalTextAlignment)] = MapVerticalTextAlignment,
         [nameof(SelectableLabel.CharacterSpacing)] = MapCharacterSpacing,
         [nameof(SelectableLabel.FormattedText)] = MapFormattedText
     };

    public SelectableLabelHandler() : base(Mapper)
    {

    }

    protected override RichTextBlock CreatePlatformView()
    {
        var richTextBlock = new RichTextBlock
        {
            IsTextSelectionEnabled = true,
            TextWrapping = Microsoft.UI.Xaml.TextWrapping.Wrap
        };
        return richTextBlock;
    }

    public static void MapText(SelectableLabelHandler handler, SelectableLabel label)
    {
        handler.PlatformView.Blocks.Clear();
        
        if (!string.IsNullOrEmpty(label.Text))
        {
            var paragraph = new Paragraph();
            var run = new Run { Text = label.Text };
            paragraph.Inlines.Add(run);
            handler.PlatformView.Blocks.Add(paragraph);
        }
    }

    public static void MapTextColor(SelectableLabelHandler handler, SelectableLabel label)
    {
        handler.PlatformView.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(label.TextColor.ToWindowsColor());
    }

    public static void MapFontAttributes(SelectableLabelHandler handler, SelectableLabel label)
    {

        var fontWeight = label.FontAttributes.HasFlag(FontAttributes.Bold) ? Microsoft.UI.Text.FontWeights.Bold : Microsoft.UI.Text.FontWeights.Normal;
        var fontStyle = label.FontAttributes.HasFlag(FontAttributes.Italic) ? FontStyle.Italic : FontStyle.Normal;

        handler.PlatformView.FontWeight = fontWeight;
        handler.PlatformView.FontStyle = fontStyle;
    }

    public static void MapFontSize(SelectableLabelHandler handler, SelectableLabel label)
    {
        if (label.FontSize > 0)
        {
            handler.PlatformView.FontSize = label.FontSize;
        }
    }

    public static void MapFontFamily(SelectableLabelHandler handler, SelectableLabel label)
    {
        if (!string.IsNullOrEmpty(label.FontFamily))
        {
            handler.PlatformView.FontFamily = new Microsoft.UI.Xaml.Media.FontFamily(label.FontFamily);
        }
    }

    public static void MapLineBreakMode(SelectableLabelHandler handler, SelectableLabel label)
    {
        handler.PlatformView.TextWrapping = label.LineBreakMode switch
        {
            LineBreakMode.NoWrap => Microsoft.UI.Xaml.TextWrapping.NoWrap,
            LineBreakMode.WordWrap => Microsoft.UI.Xaml.TextWrapping.Wrap,
            LineBreakMode.CharacterWrap => Microsoft.UI.Xaml.TextWrapping.WrapWholeWords,
            LineBreakMode.HeadTruncation or LineBreakMode.TailTruncation or LineBreakMode.MiddleTruncation => Microsoft.UI.Xaml.TextWrapping.NoWrap,
            _ => Microsoft.UI.Xaml.TextWrapping.Wrap
        };

        handler.PlatformView.TextTrimming = label.LineBreakMode switch
        {
            LineBreakMode.HeadTruncation => Microsoft.UI.Xaml.TextTrimming.WordEllipsis,
            LineBreakMode.TailTruncation => Microsoft.UI.Xaml.TextTrimming.CharacterEllipsis,
            LineBreakMode.MiddleTruncation => Microsoft.UI.Xaml.TextTrimming.WordEllipsis,
            _ => Microsoft.UI.Xaml.TextTrimming.None
        };
    }

    public static void MapTextDecorations(SelectableLabelHandler handler, SelectableLabel label)
    {
        var textDecorations = TextDeco.None;

        if (label.TextDecorations.HasFlag(TextDeco.Underline))
            textDecorations |= TextDeco.Underline;

        if (label.TextDecorations.HasFlag(TextDeco.Strikethrough))
            textDecorations |= TextDeco.Strikethrough;

        handler.PlatformView.TextDecorations = textDecorations;
    }


    public static void MapTextTransform(SelectableLabelHandler handler, SelectableLabel label)
    {
        if (!string.IsNullOrEmpty(label.Text))
        {
            var transformedText = ApplyTextTransform(label.Text, label.TextTransform);
            
            handler.PlatformView.Blocks.Clear();
            var paragraph = new Paragraph();
            var run = new Run { Text = transformedText };
            paragraph.Inlines.Add(run);
            handler.PlatformView.Blocks.Add(paragraph);
        }
    }

    public static void MapLineHeight(SelectableLabelHandler handler, SelectableLabel label)
    {
        if (label.LineHeight > 0)
        {
            handler.PlatformView.LineHeight = label.LineHeight * handler.PlatformView.FontSize;
        }
    }

    public static void MapMaxLines(SelectableLabelHandler handler, SelectableLabel label)
    {
        if (label.MaxLines > 0)
        {
            handler.PlatformView.MaxLines = label.MaxLines;
        }
        else
        {
            handler.PlatformView.MaxLines = 0; // No limit
        }
    }

    public static void MapHorizontalTextAlignment(SelectableLabelHandler handler, SelectableLabel label)
    {
        handler.PlatformView.TextAlignment = label.HorizontalTextAlignment switch
        {
            TextAlignment.Start => Microsoft.UI.Xaml.TextAlignment.Left,
            TextAlignment.Center => Microsoft.UI.Xaml.TextAlignment.Center,
            TextAlignment.End => Microsoft.UI.Xaml.TextAlignment.Right,
            _ => Microsoft.UI.Xaml.TextAlignment.Left
        };
    }

    public static void MapVerticalTextAlignment(SelectableLabelHandler handler, SelectableLabel label)
    {
        handler.PlatformView.VerticalAlignment = label.VerticalTextAlignment switch
        {
            TextAlignment.Start => Microsoft.UI.Xaml.VerticalAlignment.Top,
            TextAlignment.Center => Microsoft.UI.Xaml.VerticalAlignment.Center,
            TextAlignment.End => Microsoft.UI.Xaml.VerticalAlignment.Bottom,
            _ => Microsoft.UI.Xaml.VerticalAlignment.Top
        };
    }

    public static void MapCharacterSpacing(SelectableLabelHandler handler, SelectableLabel label)
    {
        if (label.CharacterSpacing >= 0)
        {
            handler.PlatformView.CharacterSpacing = (int)(label.CharacterSpacing * 1000); // Convert em to 1000th of an em
        }
    }

    public static void MapFormattedText(SelectableLabelHandler handler, SelectableLabel label)
    {
        if (label.FormattedText != null)
        {
            handler.PlatformView.Blocks.Clear();
            var paragraph = new Paragraph();

            foreach (var span in label.FormattedText.Spans)
            {
                var transformedText = ApplyTextTransform(span.Text, span.TextTransform);
                var run = new Run { Text = transformedText };

                // Apply font attributes
                if (span.FontAttributes.HasFlag(FontAttributes.Bold))
                {
                    run.FontWeight = Microsoft.UI.Text.FontWeights.Bold;
                }
                if (span.FontAttributes.HasFlag(FontAttributes.Italic))
                {
                    run.FontStyle = FontStyle.Italic;
                }

                // Apply font size
                if (span.FontSize > 0)
                {
                    run.FontSize = span.FontSize;
                }

                // Apply text color
                if (span.TextColor != null)
                {
                    run.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(span.TextColor.ToWindowsColor());
                }

                // Apply text decorations
                var textDecorations = TextDeco.None;

                if (span.TextDecorations.HasFlag(TextDeco.Underline))
                {
                    textDecorations |= TextDeco.Underline;
                }
                if (span.TextDecorations.HasFlag(TextDeco.Strikethrough))
                {
                    textDecorations |= TextDeco.Strikethrough;
                }
                run.TextDecorations = textDecorations;

                // Apply character spacing
                if (span.CharacterSpacing >= 0)
                {
                    run.CharacterSpacing = (int)(span.CharacterSpacing * 1000);
                }

                // Note: GestureRecognizers are not supported in the same way on Windows RichTextBlock
                // This would require additional implementation using Hyperlink elements

                paragraph.Inlines.Add(run);
            }

            handler.PlatformView.Blocks.Add(paragraph);
        }
    }

    private static string ApplyTextTransform(string text, TextTransform transform)
    {
        return transform switch
        {
            TextTransform.Uppercase => text?.ToUpper(),
            TextTransform.Lowercase => text?.ToLower(),
            //TextTransform.Capitalize => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text ?? string.Empty),
            _ => text
        };
    }
}