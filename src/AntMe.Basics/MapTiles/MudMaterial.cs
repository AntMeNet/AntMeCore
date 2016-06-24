namespace AntMe.Basics.MapTiles
{
    /// <summary>
    /// Mud Material.
    /// </summary>
    public class MudMaterial : MapMaterial
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        public MudMaterial(SimulationContext context) : base(context, 0.5f)
        {
        }
    }
}
