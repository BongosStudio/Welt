#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using Newtonsoft.Json;

namespace WeltLauncher.Core.Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class SettingsFile
    {
        public string Version;
        public string Username;
        // TODO: load launcher colors
    }
}