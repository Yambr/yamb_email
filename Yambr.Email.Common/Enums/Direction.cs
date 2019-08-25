using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Yambr.Email.Common.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Direction
    {
        Incoming,
        Outcoming
    }
}