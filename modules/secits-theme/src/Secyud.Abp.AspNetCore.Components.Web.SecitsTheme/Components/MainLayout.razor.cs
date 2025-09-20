namespace Secyud.Abp.AspNetCore.Components;

public partial class MainLayout
{
    protected bool LftMenuCollapsed;
    protected bool RhtMenuCollapsed;


    protected string LftMenuClass()
    {
        List<string> classes =
        [
            "secits-lft-menu"
        ];
        if (LftMenuCollapsed)
            classes.Add("collapsed");

        return string.Join(' ', classes);
    }

    protected string RhtMenuClass()
    {
        List<string> classes =
        [
            "secits-rht-menu"
        ];
        if (RhtMenuCollapsed)
            classes.Add("collapsed");

        return string.Join(' ', classes);
    }
}