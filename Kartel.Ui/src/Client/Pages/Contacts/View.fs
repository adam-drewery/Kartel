module Pages.Contacts.View

open Elmish
open Fable.React
open Feliz
open Theme.Color
open Theme.Font
open Shared
open Fable.Core.JsInterop
importAll "leaflet"
importAll "react-leaflet"


type Model = {
    MapModel: Components.MainMap.Model
}

type ExternalMsg =
| Noop
| LayoutMsg of Layout.Types.Msg
| SetSelectedContact of Contact

type Msg =
| Noop
| MapMsg of Components.MainMap.Msg
| ExternalMsg of ExternalMsg

let init (initContacts: Contact list) : Model * Cmd<Msg> =
    let initMapModel, initMapCmd = Components.MainMap.init initContacts
    let initialModel = {
        MapModel = initMapModel
    }
    initialModel, Cmd.batch [
        Cmd.map MapMsg initMapCmd
    ]

let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> * ExternalMsg =
    match currentModel, msg with

    // | model, LeftNavigationMsg leftNavMsg ->
    //     match model.LeftNavigationModel, leftNavMsg with
    //     | ContactsPanel subModel, ContactsPanelMsg subMsg ->
    //         let nextSubModel, nextSubCmd = Components.Contacts.update subMsg (subModel)
    //         let nextModel = { currentModel with LeftNavigationModel = ContactsPanel nextSubModel }
    //         nextModel, (Cmd.map (ContactsPanelMsg >> LeftNavigationMsg) nextSubCmd), ExternalMsg.Noop
    //     | _ -> currentModel, Cmd.none, ExternalMsg.Noop
    // | _, GameHubMsg subMsg ->
    //     let nextSubModel, nextSubCmd = Components.GameHub.update subMsg currentModel.GameHub
    //     let nextModel = { currentModel with GameHub = nextSubModel }
    //     nextModel, Cmd.map GameHubMsg nextSubCmd
    | _, MapMsg subMsg ->
        let nextSubModel, nextSubCmd = Components.MainMap.update subMsg currentModel.MapModel
        let nextModel = { currentModel with MapModel = nextSubModel }
        nextModel, Cmd.map MapMsg nextSubCmd, ExternalMsg.Noop


    | _, ExternalMsg externalMsg ->
        currentModel, Cmd.none, externalMsg
    | _, Noop -> currentModel, Cmd.none, ExternalMsg.Noop


let leftNavCollapseButtonContainer onClick =
    Html.div [
        prop.style [
            style.position.absolute
            style.top 0
            style.left 0
            style.zIndex 1000
        ]
        prop.children [
            // Layout.LeftNavigation.leftNavCollapseButton onClick
        ]
    ]

let view (model : Model) (dispatch : Msg -> unit) =

    Html.div [
        prop.style [
            style.width (length.perc 100)
            style.color "white"
            // style.overflowY.scroll
            style.position.relative
        ]
        prop.children [
            Components.MainMap.view model.MapModel (MapMsg >> dispatch)
        ]
    ]
