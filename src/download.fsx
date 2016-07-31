#r "../node_modules/fable-core/Fable.Core.dll"

open System
open Fable.Core
open Fable.Import
open Fable.Import.Node

let run (people:string list) =
  let downloadFile url filename =
    let path = sprintf "./data/%s" filename
    let file = fs.createWriteStream(path)
    printfn "Downloading %s..." path
    http.get( url, Func<_,_>(fun r-> 
                                r.pipe(file) |> ignore
                                printfn "Downloaded %s." path
                            ))

  let downloadPerson personSlug =
    let numberOfComments = 150 //bug in poltifact api includes subjects as well of speakers, going up to get 50
    let url = sprintf "http://www.politifact.com/api/statements/truth-o-meter/people/%s/json/?n=%i" personSlug numberOfComments
    let fileName = sprintf "people/%s.json" personSlug
    downloadFile url fileName
  try
    fs.mkdirSync("data/people", null)
  with _ -> ()
  
  downloadFile  "http://www.politifact.com/api/people/all/json/" "people.json"
                          |> ignore
  let statuses = people
                  |> Seq.map downloadPerson
                  |> Seq.toList
  0