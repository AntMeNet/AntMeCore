using System;
using System.Collections.Generic;
using Xunit;

namespace AntMe.Core
{
    public class KeyValueStoreTest
    {
        #region TestData

        public static IEnumerable<IList<string[]>> FullStringValues
        {
            get
            {
                return new[]
                {
                    new List<string[]> // normal Data
                    {
                        // Key , Value , Description
                        new string[] { "AntMe.Core.TestType1:TestKey1","True","Test1 Description"},
                        new string[] { "AntMe.Core.TestType1:TestKey2","42","Test2 Description"},
                        new string[] { "AntMe.Core.TestType1:TestKey3", "3.14159265", "Test3 Description"},
                        new string[] { "AntMe.Core.TestType1:TestKey4", "TestSchlüßel4", "Test4 Description"},
                        new string[] { "AntMe.Core.TestType2:TestKey1","True","Test1 Description"},
                        new string[] { "AntMe.Core.TestType2:TestKey2","42","Test2 Description"},
                        new string[] { "AntMe.Core.TestType2:TestKey3", "3.14159265", "Test3 Description"},
                        new string[] { "AntMe.Core.TestType2:TestKey4", "TestSchlüßel4", "Test4 Description"}
                    },
                    new List<string[]> // no Description
                    {
                        new string[] { "AntMe.Core.TestType1:TestKey1","True",""},
                        new string[] { "AntMe.Core.TestType1:TestKey2","42",""},
                        new string[] { "AntMe.Core.TestType1:TestKey3", "3.14159265", ""},
                        new string[] { "AntMe.Core.TestType1:TestKey4", "TestSchlüßel4", ""},
                        new string[] { "AntMe.Core.TestType2:TestKey1","True",""},
                        new string[] { "AntMe.Core.TestType2:TestKey2","42",""},
                        new string[] { "AntMe.Core.TestType2:TestKey3", "3.14159265", ""},
                        new string[] { "AntMe.Core.TestType2:TestKey4", "TestSchlüßel4", ""}
                    }
                };
            }
        }

        #endregion


        #region Constructors

        [Fact]
        public void InitWithoutParameter()
        {
            // Arrange

            // Act
            var result = new KeyValueStore();

            // Assert
            Assert.NotNull(result.Common);
            Assert.NotNull(result.Keys);

        }


        [Fact]
        public void InitWithOtherInstance()
        {
            // Arrange
            var source = new KeyValueStore();

            // Act
            var target = new KeyValueStore(source);

            // Assert
            Assert.NotSame(source, target);

        }

        #endregion

        #region SetMethodes

        [Theory]
        public void TestStringSetGetFullKey(List<string[]> testData)
        {
            // Arrange
            var KVS = new KeyValueStore();

            // Act
            foreach (string[] item in testData)
            {
                int x;
                if (int.TryParse(item[1], out x))
                    KVS.Set(item[0], x, item[2]);
                else
                    KVS.Set(item[0], int.MaxValue, item[2]);

            }

            // Assert

            foreach (string[] item in testData)
            {
                int x;

                if (int.TryParse(KVS.GetString(item[0]), out x))
                {
                    int y;
                    if (int.TryParse(item[1], out y))
                        Assert.Equal(y, x);
                    else
                        Assert.True(false, "Testing Data wrong");
                }
                //Assert.Equal(item[1], );

                Assert.Equal(item[2], KVS.GetDescription(item[0]));
            }

        }

        [Theory]
        public void TestIntSetGetFullKey(List<string[]> testData)
        {
            // Arrange
            var KVS = new KeyValueStore();

            // Act
            foreach (string[] item in testData)
            {
                KVS.Set(item[0], item[1], item[2]);
            }

            // Assert

            foreach (string[] item in testData)
            {
                Assert.Equal(item[1], KVS.GetString(item[0]));
                Assert.Equal(item[2], KVS.GetDescription(item[0]));
            }

        }

        #endregion

    }
}
