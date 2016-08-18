using System;
using Demo.DynamicCodeGen.Common;
using Demo.DynamicCodeGen.Roslyn;
using NUnit.Framework;

namespace Demo.DynamicCodeGen.Tests.Roslyn
{
    public class RoslynMapper_Tests
    {
        [Test]
        public void CreateText_WhenCalled_ReturnsText()
        {
            var context = MapContext.Create<Src, Dest>();

            string text = MapperTextBuilder.CreateText(context);

            Console.WriteLine(text);
        }

        [Test]
        public void CreateMapMethod_WhenCalled_Success()
        {
            var map = RoslynMapper.CreateMapMethod<Src, Dest>();

            var src = new Src();
            var dest = new Dest();

            map(src, dest);

            var result = ObjectComparer.AreEqual(src, dest);

            Assert.IsTrue(result.Success);
        }
    }
}
