using Volo.Abp.Validation.StringValues;

namespace Secyud.Abp.Features.JsonConverters;

public class ValueValidatorFactory<TValueValidator>(string name) : IValueValidatorFactory
    where TValueValidator : IValueValidator, new()
{
    protected readonly string Name = name;

    public bool CanCreate(string? name)
    {
        return Name == name;
    }

    public IValueValidator Create()
    {
        return new TValueValidator();
    }
}
