using Newtonsoft.Json;
using Penguin.Json.JsonConverters;
using Penguin.Web.Http;
using System;
using System.Collections.Generic;
using System.Net;
using JsonConvert = Penguin.Json.JsonConvert;

namespace Penguin.Web
{
    /// <summary>
    /// WebClient with options to upload/download objects by converting to/from Json, for Json endpoints. Use "UploadJson" and "DownloadJson"
    /// </summary>
    public partial class JsonClient : WebClientEx
    {
        /// <summary>
        /// The default settings to use for serialization/deserialization if not otherwise specified
        /// </summary>
        public JsonSerializerSettings DefaultSettings { get; protected set; }

        /// <summary>
        /// Enables odata=verbose flag on content type
        /// </summary>
        public bool ODataVerbose { get; set; }

        private string JsonAcceptContentType => !this.ODataVerbose ? "application/json, text/plain, */*" : "application/json;odata=verbose";
        private string JsonContentType => !this.ODataVerbose ? "application/json;charset=UTF-8" : "application/json;odata=verbose";

        /// <summary>
        /// Constructs a new instance of the serializing web client
        /// </summary>
        /// <param name="jsonSerializerSettings">The default settings to use for serialization/deserialization if not otherwise specified </param>
        public JsonClient(JsonSerializerSettings jsonSerializerSettings) => this.DefaultSettings = jsonSerializerSettings;

        /// <summary>
        /// Constructs a new instance of the serializing web client
        /// </summary>
        public JsonClient() : this(new JsonSerializerSettings
        {
            Converters = new List<JsonConverter>()
            {
                new ConditionalValueConverter()
            }
        })
        {
        }

        /// <summary>
        /// Uploads an HttpQuery object and returns a JsonObject
        /// </summary>
        /// <typeparam name="TReturn">The return type of the post</typeparam>
        /// <param name="url">The url to post to</param>
        /// <param name="query">The query object to upload</param>
        /// <returns>A JsonObject representation of the return</returns>
        public TReturn UploadHttpQuery<TReturn>(string url, HttpQuery query)
        {
            this.Headers[HttpRequestHeader.ContentType] = this.FormContentType;

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException($"'{nameof(url)}' cannot be null or whitespace.", nameof(url));
            }

            if (query is null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return this.UploadJson<TReturn>(new Uri(url), query.ToString());
        }
        /// <summary>
        /// Download string but with Json
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response into</typeparam>
        /// <param name="url">The url to download</param>
        /// <param name="downloadSerializerSettings">The serializer settings to use when deserializing the response</param>
        /// <returns>The deserialized response</returns>
        public virtual T DownloadJson<T>(string url, JsonSerializerSettings downloadSerializerSettings = null) => this.DownloadJson<T>(new Uri(url), downloadSerializerSettings);

        /// <summary>
        /// Download string but with Json
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response into</typeparam>
        /// <param name="url">The url to download</param>
        /// <param name="downloadSerializerSettings">The serializer settings to use when deserializing the response</param>
        /// <returns>The deserialized response</returns>
        public virtual T DownloadJson<T>(Uri url, JsonSerializerSettings downloadSerializerSettings = null)
        {
            this.Headers[HttpRequestHeader.Accept] = this.JsonAcceptContentType;
            this.PreRequest(url);
            return JsonConvert.DeserializeObject<T>(this.DownloadString(url), downloadSerializerSettings ?? this.DefaultSettings);
        }

        /// <summary>
        /// Upload string, but with Json
        /// </summary>
        /// <typeparam name="T">the type to deserialize the response to</typeparam>
        /// <param name="url">The url to post to</param>
        /// <param name="toUpload">The pre-serialized object to upload</param>
        /// <param name="downloadSerializerSettings">The settings to use when deserializing the response</param>
        /// <returns>The response, deserialized</returns>
        public virtual T UploadJson<T>(string url, string toUpload, JsonSerializerSettings downloadSerializerSettings = null) => this.UploadJson<T>(new Uri(url), toUpload, downloadSerializerSettings);

