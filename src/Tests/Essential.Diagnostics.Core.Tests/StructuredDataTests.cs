﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Essential.Diagnostics.Tests
{
    [TestClass()]
    public class StructuredDataTests
    {
        [TestMethod()]
        public void PropertyValue()
        {
            var template = "x{a}";

            IStructuredData data = new StructuredData(template, "A");
            var actual = data.Properties;

            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("a", actual.Keys.First());
            Assert.AreEqual("A", actual["a"]);
        }

        [TestMethod()]
        public void PropertyValueInOrder()
        {
            var template = "{b}x{a}";
            var a = "A";
            var b = "B";

            IStructuredData data = new StructuredData(template, a, b);
            var actual = data.Properties;

            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("A", actual["b"]);
            Assert.AreEqual("B", actual["a"]);
        }

        [TestMethod()]
        public void PropertyWithException()
        {
            var template = "x{a}";

            IStructuredData data;
            try
            {
                throw new ApplicationException("B");
            }
            catch (Exception ex)
            {
                data = new StructuredData(ex, template, "A");
            }
            var actual = data.Properties;

            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("A", actual["a"]);
            Assert.AreEqual("B", ((Exception)actual["Exception"]).Message);
        }

        [TestMethod()]
        public void PropertyWithAdditionalData()
        {
            var template = "x{a}";
            var additional = new Dictionary<string, object>() { { "b", "B" } };

            IStructuredData data = new StructuredData(additional, template, "A");
            var actual = data.Properties;

            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("A", actual["a"]);
            Assert.AreEqual("B", actual["b"]);
        }

        [TestMethod()]
        public void StringFormatProperty()
        {
            var template = "x{a}";
            var properties = new Dictionary<string, object>() {
                { "a", 1 },
            };                  

            IStructuredData data = new StructuredData(properties, template);
            var actual = data.ToString();

            Assert.AreEqual("x1", actual);
        }

        [TestMethod()]
        public void StringFormatValue()
        {
            var template = "x{a}";

            IStructuredData data = new StructuredData(template, 1);
            var actual = data.ToString();

            Assert.AreEqual("x1", actual);
        }

        [TestMethod()]
        public void StringValuesInOrder()
        {
            var template = "{b}x{a}";
            var a = "A";
            var b = "B";

            IStructuredData data = new StructuredData(template, a, b);
            var actual = data.ToString();

            Assert.AreEqual("AxB", actual);
        }

        [TestMethod()]
        public void StringValuesMissingAreBlank()
        {
            var template = "{a}x{b}y";
            var a = "A";

            IStructuredData data = new StructuredData(template, a);
            var actual = data.ToString();

            Assert.AreEqual("Axy", actual);
        }

        [TestMethod()]
        public void StringValuesOverrideProperties()
        {
            var template = "{a}x{b}";
            var a = "A";

            var properties = new Dictionary<string, object>() {
                { "a", 1 },
                { "b", 2 },
            };

            IStructuredData data = new StructuredData(properties, template, a);
            var actual = data.ToString();

            Assert.AreEqual("Ax2", actual);
        }

        [TestMethod()]
        public void StringPropertyOnly()
        {
            var properties = new Dictionary<string, object>() {
                { "a", 1 },
            };

            IStructuredData data = new StructuredData(properties);
            var actual = data.ToString();

            Assert.AreEqual("a=1", actual);
        }

        [TestMethod()]
        public void StringBasicStringValue()
        {
            var properties = new Dictionary<string, object>() {
                { "a", "A" },
            };

            IStructuredData data = new StructuredData(properties);
            var actual = data.ToString();

            Assert.AreEqual("a='A'", actual);
        }

        [TestMethod()]
        public void StringDateTimeValue()
        {
            var properties = new Dictionary<string, object>() {
                { "a", new DateTime(2001, 2, 3, 4, 5, 6, 7, DateTimeKind.Utc) },
                { "b", new DateTime(2002, 2, 3, 4, 5, 6, 7, DateTimeKind.Local) },
            };

            IStructuredData data = new StructuredData(properties);
            var actual = data.ToString();

            Assert.AreEqual("a=2001-02-03T04:05:06 b=2002-02-03T04:05:06", actual);
        }

        [TestMethod()]
        public void StringDateTimeOffsetValue()
        {
            var properties = new Dictionary<string, object>() {
                { "a", new DateTimeOffset(2001, 2, 3, 4, 5, 6, 7, TimeSpan.Zero) },
                { "b", new DateTimeOffset(2002, 2, 3, 4, 5, 6, 7, TimeSpan.FromHours(10)) },
            };

            IStructuredData data = new StructuredData(properties);
            var actual = data.ToString();

            Assert.AreEqual("a=2001-02-03T04:05:06+00:00 b=2002-02-03T04:05:06+10:00", actual);
        }

        [TestMethod()]
        public void StringByteValue()
        {
            var properties = new Dictionary<string, object>() {
                { "a", (byte)0 },
                { "b", (byte)0xa1 },
            };

            IStructuredData data = new StructuredData(properties);
            var actual = data.ToString();

            Assert.AreEqual("a=0x0 b=0xA1", actual);
        }

        [TestMethod()]
        public void StringTimeSpanValue()
        {
            var properties = new Dictionary<string, object>() {
                { "a", new TimeSpan(0, 0, 0, 4, 5) },
                { "b", TimeSpan.Zero - new TimeSpan(1, 2, 3, 4, 5) },
            };

            IStructuredData data = new StructuredData(properties);
            var actual = data.ToString();

            Assert.AreEqual("a=00:00:04.0050000 b=-1.02:03:04.0050000", actual);
        }

        [TestMethod()]
        public void StringPrimitiveValue()
        {
            var properties = new Dictionary<string, object>() {
                { "a", (short)-1 },
                { "b", (int)-2 },
                { "c", (long)-3 },
                { "d", (ushort)4 },
                { "e", (uint)5 },
                { "f", (ulong)6 },
                { "g", (sbyte)7 },
                { "h", (float)8.1 },
                { "i", (double)9.2 },
                { "j", (decimal)10.3 },
            };

            IStructuredData data = new StructuredData(properties);
            var actual = data.ToString();

            Assert.AreEqual("a=-1 b=-2 c=-3 d=4 e=5 f=6 g=7 h=8.1 i=9.2 j=10.3", actual);
        }

        [TestMethod()]
        public void StringGuidValue()
        {
            var properties = new Dictionary<string, object>() {
                { "a", new Guid("12345678-abcd-4321-8765-ba9876543210") },
            };

            IStructuredData data = new StructuredData(properties);
            var actual = data.ToString();

            Assert.AreEqual("a=12345678-abcd-4321-8765-ba9876543210", actual);
        }

        [TestMethod()]
        public void StringEscapedStringValue()
        {
            var properties = new Dictionary<string, object>() {
                { "a", @"w=x\y'z" },
            };

            IStructuredData data = new StructuredData(properties);
            var actual = data.ToString();

            Assert.AreEqual(@"a='w=x\\y\'z'", actual);
        }

        [TestMethod()]
        public void StringCustomObjectValue()
        {
            var properties = new Dictionary<string, object>() {
                { "a", new TestObject() },
            };

            IStructuredData data = new StructuredData(properties);
            var actual = data.ToString();

            Assert.AreEqual(@"a='w=x\\y\'z'", actual);
        }

        class TestObject
        {
            public override string ToString()
            {
                return @"w=x\y'z";
            }
        }

        [TestMethod()]
        public void StringMessageAndProperty()
        {
            var properties = new Dictionary<string, object>() {
                { "a", 1 },
            };

            IStructuredData data = new StructuredData(properties, "X");
            var actual = data.ToString();

            Assert.AreEqual("X; a=1", actual);
        }

        [TestMethod()]
        public void StringPropetiesAndTemplateValues()
        {
            var properties = new Dictionary<string, object>() {
                { "a", 1 },
                { "b", 2 },
                { "d", 4 },
            };

            IStructuredData data = new StructuredData(properties, "x{b}y{c}z{d}", "B", "C");
            var actual = data.ToString();

            Assert.AreEqual(@"xByCz4; a=1", actual);
        }

        [TestMethod()]
        public void StringExtraValues()
        {
            IStructuredData data = new StructuredData("x{a}", 1, 2);
            var actual = data.ToString();

            Assert.AreEqual(@"x1; Extra1=2", actual);
        }

        [TestMethod()]
        public void StringWithException()
        {
            var template = "x{a}";

            IStructuredData data;
            try
            {
                throw new ApplicationException("B");
            }
            catch (Exception ex)
            {
                data = new StructuredData(ex, template, "A");
            }
            var actual = data.ToString();

            var expectedMessageWithStartOfStackTrace = "xA; Exception='System.ApplicationException: B\r\n   at Essential.Diagnostics.Tests.StructuredDataTests.StringWithException() ";
            StringAssert.StartsWith(actual, expectedMessageWithStartOfStackTrace);
        }

        [TestMethod()]
        public void StringWithAdditionalData()
        {
            var template = "x{a}";
            var additional = new Dictionary<string, object>() { { "b", "B" } };

            IStructuredData data = new StructuredData(additional, template, "A");
            var actual = data.ToString();

            Assert.AreEqual("xA; b='B'", actual);
        }

        [TestMethod()]
        public void StringWithDuplicateTemplateValues()
        {
            IStructuredData data = new StructuredData("{a}x{a}", "A", "B");
            var actual = data.ToString();

            Assert.AreEqual("BxB", actual);
        }
    }
}