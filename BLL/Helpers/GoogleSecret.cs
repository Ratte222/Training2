using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BLL.Helpers
{
    public class GoogleSecret
    {
        //[JsonPropertyName("client_id")]
        //[JsonProperty(PropertyName = "client_id")]
        public string client_id { get; set; }
        [JsonPropertyName("project_id")]
        public string project_id { get; set; }
        [JsonPropertyName("auth_uri")]
        public string auth_uri { get; set; }
        [JsonPropertyName("token_uri")]
        public string token_uri { get; set; }
        [JsonPropertyName("auth_provider_x509_cert_url")]
        public string /*AuthProviderX509CertUri*/auth_provider_x509_cert_url { get; set; }
        [JsonPropertyName("client_secret")]
        public string client_secret { get; set; }
        [JsonPropertyName("redirect_uris")]
        public ICollection<string> redirect_uris { get; set; }
    }
}
