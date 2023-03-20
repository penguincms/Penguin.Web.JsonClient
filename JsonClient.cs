using Loxifi;
using Newtonsoft.Json;
using Penguin.Json.JsonConverters;
using Penguin.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using JsonConvert = Penguin.Json.JsonConvert;

namespace Penguin.Web
{
    /// <summary>
    /// WebClient with options to upload/download objects by converting to/from Json, for Json endpoints. Use "UploadJson" and "DownloadJson"
    /// </summary>
    public partial class JsonClient : WebClientEx
    {
        private static readonly object threadSafetyLock = new();

        /// <summary>
        /// The default settings to use for serialization/deserialization if not otherwise specified
        /// </summary>
        public JsonSerializerSettings DefaultSettings { get; protected set; }

        /// <summary>
        /// Enables odata=verbose flag on content type
        /// </summary>
        public bool ODataVerbose { get; set; }

        private string JsonAcceptContentType => !ODataVerbose ? "application/json, text/plain, */*" : "application/json;odata=verbose";

        private string JsonContentType => !ODataVerbose ? "application/json;charset=UTF-8" : "application/json;odata=verbose";

        /// <summary>
        /// Constructs a new instance of the serializing web client
        /// </summary>
        /// <param name="jsonSerializerSettings">The default settings to use for serialization/deserialization if not otherwise specified </param>
        public JsonClient(JsonSerializerSettings jsonSerializerSettings)
        {
            DefaultSettings = jsonSerializerSettings ?? throw new ArgumentNullException(nameof(jsonSerializerSettings));

            lock (threadSafetyLock)
            {
                if (!DefaultSettings.Converters.OfType<IJsonPopulatedObjectConverter>().Any())
                {
                    DefaultSettings.Converters.Add(new IJsonPopulatedObjectConverter(true));
                }
            }
        }

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
        /// Download string but with Json
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response into</typeparam>
        /// <param name="url">The url to download</param>
        /// <param name="downloadSerializerSettings">The serializer settings to use when deserializing the response</param>
        /// <returns>The deserialized response</returns>
        public virtual T DownloadJson<T>(string url, JsonSerializerSettings downloadSerializerSettings = null)
        {
            return DownloadJson<T>(new Uri(url), downloadSerializerSettings);
        }

        /// <summary>
        /// Download string but with Json
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response into</typeparam>
        /// <param name="url">The url to download</param>
        /// <param name="downloadSerializerSettings">The serializer settings to use when deserializing the response</param>
        /// <returns>The deserialized response</returns>
        public virtual T DownloadJson<T>(Uri url, JsonSerializerSettings downloadSerializerSettings = null)
        {
            Headers[HttpRequestHeader.Accept] = JsonAcceptContentType;
            PreRequest(url);

            return JsonConvert.DeserializeObject<T>(DownloadString(url), downloadSerializerSettings ?? DefaultSettings);
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
            Headers[HttpRequestHeader.ContentType] = FormContentType;

            return string.IsNullOrWhiteSpace(url)
                ? throw new ArgumentException($"'{nameof(url)}' cannot be null or whitespace.", nameof(url))
                : query is null ? throw new ArgumentNullException(nameof(query)) : UploadJson<TReturn>(new Uri(url), query.ToString());
        }

        public TReturn UploadHttpQuery<TReturn>(Uri url, HttpQuery query)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Upload string, but with Json
        /// </summary>
        /// <typeparam name="T">the type to deserialize the response to</typeparam>
        /// <param name="url">The url to post to</param>
        /// <param name="toUpload">The pre-serialized object to upload</param>
        /// <param name="downloadSerializerSettings">The settings to use when deserializing the response</param>
        /// <returns>The response, deserialized</returns>
        public virtual T UploadJson<T>(string url, string toUpload, JsonSerializerSettings downloadSerializerSettings = null)
        {
            return UploadJson<T>(new Uri(url), toUpload, downloadSerializerSettings);
        }

        /// <summary>
        /// Upload string, but with Json
        /// </summary>
        /// <param name="url">The url to post to</param>
        /// <param name="toUpload">The serialized object to upload</param>
        /// <returns>The response, deserialized</returns>
        public virtual string UploadJson(string url, string toUpload)
        {
            return UploadJson(new Uri(url), toUpload);
        }

        /// <summary>
        /// Upload string, but with Json
        /// </summary>
        /// <param name="url">The url to post to</param>
        /// <param name="toUpload">The serialized object to upload</param>
        /// <returns>The response, deserialized</returns>
        public virtual string UploadJson(Uri url, string toUpload)
        {
            Headers[HttpRequestHeader.ContentType] = JsonContentType;
            Headers[HttpRequestHeader.Accept] = JsonAcceptContentType;
            PreRequest(url);
            return UploadString(url, toUpload);
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
            Headers[HttpRequestHeader.Accept] = JsonAcceptContentType;
            PreRequest(url);
            return JsonConvert.DeserializeObject<T>(UploadString(url, toUpload), downloadSerializerSettings ?? DefaultSettings);
        }

        /// <summary>
        /// Upload string, but with Json
        /// </summary>
        /// <param name="url">The url to post to</param>
        /// <param name="toUpload">The pre-serialized object to upload</param>
        /// <param name="uploadSerializerSettings">The settings to use when serializing the uploaded object</param>
        /// <returns>The string response from the server</returns>
        public virtual string UploadJson(string url, object toUpload, JsonSerializerSettings uploadSerializerSettings = null)
        {
            return UploadJson(new Uri(url), toUpload, uploadSerializerSettings);
        }

        /// <summary>
        /// Upload string, but with Json
        /// </summary>
        /// <param name="url">The url to post to</param>
        /// <param name="toUpload">The pre-serialized object to upload</param>
        /// <param name="uploadSerializerSettings">The settings to use when serializing the uploaded object</param>
        /// <returns>The string response from the server</returns>
        public virtual string UploadJson(Uri url, object toUpload, JsonSerializerSettings uploadSerializerSettings = null)
        {
            Headers[HttpRequestHeader.ContentType] = JsonContentType;
            Headers[HttpRequestHeader.Accept] = JsonAcceptContentType;
            PreRequest(url);
            return UploadString(url, JsonConvert.SerializeObject(toUpload, uploadSerializerSettings ?? DefaultSettings));
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
        public virtual T UploadJson<T>(string url, object toUpload, JsonSerializerSettings downloadSerializerSettings = null, JsonSerializerSettings uploadSerializerSettings = null)
        {
            return UploadJson<T>(new Uri(url), toUpload, downloadSerializerSettings, uploadSerializerSettings);
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
        public virtual T UploadJson<T>(Uri url, object toUpload, JsonSerializerSettings downloadSerializerSettings = null, JsonSerializerSettings uploadSerializerSettings = null)
        {
            string postBody = "";

            if (toUpload is not null)
            {
                postBody = JsonConvert.SerializeObject(toUpload, uploadSerializerSettings ?? DefaultSettings);
            }

            Headers[HttpRequestHeader.Accept] = JsonAcceptContentType;
            Headers[HttpRequestHeader.ContentType] = JsonContentType;
            PreRequest(url);

            string response = UploadString(url, postBody);

            return JsonConvert.DeserializeObject<T>(response, downloadSerializerSettings ?? DefaultSettings);
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