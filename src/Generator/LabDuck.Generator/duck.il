.assembly extern mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                        
  .ver 4:0:0:0
}

.assembly HelloWorld
{
}

.module HelloWorld.exe

.class private auto ansi beforefieldinit HelloWorld.Program
       extends [mscorlib]System.Object
{
 .method private hidebysig static void  Main(string[] args) cil managed
  {
    .entrypoint
    // Code size       13 (0xd)
    .maxstack  8
      nop
      ldstr      "Hello World"
      call       void [mscorlib]System.Console::WriteLine(string)
      call       string [mscorlib]System.Console::ReadLine()
      pop
      ret
  } // end of method Program::Main


  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
    IL_0006:  ret
  } // end of method Program::.ctor

} // end of class HelloWorld.Program