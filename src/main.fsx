module Run
#r "../node_modules/fable-core/Fable.Core.dll"
#load "./fstypings/Fable.Import.Commander.fsx"

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
        let downloadFile url filename =
            let path = sprintf "./data/%s" filename
            let file = fs.createWriteStream(path)
            printfn "Downloading %s..." path
            http.get( url, Func<_,_>(fun r-> 
                                        r.pipe(file) |> ignore
                                        printfn "Downloaded %s." path
                                    ))

        let downloadPerson personSlug =
                let numberOfComments = 75 //bug in poltifact api includes subjects as well of speakers, going up to get 50
                let url = sprintf "http://www.politifact.com/api/statements/truth-o-meter/people/%s/json/?n=%i" personSlug numberOfComments
                let fileName = sprintf "people/%s.json" personSlug
                downloadFile url fileName

        let afterMkDirHandler _ =
            downloadFile  "http://www.politifact.com/api/people/all/json/" "people.json"
                                    |> ignore
            let statuses = people 
                            |> Seq.map downloadPerson
                            |> Seq.toList
            ()

        fs.mkdir("data/people", Func<_,_>(afterMkDirHandler))
        0
    else if program.count then
        
        0
    else
        1