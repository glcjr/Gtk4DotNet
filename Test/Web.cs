using GtkDotNet;
using CsTools.Extensions;
using System.Text;

using static System.Console;
using GtkDotNet.SafeHandles;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection.Metadata;

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
                    .SideEffect(_ => WebKitWebContext
                                        .GetDefault()
                                        .RegisterUriScheme("my", ServeCustomRequest)
                                        .RegisterUriScheme("request", ServeRequest))
                    .Child(
                        WebKit
                            .New()
                            .LoadUri($"my://index")
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

    static readonly JsonSerializerOptions defaults = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    static void ServeRequest(WebkitUriSchemeRequestHandle request)
    {
        var uri = request.GetUri();
        using var stream = request.GetHttpBody();
        var bytes = GBytes.New(Encoding.UTF8.GetBytes("128"));
        using var gstream = MemoryInputStream.New(bytes);
        request.Finish(gstream, 3, "text/html");
        
        var test = JsonSerializer.Deserialize<Test>(stream, defaults);
        var t = test;
    }

    static void ServeCustomRequest(WebkitUriSchemeRequestHandle request)
    {
        var uri = request.GetUri();
        if (uri == "my://index")
        {
            var html = 
@"
            <html>
                <body>
                    <h1>Hello from my custom scheme!</h1>
                    <div>
                        <video controls><source src='my://video/2010.mp4' type='video/mp4'>Your browser does not support the video tag.</video>
                    </div>
                    <div>
                        <img src='pic.jpg'/>
                    </div>
                    <div>
                        <button id='button'>Request</button>
                    </div>
                    <script>
                        const b = document.getElementById('button')
                        const data = {
                            name: 'Uwe Riegel',
                            id: 9865
                        }
                        b.onclick = async () => {
                            const res = await fetch('request://eineMethode', {
                                    method: 'POST',
                                    headers: { 'Content-Type': 'application/json' },
                                    body: JSON.stringify(data)
                                })
                            const text = await res.text()
                            console.log('reqId', text)
                        }
                    </script>
                </body>
            </html>
";
            var bytes = GBytes.New(Encoding.UTF8.GetBytes(html));
            var stream = MemoryInputStream.New(bytes);
            var headers = request.GetHttpHeaders().Get();
            foreach (var header in headers)
                WriteLine($"{header}");
            request.Finish(stream, html.Length, "text/html");
            stream.Dispose();
            bytes.Dispose();
        }
        else if (uri == "my://video.mp4")
        {
            var html = "<html><body><h1>Hello from my custom scheme!</h1><div><video controls autoplay src='my://video'></video></div><div><button id='req'></div></body></html>";
            var bytes = GBytes.New(Encoding.UTF8.GetBytes(html));
            var stream = MemoryInputStream.New(bytes);
            var headers = request.GetHttpHeaders().Get();
            foreach (var header in headers)
                WriteLine($"{header}");
            request.Finish(stream, html.Length, "text/html");
            stream.Dispose();
            bytes.Dispose();
        }
        else if (uri == "my://index/pic.jpg")
        {
            var res = Resources.Get("image");
            var bytes = new byte[res?.Length ?? 0];
            res?.Read(bytes, 0, bytes.Length);
            var gbytes = GBytes.New(bytes);
            var stream = MemoryInputStream.New(gbytes);
            var headers = request.GetHttpHeaders().Get();
            foreach (var header in headers)
                WriteLine($"{header}");
            request.Finish(stream, bytes.Length, "image/jpg");
            stream.Dispose();
            gbytes.Dispose();
        }
    }
}


record Test(string Name, int Id);