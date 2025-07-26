using System.Collections.Concurrent;

/// <summary>
/// Provides a way to set and retrieve contextual data that can be scoped either 
/// to a specific physical thread or to the logical call context, which flows 
/// across asynchronous calls.
/// 
/// - `SetData` and `GetData` use thread-local storage (`ThreadLocal<T>`) to associate 
///   data with a specific physical thread. The data will not be available if the 
///   execution moves to another thread.
/// 
/// - `LogicalSetData` and `LogicalGetData` use logical context storage (`AsyncLocal<T>`), 
///   which ensures that the data persists across asynchronous calls and is available 
///   even when the execution moves to different threads within the same logical flow.
/// </summary>
public static class CallContext
{
    static ConcurrentDictionary<string, AsyncLocal<object?>>  asyncLocalStates  = new ();
    static ConcurrentDictionary<string, ThreadLocal<object?>> threadLocalStates = new ();

    /// <summary>
    /// Stores a given object and associates it with the specified name in the thread-local context.
    /// This data is tied to the specific physical thread.
    /// </summary>
    /// <param name="name">The name with which to associate the new item in the call context.</param>
    /// <param name="data">The object to store in the call context.</param>
    public static void SetThreadData(string name, object data) =>
        threadLocalStates.GetOrAdd(name, _ => new ThreadLocal<object?>()).Value = data;

    /// <summary>
    /// Retrieves an object with the specified name from the thread-local context.
    /// The data is only available on the thread where it was stored.
    /// </summary>
    /// <param name="name">The name of the item in the call context.</param>
    /// <returns>The object in the call context associated with the specified name, or <see langword="null"/> if not found.</returns>
    public static object? GetThreadData(string name) =>
        threadLocalStates.TryGetValue(name, out var data) ? data.Value : null;
        
    /// <summary>
    /// Stores a given object and associates it with the specified name in the logical context.
    /// This data flows across asynchronous calls and thread switches within the same logical flow.
    /// </summary>
    /// <param name="name">The name with which to associate the new item in the call context.</param>
    /// <param name="data">The object to store in the call context.</param>
    public static void SetLogicalData(string name, object data) =>
        asyncLocalStates.GetOrAdd(name, _ => new AsyncLocal<object?>()).Value = data;

    /// <summary>
    /// Retrieves an object with the specified name from the logical context.
    /// The data is available across asynchronous calls and thread switches within the same logical flow.
    /// </summary>
    /// <param name="name">The name of the item in the call context.</param>
    /// <returns>The object in the call context associated with the specified name, or <see langword="null"/> if not found.</returns>
    public static object? GetLogicalData(string name) =>
        asyncLocalStates.TryGetValue(name, out var data) ? data.Value : null;


    public static object? GetContextData(string name) => GetLogicalData(name) ?? GetThreadData(name);
}