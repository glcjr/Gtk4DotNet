using GtkDotNet;
using GtkDotNet.SafeHandles;
using LinqTools;
using static System.Console;

static class HelloWorld
{
    public static int Run()
        => Application
            .New("org.gtk.example")
            .OnActivate(app => 
                app
                    .SideEffect(_ => WriteLine($"Gkt theme: {GtkSettings.GetDefault().ThemeName}"))
                    .NewWindow()
                    .ResourceIcon("icon")
                    .Title("Hello Gtk👍")
                    .DefaultSize(200, 200)
                    .OnClose(OnClose)
                    .SideEffect(w => w
                        .Child(
                            Box
                                .New(Orientation.Vertical)
                                .HAlign(Align.Center)
                                .VAlign(Align.Center)
                                .Append(
                                    Button
                                        .NewWithLabel("Maximize Window")
                                        .OnClicked(() => w.Maximize()))))
                    .Show())
            .Run(0, IntPtr.Zero);

    static bool OnClose(WindowHandle window)
    {
        var dialog = 
            Dialog
                .New("Hello World beenden?", window, DialogFlags.DestroyWithParent | DialogFlags.Modal, "Ok", Dialog.RESPONSE_OK)
                .Show();

        return true;
    }            
}

