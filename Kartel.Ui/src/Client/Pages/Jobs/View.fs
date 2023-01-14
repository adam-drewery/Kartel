module Pages.Jobs.View

open Elmish
open Fable.React
open Feliz
open Theme.Color
open Theme.Font
open Shared

type LeftNavigationModel =
| ContactsPanel of Components.Contacts.Model
| ExamplePanel of Components.Contacts.Model
| Nowt of int

type Model = {
    // Num: int
    // AvailableContacts: Contact list
    // SelectedContact: Contact option
    // GameHub: Components.GameHub.Model
    LeftNavigationModel: LeftNavigationModel
}

type LeftNavigationMessage =
| ContactsPanelMsg of Components.Contacts.Msg

type ExternalMsg =
| Noop
| LayoutMsg of Layout.Types.Msg

type Msg =
| Noop
// | SetSelectedContact of Contact
// | GameHubMsg of Components.GameHub.Msg
| LeftNavigationMsg of LeftNavigationMessage
| ExternalMsg of ExternalMsg

// let initContacts = [
//     { Name = "Other stuff" }
//     { Name = "Other stuff" }
//     { Name = "Other stuff" }
//     { Name = "Other stuff" }
//     { Name = "Other stuff" }
// ]

let init () : Model * Cmd<Msg> =
    // let initGameHubModel, initGameHubCmd = Components.GameHub.init()
    let initialModel = {
        // Num = 13
        // AvailableContacts = initContacts
        // SelectedContact = None
        // GameHub = initGameHubModel
        LeftNavigationModel = Nowt 1
    }
    initialModel, Cmd.batch [
        Cmd.map (ContactsPanelMsg >> LeftNavigationMsg) Cmd.none ] // TODO: Cmd.none?

let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> * ExternalMsg =
    match currentModel, msg with
    | model, LeftNavigationMsg leftNavMsg ->
        match model.LeftNavigationModel, leftNavMsg with
        | ContactsPanel subModel, ContactsPanelMsg subMsg ->
            // let nextSubModel, nextSubCmd = Components.Contacts.update subMsg (subModel)
            // let nextModel = { currentModel with LeftNavigationModel = ContactsPanel nextSubModel }
            // nextModel, Cmd.map (ContactsPanelMsg >> LeftNavigationMsg) nextSubCmd
            currentModel, Cmd.none, ExternalMsg.Noop
        | _ -> currentModel, Cmd.none, ExternalMsg.Noop
    // | _, GameHubMsg subMsg ->
    //     let nextSubModel, nextSubCmd = Components.GameHub.update subMsg currentModel.GameHub
    //     let nextModel = { currentModel with GameHub = nextSubModel }
    //     nextModel, Cmd.map GameHubMsg nextSubCmd
    // | _, ExternalMsg externalMsg ->
    //     currentModel, Cmd.none, externalMsg
    | _, Noop -> currentModel, Cmd.none, ExternalMsg.Noop

let faSquare =
    let marginHorizontal = 10
    Html.div [
        prop.style [
            style.backgroundColor Color.custom5
            style.height (length.px 15)
            style.width (length.px 30)
            style.borderRadius (length.px 2)
            style.display.flex
            style.alignItems.center
            style.custom("justifyContent","center")
            style.marginLeft marginHorizontal
            style.marginRight marginHorizontal
        ]
        prop.children [
            Html.i [
                prop.style [
                    style.color Color.custom2
                ]
                prop.className "fas fa-long-arrow-alt-right"
            ]
        ]
    ]

let printContactNameIfSome (c: Contact option) =
    if c.IsSome then c.Value.Name else "None selected"

let playerInfoPanel (selectedContact: Contact option) =
    Html.div [
        prop.style [
            style.display.flex
            style.alignItems.center
            style.custom("justifyContent","center")
            style.margin (length.px 8)
            style.border (1, borderStyle.solid, "rgba(100,100,100,0.5)")
            style.flexDirection.column
        ]
        prop.children [
            Html.img [
                prop.style [
                    style.height (length.perc 80)
                    style.padding (length.px 5)
                    ]
                prop.src "./Guy1.png"
            ]
            Html.div [
                prop.style [
                    style.fontFamily Font.First.regular
                    style.fontSize (length.em 1.2)
                    style.color Color.custom6
                    style.display.flex
                    style.alignItems.center
                    style.custom("justifyContent","center")
                    style.height (length.perc 20)
                    style.flexGrow 1
                ]
                prop.children [
                    str (printContactNameIfSome selectedContact)
                ]
            ]
        ]
    ]

let bottomNavigationTickerContent = [
    str "Dave the rave has just received a large shipment..."
    faSquare
]

let view (model : Model) (dispatch : Msg -> unit) (layoutModel: Layout.Types.Model) =
    let bottomNavMainPanelContent = [ str "jobs page" ]
    let leftNavHeader, leftNavContent =
        // match model.
        "Jobs", [ str "left nav content"]

    Html.div [
        prop.style [
            style.display.flex
            style.minHeight (length.perc 100)
        ]

        prop.children [
            Layout.LeftNavigation.view leftNavHeader true leftNavContent

            Html.div [
                prop.style [ style.height (length.perc 100) ]
                prop.children [ Html.img [ prop.src "darkmap.png" ] ]
            ]

            Layout.BottomNavigation.mainPanel
                bottomNavigationTickerContent
                bottomNavMainPanelContent
                layoutModel.BottomNavigationMenu.IsActive
                (fun _ -> dispatch (ExternalMsg (LayoutMsg Layout.Types.Msg.ToggleBottomNavigationMenu)))
        ]
    ]
