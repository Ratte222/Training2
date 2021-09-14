using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace ProjectForBenchmark.Benchmark
{
    [MemoryDiagnoser]
    [SimpleJob(RunStrategy.ColdStart, targetCount: 5)]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class TestBency
    {
        [Benchmark]
        public string Test()
        {
            string s1 = "Abc", s2 = "cde";
            return String.Concat(s1, s2);
        }
    }
}
