# Capture MSBuild Properties

Runs a source generator that dumps property key/values into a source generated dictionary. 

Currently, using dependency injection to get access to these values, but looking into making this static somehow.

## TODOs
* The ideal use of this source generator is to make these values available to LIBRARIES which is difficult if the library wants to be AOT compliant
* We don't want the library to need an overload of "what generated class to use"
* We don't want to detect multiple generations which can happen

Ideas to get around above
* Implement a "static" class in the library that is set by source generated values.  The source generated needs to "startup" though.
  * Use of `[ModuleInitializer]` on dotnet runtimes
  * `IMauiInitializeService` on MAUI - could be too late though