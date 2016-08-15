#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion
namespace WeltLauncher.Core
{
    public class ApiResources
    {
        private const string API_V1 = "api/v1/";
        public const string Changelog = API_V1 + "changelog/";
        public const string AuthGet = API_V1 + "auth/get/";
        public const string AuthLog = API_V1 + "auth/login/";
        public const string AuthReg = API_V1 + "auth/register/";
        public const string ResxObj = API_V1 + "resx/";
        public const string ResxVer = API_V1 + "resx/version/";
        public const string ResxLauncherVer = API_V1 + "/resx/launcherversion/";
        public const string ResxDl = API_V1 + "resx/download/";
        public const string ResxLauncher = API_V1 + "resx/launcher/";

        public const string TestUrl = "http://localhost:1823/";

        public const string ReleaseUrl = "http://weltsite.azurewebsites.net/";

        public static string GetUrl(string endpoint)
        {
#if DEBUG
            return TestUrl + endpoint;
#else
            return ReleaseUrl + endpoint;
#endif
        }
    }
}