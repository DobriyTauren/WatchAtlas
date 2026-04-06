using WatchAtlas.Models;
using WatchAtlas.Models.Enums;

namespace WatchAtlas.State;

public class ToastState : StateStoreBase, IDisposable
{
    private readonly Dictionary<Guid, CancellationTokenSource> _dismissTokens = new();

    public IReadOnlyList<ToastMessage> Messages { get; private set; } = Array.Empty<ToastMessage>();

    public void ShowSuccess(string title, string? description = null, int durationMs = 4200)
        => Show(new ToastMessage { Tone = ToastTone.Success, Title = title, Description = description }, durationMs);

    public void ShowError(string title, string? description = null, int durationMs = 5200)
        => Show(new ToastMessage { Tone = ToastTone.Error, Title = title, Description = description }, durationMs);

    public void ShowWarning(string title, string? description = null, int durationMs = 4800)
        => Show(new ToastMessage { Tone = ToastTone.Warning, Title = title, Description = description }, durationMs);

    public void ShowInfo(string title, string? description = null, int durationMs = 4000)
        => Show(new ToastMessage { Tone = ToastTone.Info, Title = title, Description = description }, durationMs);

    public void Dismiss(Guid id)
    {
        if (_dismissTokens.Remove(id, out var tokenSource))
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }

        var remaining = Messages.Where(message => message.Id != id).ToList();
        if (remaining.Count == Messages.Count)
        {
            return;
        }

        Messages = remaining;
        NotifyStateChanged();
    }

    private void Show(ToastMessage message, int durationMs)
    {
        var nextMessages = Messages.Concat([message]).TakeLast(4).ToList();
        var removedIds = Messages
            .Where(existing => nextMessages.All(candidate => candidate.Id != existing.Id))
            .Select(existing => existing.Id)
            .ToList();

        foreach (var removedId in removedIds)
        {
            if (_dismissTokens.Remove(removedId, out var removedToken))
            {
                removedToken.Cancel();
                removedToken.Dispose();
            }
        }

        Messages = nextMessages;
        NotifyStateChanged();

        var tokenSource = new CancellationTokenSource();
        _dismissTokens[message.Id] = tokenSource;
        _ = DismissAfterDelayAsync(message.Id, durationMs, tokenSource.Token);
    }

    private async Task DismissAfterDelayAsync(Guid id, int durationMs, CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(durationMs, cancellationToken);
            Dismiss(id);
        }
        catch (OperationCanceledException)
        {
        }
    }

    public void Dispose()
    {
        foreach (var tokenSource in _dismissTokens.Values)
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }

        _dismissTokens.Clear();
    }
}
