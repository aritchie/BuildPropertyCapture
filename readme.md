# Capture MSBuild Properties

Runs a source generator that dumps property key/values into a source generated dictionary. 

Currently, using dependency injection to get access to these values, but looking into making this static somehow.

## Setup
TODO: once this is published to nuget
1. Install nuget library into main application
2. In your dependency injection - run `services.AddBuildProperties()`
3. Resolve `BuildPropertyCapture.IBuildProperties`
4. Use readonly dictionary `Properties` to retrieve values

## To Add Properties You Want
```xml
<ItemGroup>
    <CompilerVisibleProperty Include="PublishAot" />
    <CompilerVisibleProperty Include="UseDotNetNativeToolchain" />
    <CompilerVisibleProperty Include="PublishTrimmed" />
</ItemGroup>
```

## TODOs
* The ideal use of this source generator is to make these values available to LIBRARIES which is difficult if the library wants to be AOT compliant
* We don't want the library to need an overload of "what generated class to use"
* We don't want to detect multiple generations which can happen

Ideas to get around above
* Implement a "static" class in the library that is set by source generated values.  The source generated needs to "startup" though.
  * Use of `[ModuleInitializer]` on dotnet runtimes
  * `IMauiInitializeService` on MAUI - could be too late though