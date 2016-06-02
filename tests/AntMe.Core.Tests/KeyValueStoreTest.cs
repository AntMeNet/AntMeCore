using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Xunit;

namespace AntMe.Core
{



    public class KeyValueStoreTest
    {
        #region TestClasses

        private class TestType1
        {
            public bool TestKey1 { get; set; }

            public int TestKey2 { get; set; }

            public float TestKey3 { get; set; }

            public string TestKey4 { get; set; }
        }

        private class TestType2
        {
            public bool TestKey1 { get; set; }

            public int TestKey2 { get; set; }

            public float TestKey3 { get; set; }

            public string TestKey4 { get; set; }
        }

        #endregion

        #region TestData
        /*
        FileStructur for File Tests:

        [AntMe.Core.KeyValueStoreTest+TestType1]
        TestKey1=True//Test1 Description
        TestKey2 = 42// Test2 Description
        TestKey3=3.14159265// Test3 Description
        TestKey4=TestSchlüßel4// Test4 Description
        
        [AntMe.Core.KeyValueStoreTest + TestType2]
        TestKey1 = True//Test1 Description
        TestKey2=42// Test2 Description
        TestKey3=3.14159265// Test3 Description
        TestKey4=TestSchlüßel4// Test4 Description
        
        [Common]
        Name = TestName
        Author=TestAuthor
        CommonKey1 = CommonValue1
        CommonKey2=CommonValue2
        */
        #endregion

        #region Constructors

        [Fact]
        public void InitWithoutParameter()
        {
            // Arrange

            // Act
            KeyValueStore KVS = new KeyValueStore();

            // Assert
            Assert.NotNull(KVS.Common);
            Assert.NotNull(KVS.Keys);

        }


        [Fact]
        public void InitWithOtherInstance()
        {
            // Arrange
            KeyValueStore source = new KeyValueStore();

            // Act
            KeyValueStore target = new KeyValueStore(source);

            // Assert
            Assert.NotNull(target);
            Assert.NotNull(target.Common);
            Assert.NotNull(target.Keys);
            Assert.NotSame(source, target);

        }

        [Fact]
        public void InitFormFile()
        {
            // Arrange
            string filename = Path.GetTempFileName();

            using (StreamWriter SW = new StreamWriter(File.Open(filename, FileMode.Create)))
            {
                SW.WriteLine("[AntMe.Core.KeyValueStoreTest+TestType1]");
                SW.WriteLine("TestKey1=True//Test1 Description");
                SW.WriteLine("TestKey2=42// Test2 Description");
                SW.WriteLine("TestKey3=3.14159265// Test3 Description");
                SW.WriteLine("TestKey4=TestSchlüßel4// Test4 Description");
                SW.WriteLine("");
                SW.WriteLine("[AntMe.Core.KeyValueStoreTest+TestType2]");
                SW.WriteLine("TestKey1=True//Test1 Description");
                SW.WriteLine("");
                SW.WriteLine("[Common]");
                SW.WriteLine("Name=TestName");
                SW.WriteLine("Author=TestAuthor");
                SW.WriteLine("CommonKey1=CommonValue1");
                SW.WriteLine("CommonKey2=CommonValue2");
            }

            // Act
            KeyValueStore KVS = new KeyValueStore(filename);

            // Assert
            Assert.NotNull(KVS.Keys);
            Assert.NotNull(KVS.Common);
            Assert.Equal(4, KVS.Common.Count);
        }

        [Fact]
        public void InitFromStream()
        {
            // Arrgange
            KeyValueStore KVS;
            using (Stream stream = new MemoryStream())
            {
                using (StreamWriter SW = new StreamWriter(stream))
                {
                    SW.WriteLine("[AntMe.Core.KeyValueStoreTest+TestType1]");
                    SW.WriteLine("TestKey1=True//Test1 Description");
                    SW.WriteLine("TestKey2=42// Test2 Description");
                    SW.WriteLine("TestKey3=3.14159265// Test3 Description");
                    SW.WriteLine("TestKey4=TestSchlüßel4// Test4 Description");
                    SW.WriteLine("");
                    SW.WriteLine("[AntMe.Core.KeyValueStoreTest+TestType2]");
                    SW.WriteLine("TestKey1=True//Test1 Description");
                    SW.WriteLine("");
                    SW.WriteLine("[Common]");
                    SW.WriteLine("Name=TestName");
                    SW.WriteLine("Author=TestAuthor");
                    SW.WriteLine("CommonKey1=CommonValue1");
                    SW.WriteLine("CommonKey2=CommonValue2");
                    SW.Flush();
                    stream.Position = 0;
                    // Act
                    KVS = new KeyValueStore(stream);
                }
            }

            // Assert
            Assert.NotNull(KVS.Keys);
            Assert.NotNull(KVS.Common);
            Assert.Equal(4, KVS.Common.Count);

        }

