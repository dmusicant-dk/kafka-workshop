namespace KafkaWorkshop.Solutions.Shared.Contracts;

public record User(long Id, string FirstName, string LastName, DateTimeOffset DateOfBirth);