using Confluent.Kafka;
using KafkaWorkshop.Exercises.Shared.Contracts;

namespace KafkaWorkshop.Exercises.Consumer.BasicStructure;

public class UsersConsumerService : BackgroundService {
	
	private readonly IConsumer<long ,User?> _consumer;
	
	/*
	 * How can you make this class get the consumer configuration
	 * from DI? Add that here (hint: you'll need a constructor)
	 */
	
	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		/*
		 * How would you move your consumption code into here, and use the
		 * cancellation token with the task?
		 */
		
		throw new NotImplementedException();
	}
	
	public override async Task StopAsync(CancellationToken cancellationToken)
	{
		await base.StopAsync(cancellationToken);
        
		_consumer.Close();
		_consumer.Dispose();
	}
}