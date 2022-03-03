using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Audacia.Mail.Mandrill
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "AV1708:Type name contains term that should be avoided", Justification = "This is a generic helper class for Mandrill.")]
    internal static class MandrillHelper
    {
        internal static readonly DefaultContractResolver ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        };

        internal static readonly StringEnumConverter Converter = new StringEnumConverter(new SnakeCaseNamingStrategy());

        internal static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = ContractResolver,
            Converters = { Converter }
        };

        internal static T GetObject<T>(string content) => JsonConvert.DeserializeObject<T>(content, SerializerSettings);

        internal static string GetJsonString<T>(T obj) => JsonConvert.SerializeObject(obj, SerializerSettings);
    }
}
