using System;
using Demo.DynamicCodeGen.Common;

namespace Demo.DynamicCodeGen.Benchmarks
{
    public static class HandwrittenMapper
    {
        public static Action<Src, Dest> CreateFunc()
        {
            Action<Src, Dest> action = Map;

            return action;
        }

        public static void Map(Src src, Dest dest)
        {
            dest.Name = src.Name;
            dest.DateTime = src.DateTime;
            dest.Float = src.Float;
            dest.Number = src.Number;
        }
    }
}