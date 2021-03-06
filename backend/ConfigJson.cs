﻿using Newtonsoft.Json;

namespace backend
{
    public struct ConfigJson
    {
        [JsonProperty("token")]
        public string Token { get; private set; }
        [JsonProperty("prefix")]
        public string Prefix { get; private set; }
        [JsonProperty("rootGuildId")]
        public ulong RootGuildId { get; set; }
    }
}