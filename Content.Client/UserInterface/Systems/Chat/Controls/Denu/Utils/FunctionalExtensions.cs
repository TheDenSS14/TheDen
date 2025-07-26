using System.Runtime.CompilerServices;


namespace Content.Client.UserInterface.Systems.Chat.Controls.Denu.Utils;

public static class FunctionalExtensions
{
    public static TResult With<T, TResult>(this T value, Func<T, TResult> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));
        return func(value);
    }

    public static T Also<T>(this T value, Action<T> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));
        action(value);
        return value;
    }

    public static TResult Pipe<T, TResult>(this T value, Func<T, TResult> func) => func(value);

    public static T When<T>(this T value, bool condition, Action<T> action)
    {
        if (condition) action(value);
        return value;
    }

    public static Option<TResult> Map<T, TResult>(this Option<T> option, Func<T, TResult> mapper)
    {
        return option.HasValue ? Option<TResult>.Some(mapper(option.Value)) : Option<TResult>.None();
    }

    public static Option<TResult> Bind<T, TResult>(this Option<T> option, Func<T, Option<TResult>> binder)
    {
        return option.HasValue ? binder(option.Value) : Option<TResult>.None();
    }

    public static T GetOrElse<T>(this Option<T> option, T defaultValue) => option.HasValue ? option.Value : defaultValue;

    public static T GetOrElse<T>(this Option<T> option, Func<T> defaultFunc) => option.HasValue ? option.Value : defaultFunc();

    public static Func<T2, R> Apply<T1, T2, R>(this Func<T1, T2, R> func, T1 arg1) => arg2 => func(arg1, arg2);

    public static Func<T1, Func<T2, R>> Curry<T1, T2, R>(this Func<T1, T2, R> func) => arg1 => arg2 => func(arg1, arg2);

    public static Func<T1, R> Compose<T1, T2, R>(this Func<T2, R> outer, Func<T1, T2> inner) => arg => outer(inner(arg));
}

public readonly struct Option<T>
{
    public static Option<T> Some(T value) => new Option<T>(value, true);

    public static Option<T> None() => new Option<T>(default!, false);

    private Option(T value, bool hasValue)
    {
        Value = value;
        HasValue = hasValue;
    }

    public T Value { get; }
    public bool HasValue { get; }
}