using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using GalaSoft.MvvmLight.Threading;
using CoApp.Toolkit.Extensions;
using CoApp.Gui.Toolkit.Support;

namespace CoApp.Gui.Toolkit
{
    /// <summary>
    /// 
    /// </summary>
    public class CoAppApplication : Application
    {
        private const string COAPP_PUBLIC_KEY_TOKEN = "1e373a58e25250cb";
        /// <summary>
        /// 
        /// </summary>
        public CoAppApplication()
        {
            
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ResolveAssembly);
        }

        static CoAppApplication()
        {
            DispatcherHelper.Initialize();
        }

        private static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assmName = new AssemblyName(args.Name);

            //var r = GetBestAssemblyLoaded(args);
            return null;
        }

        private static Assembly GetBestAssemblyLoaded(ResolveEventArgs args)
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assmName = new AssemblyName(args.Name);

            var test = FilterOnName(loadedAssemblies, assmName);
            var count = test.Count();
            if (count == 0)
                return null;
            if (count == 1)
                return test.First();
            

            test = FilterOnVersion(test, assmName);
            count = test.Count();
            if (count == 0)
                return null;
            if (count == 1)
                return test.First();

            test = FilterOnPkt(test, assmName);

            count = test.Count();
            if (count == 0)
            {

                return
                    test.FirstOrDefault(
                        a => a.GetName().GetPublicKeyToken().PublicKeyTokenAsString() == COAPP_PUBLIC_KEY_TOKEN);
            }
            if (count == 1)
                return test.First();


            test = FilterOnCulture(test, assmName);

            count = test.Count();
            if (count == 0)
                return null;
            if (count == 1)
                return test.First();

            

            //we didn't find a match
            return null;

        }

        private static IEnumerable<Assembly> FilterOnName(IEnumerable<Assembly> assemblies, AssemblyName assmName)
        {
            return assemblies.Where(a => a.GetName().Name == assmName.Name);
        }

        private static IEnumerable<Assembly> FilterOnVersion(IEnumerable<Assembly> assemblies, AssemblyName assmName)
        {
            if (assmName.Version != null)
            {
                var r = assemblies.Where(a => a.GetName().Version == assmName.Version);
                if (!r.Any())
                    return assemblies;
                else
                {
                    return r;
                }

            }
            else
                return assemblies;
        }

        private static IEnumerable<Assembly> FilterOnPkt(IEnumerable<Assembly> assemblies, AssemblyName assmName)
        {

            if (assmName.GetPublicKeyToken() != null)
            {
                var r = assemblies.Where(a => a.GetName().GetPublicKeyToken().SequenceEqual(assmName.GetPublicKeyToken()));
                if (!r.Any())
                    return assemblies;
                else
                {
                    return r;
                }

            }
            else
                return assemblies;
        }

        private static IEnumerable<Assembly> FilterOnCulture(IEnumerable<Assembly> assemblies, AssemblyName assmName)
        {

            if (assmName.CultureInfo != null)
            {
                var r = assemblies.Where(a => a.GetName().CultureInfo == assmName.CultureInfo);
                if (!r.Any())
                    return assemblies;
                else
                {
                    return r;
                }

            }
            else
                return assemblies;
        }
    }
}