        #endregion

        #region SetGetMethodes

        [Fact]
        public void SetGetBool_FullKey()
        {
            // Arrange
            KeyValueStore KVS = new KeyValueStore();

            // Act
            KVS.Set("AntMe.Core.KeyValueStoreTest+TestType1:TestKey1", true);

            // Assert
            Assert.True(KVS.GetBool("AntMe.Core.KeyValueStoreTest+TestType1:TestKey1"));
            Assert.Null(KVS.GetBool("WrongKey"));
        }

        [Fact]
        public void SetGetInt_FullKey()
        {
            // Arrange
            KeyValueStore KVS = new KeyValueStore();

            // Act
            KVS.Set("AntMe.Core.KeyValueStoreTest+TestType1:TestKey2", 42);

            // Assert
            Assert.Equal(42, KVS.GetInt("AntMe.Core.KeyValueStoreTest+TestType1:TestKey2"));
            Assert.Null(KVS.GetInt("WrongKey"));
        }

        [Fact]
        public void SetGetFloat_FullKey()
        {
            // Arrange
            var KVS = new KeyValueStore();

            // Act
            KVS.Set("AntMe.Core.KeyValueStoreTest+TestType1:TestKey3", 3.14159265f);

            // Assert
            Assert.Equal(3.14159265f, KVS.GetFloat("AntMe.Core.KeyValueStoreTest+TestType1:TestKey3").Value, 5);
            Assert.Null(KVS.GetFloat("WrongKey"));
        }

        [Fact]
        public void SetGetString_FullKey()
        {
            // Arrange
            var KVS = new KeyValueStore();

            // Act
            KVS.Set("AntMe.Core.KeyValueStoreTest+TestType1:TestKey4", "TestSchlüßel4");

            // Assert
            Assert.Equal("TestSchlüßel4", KVS.GetString("AntMe.Core.KeyValueStoreTest+TestType1:TestKey4"));
            Assert.Equal(string.Empty, KVS.GetString("WrongKey"));
        }

        [Fact]
        public void SetGetDescription_FullKey()
        {
            // Arrange
            var KVS = new KeyValueStore();

            // Act
            KVS.Set("AntMe.Core.KeyValueStoreTest+TestType1:TestKey4", "TestSchlüßel4", "Test4 Description");


            // Assert
            Assert.Equal("Test4 Description", KVS.GetDescription("AntMe.Core.KeyValueStoreTest+TestType1:TestKey4"));
            Assert.Equal(string.Empty, KVS.GetDescription("WrongKey"));
        }

        [Fact]
        public void SetGetBool_Type()
        {
            // Arrange
            var KVS = new KeyValueStore();

            // Act
            KVS.Set<TestType1>("TestKey1", true);

            // Assert
            Assert.True(KVS.GetBool<TestType1>("TestKey1"));
            Assert.Null(KVS.GetBool<TestType1>("WrongKey"));
        }

        [Fact]
        public void SetGetInt_Type()
        {
            // Arrange
            var KVS = new KeyValueStore();

            // Act
            KVS.Set<TestType1>("TestKey2", 42);

            // Assert
            Assert.Equal(42, KVS.GetInt<TestType1>("TestKey2"));
            Assert.Null(KVS.GetInt<TestType1>("WrongKey"));
        }

        [Fact]
        public void SetGetFloat_Type()
        {
            // Arrange
            var KVS = new KeyValueStore();

            // Act
            KVS.Set<TestType1>("TestKey3", 3.14159265f);

            // Assert
            Assert.Equal(3.14159265f, KVS.GetFloat<TestType1>("TestKey3").Value, 5);
            Assert.Null(KVS.GetFloat<TestType1>("WrongKey"));
        }

        [Fact]
        public void SetGetString_Type()
        {
            // Arrange
            var KVS = new KeyValueStore();

            // Act
            KVS.Set<TestType1>("TestKey4", "TestSchlüßel4");

            // Assert
            Assert.Equal("TestSchlüßel4", KVS.GetString<TestType1>("TestKey4"));
            Assert.Equal(string.Empty, KVS.GetString<TestType1>("WrongKey"));
        }

