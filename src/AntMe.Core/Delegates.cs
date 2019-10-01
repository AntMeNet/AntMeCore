namespace AntMe
{
    /// <summary>
    ///     Default Delegate for property change events.
    /// </summary>
    /// <typeparam name="T">Type of change</typeparam>
    /// <param name="newValue">New value of the property</param>
    public delegate void ValueUpdate<in T>(T newValue);

    /// <summary>
    ///     Default Delegate for all Index-based Value Changes.
    /// </summary>
    /// <typeparam name="I">Type of Index</typeparam>
    /// <typeparam name="V">Type of Value</typeparam>
    /// <param name="index">Index of the Value</param>
    /// <param name="value">Value</param>
    public delegate void ValueUpdate<in I, in V>(I index, V value);

    /// <summary>
    ///     Default Delegate for property changed events.
    /// </summary>
    /// <typeparam name="T">Type of change</typeparam>
    /// <param name="item">Affected Item</param>
    /// <param name="newValue">Final value of the property</param>
    public delegate void ValueChanged<in T>(Item item, T newValue);

    /// <summary>
    ///     Default Delegate for item change events.
    /// </summary>
    /// <param name="item">Affected Item</param>
    public delegate void ChangeItem(Item item);

    /// <summary>
    ///     Default Delegate for type item events.
    /// </summary>
    /// <typeparam name="T">Item Type</typeparam>
    /// <param name="item">Affected Item</param>
    public delegate void ChangeItem<in T>(T item);
}