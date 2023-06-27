using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Listeners;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Azure.WebJobs.Host.Triggers;
using RedisListener.Context;
using RedisListener.ValueBinding;
using StackExchange.Redis;

namespace RedisListener.Binding
{
	// Creates a listener and binds data to the result
	internal class RedisListenerTriggerBinding : ITriggerBinding
	{
		public Type TriggerValueType { get; } = typeof(StreamEntry);

		public IReadOnlyDictionary<string, Type> BindingDataContract { get; } = new Dictionary<string, Type>();

		private readonly RedisTriggerContext _triggerContext;

		public RedisListenerTriggerBinding(RedisTriggerContext triggerContext, Type parameterType)
		{
			_triggerContext = triggerContext;
			TriggerValueType = parameterType;
		}

		public Task<ITriggerData> BindAsync(object value, ValueBindingContext context)
		{
			Console.WriteLine("BindAsync");
			var valueBinder = new RedisTriggerValueProvider(value, TriggerValueType);
			return Task.FromResult<ITriggerData>(new TriggerData(valueBinder, new Dictionary<string, object>()));
		}

		public Task<IListener> CreateListenerAsync(ListenerFactoryContext context)
		{
			IListener listener = new Listener.RedisListener(context.Executor, _triggerContext);
			return Task.FromResult(listener);
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