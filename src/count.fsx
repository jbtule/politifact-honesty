#r "../node_modules/fable-core/Fable.Core.dll"
#load "fstypings/Politifact.fsx"

open System
open Fable.Core
open Fable.Import.Node

let run (peopleSlugs:string list) (cloudSlugs:string list) =
  let lyingSlugs = [
      "pants-fire"
      "false"
  ]

  let inbetweenSlugs = [
      "barely-true"
      "half-true"
  ]

  let honestSlugs = [
      "mostly-true"
      "true"
  ]

  let rulingSlugs = 
    lyingSlugs
    @ inbetweenSlugs
    @ honestSlugs
  
  let rulingSlugsSet = Set(rulingSlugs)
  let honsetSlugSet = Set(honestSlugs)
  let lyingSlugSet = Set(lyingSlugs)

  let countIt (personSlug:string) (statements: Politifact.Statement[]) =
    statements
      |> Array.filter (fun s-> s.speaker.name_slug = personSlug)
      |> Array.filter (fun s->rulingSlugsSet.Contains(s.ruling.ruling_slug))
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
  


  let writeData limit =
    printfn "Generating csv for %i" limit
    let file = sprintf "data/people/%i.csv" limit
    fs.writeFileSync(file,"",null)
    fs.appendFileSync(file,String.Join(",","name"::rulingSlugs),null)
    fs.appendFileSync(file, "\n" ,null)

    for personSlug,statements,count in parsed do
      if count >= limit then
        let latestByLimit =
          statements
            |> Array.filter (fun s-> s.speaker.name_slug = personSlug)
            |> Array.filter (fun s-> rulingSlugsSet.Contains(s.ruling.ruling_slug))
            |> Array.sortBy (fun x-> x.statement_date)
            |> Array.rev
            |> Array.take limit
            |> Array.countBy (fun x -> x.ruling.ruling_slug)
          
        let counts = 
          rulingSlugs
            |> List.map (fun x-> 
                              let found = 
                                latestByLimit 
                                  |> Array.tryFind (fun (k,_) -> k = x )
                              match found with
                                | Some(k,c) -> c.ToString()
                                | None -> "0"
                        )

        fs.appendFileSync(file, String.Join(",",personSlug::counts) ,null)
        fs.appendFileSync(file, "\n" ,null)
        printfn "%s:%i" personSlug count
  
  try
    fs.mkdirSync("data/cloud", null)
  with _ -> ()

  let writeCloud(personSlug,statements:Politifact.Statement[],_) = 
    printfn "Generating cloud csv for %s" personSlug

    let writeCloudType kind (slugSet:string Set) =
      let topics =
        statements 
          |> Array.filter (fun s-> s.speaker.name_slug = personSlug)
          |> Array.filter (fun s-> slugSet.Contains(s.ruling.ruling_slug))
          |> Array.collect (fun s-> s.subject)
          |> Array.map(fun su->su.subject)
          |> Array.filter(fun st-> String.IsNullOrWhiteSpace(st) |> not)
          |> Array.collect (fun st->st.Split(' '))
          |> Array.groupBy id
          |> Array.map (fun (w,l)-> (w,Array.length l))
      
      let file = sprintf "data/cloud/%s|%s.csv" personSlug kind
      fs.writeFileSync(file,"",null)
      for topic, count in topics do
         fs.appendFileSync(file, sprintf "%s,%i" topic count,null)
         fs.appendFileSync(file, "\n" ,null)
      ()

    writeCloudType "lying" lyingSlugSet
    writeCloudType "honest" honsetSlugSet

  let cloudSlugSet = Set(cloudSlugs)
  parsed 
    |> List.filter(fun (p,_,_)-> cloudSlugSet.Contains(p) )
    |> List.iter(writeCloud)

  writeData 50
  writeData 100
  writeData 150
  writeData 200
  0