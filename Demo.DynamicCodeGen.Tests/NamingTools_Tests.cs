using System;
using System.Text.RegularExpressions;
using Demo.DynamicCodeGen.Common;
using Demo.DynamicCodeGen.Roslyn;
using NUnit.Framework;

namespace Demo.DynamicCodeGen.Tests
{
    public class NamingTools_Tests
    {
        [Test]
        public void Clean_WhenCalled_RemovesChars()
        {
            string origin = "søme string.";
            string actual = NamingTools.Clean(origin);
            Console.WriteLine(actual);
            Assert.AreEqual("sme string", actual);
        } 
    }
}