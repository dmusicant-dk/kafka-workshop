public interface ITestDataPublisher
{
    Task Run(CancellationToken token);
    void Start();
    void Stop();
}

public abstract class TestDataPublisher : ITestDataPublisher
{
    protected bool _shouldRun = true;

    public abstract Task Run(CancellationToken token);

    public void Start() => _shouldRun = true;

    public void Stop() => _shouldRun = false;
}