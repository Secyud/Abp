using Volo.Abp.Validation.StringValues;

namespace Secyud.Abp.Features;

[Serializable]
[ValueValidator("URL")]
public class UrlValueValidator : ValueValidatorBase
{
    public string Scheme {
        get => this["Scheme"]!.ToString()!;
        set => this["Scheme"] = value;
    }

    public UrlValueValidator()
    {

    }

    public UrlValueValidator(string scheme)
    {
        Scheme = scheme;
    }

    public override bool IsValid(object? value)
    {
        var s = value?.ToString();
        return s != null && s.StartsWith(Scheme);
    }
}
