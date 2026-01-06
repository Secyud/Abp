namespace Secyud.Abp.Ui.Navigation;

public class ApplicationMenuItemList : List<ApplicationMenuItem>
{
    public ApplicationMenuItemList()
    {
    }

    public ApplicationMenuItemList(int capacity)
        : base(capacity)
    {
    }

    public ApplicationMenuItemList(IEnumerable<ApplicationMenuItem> collection)
        : base(collection)
    {
    }

    public void Normalize()
    {
        RemoveAll(item => item.IsLeaf && item.Url.IsNullOrEmpty());
        Sort((x, y) => x.Order.CompareTo(y));
    }
}