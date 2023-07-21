using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using StackExchange.Redis;

namespace RedisListener.Common
{
	public class NameValueEntryConverter : JsonConverter<NameValueEntry>
	{
		public override NameValueEntry Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var name = "";
			var value = "";

			while (reader.Read())
			{
				switch (reader.TokenType)
				{
					case JsonTokenType.EndObject:
						return new NameValueEntry(name, value);
					case JsonTokenType.PropertyName:
					{
						var propertyName = reader.GetString();
						reader.Read();

						switch (propertyName)
						{
							case nameof(NameValueEntry.Name):
								name = reader.GetString();
								break;
							case nameof(NameValueEntry.Value):
								value = reader.GetString();
								break;
						}

						break;
					}
				}
			}

			throw new JsonException();
		}

		public override void Write(Utf8JsonWriter writer, NameValueEntry value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();

			writer.WriteString(nameof(NameValueEntry.Name), value.Name);
			writer.WriteString(nameof(NameValueEntry.Value), value.Value);

			writer.WriteEndObject();
		}
	}
}