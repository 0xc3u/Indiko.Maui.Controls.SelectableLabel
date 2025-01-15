using Foundation;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace Indiko.Maui.Controls.SelectableLabel.Platforms.MacCatalyst;
public class SelectableLabelHandler : ViewHandler<SelectableLabel, UITextView>
{
    public static readonly IPropertyMapper<SelectableLabel, SelectableLabelHandler> Mapper =
     new PropertyMapper<SelectableLabel, SelectableLabelHandler>(ViewHandler.ViewMapper)
     {
         [nameof(SelectableLabel.Text)] = MapText,
         [nameof(SelectableLabel.TextColor)] = MapTextColor,
         [nameof(SelectableLabel.FontAttributes)] = MapFontAttributes,
         [nameof(SelectableLabel.FontSize)] = MapFontSize,
         [nameof(SelectableLabel.FontFamily)] = MapFontFamily,
         [nameof(SelectableLabel.BackgroundColor)] = MapBackgroundColor,
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

    protected override UITextView CreatePlatformView()
    {
        var textView = new UITextView
        {
            Selectable = true,
            Editable = false,
            ScrollEnabled = false,
            TextContainerInset = UIEdgeInsets.Zero,
            TextContainer = { LineFragmentPadding = 0 },
            BackgroundColor = UIColor.Clear
        };
        return textView;
    }

    public static void MapText(SelectableLabelHandler handler, SelectableLabel label)
    {
        handler.PlatformView.Text = label.Text;
    }

    public static void MapTextColor(SelectableLabelHandler handler, SelectableLabel label)
    {
        handler.PlatformView.TextColor =  label.TextColor.ToPlatform();
    }

    public static void MapFontAttributes(SelectableLabelHandler handler, SelectableLabel label)
    {
        var font = label.FontAttributes switch
        {
            FontAttributes.None => UIFont.SystemFontOfSize((nfloat)label.FontSize),
            FontAttributes.Bold => UIFont.BoldSystemFontOfSize((nfloat)label.FontSize),
            FontAttributes.Italic => UIFont.ItalicSystemFontOfSize((nfloat)label.FontSize),
            _ => UIFont.SystemFontOfSize((nfloat)label.FontSize)
        };

        handler.PlatformView.Font = font;
    }

    public static void MapFontSize(SelectableLabelHandler handler, SelectableLabel label)
    {
        handler.PlatformView.Font = handler.PlatformView.Font.WithSize((nfloat)label.FontSize);
    }

    public static void MapFontFamily(SelectableLabelHandler handler, SelectableLabel label)
    {
        if (!string.IsNullOrEmpty(label.FontFamily))
        {
            var font = UIFont.FromName(label.FontFamily, (nfloat)label.FontSize);
            if (font != null)
            {
                handler.PlatformView.Font = font;
            }
        }
    }

    public static void MapBackgroundColor(SelectableLabelHandler handler, SelectableLabel label)
    {
        handler.PlatformView.BackgroundColor = label.BackgroundColor.ToPlatform();
    }

    public static void MapLineBreakMode(SelectableLabelHandler handler, SelectableLabel label)
    {
        handler.PlatformView.TextContainer.LineBreakMode = label.LineBreakMode switch
        {
            LineBreakMode.NoWrap => UILineBreakMode.Clip,
            LineBreakMode.WordWrap => UILineBreakMode.WordWrap,
            LineBreakMode.CharacterWrap => UILineBreakMode.CharacterWrap,
            LineBreakMode.HeadTruncation => UILineBreakMode.HeadTruncation,
            LineBreakMode.TailTruncation => UILineBreakMode.TailTruncation,
            LineBreakMode.MiddleTruncation => UILineBreakMode.MiddleTruncation,
            _ => UILineBreakMode.TailTruncation
        };
    }

    public static void MapTextDecorations(SelectableLabelHandler handler, SelectableLabel label)
    {
        var text = new NSMutableAttributedString(handler.PlatformView.Text);

        if (label.TextDecorations.HasFlag(TextDecorations.Underline))
        {
            text.AddAttribute(UIStringAttributeKey.UnderlineStyle, NSNumber.FromInt32((int)NSUnderlineStyle.Single), new NSRange(0, text.Length));
        }

        if (label.TextDecorations.HasFlag(TextDecorations.Strikethrough))
        {
            text.AddAttribute(UIStringAttributeKey.StrikethroughStyle, NSNumber.FromInt32((int)NSUnderlineStyle.Single), new NSRange(0, text.Length));
        }

        handler.PlatformView.AttributedText = text;
    }

    public static void MapTextTransform(SelectableLabelHandler handler, SelectableLabel label)
    {
        handler.PlatformView.Text = ApplyTextTransform(label.Text, label.TextTransform);
    }

    public static void MapLineHeight(SelectableLabelHandler handler, SelectableLabel label)
    {
        if (label.LineHeight > 0)
        {
            var paragraphStyle = new NSMutableParagraphStyle
            {
                LineHeightMultiple = (nfloat)label.LineHeight
            };

            var attributedText = new NSMutableAttributedString(handler.PlatformView.Text);
            attributedText.AddAttribute(UIStringAttributeKey.ParagraphStyle, paragraphStyle, new NSRange(0, attributedText.Length));

            handler.PlatformView.AttributedText = attributedText;
        }
    }

    public static void MapMaxLines(SelectableLabelHandler handler, SelectableLabel label)
    {
        if (label.MaxLines > 0)
        {
            handler.PlatformView.TextContainer.MaximumNumberOfLines = (nuint)label.MaxLines;
            handler.PlatformView.TextContainer.LineBreakMode = UILineBreakMode.WordWrap;
        }
        else
        {
            handler.PlatformView.TextContainer.MaximumNumberOfLines = 0;
        }
    }

    public static void MapHorizontalTextAlignment(SelectableLabelHandler handler, SelectableLabel label)
    {
        handler.PlatformView.TextAlignment = label.HorizontalTextAlignment switch
        {
            TextAlignment.Start => UITextAlignment.Left,
            TextAlignment.Center => UITextAlignment.Center,
            TextAlignment.End => UITextAlignment.Right,
            _ => UITextAlignment.Left
        };
    }

    public static void MapVerticalTextAlignment(SelectableLabelHandler handler, SelectableLabel label)
    {
        var textHeight = handler.PlatformView.ContentSize.Height;
        var containerHeight = handler.PlatformView.Bounds.Height;

        var verticalInset = label.VerticalTextAlignment switch
        {
            TextAlignment.Start => 0,
            TextAlignment.Center => Math.Max(0, (containerHeight - textHeight) / 2),  // Center text vertically
            TextAlignment.End => Math.Max(0, containerHeight - textHeight),  // Align text to the bottom
            _ => 0
        };

        handler.PlatformView.TextContainerInset = new UIEdgeInsets((nfloat)verticalInset, 0, 0, 0);
    }

    public static void MapCharacterSpacing(SelectableLabelHandler handler, SelectableLabel label)
    {
        if (label.CharacterSpacing >= 0)
        {
            var attributedString = new NSMutableAttributedString(handler.PlatformView.Text);

            // Set the Kern attribute (character spacing) in the attributed string
            attributedString.AddAttribute(UIStringAttributeKey.KerningAdjustment, NSNumber.FromDouble(label.CharacterSpacing), new NSRange(0, attributedString.Length));

            handler.PlatformView.AttributedText = attributedString;
        }
    }

    public static void MapFormattedText(SelectableLabelHandler handler, SelectableLabel label)
    {
        if (label.FormattedText != null)
        {
            var formattedText = new NSMutableAttributedString();

            foreach (var span in label.FormattedText.Spans)
            {
                // Apply TextTransform (Uppercase, Lowercase)
                var transformedText = ApplyTextTransform(span.Text, span.TextTransform);

                // Create an attributed string for the transformed text
                var attributedSpan = new NSMutableAttributedString(transformedText);

                var range = new NSRange(0, attributedSpan.Length);

                // Apply font attributes (Bold, Italic)
                if (span.FontAttributes.HasFlag(FontAttributes.Bold))
                {
                    attributedSpan.AddAttribute(UIStringAttributeKey.Font, UIFont.BoldSystemFontOfSize((nfloat)(span.FontSize > 0 ? span.FontSize : handler.PlatformView.Font.PointSize)), range);
                }
                else if (span.FontAttributes.HasFlag(FontAttributes.Italic))
                {
                    attributedSpan.AddAttribute(UIStringAttributeKey.Font, UIFont.ItalicSystemFontOfSize((nfloat)(span.FontSize > 0 ? span.FontSize : handler.PlatformView.Font.PointSize)), range);
                }
                else if (span.FontSize > 0)
                {
                    attributedSpan.AddAttribute(UIStringAttributeKey.Font, UIFont.SystemFontOfSize((nfloat)span.FontSize), range);
                }

                // Apply text color
                if (span.TextColor != null)
                {
                    attributedSpan.AddAttribute(UIStringAttributeKey.ForegroundColor, span.TextColor.ToPlatform(), range);
                }

                // Apply background color
                if (span.BackgroundColor != null)
                {
                    attributedSpan.AddAttribute(UIStringAttributeKey.BackgroundColor, span.BackgroundColor.ToPlatform(), range);
                }

                // Apply underline
                if (span.TextDecorations.HasFlag(TextDecorations.Underline))
                {
                    attributedSpan.AddAttribute(UIStringAttributeKey.UnderlineStyle, NSNumber.FromInt32((int)NSUnderlineStyle.Single), range);
                }

                // Apply strikethrough
                if (span.TextDecorations.HasFlag(TextDecorations.Strikethrough))
                {
                    attributedSpan.AddAttribute(UIStringAttributeKey.StrikethroughStyle, NSNumber.FromInt32((int)NSUnderlineStyle.Single), range);
                }

                // Add gesture recognizers (e.g., TapGestureRecognizer)
                if (span.GestureRecognizers != null && span.GestureRecognizers.Count > 0)
                {
                    var tapGestureRecognizer = span.GestureRecognizers[0] as TapGestureRecognizer;
                    if (tapGestureRecognizer != null)
                    {
                        handler.PlatformView.UserInteractionEnabled = true;
                        var tapGesture = new UITapGestureRecognizer(() => tapGestureRecognizer.Command?.Execute(tapGestureRecognizer.CommandParameter));
                        handler.PlatformView.AddGestureRecognizer(tapGesture);
                    }
                }

                formattedText.Append(attributedSpan);
            }

            handler.PlatformView.AttributedText = formattedText;
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
