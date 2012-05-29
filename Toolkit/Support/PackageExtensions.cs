using System;
using System.Text;
using CoApp.Packaging.Common;

namespace CoApp.Gui.Toolkit.Support
{
    public static class PackageExtensions
    {
        public static string GetNicestPossibleName(this IPackage p)
        {
            var sb = new StringBuilder();
            sb.Append(p.GetNicestName());
            sb.Append(" ");
            sb.Append(p.GetNicestVersion());
            return sb.ToString();
        }

        public static string GetNicestName(this IPackage p)
        {
            return String.IsNullOrWhiteSpace(p.DisplayName) ? p.Name : p.DisplayName;
        }

        public static string GetNicestVersion(this IPackage p)
        {
            return String.IsNullOrWhiteSpace(p.PackageDetails.AuthorVersion)
                       ? p.Version.ToString()
                       : p.PackageDetails.AuthorVersion;
        }
    }
}
