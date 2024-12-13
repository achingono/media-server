using MediaServer.Web.State;

namespace MediaServer.Web.Services;

public interface IStateService<T>
{
    event Action OnChange;
    T State { get; set; }
    void Patch(Action<T> action);
}