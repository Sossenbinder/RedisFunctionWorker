using System;
using Microsoft.Azure.Functions.Worker.Extensions.Abstractions;

namespace RedisListener.Context
{
	// The attribute providing the context of the trigger
	[AttributeUsage(AttributeTargets.Parameter)]
	public class RedisTriggerAttribute : TriggerBindingAttribute
	{
		public string ConnectionString { get; set; }

		public string StreamName { get; set; }

		public RedisTriggerAttribute(
			string connectionString,
			string streamName)
		{
			ConnectionString = connectionString;
			StreamName = streamName;
		}
	}
}