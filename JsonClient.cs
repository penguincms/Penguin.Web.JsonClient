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
    public class JsonClient : WebClient
    {
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

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public T DownloadJson<T>(string url, JsonSerializerSettings downloadSerializerSettings = null)
        {
            return JsonConvert.DeserializeObject<T>(this.DownloadString(url), downloadSerializerSettings ?? DefaultSettings);
        }

        public T UploadJson<T>(string url, object toUpload, JsonSerializerSettings downloadSerializerSettings = null, JsonSerializerSettings uploadSerializerSettings = null)
        {
            return JsonConvert.DeserializeObject<T>(this.UploadString(url, JsonConvert.SerializeObject(toUpload, uploadSerializerSettings ?? DefaultSettings)), downloadSerializerSettings ?? DefaultSettings);
        }

        public string UploadJson(string url, object toUpload, JsonSerializerSettings uploadSerializerSettings = null)
        {
            return this.UploadString(url, JsonConvert.SerializeObject(toUpload, uploadSerializerSettings ?? DefaultSettings));
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
