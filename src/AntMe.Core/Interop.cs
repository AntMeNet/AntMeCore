namespace AntMe
{
    /// <summary>
    /// Base class of every Interop-Class.
    /// </summary>
    public abstract class Interop : PropertyList<InteropProperty>
    {
        internal void InternalUpdate(int round)
        {
            Update(round);
            foreach (var property in Properties)
                property.InternalUpdate(round);
        }

        /// <summary>
        /// Protected Update-Call.
        /// </summary>
        protected virtual void Update(int round) { }
    }
}
