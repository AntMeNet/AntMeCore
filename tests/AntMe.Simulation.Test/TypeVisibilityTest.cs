using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Reflection;

namespace AntMe.Simulation.Test
{
    [TestClass]
    public class TypeVisibilityTest
    {
        [TestMethod]
        public void VisibiltiyCheck()
        {
            // TODO!!!

            #region Define Visibles

            List<Type> visibleTypes = new List<Type>();

            #endregion

            #region Define Abstacts

            // Abstract Types
            List<Type> abstractTypes = new List<Type>();

            #endregion

            #region define Sealeds

            // Sealed Types
            List<Type> notSealedTypes = new List<Type>();

            #endregion

            Assembly assembly = Assembly.GetAssembly(typeof(Faction));
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
