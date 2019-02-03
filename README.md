# PUML Sequence Diagrams from C# Code
 This started as sort of a research project whether it is possible to draw sequence diagrams directly from C#-Code using roslyn. Part of it was inspired by [Gabriele Tomassetti](https://tomassetti.me/generate-diagrams-csharp/).

 As it looks not totally discouraging I have decided to put the code online. Maybe some ouf you out there can use it as starting point or even want to contribute. I plan to keep this as my project aside work. So do not expect fast progress. Things I plan to set out in the near future:
 * Using it on a larger code base than I have at home
 * Write more tests
 * Try to add a GUI or at least an UI which does not throw away the analyzed data every time it finished analyzing a method
 * Discover how overloaded methods are handeled
 * Look into the interface resolving
 * Look into resolving dependencies across solutions

 Feel free to open an issue or comment on existing ones.  

## How to test it
Go to snapshots and load either the .net472 (WinSeqDiag.exe) version or the netcore (SeqDiagram.dll) versions. Both are CLI only. You have to supply them with a path to a solution, projectname, classname and methodname to analyze. The diagram will be written to the specified output file.
In case of the netcore version you will also have to supply the path to the MSBuil.dll of the netcore SDK.

A typical call will be look like:
```WinSeqDiag -s C:\source\myprojec\mysolution.sln -p MyProject -c MyClass -m MyMethod -o C:\temp\somediagram.wsd```

## Useful resources
* [Generate diagrams from C# source code using Roslyn](https://tomassetti.me/generate-diagrams-csharp/)
* [Learn roslyn](https://joshvarty.com/learn-roslyn-now/)
* [PlantUML](http://plantuml.com/de/sequence-diagram)
* VisualStudioCode with the PlantUML extension