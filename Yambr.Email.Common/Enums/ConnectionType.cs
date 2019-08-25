using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Yambr.Email.Common.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ConnectionType
    {
        IMAP = 0,
        POP3 = 1,
        SMTP = 2
    }
}