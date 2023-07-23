using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Listeners;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Azure.WebJobs.Host.Triggers;
using Newtonsoft.Json.Linq;
using RedisListener.Context;
using RedisListener.ValueBinding;
using StackExchange.Redis;

namespace RedisListener.Binding
{
	// Creates a listener and binds data to the result
	internal class RedisListenerTriggerBinding : ITriggerBinding
	{
		public Type TriggerValueType { get; } = typeof(StreamEntry);

		public IReadOnlyDictionary<string, Type> BindingDataContract { get; } = CreateBindingDataContract();

		private readonly RedisTriggerContext _triggerContext;

		private readonly Type _parameterType;

		public RedisListenerTriggerBinding(RedisTriggerContext triggerContext, Type parameterType)
		{
			_triggerContext = triggerContext;
			_parameterType = parameterType;
		}

		public Task<ITriggerData> BindAsync(object value, ValueBindingContext context)
		{
			var streamEntry = (StreamEntry) value;
			var valueBinder = new RedisTriggerValueProvider(streamEntry, _parameterType);
			return Task.FromResult<ITriggerData>(new TriggerData(valueBinder, CreateBindingData(streamEntry)));
		}

		public Task<IListener> CreateListenerAsync(ListenerFactoryContext context)
		{
			IListener listener = new Listener.RedisListener(context.Executor, _triggerContext);
			return Task.FromResult(listener);
		}

		internal static IReadOnlyDictionary<string, object> CreateBindingData(StreamEntry value)
		{
			var bindingData = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase) {{nameof(value), value}};

			return bindingData;
		}

		private static IReadOnlyDictionary<string, Type> CreateBindingDataContract()
		{
			var contract = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
			{
				["StreamEntry"] = typeof(StreamEntry),
			};

			return contract;
		}

		public ParameterDescriptor ToParameterDescriptor()
		{
			return new TriggerParameterDescriptor
			{
				Name = "Redis Listener",
				DisplayHints = new ParameterDisplayHints
				{
					Prompt = "Redis",
					Description = "Redis Trigger"
				}
			};
		}
	}
}