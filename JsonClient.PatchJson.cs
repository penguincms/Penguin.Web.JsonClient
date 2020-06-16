using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Penguin.Web
{
    public partial class JsonClient
    {
        /// <summary>
        /// Upload string, but with Json. Uses the PATCH method
        /// </summary>
        /// <param name="url">The url to post to</param>
        /// <param name="toUpload">The pre-serialized object to upload</param>
        /// <param name="uploadSerializerSettings">The settings to use when serializing the uploaded object</param>
        /// <returns>The string response from the server</returns>
        public virtual string PatchJson(string url, object toUpload, JsonSerializerSettings uploadSerializerSettings = null) => PatchJson(new Uri(url), toUpload, uploadSerializerSettings);

        /// <summary>
        /// Upload string, but with Json. Uses the PATCH method
        /// </summary>
        /// <param name="url">The url to post to</param>
        /// <param name="toUpload">The pre-serialized object to upload</param>
        /// <param name="uploadSerializerSettings">The settings to use when serializing the uploaded object</param>
        /// <returns>The string response from the server</returns>
        public virtual string PatchJson(Uri url, object toUpload, JsonSerializerSettings uploadSerializerSettings = null)
        {
            this.Headers[HttpRequestHeader.ContentType] = CONTENTTYPE;
            this.Headers[HttpRequestHeader.Accept] = ACCEPT_CONTENTTYPE;
            PreRequest(url);
            return this.UploadString(url, "PATCH", toUpload is null ? string.Empty : JsonConvert.SerializeObject(toUpload, uploadSerializerSettings ?? DefaultSettings));
        }

        /// <summary>
        /// Upload string, but with Json. Uses the PATCH method
        /// </summary>
        /// <typeparam name="T">the type to deserialize the response to</typeparam>
        /// <param name="url">The url to post to</param>
        /// <param name="toUpload">The pre-serialized object to upload</param>
        /// <param name="downloadSerializerSettings">The settings to use when deserializing the response</param>
        /// <param name="uploadSerializerSettings">The settings to use when serializing the request</param>
        /// <returns>The response, deserialized</returns>
        public virtual T PatchJson<T>(string url, object toUpload, JsonSerializerSettings downloadSerializerSettings = null, JsonSerializerSettings uploadSerializerSettings = null) => PatchJson<T>(new Uri(url), toUpload, downloadSerializerSettings, uploadSerializerSettings);

        /// <summary>
        /// Upload string, but with Json. Uses the PATCH method
        /// </summary>
        /// <typeparam name="T">the type to deserialize the response to</typeparam>
        /// <param name="url">The url to post to</param>
        /// <param name="toUpload">The pre-serialized object to upload</param>
        /// <param name="downloadSerializerSettings">The settings to use when deserializing the response</param>
        /// <param name="uploadSerializerSettings">The settings to use when serializing the request</param>
        /// <returns>The response, deserialized</returns>
        public virtual T PatchJson<T>(Uri url, object toUpload, JsonSerializerSettings downloadSerializerSettings = null, JsonSerializerSettings uploadSerializerSettings = null)
        {
            this.Headers[HttpRequestHeader.Accept] = ACCEPT_CONTENTTYPE;
            this.Headers[HttpRequestHeader.ContentType] = CONTENTTYPE;
            PreRequest(url);
            return JsonConvert.DeserializeObject<T>(this.UploadString(url, "PATCH", toUpload is null ? string.Empty : JsonConvert.SerializeObject(toUpload, uploadSerializerSettings ?? DefaultSettings)), downloadSerializerSettings ?? DefaultSettings);
        }
    }
}
