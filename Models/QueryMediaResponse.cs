using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace webui.Models
{
    [Serializable]
    [JsonObject]
    public class QueryMediaResponse
    {
        [JsonProperty("data")]
        public IList<ResponseData> Data{ get; set; }
    }


    [Serializable]
    [JsonObject]
    public class ResponseData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("media_type")]
        public string MediaType { get; set; }
        [JsonProperty("media_url")]
        public string MediaUrl { get; set; }
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }
}