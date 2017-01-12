#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeltLauncher.Core.Net
{
    /// <summary>
    /// Http client content helper that will take either json string content, 
    /// a jtoken json object, or a serializable object, renders it to a
    /// string, and adds an application/json accept header
    /// </summary>
    public class JsonContent : StringContent
    {
        /// <summary>
        /// Http content created from a json-formatted string
        /// </summary>
        /// <param name="json">A json-formatted string to be passed to the server</param>
        public JsonContent(string json)
            : base(json)
        {
            Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        /// <summary>
        /// Http content created from a jtoken, which will be rendered to a string content
        /// </summary>
        /// <param name="json">Json content to be rendered to the server</param>
        public JsonContent(JToken json)
            : this(json.ToString(Formatting.None))
        {
        }

        /// <summary>
        /// Json content rendered from a serializable object
        /// </summary>
        /// <param name="json">Object to be serialized into a string</param>
        public JsonContent(object json)
            : this(JObject.FromObject(json).ToString(Formatting.None))
        {
        }
    }
}