        [Fact]
        public void SetGetDescription_Type()
        {
            // Arrange
            var KVS = new KeyValueStore();

            // Act
            KVS.Set<TestType1>("TestKey4", "TestSchlüßel4", "Test4 Description");


            // Assert
            Assert.Equal("Test4 Description", KVS.GetDescription<TestType1>("TestKey4"));
            Assert.Equal(string.Empty, KVS.GetDescription<TestType1>("WrongKey"));
        }

        #endregion

        #region InstanceMethods

        [Fact]
        public void CloneInstance()
        {
            // Arrange
            KeyValueStore source = new KeyValueStore();
            source.Set("AntMe.Core.KeyValueStoreTest+TestType1:TestKey4", "TestSchlüßel4", "Test4 Description");
            source.Common.Add("Name", "TestName");

            // Act
            KeyValueStore target = source.Clone();

            // Assert
            Assert.NotSame(target, source);
            Assert.Equal("TestSchlüßel4", target.GetString("AntMe.Core.KeyValueStoreTest+TestType1:TestKey4"));
            Assert.Equal("Test4 Description", target.GetDescription("AntMe.Core.KeyValueStoreTest+TestType1:TestKey4"));
            string result;
            Assert.True(target.Common.TryGetValue("Name", out result));
            Assert.Equal("TestName", result);
        }

        [Fact]
        public void MergeWithInstance()
        {
            // Arrange
            KeyValueStore source = new KeyValueStore();
            source.Set("AntMe.Core.KeyValueStoreTest+TestType1:TestKey4", "TestSchlüßel4Source", "Test4 DescriptionSource");
            source.Set("AntMe.Core.KeyValueStoreTest+TestType1:TestKey1", true);
            source.Common.Add("Name", "TestNameSource");

            KeyValueStore target = new KeyValueStore();
            target.Set("AntMe.Core.KeyValueStoreTest+TestType1:TestKey4", "TestSchlüßel4Target", "Test4 DescriptionTaget");
            target.Set("AntMe.Core.KeyValueStoreTest+TestType1:TestKey1", false, "Test1 DescriptionTarget");
            target.Set("AntMe.Core.KeyValueStoreTest+TestType2:TestKey4", "TestSchlüßel4", "Test4 Description");
            target.Common.Add("Name", "TestNameTarget");
            target.Common.Add("Author", "TestAuthor");

            // Act
            target.Merge(source);

            // Assert    
            Assert.Equal("TestSchlüßel4Source", target.GetString("AntMe.Core.KeyValueStoreTest+TestType1:TestKey4")); // should be the source (overridning the value)
            Assert.Equal("Test4 DescriptionSource", target.GetDescription("AntMe.Core.KeyValueStoreTest+TestType1:TestKey4")); // should be the source (overridning the description)

            Assert.Equal(true, target.GetBool("AntMe.Core.KeyValueStoreTest+TestType1:TestKey1")); // should be the source (overridning the value)
            Assert.Equal("Test1 DescriptionTarget", target.GetDescription("AntMe.Core.KeyValueStoreTest+TestType1:TestKey1")); // should be the target (source is empty)

            Assert.Equal("TestSchlüßel4", target.GetString("AntMe.Core.KeyValueStoreTest+TestType2:TestKey4")); // should be the target (additional Item)
            Assert.Equal("Test4 Description", target.GetDescription("AntMe.Core.KeyValueStoreTest+TestType2:TestKey4")); // should be the target (additional Item)

            string resultName;
            Assert.True(target.Common.TryGetValue("Name", out resultName));
            Assert.Equal("TestNameSource", resultName); // should be the source (overrinding the Value)
            string resultAuthor;
            Assert.True(target.Common.TryGetValue("Author", out resultAuthor));
            Assert.Equal("TestAuthor", resultAuthor); // should be the target (additional Item)

        }

