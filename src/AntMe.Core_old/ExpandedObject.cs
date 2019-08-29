namespace AntMe
{
    internal class ExpandedObject<T>
    {
        public readonly T obj;
        public readonly Vector3 pos;
        public readonly float radius;

        public ExpandedObject(T obj, Vector3 pos, float radius)
        {
            this.obj = obj;
            this.pos = pos;
            this.radius = radius;
        }
    }
}