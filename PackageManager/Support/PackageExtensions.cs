using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoApp.Packaging.Client;

namespace CoApp.PackageManager.Support
{
    public static class PackageExtensions
    {
        public static string GetNicestPossibleName(this Package p)
        {
            var sb = new StringBuilder();
            sb.Append(!String.IsNullOrWhiteSpace(p.DisplayName) ? p.Name : p.DisplayName);
            sb.Append(" ");
            sb.Append(!String.IsNullOrWhiteSpace(p.PackageDetails.AuthorVersion) ? p.Version.ToString() : p.PackageDetails.AuthorVersion);
            return sb.ToString();
        }
    }
}
