namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("Kunai")>]
[<assembly: AssemblyProductAttribute("Kunai")>]
[<assembly: AssemblyDescriptionAttribute("kunai4ninja")>]
[<assembly: AssemblyVersionAttribute("0.0.1")>]
[<assembly: AssemblyFileVersionAttribute("0.0.1")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.0.1"
