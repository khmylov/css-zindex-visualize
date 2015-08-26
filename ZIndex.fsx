open System
open System.IO
open System.Text.RegularExpressions

let getFiles dir = Directory.GetFiles(dir, "*.css", SearchOption.AllDirectories)
let fileContents = Seq.map (File.ReadAllText)

let regex = Regex(@"z-index:\s*([A-Za-z0-9]*);")

let toValue (m : Match) = m.Groups.[1].Value

let matcher = 
    getFiles
    >> fileContents
    >> Seq.map (regex.Matches)
    >> Seq.map (Seq.cast >> Seq.map toValue)
    >> Seq.collect id
    >> Seq.groupBy id
    >> Seq.map (fun (v, g) -> (v, Seq.length g))
    >> Seq.sortBy fst
    >> Seq.toArray


#load "packages/FSharp.Charting.0.90.6/FSharp.Charting.fsx"
open FSharp.Charting

let targetDir = @"d:\projects\Work\Taucraft\TP\Code\Main\Tp.Web\JavaScript\tau\"
let input = matcher targetDir

(Chart.Pie input).ShowChart()