open System

type Subject = {
  subject_slug:string
  subject:string
}

type StatementType = {
  statement_type: string
}

type Party  = {
  party: string
  party_slug:string
}

type Person = {
  party: Party
  name_slug: string
  last_name: string
  first_name: string
}

type Ruling = {
  ruling_slug: string
  ruling: string
  canonical_ruling_graphic: string
}

type Statement = {
  statement_url: string
  statement_date: DateTime
  target: Person[]
  subject:Subject
  statement:string
  ruling_headline: string
  ruling_link_text: string
  statement_context: string
  statement_type: StatementType
  ruling_date: DateTime
  ruling: Ruling
  speaker: Person 
}