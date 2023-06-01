using System.Collections.Concurrent;
using KafkaWorkshop.Solutions.Shared.Contracts;

namespace KafkaWorkshop.Solutions.UsersApi;

public class UsersDb
{
    private readonly ConcurrentDictionary<long, User> _users = new();
    
    public IEnumerable<User> GetAll() => _users.Values;

    public User? GetUser(long id) => _users.TryGetValue(id, out var user) ? user : null;

    public User AddOrUpdate(User user)
    {
        return _users.AddOrUpdate(user.Id, user, (_, _) => user);
    }
    
    public void Remove(long id)
    {
        _users.TryRemove(id, out _);
    }
}

public class UsersDbLoaderMonitor
{
    private bool _isLoaded;

    public bool Loaded => _isLoaded;

    public void SignalLoaded()
    {
        _isLoaded = true;
    }
}