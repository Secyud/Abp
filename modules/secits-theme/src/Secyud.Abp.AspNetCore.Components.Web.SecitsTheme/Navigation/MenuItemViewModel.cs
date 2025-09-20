using Volo.Abp.UI.Navigation;

namespace Secyud.Abp.AspNetCore.Navigation
{
    public class MenuItemViewModel(ApplicationMenuItem menuItem, MenuViewModel menu)
    {
        private MenuItemViewModel? _parent;
        private readonly List<MenuItemViewModel> _children = [];

        public ApplicationMenuItem MenuItem { get; } = menuItem;
        public IReadOnlyList<MenuItemViewModel> Children => _children;
        public MenuViewModel Menu { get; } = menu;

        public int Order { get; set; }
        
        public bool IsActive { get; set; }

        public bool IsOpen { get; set; }

        public void AddChild(MenuItemViewModel item)
        {
            item.Parent = this;
        }

        public MenuItemViewModel? Parent
        {
            get => _parent;
            set
            {
                _parent?._children.Remove(this);
                _parent = value;
                _parent?._children.Add(this);
            }
        }

        public void Activate()
        {
            Parent?.Activate();
            IsActive = true;
        }

        public void Deactivate()
        {
            foreach (var childItem in Children)
            {
                childItem.Deactivate();
            }

            IsActive = false;
        }

        public void Open()
        {
            Parent?.Open();
            IsOpen = true;
        }

        public void Close()
        {
            foreach (var childItem in Children)
            {
                childItem.Close();
            }

            IsOpen = false;
        }
    }
}