using System;
using Microsoft.Azure.WebJobs.Description;

namespace RedisListener.Context
{
	// The attribute providing the context of the trigger
	[AttributeUsage(AttributeTargets.Parameter)]
	[Binding]
	public class RedisTriggerAttribute : Attribute
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