#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion
namespace WeltLauncher.Core
{
    public class ApiResources
    {
        private const string API_V1 = "api/v1/";
        public const string CHANGELOG = API_V1 + "changelog/";
        public const string AUTH_GET = API_V1 + "auth/get/";
        public const string AUTH_LOG = API_V1 + "auth/login/";
        public const string AUTH_REG = API_V1 + "auth/register/";
        public const string RESX_OBJ = API_V1 + "resx/";
        public const string RESX_VER = API_V1 + "resx/version/";
        public const string RESX_LAUNCHER_VER = API_V1 + "/resx/launcherversion/";
        public const string RESX_DL = API_V1 + "resx/download/";
        public const string RESX_LAUNCHER = API_V1 + "resx/launcher/";

        public const string TEST_URL = "http://localhost:1823/";

        public const string RELEASE_URL = "http://weltsite.azurewebsites.net/";

        public static string GetUrl(string endpoint)
        {
#if DEBUG
            return TEST_URL + endpoint;
#else
            return RELEASE_URL + endpoint;
#endif
        }
    }
}