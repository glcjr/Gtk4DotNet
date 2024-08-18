namespace GtkDotNet;

[Flags]
public enum DragAction
{
    Default = 1 << 0,
    Copy = 1 << 1,
    Move = 1 << 2,
    Link = 1 << 3,
    Aks = 1 << 4,
    Private = 1 << 5,
}