        [Fact]
        public void MergeWithFile()
        {
            // Arrange
            string filename = Path.GetTempFileName();
            using (Stream stream = File.Open(filename, FileMode.Create))
            {
                using (StreamWriter SW = new StreamWriter(stream))
                {
                    SW.WriteLine("[AntMe.Core.KeyValueStoreTest+TestType1]");
                    SW.WriteLine("TestKey4=TestSchlüßel4Source // Test4 DescriptionSource");
                    SW.WriteLine("TestKey1=True");
                    SW.WriteLine("");
                    SW.WriteLine("[Common]");
                    SW.WriteLine("Name=TestNameSource");
                }
            }
            KeyValueStore target = new KeyValueStore();
            target.Set("AntMe.Core.KeyValueStoreTest+TestType1:TestKey4", "TestSchlüßel4Target", "Test4 DescriptionTaget");
            target.Set("AntMe.Core.KeyValueStoreTest+TestType1:TestKey1", false, "Test1 DescriptionTarget");
            target.Set("AntMe.Core.KeyValueStoreTest+TestType2:TestKey4", "TestSchlüßel4", "Test4 Description");
            target.Common.Add("Name", "TestNameTarget");
            target.Common.Add("Author", "TestAuthor");

            // Act
            target.Merge(filename);

            // Assert    
            Assert.Equal("TestSchlüßel4Source", target.GetString("AntMe.Core.KeyValueStoreTest+TestType1:TestKey4")); // should be the source (overridning the value)
            Assert.Equal("Test4 DescriptionSource", target.GetDescription("AntMe.Core.KeyValueStoreTest+TestType1:TestKey4")); // should be the source (overridning the description)

            Assert.Equal(true, target.GetBool("AntMe.Core.KeyValueStoreTest+TestType1:TestKey1")); // should be the source (overridning the value)
            Assert.Equal("Test1 DescriptionTarget", target.GetDescription("AntMe.Core.KeyValueStoreTest+TestType1:TestKey1")); // should be the target (source is empty)

            Assert.Equal("TestSchlüßel4", target.GetString("AntMe.Core.KeyValueStoreTest+TestType2:TestKey4")); // should be the target (additional Item)
            Assert.Equal("Test4 Description", target.GetDescription("AntMe.Core.KeyValueStoreTest+TestType2:TestKey4")); // should be the target (additional Item)

            string resultName;
            Assert.True(target.Common.TryGetValue("Name", out resultName));
            Assert.Equal("TestNameSource", resultName); // should be the source (overrinding the Value)
            string resultAuthor;
            Assert.True(target.Common.TryGetValue("Author", out resultAuthor));
            Assert.Equal("TestAuthor", resultAuthor); // should be the target (additional Item)
        }

        [Fact]
        public void MergeWithStream()
        {
            // Arrange
            KeyValueStore target = new KeyValueStore();
            target.Set("AntMe.Core.KeyValueStoreTest+TestType1:TestKey4", "TestSchlüßel4Target", "Test4 DescriptionTaget");
            target.Set("AntMe.Core.KeyValueStoreTest+TestType1:TestKey1", false, "Test1 DescriptionTarget");
            target.Set("AntMe.Core.KeyValueStoreTest+TestType2:TestKey4", "TestSchlüßel4", "Test4 Description");
            target.Common.Add("Name", "TestNameTarget");
            target.Common.Add("Author", "TestAuthor");

            using (Stream stream = new MemoryStream())
            {
                using (StreamWriter SW = new StreamWriter(stream))
                {
                    SW.WriteLine("[AntMe.Core.KeyValueStoreTest+TestType1]");
                    SW.WriteLine("TestKey4=TestSchlüßel4Source // Test4 DescriptionSource");
                    SW.WriteLine("TestKey1=True");
                    SW.WriteLine("");
                    SW.WriteLine("[Common]");
                    SW.WriteLine("Name=TestNameSource");
                    
                    SW.Flush();

                    // Act
                    stream.Position = 0;
                    target.Merge(stream);
                }
            }
            // Assert    
            Assert.Equal("TestSchlüßel4Source", target.GetString("AntMe.Core.KeyValueStoreTest+TestType1:TestKey4")); // should be the source (overridning the value)
            Assert.Equal("Test4 DescriptionSource", target.GetDescription("AntMe.Core.KeyValueStoreTest+TestType1:TestKey4")); // should be the source (overridning the description)

            Assert.Equal(true, target.GetBool("AntMe.Core.KeyValueStoreTest+TestType1:TestKey1")); // should be the source (overridning the value)
            Assert.Equal("Test1 DescriptionTarget", target.GetDescription("AntMe.Core.KeyValueStoreTest+TestType1:TestKey1")); // should be the target (source is empty)

            Assert.Equal("TestSchlüßel4", target.GetString("AntMe.Core.KeyValueStoreTest+TestType2:TestKey4")); // should be the target (additional Item)
            Assert.Equal("Test4 Description", target.GetDescription("AntMe.Core.KeyValueStoreTest+TestType2:TestKey4")); // should be the target (additional Item)

            string resultName;
            Assert.True(target.Common.TryGetValue("Name", out resultName));
            Assert.Equal("TestNameSource", resultName); // should be the source (overrinding the Value)
            string resultAuthor;
            Assert.True(target.Common.TryGetValue("Author", out resultAuthor));
            Assert.Equal("TestAuthor", resultAuthor); // should be the target (additional Item)

        }

