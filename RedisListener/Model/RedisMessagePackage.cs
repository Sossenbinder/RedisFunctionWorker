using StackExchange.Redis;

namespace RedisListener.Model
{
	public class RedisMessagePackage
	{
		public StreamEntry StreamEntry { get; set; }
	}
}