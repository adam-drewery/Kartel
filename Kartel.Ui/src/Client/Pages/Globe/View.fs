module Pages.Globe.View

open Elmish
open Fable.React
open Fable.React.Props
open Feliz
open Fable.Import.D3
open Fable.Core
open Fable.Core.JsInterop

type CountryData = {
    PolygonsJson: obj
    NamesTsv: string
}

type Model = {
    CountryData: CountryData option
}

type Msg =
| LoadGlobe
| LoadCountryData of CountryData
| FetchCountryData
| FetchCountryDataFailed of string

importAll "./../../../../node_modules/d3-fetch/src/index.js"

let loadData() =
    let urlJson = "mapdata/d3-countries.json"
    let urlTsv = "mapdata/world-country-names.tsv"

    let world =
        Fetch.fetch urlJson []
        |> Promise.bind (fun resp -> resp.json())

    let countries : JS.Promise<string> =
        d3?tsv (urlTsv,id)

    promise {
        let! worldResult = world
        let! countriesResult = countries
        // let countriesResult = ""
        return { PolygonsJson = worldResult; NamesTsv = countriesResult; }
    }

let initModel : Model = {
    CountryData = None
}

let init() =
    // initModel, Cmd.none
    initModel, Cmd.ofMsg FetchCountryData

// let inline render() = Components.Globe.renderView()

let update (msg: Msg) (model: Model) =
    match model, msg with
    | _, LoadGlobe ->
        Components.Globe.renderView model.CountryData.Value.PolygonsJson model.CountryData.Value.NamesTsv
        model, Cmd.none

    | _, LoadCountryData countryData ->
        let nextModel = { model with CountryData = Some countryData }
        nextModel, Cmd.ofMsg LoadGlobe

    | _, FetchCountryDataFailed error ->
        Browser.Dom.console.log("Failed to load CountryData with error: " + error)
        model, Cmd.none

    | _, FetchCountryData ->
        model, Cmd.OfPromise.either (loadData) () (LoadCountryData) (fun a -> FetchCountryDataFailed a.Message)

let view (model: Model) dispatch =
    Html.div [
        prop.children [
            button
                [ OnClick (fun _ -> dispatch FetchCountryData) ]
                [ str "fetch country data" ]
            button [ OnClick (fun _ -> dispatch LoadGlobe) ] [ str "load globe" ]
            Html.div [
                prop.style [ style.height (length.px 30); style.color "#ff0000" ]
                prop.children [ Html.h2 [ prop.id "current"; prop.children [ str "Heading" ] ] ]
            ]
            Html.canvas [ prop.id "globe" ]
            str "stuff"
        ]
    ]