        [Fact]
        public void SaveToFile()
        {
            // Arrange
            KeyValueStore KVS = new KeyValueStore();
            KVS.Set<TestType1>("TestKey1", true, "Test1 Description");
            KVS.Set<TestType1>("TestKey2", 42, "Test2 Description");
            KVS.Set<TestType1>("TestKey3", 3.14159265f, "Test3 Description");
            KVS.Set<TestType1>("TestKey4", "TestSchlüßel4", "Test4 Description");

            KVS.Set<TestType2>("TestKey1", true);
            KVS.Set<TestType2>("TestKey2", 42);
            KVS.Set<TestType2>("TestKey3", 3.14159265f);
            KVS.Set<TestType2>("TestKey4", "TestSchlüßel4");

            KVS.Common.Add("Name", "TestName");
            // Act
            string filename = Path.GetTempFileName();
            KVS.Save(filename);

            // Assert 
            using (StreamReader SR = new StreamReader(filename))
            {
                Assert.Equal("[Common]", SR.ReadLine());
                Assert.Equal("Name=TestName", SR.ReadLine());
                Assert.Equal("", SR.ReadLine());
                Assert.Equal("[AntMe.Core.KeyValueStoreTest+TestType1]", SR.ReadLine());
                Assert.Equal("TestKey1=True // Test1 Description", SR.ReadLine());
                Assert.Equal("TestKey2=42 // Test2 Description", SR.ReadLine());
                Assert.Equal("TestKey3=3.141593 // Test3 Description", SR.ReadLine());
                Assert.Equal("TestKey4=TestSchlüßel4 // Test4 Description", SR.ReadLine());
                Assert.Equal("", SR.ReadLine());
                Assert.Equal("[AntMe.Core.KeyValueStoreTest+TestType2]", SR.ReadLine());
                Assert.Equal("TestKey1=True", SR.ReadLine());
                Assert.Equal("TestKey2=42", SR.ReadLine());
                Assert.Equal("TestKey3=3.141593", SR.ReadLine());
                Assert.Equal("TestKey4=TestSchlüßel4", SR.ReadLine());
            }

        }

        [Fact]
        public void SaveToStream()
        {
            // Arrange
            KeyValueStore KVS = new KeyValueStore();
            KVS.Set<TestType1>("TestKey1", true, "Test1 Description");
            KVS.Set<TestType1>("TestKey2", 42, "Test2 Description");
            KVS.Set<TestType1>("TestKey3", 3.14159265f, "Test3 Description");
            KVS.Set<TestType1>("TestKey4", "TestSchlüßel4", "Test4 Description");

            KVS.Set<TestType2>("TestKey1", true);
            KVS.Set<TestType2>("TestKey2", 42);
            KVS.Set<TestType2>("TestKey3", 3.14159265f);
            KVS.Set<TestType2>("TestKey4", "TestSchlüßel4");

            KVS.Common.Add("Name", "TestName");
            // Act
            using (Stream stream = new MemoryStream())
            {
                KVS.Save(stream);
                
                stream.Position = 0;
                // Assert 
                using (StreamReader SR = new StreamReader(stream))
                {
                    Assert.Equal("[Common]", SR.ReadLine());
                    Assert.Equal("Name=TestName", SR.ReadLine());
                    Assert.Equal("", SR.ReadLine());
                    Assert.Equal("[AntMe.Core.KeyValueStoreTest+TestType1]", SR.ReadLine());
                    Assert.Equal("TestKey1=True // Test1 Description", SR.ReadLine());
                    Assert.Equal("TestKey2=42 // Test2 Description", SR.ReadLine());
                    Assert.Equal("TestKey3=3.141593 // Test3 Description", SR.ReadLine());
                    Assert.Equal("TestKey4=TestSchlüßel4 // Test4 Description", SR.ReadLine());
                    Assert.Equal("", SR.ReadLine());
                    Assert.Equal("[AntMe.Core.KeyValueStoreTest+TestType2]", SR.ReadLine());
                    Assert.Equal("TestKey1=True", SR.ReadLine());
                    Assert.Equal("TestKey2=42", SR.ReadLine());
                    Assert.Equal("TestKey3=3.141593", SR.ReadLine());
                    Assert.Equal("TestKey4=TestSchlüßel4", SR.ReadLine());
                    
                }
            }

        }
        #endregion

    }
}
