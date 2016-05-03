namespace AntMe.ItemProperties.Basics
{
    /// <summary>
    ///     Information zu einer Umgebungszelle
    /// </summary>
    public struct VisibleCell
    {
        /// <summary>
        ///     Höheninformation zu dieser Zelle.
        /// </summary>
        public TileHeight Height;

        /// <summary>
        ///     Speedmodifikator dieser Zelle. 1 bedeutet normale Geschwindigkeit,
        ///     0.5 halbiert die Fortbewegungsgeschwindigkeit,...
        /// </summary>
        public float Speed;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is VisibleCell))
                return false;

            var other = (VisibleCell) obj;
            return Height.Equals(other.Height) &&
                   Speed.Equals(other.Speed);
        }

        public override int GetHashCode()
        {
            return Speed.GetHashCode() +
                   Height.GetHashCode();
        }
    }
}