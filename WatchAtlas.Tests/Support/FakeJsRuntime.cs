using Microsoft.JSInterop;

namespace WatchAtlas.Tests.Support;

internal sealed class FakeJsRuntime : IJSRuntime
{
    private readonly Dictionary<string, string?> _storage = new(StringComparer.Ordinal);

    public void Seed(string key, string? value)
    {
        if (value is null)
        {
            _storage.Remove(key);
            return;
        }

        _storage[key] = value;
    }

    public string? GetStoredValue(string key)
        => _storage.TryGetValue(key, out var value) ? value : null;

    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
        => InvokeAsync<TValue>(identifier, CancellationToken.None, args);

    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
    {
        cancellationToken.ThrowIfCancellationRequested();

        switch (identifier)
        {
            case "watchAtlasStorage.getItem":
                return FromValue<TValue>(GetArgument<string>(args, 0) is { } key && _storage.TryGetValue(key, out var value)
                    ? value
                    : null);

            case "watchAtlasStorage.setItem":
                _storage[GetArgument<string>(args, 0)] = GetArgument<string>(args, 1);
                return FromValue<TValue>(default);

            case "watchAtlasStorage.removeItem":
                _storage.Remove(GetArgument<string>(args, 0));
                return FromValue<TValue>(default);

            default:
                throw new InvalidOperationException($"Unsupported JS interop identifier '{identifier}'.");
        }
    }

    private static T GetArgument<T>(object?[]? args, int index)
    {
        if (args is null || index >= args.Length || args[index] is not T value)
        {
            throw new InvalidOperationException($"Argument {index} was expected to be of type '{typeof(T).Name}'.");
        }

        return value;
    }

    private static ValueTask<TValue> FromValue<TValue>(object? value)
        => new(value is null ? default! : (TValue)value);
}
