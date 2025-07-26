using System.Collections.Concurrent;

namespace FortitudeCommon.Framework.Fillers;

internal class CallContextLongs
{
    static ConcurrentDictionary<string, AsyncLocal<long>>  asyncLocalStates  = new ();
    static ConcurrentDictionary<string, ThreadLocal<long>> threadLocalStates = new ();

    /// <summary>
    /// Stores a given object and associates it with the specified name in the thread-local context.
    /// This data is tied to the specific physical thread.
    /// </summary>
    /// <param name="name">The name with which to associate the new item in the call context.</param>
    /// <param name="data">The object to store in the call context.</param>
    public static void SetThreadData(string name, long data) =>
        threadLocalStates.GetOrAdd(name, _ => new ThreadLocal<long>()).Value = data;

    /// <summary>
    /// Retrieves an object with the specified name from the thread-local context.
    /// The data is only available on the thread where it was stored.
    /// </summary>
    /// <param name="name">The name of the item in the call context.</param>
    /// <returns>The object in the call context associated with the specified name, or <see langword="null"/> if not found.</returns>
    public static long? GetThreadData(string name) =>
        threadLocalStates.TryGetValue(name, out var data) ? data.Value : null;
        
    /// <summary>
    /// Stores a given object and associates it with the specified name in the logical context.
    /// This data flows across asynchronous calls and thread switches within the same logical flow.
    /// </summary>
    /// <param name="name">The name with which to associate the new item in the call context.</param>
    /// <param name="data">The object to store in the call context.</param>
    public static void SetLogicalData(string name, long data) =>
        asyncLocalStates.GetOrAdd(name, _ => new AsyncLocal<long>()).Value = data;

    /// <summary>
    /// Retrieves an object with the specified name from the logical context.
    /// The data is available across asynchronous calls and thread switches within the same logical flow.
    /// </summary>
    /// <param name="name">The name of the item in the call context.</param>
    /// <returns>The object in the call context associated with the specified name, or <see langword="null"/> if not found.</returns>
    public static long? GetLogicalData(string name) =>
        asyncLocalStates.TryGetValue(name, out var data) ? data.Value : 0;


    public static long? GetContextData(string name) => GetLogicalData(name) ?? GetThreadData(name);
}