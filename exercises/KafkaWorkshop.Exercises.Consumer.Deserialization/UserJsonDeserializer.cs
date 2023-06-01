using KafkaWorkshop.Exercises.Shared.Contracts;

namespace KafkaWorkshop.Exercises.Consumer.Deserialization;

using Confluent.Kafka;

// You can move this class to the KafkaWorkshop.Exercises.Shared project later and reuse it in the other exercises
public class UserJsonDeserializer: IDeserializer<User> {
	
	public User Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context) {
		// Put your deserialization code here
		throw new NotImplementedException();
	}
}