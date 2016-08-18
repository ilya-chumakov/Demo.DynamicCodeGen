using Demo.DynamicCodeGen.Common;
using Demo.DynamicCodeGen.Emit;
using NUnit.Framework;

namespace Demo.DynamicCodeGen.Tests.Emit
{
    public class MyEmitMapper_Tests
    {
        [Test]
        public void CreateMapMethod_WhenCalled_Success()
        {
            var map = MyEmitMapper.CreateMapMethod<Src, Dest>();

            var src = new Src();
            var dest = new Dest();

            map(src, dest);

            var result = ObjectComparer.AreEqual(src, dest);

            Assert.IsTrue(result.Success);
        }
    }
}
