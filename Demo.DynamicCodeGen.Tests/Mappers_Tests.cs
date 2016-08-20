using Demo.DynamicCodeGen.Common;
using Demo.DynamicCodeGen.Emit;
using Demo.DynamicCodeGen.ExpressionTrees;
using Demo.DynamicCodeGen.ExternalMappers;
using Demo.DynamicCodeGen.Roslyn;
using NUnit.Framework;

namespace Demo.DynamicCodeGen.Tests
{
    public class Mappers_Tests
    {
        [Test]
        public void MyEmitMapper_Map_Success()
        {
            Test(MyEmitMapper.Instance);
        }
        
        [Test]
        public void MyExpressionMapperV1_Map_Success()
        {
            Test(MyExpressionMapperV1.Instance);
        }
        
        [Test]
        public void MyExpressionMapperV2_Map_Success()
        {
            Test(MyExpressionMapperV2.Instance);
        }
        
        [Test]
        public void MyRoslynMapper_Map_Success()
        {
            Test(MyRoslynMapper.Instance);
        }

        [Test]
        public void AutoMapperFacade_Map_Success()
        {
            Test(AutoMapperFacade.Instance);
        }

        [Test]
        public void EmitMapperFacade_Map_Success()
        {
            Test(EmitMapperFacade.Instance);
        }

        [Test]
        public void FastMapperFacade_Map_Success()
        {
            Test(FastMapperFacade.Instance);
        }

        private static void Test(ITestableMapper mapper)
        {
            var map = mapper.CreateMapMethod<Src, Dest>();

            var src = new Src();
            var dest = new Dest();

            map(src, dest);

            var result = ObjectComparer.AreEqual(src, dest);

            Assert.IsTrue(result.Success);
        }
    }
}
