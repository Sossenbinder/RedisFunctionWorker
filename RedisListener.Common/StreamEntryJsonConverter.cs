using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using StackExchange.Redis;

namespace RedisListener.Common
{
	public class StreamEntryJsonConverter : JsonConverter<StreamEntry>
	{
		public override StreamEntry Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var id = "";
			var values = Array.Empty<NameValueEntry>();

			while (reader.Read())
			{
				switch (reader.TokenType)
				{
					case JsonTokenType.EndObject:
						return new StreamEntry(id, values);
					case JsonTokenType.PropertyName:
					{
						var propertyName = reader.GetString();

						reader.Read(); // advance to value

						switch (propertyName)
						{
							case nameof(StreamEntry.Id):
								id = reader.GetString();
								break;
							case nameof(StreamEntry.Values):
								values = JsonSerializer.Deserialize<NameValueEntry[]>(ref reader, options);
								break;
						}

						break;
					}
				}
			}

			throw new JsonException();
		}

		public override void Write(Utf8JsonWriter writer, StreamEntry value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();

			writer.WriteString(nameof(StreamEntry.Id), value.Id.ToString());

			writer.WritePropertyName(nameof(StreamEntry.Values));
			JsonSerializer.Serialize(writer, value.Values, options);

			writer.WriteEndObject();
		}
	}
}