namespace AntMe
{
    /// <summary>
    /// Standard Delegat für Property Change Events.
    /// </summary>
    /// <typeparam name="T">Datentyp der Änderung</typeparam>
    /// <param name="newValue">Neuer Wert der Eigenschaft</param>
    public delegate void ValueUpdate<in T>(T newValue);

    /// <summary>
    /// Default Delegate for all Index-based Value Changes.
    /// </summary>
    /// <typeparam name="I">Type of Index</typeparam>
    /// <typeparam name="V">Type of Value</typeparam>
    /// <param name="index">Index of the Value</param>
    /// <param name="value">Value</param>
    public delegate void ValueUpdate<in I, in V>(I index, V value);

    /// <summary>
    /// Standard Delegat für Property Changed Events.
    /// </summary>
    /// <typeparam name="T">Datentyp der Änderung</typeparam>
    /// <param name="item">Betroffenes Item</param>
    /// <param name="newValue">Finaler Wert der Eigenschaft</param>
    public delegate void ValueChanged<in T>(Item item, T newValue);

    /// <summary>
    /// Standard Delegat für Item Change Events.
    /// </summary>
    /// <param name="item">Betroffenes Item</param>
    public delegate void ChangeItem(Item item);

    /// <summary>
    /// Standard Delegat für typisierte Item Events.
    /// </summary>
    /// <typeparam name="T">Item Type</typeparam>
    /// <param name="item">Betroffenes Item</param>
    public delegate void ChangeItem<in T>(T item);
}