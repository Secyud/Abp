using Volo.Abp.Validation.StringValues;

namespace Secyud.Abp.Features.JsonConverters;

public interface IValueValidatorFactory
{
    bool CanCreate(string? name);

    IValueValidator Create();
}