        /// <summary>
        /// Upload string, but with Json
        /// </summary>
        /// <param name="url">The url to post to</param>
        /// <param name="toUpload">The serialized object to upload</param>
        /// <returns>The response, deserialized</returns>
        public virtual string UploadJson(string url, string toUpload) => this.UploadJson(new Uri(url), toUpload);

        /// <summary>
        /// Upload string, but with Json
        /// </summary>
        /// <param name="url">The url to post to</param>
        /// <param name="toUpload">The serialized object to upload</param>
        /// <returns>The response, deserialized</returns>
        public virtual string UploadJson(Uri url, string toUpload)
        {
            this.Headers[HttpRequestHeader.ContentType] = this.JsonContentType;
            this.Headers[HttpRequestHeader.Accept] = this.JsonAcceptContentType;
            this.PreRequest(url);
            return this.UploadString(url, toUpload);
        }


        /// <summary>
        /// Upload string, but with Json
        /// </summary>
        /// <typeparam name="T">the type to deserialize the response to</typeparam>
        /// <param name="url">The url to post to</param>
        /// <param name="toUpload">The pre-serialized object to upload</param>
        /// <param name="downloadSerializerSettings">The settings to use when deserializing the response</param>
        /// <returns>The response, deserialized</returns>
        public virtual T UploadJson<T>(Uri url, string toUpload, JsonSerializerSettings downloadSerializerSettings = null)
        {
            this.Headers[HttpRequestHeader.Accept] = this.JsonAcceptContentType;
            this.PreRequest(url);
            return JsonConvert.DeserializeObject<T>(this.UploadString(url, toUpload), downloadSerializerSettings ?? this.DefaultSettings);
        }

        /// <summary>
        /// Upload string, but with Json
        /// </summary>
        /// <param name="url">The url to post to</param>
        /// <param name="toUpload">The pre-serialized object to upload</param>
        /// <param name="uploadSerializerSettings">The settings to use when serializing the uploaded object</param>
        /// <returns>The string response from the server</returns>
        public virtual string UploadJson(string url, object toUpload, JsonSerializerSettings uploadSerializerSettings = null) => this.UploadJson(new Uri(url), toUpload, uploadSerializerSettings);

        /// <summary>
        /// Upload string, but with Json
        /// </summary>
        /// <param name="url">The url to post to</param>
        /// <param name="toUpload">The pre-serialized object to upload</param>
        /// <param name="uploadSerializerSettings">The settings to use when serializing the uploaded object</param>
        /// <returns>The string response from the server</returns>
        public virtual string UploadJson(Uri url, object toUpload, JsonSerializerSettings uploadSerializerSettings = null)
        {
            this.Headers[HttpRequestHeader.ContentType] = this.JsonContentType;
            this.Headers[HttpRequestHeader.Accept] = this.JsonAcceptContentType;
            this.PreRequest(url);
            return this.UploadString(url, JsonConvert.SerializeObject(toUpload, uploadSerializerSettings ?? this.DefaultSettings));
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
        public virtual T UploadJson<T>(string url, object toUpload, JsonSerializerSettings downloadSerializerSettings = null, JsonSerializerSettings uploadSerializerSettings = null) => this.UploadJson<T>(new Uri(url), toUpload, downloadSerializerSettings, uploadSerializerSettings);

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
                postBody = JsonConvert.SerializeObject(toUpload, uploadSerializerSettings ?? this.DefaultSettings);
            }

            this.Headers[HttpRequestHeader.Accept] = this.JsonAcceptContentType;
            this.Headers[HttpRequestHeader.ContentType] = this.JsonContentType;
            this.PreRequest(url);

            string response = this.UploadString(url, postBody);

            return JsonConvert.DeserializeObject<T>(response, downloadSerializerSettings ?? this.DefaultSettings);
        }

        /// <summary>
        /// Gets called before each request is made incase there is code that needs to be run each time
        /// </summary>
        /// <param name="url">The URL being requested</param>
        protected virtual void PreRequest(Uri url)
        {
        }

        public TReturn UploadHttpQuery<TReturn>(Uri url, HttpQuery query)
        {
            throw new NotImplementedException();
        }
    }
}