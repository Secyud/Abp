namespace SecitsDemoApp.Menus;

public static class AppMenus
{
    public const string Index = "Index";
    public const string IndexUri = "/";

    public static class Grid
    {
        public const string Name = "Grid";
        public const string Uri = "/grid";
        public const string Paged = Name + ".Paged";
        public const string PagedUri = Uri + "/paged";
        public const string Visualized = Name + ".Visualized";
        public const string VisualizedUri = Uri + "/visualized";
    }

    public static class Theme
    {
        public const string Name = "Theme";
        public const string Uri = "/theme";

        public const string Button = Name + ".Button";
        public const string ButtonUri = Uri + "/button";
        public const string Input = Name + ".Input";
        public const string InputUri = Uri + "/input";
    }

    public static class Function
    {
        public const string Name = "Function";
        public const string Uri = "/theme";

        public const string Notify = Name + ".Notify";
        public const string NotifyUri = Uri + "/notify";
        public const string Message = Name + ".Message";
        public const string MessageUri = Uri + "/message";
        public const string PageProgress = Name + ".PageProgress";
        public const string PageProgressUri = Uri + "/page-progress";
    }
}