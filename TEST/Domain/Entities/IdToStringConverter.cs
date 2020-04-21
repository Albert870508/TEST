using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TEST.Domain.Entities
{
    /// <summary>
    /// 在返回内容的时候 将long自动转为string类型
    /// </summary>
    public class IdToStringConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken jt = JValue.ReadFrom(reader);

            return jt.Value<long>();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(System.Int64).Equals(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }
    }
}
