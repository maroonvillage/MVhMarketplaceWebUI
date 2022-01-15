using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace webui.Models
{
    [JsonObject]
    public partial class BlockContent
    {

        [JsonProperty]
        public string ContentName { get; set; }
        [JsonProperty]
        public string ContentValue { get; set; }
        [JsonProperty]
        public string DynamicContentType { get; set; }

    }

}
