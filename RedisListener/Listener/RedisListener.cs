using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Azure.WebJobs.Host.Listeners;
using RedisListener.Context;
using RedisListener.Model;

namespace RedisListener.Listener
{
	internal class RedisListener : IListener
	{
		private readonly ITriggeredFunctionExecutor _triggeredFunctionExecutor;

		private readonly RedisTriggerContext _triggerContext;

		private CancellationTokenSource _cts;

		public RedisListener(
			ITriggeredFunctionExecutor triggeredFunctionExecutor,
			RedisTriggerContext triggerContext)
		{
			_triggeredFunctionExecutor = triggeredFunctionExecutor;
			_triggerContext = triggerContext;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_cts = new CancellationTokenSource();
			_ = RunRedisListener(_cts.Token);
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			Cancel();
			return Task.CompletedTask;
		}

		public void Cancel()
		{
			_cts.Cancel();
		}

		public void Dispose()
		{
			Cancel();
		}

		private async Task RunRedisListener(CancellationToken ct)
		{
			var db = _triggerContext.ConnectionMultiplexer.GetDatabase();

			while (!ct.IsCancellationRequested)
			{
				try
				{
					var streamEntries = await db.StreamReadAsync(_triggerContext.RedisTriggerAttribute.StreamName, 0);

					await Task.WhenAll(streamEntries.Select(x => _triggeredFunctionExecutor.TryExecuteAsync(new TriggeredFunctionData
					{
						TriggerValue = new RedisMessagePackage()
						{
							StreamEntry = x
						}
					}, ct)));

					await Task.Delay(500, ct);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
				}
			}
		}
	}
}