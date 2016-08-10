#r "../node_modules/fable-core/Fable.Core.dll"
#load "../node_modules/fable-import-d3/Fable.Import.D3.fs"
#load "./fstypings/Fable.Import.jsdom.fsx"
open System
open Fable.Core
open Fable.Import
open Fable.Import.Node
open Fable.Import.D3.Layout
open Fable.Import.jsdom
open Fable.Import.Browser

type Done(func:ResizeArray<Fable.Import.Node.Error>->Window-> obj ) =
  interface Callback with 
      member this.Invoke(errors: ResizeArray<Fable.Import.Node.Error>, window: Window)  = 
        func errors window

let run () =
    let width, height = 500.0, 500.0
    
    let export data output =  

      let doneCallback errors win =
        obj()
      0
      (*
      jsdom.Globals.env(config={
                                  html="";
                                  features={QuerySelector = true}
                        },
                        callback = Done(doneCallback))           
      *)
    
    0