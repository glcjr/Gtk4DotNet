using GtkDotNet;
using CsTools.Extensions;
using static System.Console;

static class HelloWorld
{
    public static int Run()
        => Application
            .NewAdwaita("org.gtk.example")
            .OnActivate(app => 
                app
                    .SideEffect(_ => WriteLine($"Gkt theme: {GtkSettings.GetDefault().ThemeName}"))
                    .NewWindow()
                    .Title("Hello Gtk👍")
                    .DefaultSize(200, 200)
                    .OnClose(_ => false.SideEffect(_ => WriteLine("Window is closing")))
                    .SideEffect(w => w
                        .Child(
                            Box
                                .New(Orientation.Vertical)
                                .HAlign(Align.Center)
                                .VAlign(Align.Center)
                                .Append(
                                    Button
                                        .NewWithLabel("Maximize Window")
                                        .OnClicked(() => w.Maximize())
                                        .Tooltip("This is a sample Button\tCtrl-H"))))

                    .Show())
            .Run(0, IntPtr.Zero);
}

