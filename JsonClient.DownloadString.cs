using System;

namespace Penguin.Web
{
    public partial class JsonClient
    {
        /// <summary>
        /// Downloads the Uri as a string
        /// </summary>
        /// <param name="address">The Uri to download</param>
        /// <returns>The response as a string</returns>
        public new virtual string DownloadString(string address) => this.DownloadString(new Uri(address));

        /// <summary>
        /// Downloads the Uri as a string
        /// </summary>
        /// <param name="address">The Uri to download</param>
        /// <returns>The response as a string</returns>
        public new virtual string DownloadString(Uri address)
        {
            this.PreRequest(address);

            return base.DownloadString(address);
        }
    }
}