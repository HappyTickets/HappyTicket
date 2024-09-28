using LanguageExt.Common;

namespace Shared.Extensions;

public static class ResultExtensions
{
    public static Result<T> ToResult<T>(this T entity) => new Result<T>(entity);
    public static Result<T> ToResult<T>(this Exception exception) => new Result<T>(exception);
    public static Result<Tout> ToResult<Tout>(this Func<Tout> callingSiteMethod) => new Result<Tout>(callingSiteMethod.Invoke());

    public static async Task<Result<T>> ToResultAsync<T>(this T entity, CancellationToken cancellationToken = default)
        => await Task.Run(() => new Result<T>(entity), cancellationToken);
    public static async Task<Result<T>> ToResultAsync<T>(this Exception exception, CancellationToken cancellationToken = default)
        => await Task.Run(() => new Result<T>(exception), cancellationToken);
    public static async Task<Result<Tout>> ToResultAsync<Tout>(this Func<Tout> callingSiteMethod, CancellationToken cancellationToken = default)
        => await Task.Run(() => new Result<Tout>(callingSiteMethod.Invoke()), cancellationToken);
    public static async Task<Result<Tout>> ToResultAsync<Tout>(this Func<Task<Tout>> callingSiteAsyncMethod, CancellationToken cancellationToken = default)
        => await Task.Run(async () => new Result<Tout>(await callingSiteAsyncMethod.Invoke()), cancellationToken);

    public static T GetValue<T>(this Result<T> result)
    {
        return result.Match<T>(
            succ => succ,
            fail =>
            {
                throw fail;
            });
    }
    public static bool TryGetValue<T>(this Result<T> result, out T? value, out Exception? exception)
    {
        bool isSuccess = false;
        var resultData = result.Match<(T?, Exception?)>(
            succ =>
            {
                isSuccess = true;
                return (succ, null);
            },
            fail =>
            {
                return (default, fail);
            });
        value = resultData.Item1;
        exception = resultData.Item2;
        return isSuccess;
    }
}
