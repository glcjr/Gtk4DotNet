# Gtk4DotNet
.Net 8 bindings for GTK 4

Gtk4DotNet uses a functional declarative approach to GTK4 similar to REACT or Kotlin Compose:

```
return Application
    .New("org.gtk.example")
    .OnActivate(app => 
        app
            .NewWindow()
            .Title("Hello Gtk👍")
            .SideEffect(win => win
            .Child(
                Grid
                    .New()
                    .Attach(                                
                        Button
                            .NewWithLabel("Button 1")
                            .OnClicked(() => WriteLine("Button1 clicked")), 0, 0, 1, 1)
                    .Attach(                                
                        Button
                            .NewWithLabel("Button 2")
                            .OnClicked(() => WriteLine("Button2 clicked")), 1, 0, 1, 1)
                    .Attach(                                
                        Button
                            .NewWithLabel("Quit")
                            .OnClicked(() => win.CloseWindow()), 0, 1, 2, 1)))
            .Show())
    .Run(0, IntPtr.Zero);
}
```
You don't have to use window.ui XML files for describing the UI, instead declare the UI declarative with C#. 

Contained in this Repo are samples how to use Gtk4DotNet. All examples of the official GTK4 are transformed to C# with Gtk4DotNet.

## Prerequisites

### Necessary only depending on the version of Linux

if you want to use WebView (WebKitGTK) on Ubuntu 22.04:

* sudo apt install libwebkitgtk-6.0-dev

With LibAdwaita

* sudo apt install libadwaita-1-dev

Gtk4
* sudo apt install libgtk-4-dev

### If you want to use GTK resources

* sudo apt install libglib2.0-dev-bin

## Installation of GTK Schema
```
    sudo install -D ./Test/org.gtk.example.gschema.xml /usr/share/glib-2.0/schemas/
    sudo glib-compile-schemas /usr/share/glib-2.0/schemas/
```     
## Usage

Look at the sample programs (https://github.com/uriegel/Gtk4DotNet/tree/Main/Test)

## Checking if memory is being freed
To check if GObjects are being freed, just run
```
Widget.AddWeakRef(() => Console.WriteLine("... is being freed));
```
If this object is finalized, then the callback will be called.

