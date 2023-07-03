using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Newtonsoft.Json;
using RedisListener.Model;

namespace RedisListener.ValueBinding
{
	internal class RedisTriggerValueProvider : IValueProvider
	{
		public Type Type { get; }

		private readonly RedisMessagePackage _messagePackage;

		public RedisTriggerValueProvider(RedisMessagePackage messagePackage, Type requestedParameterType)
		{
			Type = requestedParameterType;
			_messagePackage = messagePackage;
		}

		public Task<object> GetValueAsync()
		{
			Console.WriteLine($"Serializing to ${Type.Name}");
			return Task.FromResult<object>(ToInvokeString());
		}

		public string ToInvokeString()
		{
			return JsonConvert.SerializeObject(_messagePackage.StreamEntry);
		}
	}
}