using CsTools.Extensions;

namespace GtkDotNet.SafeHandles;

public class SoupMessageHeadersHandle : BaseHandle
{
    public SoupMessageHeadersHandle() : base() {}
}

public class SoupMessageHeadersNewHandle : SoupMessageHeadersHandle
{
    public SoupMessageHeadersNewHandle() : base() {}

    protected override bool ReleaseHandle() 
        => true.SideEffect(_ => GObject.Unref(handle));
}
