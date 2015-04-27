namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("Kunai")>]
[<assembly: AssemblyProductAttribute("Kunai")>]
[<assembly: AssemblyDescriptionAttribute("kunai4ninja")>]
[<assembly: AssemblyVersionAttribute("1.0")>]
[<assembly: AssemblyFileVersionAttribute("1.0")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.0"
