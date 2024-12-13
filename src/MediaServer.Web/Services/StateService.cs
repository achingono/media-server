using MediaServer.Web.State;

namespace MediaServer.Web.Services;

public class StateService<T> : IStateService<T>
{
    private T state;

    public T State
    {
        get => state;
        set
        {
            state = value;
            OnChange?.Invoke();
        }
    }

    private ILogger Logger { get; set; }

    public StateService(T state, ILogger<StateService<T>> logger)
    {
        this.state = state;
        this.Logger = logger;
    }

    public void Patch(Action<T> action)
    {
        Logger.LogDebug("Patching state...");
        action?.Invoke(State);

        Logger.LogDebug("Notifying state change...");
        OnChange?.Invoke();
    }

    public event Action OnChange = default!;
}