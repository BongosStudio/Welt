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
            var url = ApiResources.GetUrl(ApiResources.AuthLog);

            var result = await _netClient.PostAsync(url, new JsonContent(new {username, password}));
            return result;
        }
        
    }
}