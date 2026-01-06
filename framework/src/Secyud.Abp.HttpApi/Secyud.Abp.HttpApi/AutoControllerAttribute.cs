namespace Secyud.Abp.HttpApi;

public class AutoControllerAttribute(Type type, string moduleName = "bm") : Attribute
{
    public Type Type { get; } = type;
    public string ModuleName { get; } = moduleName;
}