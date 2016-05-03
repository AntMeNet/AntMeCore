using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Collections.Generic;

namespace AntMe.Core.Test
{
    [TestClass]
    public class TypeVisibilityTest
    {
        [TestMethod]
        public void VisibiltiyCheck()
        {
            #region Define Visibles

            List<Type> visibleTypes = new List<Type>();

            // Basis Variablen
            visibleTypes.Add(typeof(Angle));
            visibleTypes.Add(typeof(Compass));
            visibleTypes.Add(typeof(Index2));
            visibleTypes.Add(typeof(Vector2));
            visibleTypes.Add(typeof(Vector3));
            visibleTypes.Add(typeof(MipMap<>));
            visibleTypes.Add(typeof(MipMapLayer<>)); // TODO: Not sure...

            // Engine Types
            visibleTypes.Add(typeof(Engine));
            visibleTypes.Add(typeof(EngineState));
            visibleTypes.Add(typeof(EngineProperty));
            visibleTypes.Add(typeof(Map));
            visibleTypes.Add(typeof(MapTile));
            visibleTypes.Add(typeof(TileShape));
            visibleTypes.Add(typeof(TileHeight));
            visibleTypes.Add(typeof(TileSpeed));
            visibleTypes.Add(typeof(Item));
            visibleTypes.Add(typeof(ItemProperty));

            // Exceptions
            visibleTypes.Add(typeof(InvalidMapException));

            // Delegates
            visibleTypes.Add(typeof(ValueUpdate<>));
            visibleTypes.Add(typeof(ValueChanged<>));
            visibleTypes.Add(typeof(ChangeItem));
            visibleTypes.Add(typeof(ChangeItem<>));

            #endregion

            #region Define Abstacts

            // Abstract Types
            List<Type> abstractTypes = new List<Type>();

            // Engine Types
            abstractTypes.Add(typeof(EngineProperty));
            abstractTypes.Add(typeof(Item));
            abstractTypes.Add(typeof(ItemProperty));

            // Extensions & Properties

            #endregion

            #region define Sealeds

            // Sealed Types
            List<Type> notSealedTypes = new List<Type>();

            // Engine Types
            notSealedTypes.Add(typeof(EngineProperty));
            notSealedTypes.Add(typeof(Item));
            notSealedTypes.Add(typeof(ItemProperty));

            // Exceptions

            // Extensions & Properties

            // Interaction

            #endregion

            Assembly assembly = Assembly.GetAssembly(typeof(Engine));
            foreach (var item in assembly.GetTypes())
            {
                if (item.IsVisible)
                {
                    if (!visibleTypes.Contains(item))
                        throw new Exception("Type " + item.ToString() + " should not be visible");

                    if (item.IsAbstract != abstractTypes.Contains(item))
                        throw new Exception("Type " + item.ToString() + " has wrong abstract Flag");

                    if (item.IsSealed == notSealedTypes.Contains(item))
                        throw new Exception("Type " + item.ToString() + " has wrong sealed Flag");
                }
            }
        }
    }
}
