using System.Drawing;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
internal struct GtkRgba
{
    float Red;
    float Green;
    float Blue;
    float Alpha;

    public static Color ToColor(GtkRgba gtkRgba)
        => Color.FromArgb((int)(gtkRgba.Alpha * 255), (int)(gtkRgba.Red * 255), (int)(gtkRgba.Green * 255), (int)(gtkRgba.Blue * 255));
    public static GtkRgba FromColor(Color color)
    {
        var affe = new GtkRgba
        {
            Alpha = color.A / 255.0f,
            Red = color.R / 255.0f,
            Green = color.G / 255.0f,
            Blue = color.B / 255.0f,
        };
        return affe;
    }
}
