using System.Reflection;

namespace Secyud.Abp.AspNetCore.Theming;

[AttributeUsage(AttributeTargets.Class)]
public class ThemeNameAttribute(string name) : Attribute
{
    public string Name { get; set; } = name;

    public static string GetName(Type themeType)
    {
        return themeType
            .GetCustomAttribute<ThemeNameAttribute>(true)
            ?.Name ?? themeType.Name;
    }
}