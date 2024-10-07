using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Servises;

public class PluginLoader
{
    public static object LoadPlugin(string dllPath)
    {
        Assembly assembly = Assembly.LoadFrom(dllPath);
        var pluginType = assembly.GetTypes().FirstOrDefault(t => t.IsClass && !t.IsAbstract);

        if (pluginType != null)
        {
            return Activator.CreateInstance(pluginType);
        }

        return null;
    }

    public static object[] LoadPlugins(string folderPath)
    {
        return Directory.GetFiles(folderPath, "*.dll")
                        .Select(LoadPlugin)
                        .Where(plugin => plugin != null)
                        .ToArray();
    }

    public static int? ExecuteMethod(object plugin, string methodName, int a, int b)
    {
        var method = plugin.GetType().GetMethod(methodName);
        if (method != null)
        {
            return (int?)method.Invoke(plugin, new object[] { a, b });
        }
        return null;
    }
}
