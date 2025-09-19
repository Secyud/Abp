namespace SecitsDemoApp;

public static class SecitsDemoAppDbProperties
{
    public static string DbTablePrefix { get; set; } = "SecitsDemoApp";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "SecitsDemoApp";
}
