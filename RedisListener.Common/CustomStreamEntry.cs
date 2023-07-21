using StackExchange.Redis;

namespace RedisListener.Common
{
	public class CustomStreamEntry
	{
		// The properties of the StreamEntry struct
		public string Id { get; set; }
		public NameValueEntry[] Values { get; set; }

		// Parameterless constructor
		public CustomStreamEntry()
		{
		}
	}
}