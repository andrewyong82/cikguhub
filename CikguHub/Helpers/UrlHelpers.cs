using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CikguHub.Helpers
{
    public static class UrlHelpers
    {
        public static string VideoUrlToEmbedUrl(string url)
        {
            if (!String.IsNullOrWhiteSpace(url) && url.Contains("youtube"))
            {
                Uri uri = new Uri(url);
                string videoId = HttpUtility.ParseQueryString(uri.Query).Get("v");
                return "https://www.youtube.com/embed/" + videoId;
            }

            return "";
        }

        public static string ChannelUrlToEmbedUrl(string url)
        {
            if (url.Contains("t.me"))
            {
                Uri uri = new Uri(url);
                string channelId = uri.PathAndQuery;

                return channelId;
            }

            return "";
        }
    }
}
