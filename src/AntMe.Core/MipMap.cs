using System.Collections.Generic;
using System.Linq;

namespace AntMe
{
    /// <summary>
    ///     Represents a mip map, that is a geometric data structure with different levels of detail.
    ///     The main functionality is searching for objects in the vincinity of a given center point, which is of O(1)
    ///     complexity.
    ///     Objects with a large body are stored in higher levels of the mip map with fewer cells, smaller objects in lower
    ///     levels with more cells.
    /// </summary>
    public sealed class MipMap<T>
    {
        /// <summary>
        ///     Initializes the mipmap with width and height of the largest (base) layer.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public MipMap(float width, float height)
        {
            Width = width;
            Height = height;

            Layers = new List<MipMapLayer<T>>();

            // Add the base layer, accepting all radii
            AddLayer(float.PositiveInfinity);
        }

        /// <summary>
        ///     All layers of this mipmap.
        /// </summary>
        public List<MipMapLayer<T>> Layers { get; private set; }

        /// <summary>
        ///     Height of the base layer.
        /// </summary>
        public float Height { get; }

        /// <summary>
        ///     Width of the base layer.
        /// </summary>
        public float Width { get; }

        /// <summary>
        ///     Adds a new layer to the mipmap, conserving the ordering by rejected radius.
        /// </summary>
        /// <param name="maxRadius"></param>
        public void AddLayer(float maxRadius)
        {
            Layers.Add(new MipMapLayer<T>(Width, Height, maxRadius));

            // sort the list by radii so that when adding objects, one can iterate over the list and always find the layer with the objects with smallest radius on top.
            Layers = Layers.OrderByDescending(layer => layer.MaxRadius).ToList();
        }

        /// <summary>
        ///     Adds an object to the mipmap.
        ///     The given radius specifies in which layer the object is stored.
        /// </summary>
        /// <param name="obj">object to store</param>
        /// <param name="pos">position of the object</param>
        /// <param name="radius">radius of the object</param>
        public void Add(T obj, Vector3 pos, float radius)
        {
            // the base layer is the default
            var lastLayer = Layers[0];
            foreach (var layer in Layers)
            {
                // check greater than current max radius and add to the last checked layer since its max radius was ok.
                if (radius > layer.MaxRadius)
                {
                    lastLayer.Add(obj, pos, radius);
                    return;
                }

                lastLayer = layer;
            }

            lastLayer.Add(obj, pos, radius);
        }

        /// <summary>
        ///     Find all objects in the ball with given radius and center.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <returns>a list of all objects that intersect the given ball with their ball defined by their position and radius.</returns>
        public HashSet<T> FindAll(Vector3 center, float radius)
        {
            var result = new HashSet<T>();
            foreach (var layer in Layers)
            foreach (var item in layer.FindAll(center, radius))
                result.Add(item);
            return result;
        }

        /// <summary>
        ///     Removes all objects from all layers.
        /// </summary>
        public void Clear()
        {
            foreach (var layer in Layers) layer.Clear();
        }

        /// <summary>
        ///     Removes all objects and all layers.
        /// </summary>
        public void ClearLayers()
        {
            Layers.Clear();
        }

        /// <summary>
        ///     Entfernt das angegeben Objekt.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="pos"></param>
        public void Remove(T obj, Vector3 pos)
        {
            foreach (var layer in Layers) layer.Remove(obj, pos);
        }
    }
}