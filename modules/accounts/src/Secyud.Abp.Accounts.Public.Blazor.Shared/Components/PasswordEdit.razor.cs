using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Secyud.Abp.Account.Localization;

namespace Secyud.Abp.Accounts.Components;

public partial class PasswordEdit : ComponentBase
{
    [Inject]
    protected IStringLocalizer<AbpAccountsResource> L { get; set; } = null!;

    [Parameter]
    public string Password { get; set; } = null!;

    [Parameter]
    public EventCallback<string> PasswordChanged { get; set; }

    [Parameter]
    public bool Visible { get; set; } = true;

    [Parameter]
    public bool ShowPasswordIcon { get; set; }

    [Parameter]
    public bool ShowPasswordStrength { get; set; }

    [Parameter]
    public string[] Colors { get; set; } =
    [
        "#B0284B",
        "#F2A34F",
        "#5588A4",
        "#3E5CF6",
        "#6EBD70"
    ];

    [Parameter]
    public string[] Texts { get; set; } = [];

    protected string Progress { get; set; } = "0%";

    protected string? ProgressColor { get; set; }

    protected string? ProgressText { get; set; }

    protected override void OnInitialized()
    {
        Texts =
        [
            L["Weak"].Value,
            L["Fair"].Value,
            L["Normal"].Value,
            L["Good"].Value,
            L["Strong"].Value
        ];
    }
    protected virtual void OnTextChanged(string value)
    {
        Password = value;
        PasswordChanged.InvokeAsync(value);

        if (ShowPasswordStrength)
        {
            CheckPasswordStrength(value);
        }
    }

    protected virtual void CheckPasswordStrength(string value)
    {
        if (value.IsNullOrWhiteSpace())
        {
            return;
        }

        var result = Zxcvbn.Zxcvbn.MatchPassword(value);
        var progress = result.Score;

        Progress = $"{(progress + 1) * 20}%";
        ProgressColor = Colors[progress];
        ProgressText = Texts[progress];
    }
}