using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using RedisListener.Common;
using StackExchange.Redis;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace RedisListener.ValueBinding
{
	internal class RedisTriggerValueProvider : IValueProvider
	{
		public Type Type { get; }

		private readonly StreamEntry _streamEntry;

		private readonly JsonSerializerOptions _options;

		public RedisTriggerValueProvider(StreamEntry streamEntry, Type requestedParameterType)
		{
			Type = requestedParameterType;
			_streamEntry = streamEntry;

			var options = new JsonSerializerOptions();
			options.Converters.Add(new StreamEntryJsonConverter());
			options.Converters.Add(new NameValueEntryConverter());
			_options = options;
		}

		public Task<object> GetValueAsync() => Task.FromResult<object>(ToInvokeString());

		public string ToInvokeString() => JsonSerializer.Serialize(_streamEntry, _options);
	}
}