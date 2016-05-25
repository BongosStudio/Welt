#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion
namespace WeltLauncher.Core
{
    public class ApiResources
    {
        private const string API_V1 = "api/v1/";
        private const string CHANGELOG = API_V1 + "changelog/";
        private const string AUTH_GET = API_V1 + "auth/get/";
        private const string AUTH_LOG = API_V1 + "auth/login/";
        private const string AUTH_REG = API_V1 + "auth/register/";

        public const string TEST_URL = "http://localhost:1823/";
        public const string TEST_CHANGELOG_URL = TEST_URL + CHANGELOG;
        public const string TEST_USER_GET_URL = TEST_URL + AUTH_GET;
        public const string TEST_USER_REG_URL = TEST_URL + AUTH_REG;
        public const string TEST_USER_LOG_URL = TEST_URL + AUTH_LOG;

        public const string RELEASE_URL = "http://weltsite.azurewebsites.net/";
        public const string RELEASE_CHANGELOG_URL = RELEASE_URL + CHANGELOG;
        public const string RELEASE_USER_GET_URL = RELEASE_URL + AUTH_GET;
        public const string RELEASE_USER_LOG_URL = RELEASE_URL + AUTH_LOG;
        public const string RELEASE_USER_REG_URL = RELEASE_URL + AUTH_REG;
    }
}