namespace CustomControls.Helpers;

public static class AppServiceProvider
{
    public static TService GetService<TService>()
    {
        var scope = Current.CreateScope();
        return scope.ServiceProvider.GetRequiredService<TService>();
    }

    public static IServiceProvider Current =>
#if WINDOWS10_0_17763_0_OR_GREATER
        MauiWinUIApplication.Current.Services;
#elif ANDROID
        MauiApplication.Current.Services;

#elif IOS || MACCATALYST
            MauiUIApplicationDelegate.Current.Services;
#else
        null;
#endif
}