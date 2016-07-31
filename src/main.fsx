module Run
#r "../node_modules/fable-core/Fable.Core.dll"
#load "./fstypings/Fable.Import.Commander.fsx"
#load "count.fsx"
#load "download.fsx"
#load "graph.fsx"

open System
open Fable.Core
open Fable.Import
open Fable.Import.Node
open Fable.Import.commander

// This just shows how a F# Console Application can be
// translated to node.js. The entry point we'll be called automatically
// with the arguments passed to the node script.
// Note that for this simple example no dependencies but fable-core are needed.

[<EntryPoint>]
let main argv =
  let nodeArgs = ResizeArray(argv)
  nodeArgs.Insert(0,"poltiface-honstey.js")
  nodeArgs.Insert(0,"node")

  let program = Fable.Import.commander.Globals.``default``

  program
      .version("1.0.0")
      .option("-d, --download", "Download data from Politifact")
      .parse(nodeArgs)
      |> ignore

  let people = [ //people used in the Robert Mann Graph
                  "donald-trump"
                  "michele-bachmann"
                  "ted-cruz"
                  "newt-gingrich"
                  "sarah-palin"
                  "rick-santorum"
                  "scott-walker"
                  "rick-perry"
                  "paul-ryan"
                  "john-mccain"
                  "mitt-romney"
                  "rand-paul"
                  "chris-christie"
                  "joe-biden"
                  "john-kasich"
                  "bernie-s"
                  "jeb-bush"
                  "hillary-clinton"
                  "barack-obama"

                ]

  if program.download then
      Download.run people
  else if program.count then
      Count.run people
  else if program.graph then
      Graph.run()
  else
      1