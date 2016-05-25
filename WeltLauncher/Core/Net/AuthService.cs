#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WeltLauncher.Core.Net
{
    public class AuthService
    {
        private HttpClient _netClient;

        public AuthService()
        {
            _netClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> AttemptLogin(string username, string password)
        {
            string url;
#if DEBUG
            url = ApiResources.TEST_USER_LOG_URL;
#else
            url = ApiResources.RELEASE_USER_LOG_URL;
#endif

            var result = await _netClient.PostAsync(url, new JsonContent(new {username, password}));
            return result;
        }
        
    }
}