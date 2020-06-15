using System;
using JsonConvert = Penguin.Json.JsonConvert;

namespace Penguin.Web
{
    public partial class JsonClient
    {
        /// <summary>
        /// Posts the provided body and downloads the Uri as a string
        /// </summary>
        /// <param name="address">The Uri to download</param>
        /// <param name="body">The content to post in the body</param>
        /// <returns>The response as a string</returns>
        public virtual new string UploadString(string address, string body) => this.UploadString(new Uri(address), body);

        /// <summary>
        /// Posts the provided body and downloads the Uri as a string
        /// </summary>
        /// <param name="address">The Uri to download</param>
        /// <param name="body">The content to post in the body</param>
        /// <returns>The response as a string</returns>
        public virtual new string UploadString(Uri address, string body)
        {
            PreRequest(address);

            return base.UploadString(address, body);
        }

        /// <summary>
        /// Posts the provided body and downloads the Uri as a string
        /// </summary>
        /// <param name="address">The Uri to download</param>
        /// <param name="body">The content to post in the body</param>
        /// <returns>The response as a string</returns>
        public virtual TReturn UploadString<TReturn>(string address, string body) => this.UploadString<TReturn>(new Uri(address), body);

        /// <summary>
        /// Posts the provided body and downloads the Uri as a string
        /// </summary>
        /// <param name="address">The Uri to download</param>
        /// <param name="body">The content to post in the body</param>
        /// <returns>The response as a string</returns>
        public virtual TReturn UploadString<TReturn>(Uri address, string body)
        {
            PreRequest(address);

            return JsonConvert.DeserializeObject<TReturn>(base.UploadString(address, body));
        }
    }
}