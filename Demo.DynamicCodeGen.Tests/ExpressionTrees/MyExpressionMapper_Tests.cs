using System;
using Demo.DynamicCodeGen.Common;
using Demo.DynamicCodeGen.ExpressionTrees;
using NUnit.Framework;

namespace Demo.DynamicCodeGen.Tests.ExpressionTrees
{
    public class MyExpressionMapper_Tests
    {
        [Test]
        public void CreateMapMethod_WhenCalled_Success()
        {
            var map = MyExpressionMapper.CreateMapMethod<Src, Dest>();

            var src = new Src();
            var dest = new Dest();

            map(src, dest);

            var result = ObjectComparer.AreEqual(src, dest);

            Assert.IsTrue(result.Success);
        }
    }
}
