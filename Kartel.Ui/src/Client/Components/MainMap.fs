module Components.MainMap

open Elmish
open Fable
open Fable.Core
open Fable.Core.JS
open Fable.Core.JsInterop
// open Fable.React.Props
open Feliz
open Fable.React
open Leaflet
open Geojson
open Shared

module RL = ReactLeaflet

let latLng : LatLngTuple = (51.05, -0.09)

type Model = {
    CountriesDataJson: obj option
    CountriesDataJsonResult: string
    Contacts: Contact list
}

type Msg =
| FetchCountriesJson of string
| CountriesJsonReceived of obj
| CountriesJsonFailed

let initModelPartial = {
    CountriesDataJson = None
    CountriesDataJsonResult = ""
    Contacts = []
}

let init (contacts: Contact list) =
    let initModel = { initModelPartial with Contacts = contacts }
    initModel, Cmd.ofMsg (FetchCountriesJson "")

let fetchCountriesJson() =
    promise {
        let! mapdata = Fetch.fetch "/mapdata/countries.geo.json" []
        return! mapdata.json()
    }

let update msg model =
    match model, msg with
    | _, FetchCountriesJson warbler ->
        model, Cmd.OfPromise.either (fetchCountriesJson) () (CountriesJsonReceived) (fun err -> CountriesJsonFailed)
    | _, CountriesJsonReceived data ->
        let nextModel = { model with CountriesDataJson = Some data }
        nextModel, Cmd.none
    | _, CountriesJsonFailed ->
        model, Cmd.none

let view (model: Model) dispatch =
    // div [
        // Style [ Props.Position PositionOptions.Relative]
    // ] [

        Html.div [
            prop.style [ Feliz.style.height (length.perc 100); Feliz.style.width (length.perc 100); Feliz.style.top 0; Feliz.style.position.relative ]
            prop.children [
                // button [ OnClick (fun _ -> dispatch (FetchCountriesJson "")) ] [ str "fetch" ]
                RL.map
                    [
                        RL.MapProps.Center (U3.Case3 latLng)
                        RL.MapProps.Zoom 13.0
                        RL.MapProps.Id "main-map"
                        RL.MapProps.Key "main-map"
                        RL.MapProps.ZoomControl false
                        RL.MapProps.Style [
                            Props.CSSProp.Height "100%";
                            Props.CSSProp.Width "100%";
                            Props.Position Props.PositionOptions.Absolute
                            Props.ZIndex 950 ]
                    ]
                    [
                        RL.tileLayer
                            [
                                RL.TileLayerProps.Url "https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                            ]
                            []
                        RL.geoJSON
                            (if model.CountriesDataJson.IsSome
                            then [RL.GeoJSONProps.Data (model.CountriesDataJson.Value :?> GeometryCollection)]
                            else [])
                            []
                        RL.zoomControl
                            [
                                RL.ZoomControlProps.Position RL.ControlPosition.Bottomright
                            ]
                            []
                        // model.Contacts
                        model.Contacts
                        |> List.map (fun c ->
                            RL.marker
                                [
                                    RL.MarkerProps.Position (U3.Case3 c.Location)
                                ]
                                [
                                    RL.popup [ ] [ str c.Name ]
                                ])
                        |> ofList
                    ]
            ]
        ]
    // ]