namespace AntMe
{
    /// <summary>
    ///     Basisklasse für jegliche Item-, State- und Info-Eigenschaften.
    /// </summary>
    public abstract class Property
    {
        /// <summary>
        ///     Standard-Überladung für die Ausgabe als Text.
        /// </summary>
        /// <returns>Name der Eigenschaft</returns>
        public override string ToString()
        {
            return GetType().Name;
        }
    }
}