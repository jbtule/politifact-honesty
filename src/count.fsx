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
  
  let slugs = [
      "pants-fire"
      "false"
      "barely-true"
      "half-true"
      "mostly-true"
      "true"
  ]

  fs.writeFileSync("data/people/50.csv","0.0",null)
  fs.appendFileSync("data/people/50.csv",String.Join(",","name"::slugs),null)
  fs.appendFileSync("data/people/50.csv", "\n" ,null)

  for slug,statements,count in parsed do
    if count >= 50 then
      let latest50 =
        statements
          |> Array.sortBy (fun x-> x.statement_date)
          |> Array.rev
          |> Array.take 50
          |> Array.countBy (fun x -> x.ruling.ruling_slug)
        
      let counts = 
        slugs
          |> List.map (fun x-> 
                            let found = 
                              latest50 
                                |> Array.tryFind (fun (k,_) -> k = x )
                            match found with
                              | Some(k,c) -> c.ToString()
                              | None -> "0"
                      )

      fs.appendFileSync("data/people/50.csv", String.Join(",",slug::counts) ,null)
      fs.appendFileSync("data/people/50.csv", "\n" ,null)
      printfn "%s:%i" slug count

  0