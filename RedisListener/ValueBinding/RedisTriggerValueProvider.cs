using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using RedisListener.Common;
using RedisListener.Model;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace RedisListener.ValueBinding
{
	internal class RedisTriggerValueProvider : IValueProvider
	{
		public Type Type { get; }

		private readonly RedisMessagePackage _messagePackage;

		private readonly JsonSerializerOptions _options;

		public RedisTriggerValueProvider(RedisMessagePackage messagePackage, Type requestedParameterType)
		{
			Type = requestedParameterType;
			_messagePackage = messagePackage;

			var options = new JsonSerializerOptions();
			options.Converters.Add(new StreamEntryJsonConverter());
			options.Converters.Add(new NameValueEntryConverter());
			_options = options;
		}

		public Task<object> GetValueAsync() => Task.FromResult<object>(ToInvokeString());

		public string ToInvokeString() => JsonSerializer.Serialize(new CustomStreamEntry()
		{
			Id = _messagePackage.StreamEntry.Id,
			Values = _messagePackage.StreamEntry.Values,
		}, _options);
	}
}