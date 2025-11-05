using Volo.Abp;

namespace Secyud.Abp.Account;

[Serializable]
public class ScoreBelowThresholdException(string message) : UserFriendlyException(message);
