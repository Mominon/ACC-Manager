﻿using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Race Element")]
[assembly: AssemblyDescription("Universal Tools for Sim Racing.")]
[assembly: AssemblyConfiguration(
#if DEBUG 
    "Dev"
#else
    "Release"
#endif
)]
[assembly: AssemblyCompany("Element Future")]
[assembly: AssemblyProduct("Race Element")]
[assembly: AssemblyCopyright("Copyright 2023 © Reinier Klarenberg")]
[assembly: AssemblyTrademark("Element Future")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

//In order to begin building localizable applications, set
//<UICulture>CultureYouAreCodingWith</UICulture> in your .csproj file
//inside a <PropertyGroup>.  For example, if you are using US english
//in your source files, set the <UICulture> to en-US.  Then uncomment
//the NeutralResourceLanguage attribute below.  Update the "en-US" in
//the line below to match the UICulture setting in the project file.

//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]


[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located (used if a resource is not found in the page,or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located (used if a resource is not found in the page, app, or any theme specific resource dictionaries)
)]

//      Major Version, Minor Version, Build Number, Revision
[assembly: AssemblyVersion("0.1.7.7")]
[assembly: AssemblyFileVersion("0.1.7.7")]
[assembly: NeutralResourcesLanguage("")]
