namespace KafkaWorkshop.Exercises.Shared.Contracts; 

public record User(long Id, string FirstName, string LastName, DateTimeOffset DateOfBirth);