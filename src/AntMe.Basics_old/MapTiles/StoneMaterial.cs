namespace AntMe.Basics.MapTiles
{
    /// <summary>
    /// Stone Material.
    /// </summary>
    public class StoneMaterial : MapMaterial
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        public StoneMaterial(SimulationContext context) : base(context, 1.2f)
        {
        }
    }
}
