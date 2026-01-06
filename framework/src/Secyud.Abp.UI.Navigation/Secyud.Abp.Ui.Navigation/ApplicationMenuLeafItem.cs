namespace Secyud.Abp.Ui.Navigation;

public class ApplicationMenuLeafItem(
    string name,
    string displayName,
    string url,
    string? icon = null,
    int order = 1000,
    ApplicationMenuItem? parent = null)
    : ApplicationMenuItem(name, displayName, icon, order, parent)
{
    public override bool IsLeaf => true;

    public override string Url { get; } = url;
}