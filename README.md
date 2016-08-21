# Demo.DynamicCodeGen

Examples of dynamic code generation using various technologies: 

 - parsing and compilation C# code via Roslyn
 - direct MSIL emitting
 - expression trees compilation

The only thing implemented is simple object-to-object mapping.  The benchmark estimates map perfomance and provides comparison to following popular .NET mapping tools:

 - AutoMapper 
 - EmitMapper 
 - FastMapper

Result:

**Where are AutoMapper and FastMapper?** They are too slow to include it to chart: 5-10x times slower than EmitMapper. DISCLAIMER: I like AutoMapper for its convenient and flexible API. AutoMapper is fast enough in many real-world cases, and it is slower than handwritten code just because it provides a lot of awesome features to customize your mappings.

**What is the difference between expression tree mappers?**
Well, you can compile an `Expression<T>` to a delegate by simple call:

    expression.Compile() //V1

However, that delegate perfomance is greatly degrades in this case! The workaround it to explicitly define a dynamic assembly, define a type inside and compile the expression to this type's method:

    expression.CompileToMethod(methodBuilder); //V2
    
You can find similar case and more details at [this](http://stackoverflow.com/questions/5053032/performance-of-compiled-to-delegate-expression) StackOverflow thread.
