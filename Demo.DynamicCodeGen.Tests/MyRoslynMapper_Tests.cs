using System;
using Demo.DynamicCodeGen.Common;
using Demo.DynamicCodeGen.ExternalMappers;
using Demo.DynamicCodeGen.Roslyn;
using NUnit.Framework;

namespace Demo.DynamicCodeGen.Tests.Roslyn
{
    public class AutoMapperFacade_Tests
    {
        [Test]
        public void CreateMapMethod_WhenCalled_Success()
        {
            var map = AutoMapperFacade.Instance.CreateMapMethod<Src, Dest>();

            var src = new Src();
            var dest = new Dest();

            map(src, dest);

            var result = ObjectComparer.AreEqual(src, dest);

            Assert.IsTrue(result.Success);
        }
    }
}
