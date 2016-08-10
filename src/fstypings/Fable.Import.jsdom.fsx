namespace Fable.Import
#r "../../node_modules/fable-core/Fable.Core.dll"
open System
open System.Text.RegularExpressions
open Fable.Core
open Fable.Import.Browser
open Fable.Import.Node.NodeJS
#load "Fable.Import.JQuery.fsx"

module jsdom =
    type VirtualConsole =
        inherit EventEmitter
        abstract sendTo: console: Console -> VirtualConsole

    and [<AllowNullLiteral>] VirtualConsoleOptions =
        interface end

    and [<AllowNullLiteral>] WindowProperties =
        abstract parsingMode: string option with get, set
        abstract contentType: string option with get, set
        abstract parser: obj option with get, set
        abstract url: string option with get, set
        abstract referrer: string option with get, set
        abstract cookieJar: CookieJar option with get, set
        abstract cookie: string option with get, set
        abstract resourceLoader: obj option with get, set
        abstract deferClose: bool option with get, set
        abstract concurrentNodeIterators: float option with get, set
        abstract virtualConsole: VirtualConsole option with get, set
        abstract created: Func<obj, Window, obj> option with get, set
        abstract features: FeatureOptions option with get, set
        abstract top: Window option with get, set

    and [<AllowNullLiteral>] CookieJar =
        interface end

    and DocumentWithParentWindow =
        inherit Document
        abstract parentWindow: Window with get, set

    and [<AllowNullLiteral>] Callback =
        [<Emit("$0($1...)")>] abstract Invoke: errors: ResizeArray<Fable.Import.Node.Error> * window: Window -> obj

    and [<AllowNullLiteral>] FeatureOptions =
        abstract FetchExternalResources: U2<ResizeArray<string>, bool> option with get, set
        abstract ProcessExternalResources: U2<ResizeArray<string>, bool> option with get, set
        abstract SkipExternalResources: U2<string, bool> option with get, set
        abstract QuerySelector: U2<string, bool> option with get, set

    and [<AllowNullLiteral>] EnvDocument =
        abstract referrer: string option with get, set
        abstract cookie: string option with get, set
        abstract cookieDomain: string option with get, set

    and [<AllowNullLiteral>] Config =
        abstract html: string option with get, set
        abstract file: string option with get, set
        abstract url: string option with get, set
        abstract scripts: ResizeArray<string> option with get, set
        abstract src: ResizeArray<string> option with get, set
        abstract jar: CookieJar option with get, set
        abstract parsingMode: string option with get, set
        abstract document: EnvDocument option with get, set
        abstract features: FeatureOptions option with get, set
        abstract virtualConsole: VirtualConsole option with get, set
        abstract ``done``: Callback option with get, set
        abstract loaded: Callback option with get, set
        abstract created: Func<Fable.Import.Node.Error, Window, unit> option with get, set

    type [<Import("*","jsdom")>] Globals =
        static member debugMode with get(): bool = failwith "JS only" and set(v: bool): unit = failwith "JS only"
        static member availableDocumentFeatures with get(): FeatureOptions = failwith "JS only" and set(v: FeatureOptions): unit = failwith "JS only"
        static member defaultDocumentFeatures with get(): FeatureOptions = failwith "JS only" and set(v: FeatureOptions): unit = failwith "JS only"
        static member applyDocumentFeatures with get(): FeatureOptions = failwith "JS only" and set(v: FeatureOptions): unit = failwith "JS only"
        static member env(urlOrHtml: string, scripts: string, config: Config, ?callback: Callback): unit = failwith "JS only"
        static member env(urlOrHtml: string, scripts: string, callback: Callback): unit = failwith "JS only"
        static member env(urlOrHtml: string, scripts: ResizeArray<string>, config: Config, ?callback: Callback): unit = failwith "JS only"
        static member env(urlOrHtml: string, scripts: ResizeArray<string>, callback: Callback): unit = failwith "JS only"
        static member env(urlOrHtml: string, callback: Callback): unit = failwith "JS only"
        static member env(urlOrHtml: string, config: Config, ?callback: Callback): unit = failwith "JS only"
        static member env(config: Config, ?callback: Callback): unit = failwith "JS only"
        static member serializeDocument(doc: Document): string = failwith "JS only"
        static member createVirtualConsole(?options: VirtualConsoleOptions): VirtualConsole = failwith "JS only"
        static member getVirtualConsole(window: Window): VirtualConsole = failwith "JS only"
        static member createCookieJar(): CookieJar = failwith "JS only"
        static member nodeLocation(node: Node): obj = failwith "JS only"
        static member reconfigureWindow(window: Window, newProps: WindowProperties): unit = failwith "JS only"
        static member jQueryify(window: Window, jqueryUrl: string, callback: Func<Window, JQuery, obj>): unit = failwith "JS only"
        static member jsdom(markup: string, ?config: Config): DocumentWithParentWindow = failwith "JS only"


