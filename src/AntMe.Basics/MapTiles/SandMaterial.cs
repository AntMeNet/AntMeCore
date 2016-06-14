namespace AntMe.Basics.MapTiles
{
    /// <summary>
    /// Sand Material.
    /// </summary>
    public class SandMaterial : MapMaterial
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        public SandMaterial(SimulationContext context) : base(context, 0.8f)
        {
        }
    }
}
