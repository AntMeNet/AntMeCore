namespace AntMe
{
    /// <summary>
    ///     Default Attribute to decorate a Player Factory.
    /// </summary>
    [FactoryAttributeMapping(
        NameProperty = "Name",
        AuthorProperty = "Author"
    )]
    public sealed class PlayerAttribute : FactoryAttribute
    {
        /// <summary>
        ///     Holds the Display Name of the Ai.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Holds the Player Name.
        /// </summary>
        public string Author { get; set; }
    }
}