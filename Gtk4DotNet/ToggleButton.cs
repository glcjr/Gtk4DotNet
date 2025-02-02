using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class ToggleButton
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_toggle_button_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static ToggleButtonHandle New();

    public static bool Active(this ToggleButtonHandle toggleButton)
        => GetActive(toggleButton);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_toggle_button_set_active", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetActive(this ToggleButtonHandle toggleButton, bool active);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_toggle_button_toggled", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Toggle(this ToggleButtonHandle toggleButton);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_toggle_button_get_active", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetActive(this ToggleButtonHandle toggleButton);
}


 