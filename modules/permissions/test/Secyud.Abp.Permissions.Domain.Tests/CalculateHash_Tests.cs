using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Shouldly;
using Volo.Abp.Json.SystemTextJson.Modifiers;
using Xunit;

namespace Secyud.Abp.Permissions;

public class CalculateHash_Tests: PermissionTestBase
{
    [Fact]
    public void Test()
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers =
                {
                    new AbpIgnorePropertiesModifiers<PermissionGroupDefinitionRecord, Guid>().CreateModifyAction(x => x.Id),
                    new AbpIgnorePropertiesModifiers<PermissionDefinitionRecord, Guid>().CreateModifyAction(x => x.Id)
                }
            }
        };
        var id = Guid.NewGuid();
        var json = JsonSerializer.Serialize(new List<PermissionGroupDefinitionRecord>()
            {
                new PermissionGroupDefinitionRecord(id, "Test", "Test")
            },
            jsonSerializerOptions);
        json.ShouldNotContain("\"Id\"");
        json.ShouldNotContain(id.ToString("D"));
        json = JsonSerializer.Serialize(new List<PermissionDefinitionRecord>()
            {
                new(id, "Test", "Test", "Test", "Test", true)
            },
            jsonSerializerOptions);
        json.ShouldNotContain("\"Id\"");
        json.ShouldNotContain(id.ToString("D"));
    }
}
