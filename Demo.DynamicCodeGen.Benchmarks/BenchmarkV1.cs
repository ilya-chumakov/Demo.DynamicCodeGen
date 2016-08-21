using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.DynamicCodeGen.Common;
using Demo.DynamicCodeGen.Emit;
using Demo.DynamicCodeGen.ExpressionTrees;
using Demo.DynamicCodeGen.ExternalMappers;
using Demo.DynamicCodeGen.Roslyn;
using NUnit.Framework;

namespace Demo.DynamicCodeGen.Benchmarks
{
    public class BenchmarkV1
    {
        public int NameMaxLength { get; private set; }
        public Dictionary<string, Action<Src, Dest>> Mappers { get; set; }

        [SetUp]
        public void SetUp()
        {
            Mappers = new Dictionary<string, Action<Src, Dest>>();

            Register(MyEmitMapper.Instance);
            Register(MyExpressionMapperV1.Instance);
            Register(MyExpressionMapperV2.Instance);
            Register(MyRoslynMapper.Instance);
            Register(HandwrittenMapper.Instance);
            Register(AutoMapperFacade.Instance);
            Register(EmitMapperFacade.Instance);
            Register(FastMapperFacade.Instance);

            NameMaxLength = Mappers.Keys.Max(k => k.Length);
        }

        private void Register(ITestableMapper mapper)
        {
            Mappers.Add(mapper.GetType().Name, mapper.CreateMapMethod<Src, Dest>());
        }

        [Test]
        public void Run_AllMappers_MeasuresTime()
        {
            //int[] exponents = new[] { 5, 6 };
            int[] exponents = new[] { 5, 6, 7, 8 };
            Console.Write("Exponents:  ");
            Array.ForEach(exponents, e => Console.Write(e + " "));
            Console.WriteLine();
            Console.WriteLine("--------------------------");

            foreach (var kvp in Mappers)
            {
                Console.Write(MapperNameFormatted(kvp.Key));

                Action<Src, Dest> mapMethod = kvp.Value;

                GC.Collect();

                //warmup
                mapMethod(new Src(), new Dest());
                
                foreach (int exponent in exponents)
                {
                    int iterationCount = (int)Math.Pow(10, exponent);

                    var stopwatch = new Stopwatch();
                    for (int i = 0; i < iterationCount; i++)
                    {
                        var src = new Src();
                        var dest = new Dest();

                        stopwatch.Start();
                        mapMethod(src, dest);
                        stopwatch.Stop();
                    }
                    Console.Write(stopwatch.ElapsedMilliseconds + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("--------------------------");
        }

        private string MapperNameFormatted(string key)
        {
            return key + new String(' ', NameMaxLength - key.Length + 2);
        }
    }
}
