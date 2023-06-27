using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;

namespace RedisListener.ValueBinding
{
	internal class RedisTriggerValueProvider : IValueProvider
	{
		public Type Type { get; }

		private object _value;

		public RedisTriggerValueProvider(object value, Type type)
		{
			Type = type;
			_value = value;
		}

		public Task<object> GetValueAsync()
		{
			if (Type == typeof(string))
			{
				return Task.FromResult<object>(ToInvokeString());
			}

			return Task.FromResult(_value);
		}

		public string ToInvokeString()
		{
			return _value.ToString();
		}
	}
}