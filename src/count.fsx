#r "../node_modules/fable-core/Fable.Core.dll"
#load "fstypings/Politifact.fsx"

open System
open Fable.Core
open Fable.Import.Node

let run (peopleSlugs:string list) =
  let countIt (personSlug:string) (statements: Politifact.Statement[]) =
    statements
      |> Array.filter (fun s-> s.speaker.name_slug = personSlug)
      |> Array.length
  let parseIt person =
    let fileName = sprintf "data/people/%s.json" person
    let json = fs.readFileSync(fileName).toString()
    let fixRemoveNulls = json.Replace("null", "\"\"") //null crashes fables json parsing
    let statements = Serialize.ofJson(fixRemoveNulls)
    (person, statements, statements |> countIt person)

  let parsed =
     peopleSlugs
      |> List.map parseIt

  try
    fs.mkdirSync("data/people-50", 777.0)
  with _ -> ()
  
  for slug,statements,count in parsed do
    if count >= 50 then
      let latest50 =
        statements
          |> Array.sortBy (fun x-> x.statement_date)
          |> Array.rev
          |> Array.take 50
    
      let fileName = sprintf "data/people-50/%s.json" slug
      fs.writeFileSync(fileName, Serialize.toJson latest50, null )
      printfn "%s:%i" slug count

  0