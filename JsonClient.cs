using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Penguin.Web
{
    /// <summary>
    /// WebClient with options to upload/download objects by converting to/from Json, for Json endpoints. Use "UploadJson" and "DownloadJson"
    /// </summary>
    public partial class JsonClient : WebClientEx
    {
        private const string ACCEPT_CONTENTTYPE = "application/json, text/plain, */*";

        private const string CONTENTTYPE = "application/json;charset=UTF-8";

        /// <summary>
        /// The default settings to use for serialization/deserialization if not otherwise specified
        /// </summary>
        public JsonSerializerSettings DefaultSettings { get; protected set; }

        /// <summary>
        /// Constructs a new instance of the serializing web client
        /// </summary>
        /// <param name="jsonSerializerSettings">The default settings to use for serialization/deserialization if not otherwise specified </param>
        public JsonClient(JsonSerializerSettings jsonSerializerSettings = null)
        {
            DefaultSettings = jsonSerializerSettings ?? new JsonSerializerSettings();
        }

        /// <summary>
        /// Download string but with Json
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response into</typeparam>
        /// <param name="url">The url to download</param>
        /// <param name="downloadSerializerSettings">The serializer settings to use when deserializing the response</param>
        /// <returns>The deserialized response</returns>
        public virtual T DownloadJson<T>(string url, JsonSerializerSettings downloadSerializerSettings = null) => DownloadJson<T>(new Uri(url), downloadSerializerSettings);

        /// <summary>
        /// Download string but with Json
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response into</typeparam>
        /// <param name="url">The url to download</param>
        /// <param name="downloadSerializerSettings">The serializer settings to use when deserializing the response</param>
        /// <returns>The deserialized response</returns>
        public virtual T DownloadJson<T>(Uri url, JsonSerializerSettings downloadSerializerSettings = null)
        {
            this.Headers[HttpRequestHeader.Accept] = ACCEPT_CONTENTTYPE;
            PreRequest(url);
            return JsonConvert.DeserializeObject<T>(this.DownloadString(url), downloadSerializerSettings ?? DefaultSettings);
        }

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
            return this.UploadString(url, "PATCH", JsonConvert.SerializeObject(toUpload, uploadSerializerSettings ?? DefaultSettings));
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
            return JsonConvert.DeserializeObject<T>(this.UploadString(url, "PATCH", JsonConvert.SerializeObject(toUpload, uploadSerializerSettings ?? DefaultSettings)), downloadSerializerSettings ?? DefaultSettings);
        }

        /// <summary>
        /// Upload string, but with Json
        /// </summary>
        /// <param name="url">The url to post to</param>
        /// <param name="toUpload">The pre-serialized object to upload</param>
        /// <param name="uploadSerializerSettings">The settings to use when serializing the uploaded object</param>
        /// <returns>The string response from the server</returns>
        public virtual string UploadJson(string url, object toUpload, JsonSerializerSettings uploadSerializerSettings = null) => UploadJson(new Uri(url), toUpload, uploadSerializerSettings);

        /// <summary>
        /// Upload string, but with Json
        /// </summary>
        /// <param name="url">The url to post to</param>
        /// <param name="toUpload">The pre-serialized object to upload</param>
        /// <param name="uploadSerializerSettings">The settings to use when serializing the uploaded object</param>
        /// <returns>The string response from the server</returns>
        public virtual string UploadJson(Uri url, object toUpload, JsonSerializerSettings uploadSerializerSettings = null)
        {
            this.Headers[HttpRequestHeader.ContentType] = CONTENTTYPE;
            this.Headers[HttpRequestHeader.Accept] = ACCEPT_CONTENTTYPE;
            PreRequest(url);
            return this.UploadString(url, JsonConvert.SerializeObject(toUpload, uploadSerializerSettings ?? DefaultSettings));
        }

        /// <summary>
        /// Upload string, but with Json
        /// </summary>
        /// <typeparam name="T">the type to deserialize the response to</typeparam>
        /// <param name="url">The url to post to</param>
        /// <param name="toUpload">The pre-serialized object to upload</param>
        /// <param name="downloadSerializerSettings">The settings to use when deserializing the response</param>
        /// <param name="uploadSerializerSettings">The settings to use when serializing the request</param>
        /// <returns>The response, deserialized</returns>
        public virtual T UploadJson<T>(string url, object toUpload, JsonSerializerSettings downloadSerializerSettings = null, JsonSerializerSettings uploadSerializerSettings = null) => UploadJson<T>(new Uri(url), toUpload, downloadSerializerSettings, uploadSerializerSettings);

        /// <summary>
        /// Upload string, but with Json
        /// </summary>
        /// <typeparam name="T">the type to deserialize the response to</typeparam>
        /// <param name="url">The url to post to</param>
        /// <param name="toUpload">The pre-serialized object to upload</param>
        /// <param name="downloadSerializerSettings">The settings to use when deserializing the response</param>
        /// <param name="uploadSerializerSettings">The settings to use when serializing the request</param>
        /// <returns>The response, deserialized</returns>
        public virtual T UploadJson<T>(Uri url, object toUpload, JsonSerializerSettings downloadSerializerSettings = null, JsonSerializerSettings uploadSerializerSettings = null)
        {
            string postBody = "";

            if (!(toUpload is null))
            {
                postBody = JsonConvert.SerializeObject(toUpload, uploadSerializerSettings ?? DefaultSettings);
            }

            this.Headers[HttpRequestHeader.Accept] = ACCEPT_CONTENTTYPE;
            this.Headers[HttpRequestHeader.ContentType] = CONTENTTYPE;
            PreRequest(url);
            return JsonConvert.DeserializeObject<T>(this.UploadString(url, postBody), downloadSerializerSettings ?? DefaultSettings);
        }

        /// <summary>
        /// Gets called before each request is made incase there is code that needs to be run each time
        /// </summary>
        /// <param name="url">The URL being requested</param>
        protected virtual void PreRequest(Uri url)
        {
        }
    }
}