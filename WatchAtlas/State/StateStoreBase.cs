namespace WatchAtlas.State;

public abstract class StateStoreBase
{
    public event Action? Changed;

    protected void NotifyStateChanged() => Changed?.Invoke();
}
