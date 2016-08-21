namespace AntMe
{
    /// <summary>
    /// Base Class for all Interop Classes.
    /// </summary>
    public abstract class Interop : PropertyList<InteropProperty>
    {
        internal void InternalUpdate()
        {
            OnUpdate();
            foreach (var property in Properties)
                property.OnUpdate();
        }

        /// <summary>
        /// Update
        /// </summary>
        protected virtual void OnUpdate() { }
    }
}
