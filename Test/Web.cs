using GtkDotNet;
using CsTools.Extensions;
using System.Text;

using static System.Console;

static class Web
{
    public static int Run()
        =>  Application
            .New("org.gtk.example")
            .OnActivate(app =>
                app
                    .NewWindow()
                    .Title("Hello Web View👍")
                    .DefaultSize(800, 600)
                    .SideEffect(_ => {
                        var context = WebKitWebContext.GetDefault();
                        context.RegisterUriScheme("my", request =>
                        {
                            var uri = request.GetUri();
                            var html = "<html><body><h1>Hello from my custom scheme!</h1></body></html>";
                            var bytes = GBytes.New(Encoding.UTF8.GetBytes(html));
                            var stream = MemoryInputStream.New(bytes);
                            var headers = request.GetHttpHeaders().Get();
                            foreach (var header in headers)
                                WriteLine($"{header}");
                            request.Finish(stream, html.Length, "text/html");
                            stream.Dispose();
                            bytes.Dispose();
                        });
                    })
                    .Child(
                        WebKit
                            .New()
                            .LoadUri($"my://google.de")
                            .SideEffect(wk => 
                                wk.AddController(
                                EventControllerKey
                                    .New()
                                    .RefSink()
                                    .OnKeyPressed((k, kc, m) => {
                                        if (kc == 73)
                                        {
                                            // prevent blink_cb crash!
                                            wk.RunJavascript(
"""
    console.log("Der F7")
    document.dispatchEvent(new KeyboardEvent('keydown', {
        key: "F7",
        code: "F7"
    })) 
""");
                                            GC.Collect();
                                            return true;
                                        }
                                        else
                                            return false;
                                    })))
                            .SideEffect(w => w.GetSettings()
                                .SideEffect(s => 
                                {
                                    WriteLine($"EnableDevExtras: {s.EnableDeveloperExtras}");
                                    WriteLine($"CursiveFontFamily: {s.CursiveFontFamily}");
                                    s.EnableDeveloperExtras = true;
                                    WriteLine($"EnableDevExtras: {s.EnableDeveloperExtras}");
                                }))
                            .OnLoadChanged((w, e) => 
                                e.SideEffectIf(e == WebViewLoad.Finished, 
                                    _ => w.RunJavascript("console.log('called from C#')")))
                            //.DisableContextMenu()
                            .OnAlert((w, text) => 
                                text
                                    .SideEffectIf(text == "showDevTools",
                                        _ => w.GetInspector().Show())
                                    .SideEffect(text => WriteLine($"on alert: {text}")))
                    )
                    .Show())
            .Run(0, IntPtr.Zero);
}


