# Demo.DynamicCodeGen

Examples of dynamic code generation using various technologies: 

 - parsing and compilation C# code via Roslyn
 - direct MSIL emitting
 - expression tree compilation

The only thing implemented is simple object-to-object mapping.  The benchmark estimates map perfomance and provides comparison to following popular .NET mapping tools:

 - AutoMapper 
 - EmitMapper 
 - FastMapper

Mapping task is to copy all properties from `Src` instance to equivalent `Dest` instance:
    
    public class Dest
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public float Float { get; set; }
        public DateTime DateTime { get; set; }
    }
    
Summarized time to execute 10^8 map operations is on the chart:
![Alt text](/content/Chart.png?raw=true "Title")
The results are quite interesting: technically it is possible to generate optimal code (I mean "optimal" the code hand-written mapper contains) via any of these technologies: Roslyn, MSIL, expression trees.

**Where are AutoMapper and FastMapper?** They are too slow to include it to chart: 5-10x times slower than EmitMapper. DISCLAIMER: I like AutoMapper for its convenient and flexible API. AutoMapper is fast enough in many real-world cases, and it is slower than handwritten code just because it provides a lot of awesome features to customize your mappings.

**What is the difference between expression tree mappers?**
Well, you can compile an `Expression<T>` to a delegate by simple call:

    expression.Compile() //V1

However, that delegate perfomance is greatly degrades in this case! The workaround it to explicitly define a dynamic assembly, define a type inside and compile the expression to this type's method:

    expression.CompileToMethod(methodBuilder); //V2
    
You can find similar case and more details at [this](http://stackoverflow.com/questions/5053032/performance-of-compiled-to-delegate-expression) StackOverflow thread.
