using System.Text;
using System.Windows.Input;
using Android.Graphics;
using Android.Text.Style;
using Android.Widget;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using aGraphics = Android.Graphics;
using aText = Android.Text;
using aViews = Android.Views;

namespace Indiko.Maui.Controls.SelectableLabel.Platforms.Android;


public class SelectableLabelHandler : ViewHandler<SelectableLabel, TextView>
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

    protected override TextView CreatePlatformView()
    {
        var textView = new TextView(Context)
        {
            Focusable = true,
            LongClickable = true
        };
        textView.SetTextIsSelectable(true);
        return textView;
    }

    public static void MapText(SelectableLabelHandler handler, SelectableLabel label)
    {
        handler.PlatformView.Text = label.Text;
    }

    public static void MapTextColor(SelectableLabelHandler handler, SelectableLabel label)
    {
        handler.PlatformView.SetTextColor(label.TextColor.ToPlatform());
    }

    public static void MapFontAttributes(SelectableLabelHandler handler, SelectableLabel label)
    {
        

        var typefaceStyle = label.FontAttributes switch
        {
            FontAttributes.None => aGraphics.TypefaceStyle.Normal,
            FontAttributes.Bold => aGraphics.TypefaceStyle.Bold,
            FontAttributes.Italic => aGraphics.TypefaceStyle.Italic,
            _ => aGraphics.TypefaceStyle.Normal
        };

        handler.PlatformView.SetTypeface(null, typefaceStyle);
    }

    public static void MapFontSize(SelectableLabelHandler handler, SelectableLabel label)
    {
        handler.PlatformView.TextSize = (float)label.FontSize;
    }

    public static void MapFontFamily(SelectableLabelHandler handler, SelectableLabel label)
    {
        if (!string.IsNullOrEmpty(label.FontFamily))
        {
            var typeface = Typeface.Create(label.FontFamily, TypefaceStyle.Normal);
            handler.PlatformView.Typeface = typeface;
        }
    }

    public static void MapBackgroundColor(SelectableLabelHandler handler, SelectableLabel label)
    {
        handler.PlatformView.SetBackgroundColor(label.BackgroundColor.ToPlatform());
    }

    public static void MapLineBreakMode(SelectableLabelHandler handler, SelectableLabel label)
    {
        switch (label.LineBreakMode)
        {
            case LineBreakMode.NoWrap:
                handler.PlatformView.SetSingleLine(true);
                handler.PlatformView.Ellipsize = null;
                break;

            case LineBreakMode.WordWrap:
                handler.PlatformView.SetSingleLine(false);
                handler.PlatformView.SetMaxLines(int.MaxValue);
                handler.PlatformView.Ellipsize = null;
                break;

            case LineBreakMode.CharacterWrap:
                handler.PlatformView.SetSingleLine(false);
                handler.PlatformView.SetMaxLines(int.MaxValue);
                handler.PlatformView.HyphenationFrequency = aText.HyphenationFrequency.None;  // Approximate character wrapping
                handler.PlatformView.Ellipsize = null; 
                break;

            case LineBreakMode.HeadTruncation:
                handler.PlatformView.SetSingleLine(true);
                handler.PlatformView.Ellipsize = aText.TextUtils.TruncateAt.Start;
                break;

            case LineBreakMode.TailTruncation:
                handler.PlatformView.SetSingleLine(true);
                handler.PlatformView.Ellipsize = aText.TextUtils.TruncateAt.End;
                break;

            case LineBreakMode.MiddleTruncation:
                handler.PlatformView.SetSingleLine(true);
                handler.PlatformView.Ellipsize = aText.TextUtils.TruncateAt.Middle;
                break;

            default:
                handler.PlatformView.SetSingleLine(true);
                handler.PlatformView.Ellipsize = aText.TextUtils.TruncateAt.End;
                break;
        }
    }

    public static void MapTextDecorations(SelectableLabelHandler handler, SelectableLabel label)
    {
        var paintFlags = handler.PlatformView.PaintFlags;

        if (label.TextDecorations.HasFlag(TextDecorations.Underline))
        {
            paintFlags |= aGraphics.PaintFlags.UnderlineText;
        }
        else
        {
            paintFlags &= ~aGraphics.PaintFlags.UnderlineText;
        }

        if (label.TextDecorations.HasFlag(TextDecorations.Strikethrough))
        {
            paintFlags |= aGraphics.PaintFlags.StrikeThruText;
        }
        else
        {
            paintFlags &= ~aGraphics.PaintFlags.StrikeThruText;
        }

        handler.PlatformView.PaintFlags = paintFlags;
    }


    public static void MapTextTransform(SelectableLabelHandler handler, SelectableLabel label)
    {
        handler.PlatformView.Text = ApplyTextTransform(label.Text, label.TextTransform);
    }

    public static void MapLineHeight(SelectableLabelHandler handler, SelectableLabel label)
    {
        if (label.LineHeight > 0)
        {
            handler.PlatformView.SetLineSpacing(0, (float)label.LineHeight);
        }
    }

    public static void MapMaxLines(SelectableLabelHandler handler, SelectableLabel label)
    {
        if (label.MaxLines > 0)
        {
            handler.PlatformView.SetMaxLines(label.MaxLines);
        }
        else
        {
            handler.PlatformView.SetMaxLines(int.MaxValue);
        }
    }

    public static void MapHorizontalTextAlignment(SelectableLabelHandler handler, SelectableLabel label)
    {
        var gravity = handler.PlatformView.Gravity;

        // Remove existing horizontal flags
        gravity &= ~(aViews.GravityFlags.Start | aViews.GravityFlags.CenterHorizontal | aViews.GravityFlags.End);

        gravity |= label.HorizontalTextAlignment switch
        {
            TextAlignment.Start => aViews.GravityFlags.Start,
            TextAlignment.Center => aViews.GravityFlags.CenterHorizontal,
            TextAlignment.End => aViews.GravityFlags.End,
            _ => aViews.GravityFlags.Start
        };

        handler.PlatformView.Gravity = gravity;
    }

    public static void MapVerticalTextAlignment(SelectableLabelHandler handler, SelectableLabel label)
    {
        var gravity = handler.PlatformView.Gravity;

        // Remove existing vertical flags
        gravity &= ~(aViews.GravityFlags.Top | aViews.GravityFlags.CenterVertical | aViews.GravityFlags.Bottom);

        gravity = label.VerticalTextAlignment switch
        {
            TextAlignment.Start => gravity | aViews.GravityFlags.Top,
            TextAlignment.Center => gravity | aViews.GravityFlags.CenterVertical,
            TextAlignment.End => gravity | aViews.GravityFlags.Bottom,
            _ => gravity | aViews.GravityFlags.Top
        };

        handler.PlatformView.Gravity = gravity;
    }

    public static void MapCharacterSpacing(SelectableLabelHandler handler, SelectableLabel label)
    {
        if (label.CharacterSpacing >= 0)
        {
            // Convert the CharacterSpacing (in em) from Xamarin.Forms to the Android LetterSpacing format.
            handler.PlatformView.LetterSpacing = (float)label.CharacterSpacing;
        }
    }


    public static void MapFormattedText(SelectableLabelHandler handler, SelectableLabel label)
    {
        if (label.FormattedText != null)
        {
            var formattedText = new StringBuilder();

            foreach (var span in label.FormattedText.Spans)
            {
                var transformedText = ApplyTextTransform(span.Text, span.TextTransform);
                formattedText.Append(transformedText);
            }

            var spannableString = new aText.SpannableString(formattedText.ToString());
            int currentIndex = 0;

            foreach (var span in label.FormattedText.Spans)
            {
                var transformedText = ApplyTextTransform(span.Text, span.TextTransform);
                var start = currentIndex;
                var end = start + transformedText.Length;

                if (span.FontAttributes.HasFlag(FontAttributes.Bold))
                {
                    spannableString.SetSpan(new aText.Style.StyleSpan(aGraphics.TypefaceStyle.Bold), start, end, aText.SpanTypes.InclusiveInclusive);
                }
                if (span.FontAttributes.HasFlag(FontAttributes.Italic))
                {
                    spannableString.SetSpan(new aText.Style.StyleSpan(aGraphics.TypefaceStyle.Italic), start, end, aText.SpanTypes.InclusiveInclusive);
                }

                if (span.TextColor != null)
                {
                    spannableString.SetSpan(new ForegroundColorSpan(span.TextColor.ToPlatform()), start, end, aText.SpanTypes.InclusiveInclusive);
                }

                if (span.BackgroundColor != null)
                {
                    spannableString.SetSpan(new aText.Style.BackgroundColorSpan(span.BackgroundColor.ToPlatform()), start, end, aText.SpanTypes.InclusiveInclusive);
                }

                if (span.TextDecorations.HasFlag(TextDecorations.Underline))
                {
                    spannableString.SetSpan(new UnderlineSpan(), start, end, aText.SpanTypes.InclusiveInclusive);
                }
                if (span.TextDecorations.HasFlag(TextDecorations.Strikethrough))
                {
                    spannableString.SetSpan(new StrikethroughSpan(), start, end, aText.SpanTypes.InclusiveInclusive);
                }

                if (span.GestureRecognizers != null && span.GestureRecognizers.Count > 0)
                {
                    var gestureRecognizer = span.GestureRecognizers[0] as TapGestureRecognizer;
                    if (gestureRecognizer != null)
                    {
                        spannableString.SetSpan(new ClickableSpanWithTapHandler(gestureRecognizer.Command), start, end, aText.SpanTypes.InclusiveInclusive);
                        handler.PlatformView.MovementMethod = aText.Method.LinkMovementMethod.Instance;
                    }
                }

                currentIndex = end;
            }

            handler.PlatformView.TextFormatted = spannableString;
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

public class ClickableSpanWithTapHandler : aText.Style.ClickableSpan
{
    private readonly ICommand _tapCommand;

    public ClickableSpanWithTapHandler(ICommand tapCommand)
    {
        _tapCommand = tapCommand;
    }

    public override void OnClick(aViews.View widget)
    {
        _tapCommand?.Execute(null);
    }
}