namespace AntMe
{
    internal class ExpandedObject<T>
    {
        public readonly T Obj;
        public readonly Vector3 Pos;
        public readonly float Radius;

        public ExpandedObject(T obj, Vector3 pos, float radius)
        {
            Obj = obj;
            Pos = pos;
            Radius = radius;
        }
    }
}