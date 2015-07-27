using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb.DataTypes;

namespace Pls.UnitTestsOf.SimpleMongoDb.DataTypes
{
    internal static class SimoKeyValuesAssertExtensions
    {
        internal static void AssertValue<T>(this SimoKeyValues kv, string key, T expected, Func<string, T> actual)
        {
            Assert.AreEqual(expected, actual(key));
        }
    }
}