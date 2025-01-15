
#if ANDROID
using Indiko.Maui.Controls.SelectableLabel.Platforms.Android;
#endif

#if IOS
using Indiko.Maui.Controls.SelectableLabel.Platforms.iOS;
#endif

#if MACCATALYST
using Indiko.Maui.Controls.SelectableLabel.Platforms.MacCatalyst;
#endif

namespace Indiko.Maui.Controls.SelectableLabel;

public static class BuilderExtension
{
    public static MauiAppBuilder UseSelectableLabel(this MauiAppBuilder builder)
    {
        builder.ConfigureMauiHandlers(handlers =>
         {
             handlers.AddHandler(typeof(SelectableLabel), typeof(SelectableLabelHandler));
         });

        return builder;
    }
}
