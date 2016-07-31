#r "../node_modules/fable-core/Fable.Core.dll"
#load "fstypings/Politifact.fsx"

open Fable.Core
open Fable.Import.Node

let run (peopleSlugs:string list) =
  let countIt (group:string * Politifact.Statement[]) =
    let personSlug, statements = group
    personSlug,(statements
                  |> Array.filter (fun s-> s.speaker.name_slug = personSlug)
                  |> Array.length)
  let parseIt person =
    let fileName = sprintf "data/people/%s.json" person
    let json = fs.readFileSync(fileName).toString()
    let fixRemoveNulls = json.Replace("null", "\"\"")
    (person, Serialize.ofJson(fixRemoveNulls))
  let counts = 
    peopleSlugs
      |> List.map parseIt
      |> List.map countIt
  
  for slug,count in counts do
    printfn "%s:%i" slug count


  0