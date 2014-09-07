using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace Boomers
{
    [SuppressUnmanagedCodeSecurity]
    internal static class SafeNativeMethods
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int StrCmpLogicalW(string psz1, string psz2);
    }

    public sealed class NaturalStringComparer : IComparer
    {
        public static readonly NaturalStringComparer Default = new NaturalStringComparer();

        public int Compare(object x, object y)
        {
            return SafeNativeMethods.StrCmpLogicalW((string)x, (string)y);
        }
    }

    public sealed class NaturalFileInfoNameComparer : IComparer
    {
        public static readonly NaturalFileInfoNameComparer Default = new NaturalFileInfoNameComparer();

        public int Compare(object x, object y)
        {
            return SafeNativeMethods.StrCmpLogicalW(((FileInfo)x).Name, ((FileInfo)y).Name);
        }
    }
}