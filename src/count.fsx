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

  let writeData limit =
    printfn "Generating csv for %i" limit
    let file = sprintf "data/people/%i.csv" limit
    fs.writeFileSync(file,"0.0",null)
    fs.appendFileSync(file,String.Join(",","name"::slugs),null)
    fs.appendFileSync(file, "\n" ,null)

    for slug,statements,count in parsed do
      if count >= limit then
        let latestByLimit =
          statements
            |> Array.sortBy (fun x-> x.statement_date)
            |> Array.rev
            |> Array.take limit
            |> Array.countBy (fun x -> x.ruling.ruling_slug)
          
        let counts = 
          slugs
            |> List.map (fun x-> 
                              let found = 
                                latestByLimit 
                                  |> Array.tryFind (fun (k,_) -> k = x )
                              match found with
                                | Some(k,c) -> c.ToString()
                                | None -> "0"
                        )

        fs.appendFileSync(file, String.Join(",",slug::counts) ,null)
        fs.appendFileSync(file, "\n" ,null)
        printfn "%s:%i" slug count

  writeData 50
  writeData 100
  writeData 150
  writeData 200
  0