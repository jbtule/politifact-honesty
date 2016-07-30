namespace Fable.Import
#r "../../node_modules/fable-core/Fable.Core.dll"
open System
open System.Text.RegularExpressions
open Fable.Core
open Fable.Import.Node
open Fable.Import.JS

module commander =
    type [<AllowNullLiteral>] ICommandStatic =
        [<Emit("new $0($1...)")>] abstract Create: ?name: string -> ICommand

    and ICommand =
        inherit NodeJS.EventEmitter
        abstract args: ResizeArray<string> with get, set
        abstract _args: ResizeArray<obj> with get, set
        abstract command: name: string * ?desc: string * ?opts: obj -> ICommand
        abstract addImplicitHelpCommand: unit -> unit
        abstract parseExpectedArgs: args: ResizeArray<string> -> ICommand
        abstract action: fn: Func<obj, unit> -> ICommand
        abstract option: flags: string * ?description: string * ?fn: U2<Func<obj, obj, unit>, Regex> * ?defaultValue: obj -> ICommand
        abstract option: flags: string * ?description: string -> ICommand
        abstract allowUnknownOption: ?arg: bool -> ICommand
        abstract parse: argv: ResizeArray<string> -> ICommand
        abstract executeSubCommand: argv: ResizeArray<string> * args: ResizeArray<string> * unknown: ResizeArray<string> -> obj
        abstract normalize: args: ResizeArray<string> -> ResizeArray<string>
        abstract parseArgs: args: ResizeArray<string> * unknown: ResizeArray<string> -> ICommand
        abstract optionFor: arg: string -> IOption
        abstract parseOptions: argv: ResizeArray<string> -> obj
        abstract opts: unit -> obj
        abstract missingArgument: name: string -> unit
        abstract optionMissingArgument: option: obj * ?flag: string -> unit
        abstract unknownOption: flag: string -> unit
        abstract version: str: string * ?flags: string -> ICommand
        abstract description: str: string -> ICommand
        abstract description: unit -> string
        abstract alias: alias: string -> ICommand
        abstract alias: unit -> string
        abstract usage: str: string -> ICommand
        abstract usage: unit -> string
        abstract name: unit -> string
        abstract largestOptionLength: unit -> float
        abstract optionHelp: unit -> string
        abstract commandHelp: unit -> string
        abstract helpInformation: unit -> string
        abstract outputHelp: unit -> unit
        abstract help: unit -> unit

    and [<AllowNullLiteral>] IOptionStatic =
        [<Emit("new $0($1...)")>] abstract Create: flags: string * ?description: string -> IOption

    and [<AllowNullLiteral>] IOption =
        abstract flags: string with get, set
        abstract required: bool with get, set
        abstract optional: bool with get, set
        abstract bool: bool with get, set
        abstract short: string option with get, set
        abstract long: string with get, set
        abstract description: string with get, set
        abstract name: unit -> string
        abstract is: arg: string -> bool

    and IExportedCommand =
        inherit ICommand
        abstract Command: ICommandStatic with get, set
        abstract Option: IOptionStatic with get, set
        //custom fields
        abstract download:bool with get
        abstract count:bool with get
        abstract graph:bool with get

    type [<Import("*","commander")>] Globals =
        static member ``default`` with get(): IExportedCommand = failwith "JS only" and set(v: IExportedCommand): unit = failwith "JS only"


