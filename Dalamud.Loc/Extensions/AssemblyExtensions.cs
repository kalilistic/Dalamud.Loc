using System;
using System.IO;
using System.Reflection;

namespace Dalamud.Loc.Extensions;

/// <summary>
/// Assembly extensions.
/// </summary>
public static class AssemblyExtensions
{
    /// <summary>
    /// Get embedded resource content.
    /// </summary>
    /// <param name="value">assembly.</param>
    /// <param name="resourcePath">resource path.</param>
    /// <returns>resource content.</returns>
    public static string GetResourceContent(this Assembly value, string resourcePath)
    {
        var resourceStream = value.GetManifestResourceStream(resourcePath);
        if (resourceStream == null)
        {
            return string.Empty;
        }

        using var reader = new StreamReader(resourceStream ?? throw new InvalidOperationException());
        var resourceContent = reader.ReadToEnd();

        return resourceContent;
    }
}
