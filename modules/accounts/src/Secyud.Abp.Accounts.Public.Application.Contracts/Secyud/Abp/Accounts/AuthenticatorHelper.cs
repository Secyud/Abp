using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;

namespace Secyud.Abp.Accounts;

public static class AuthenticatorHelper
{
    private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

    public static string? FormatKey(string? unformattedKey)
    {
        if (unformattedKey is null) return null;

        var result = new StringBuilder();
        var currentPosition = 0;
        while (currentPosition + 4 < unformattedKey.Length)
        {
            result.Append(unformattedKey.Substring(currentPosition, 4)).Append(' ');
            currentPosition += 4;
        }

        if (currentPosition < unformattedKey.Length)
        {
            result.Append(unformattedKey[currentPosition..]);
        }

        return result.ToString().ToLowerInvariant();
    }

    public static string GenerateQrCodeUri(string? email, string? unformattedKey, string? applicationName)
    {
        return string.Format(
            CultureInfo.InvariantCulture,
            AuthenticatorUriFormat,
            Encode(applicationName),
            Encode(email),
            unformattedKey);

        string? Encode(string? text)
        {
            return text is null ? null : UrlEncoder.Default.Encode(text);
        }
    }
}