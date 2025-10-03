namespace Secyud.Abp.Features;

public class FeatureGroupDto
{
    public string Name { get; set; }= "";

    public string? DisplayName { get; set; }

    public List<FeatureDto> Features { get; set; } = [];

    public string GetNormalizedGroupName()
    {
        return Name.Replace(".", "_");
    }
}
