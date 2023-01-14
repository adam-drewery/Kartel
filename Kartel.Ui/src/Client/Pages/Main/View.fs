module Pages.Main.View

open Elmish
open Fable.React
open Fable.React.Props
open Feliz
open Theme.Color
open Theme.Font
open Shared
open Browser.Types
// open Layout.Types
open Types
open State

let topPersistentMenuItem label onClick =
    Html.div [
        prop.className "top-persistent-menu-item"
        prop.onClick onClick
        prop.style [
            style.paddingTop 10
            style.paddingBottom 10
            style.paddingRight 10
            style.paddingLeft 10
            style.cursor "default"
        ]
        prop.children [
            str label
        ]
    ]

let topPersistentMenuButton
        isActive
        (topPersistentMenuLinks: (Msg -> unit) -> (TopPersistentMenuItemData<MouseEvent> list))
        dispatch =
    let circleDimension = 35
    div [] [
        Html.div [
            prop.style [
                style.height (length.px circleDimension)
                style.width (length.px circleDimension)
                style.borderRadius (length.px circleDimension)
                style.backgroundColor Color.custom6
                style.display.flex
                style.alignItems.center
                style.color Color.custom2
                style.custom("justifyContent","center")
                style.zIndex 99999
                style.boxShadow (4,4,8,"rgba(0,0,0,0.2)")
            ]
            prop.onClick (fun _ -> dispatch (LayoutMsg Layout.Types.Msg.TogglePersistentMenu))
            prop.children [
                Html.i [
                    prop.style [ style.fontSize (length.rem 1.0) ]
                    prop.className "fa fa-bars"
                ]
            ]
        ]
        Html.div [
            prop.className (if isActive then "persistent-menu" else "persistent-menu persistent-menu-hidden")
            prop.style [
                style.color "black"
                style.backgroundColor "white"
                style.borderRadius (length.px 2)
                style.position.absolute
                style.width (length.px 150)
                style.right 0
                style.top (length.px circleDimension)
            ]
            prop.onMouseLeave (fun _ -> dispatch (LayoutMsg Layout.Types.Msg.ClosePersistentMenu))
            prop.children [
                topPersistentMenuLinks dispatch
                |> List.map (
                    fun l ->
                        topPersistentMenuItem l.Label l.OnClick
                )
                |> ofList
            ]
        ]
    ]

let topPersistentMenuLinks dispatch = [
    { Label = "contacts"; OnClick = (fun _ -> dispatch (SwitchViewContext ContactsViewContext)) }
    { Label = "jobs"; OnClick = (fun _ -> dispatch (SwitchViewContext JobsViewContext)) }
    { Label = "globe"; OnClick = (fun _ -> dispatch (SwitchViewContext GlobeViewContext)) }
    { Label = "another item2"; OnClick = (fun _ -> ()) }
    { Label = "another item2"; OnClick = (fun _ -> ()) }
]

let topPersistentMenu isActive dispatch =
    Html.div [
        prop.style [
            style.position.absolute
            style.right (length.px 10)
            style.top (length.px 10)
            style.zIndex 1000
        ]
        prop.children [
            topPersistentMenuButton isActive topPersistentMenuLinks dispatch
        ]
    ]

let chooseContext viewContextModel dispatch topPersistentMenuComponent (layoutModel: Layout.Types.Model) =
    match viewContextModel with
    | Contacts viewContextModel ->
        Pages.Contacts.View.view viewContextModel (ContactsViewContextMsg >> ViewContextMsg >> dispatch)
    | Jobs viewContextModel ->
        Pages.Jobs.View.view viewContextModel (JobsViewContextMsg >> ViewContextMsg >> dispatch) layoutModel
    | Globe viewContextModel ->
        Pages.Globe.View.view viewContextModel (GlobeViewContextMsg >> ViewContextMsg >> dispatch)

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
                prop.className "contact-info-panel-image"
                prop.style [
                    style.height (length.perc 80)
                    style.padding (length.px 5)
                    ]
                prop.src selectedContact.Value.ImgUrl
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



let bottomNavigationMainPaneContent model dispatch : ReactElement list = [
    playerInfoPanel model.SelectedContact
    Components.GameHub.view model.GameHub (GameHubMsg >> dispatch)
    Components.PlayerAttributes.view()
]

let bottomNavigationTickerContent = [
    str "Dave the rave has just received a large shipment..."
    faSquare
]

let view (model : Model) (dispatch : Msg -> unit) =
    let bottomNavMainPanelContent = bottomNavigationMainPaneContent model dispatch
    let leftNavHeader, leftNavContent =
        // match model.
        "Contacts", (Components.Contacts.view model.AvailableContacts (ContactsPanelMsg >> LeftNavigationMsg >> dispatch))
    Html.div [
        prop.style [ style.height (length.perc 100) ]
        prop.children [
            // topPersistentMenu model.PersistentMenu.IsActive dispatch

            Html.div [
                prop.style [
                    style.display.flex
                    style.flexDirection.column
                    style.height (length.perc 100)
                ]

                prop.children [

                    Html.div [
                        prop.style [ style.display.flex; style.flexDirection.row; style.height (length.perc 100) ]
                        prop.children [

                            Layout.LeftNavigation.view leftNavHeader (model.LayoutModel.LeftNavigationMenu.IsActive) leftNavContent

                            Html.div [
                                prop.style [
                                    style.width (length.perc 100)
                                    style.color "white"
                                    // style.overflowY.scroll
                                    style.position.relative
                                ]
                                prop.children [
                                    (topPersistentMenu model.LayoutModel.PersistentMenu.IsActive dispatch)

                                    chooseContext
                                        model.ViewContextModel
                                        dispatch
                                        (topPersistentMenu model.LayoutModel.PersistentMenu.IsActive dispatch)
                                        model.LayoutModel


                                    Elements.LeftNavigation.CollapseButton.view
                                        (fun _ -> dispatch ((LayoutMsg Layout.Types.Msg.ToggleLeftNavigationMenu)))

                                    Elements.BottomNavigation.CollapseButton.view
                                        (fun _ -> dispatch ((LayoutMsg Layout.Types.Msg.ToggleBottomNavigationMenu)))

                                    Html.div [
                                        prop.style [
                                            // style.overflow.hidden
                                            style.top 10
                                            style.left 10
                                            style.position.absolute
                                            ]
                                        prop.children [

                                        ]

                                    ]
                                ]
                            ]
                        ]
                    ]

                    Layout.BottomNavigation.mainPanel
                        bottomNavigationTickerContent
                        bottomNavMainPanelContent
                        model.LayoutModel.BottomNavigationMenu.IsActive
                        (fun _ -> dispatch (LayoutMsg Layout.Types.Msg.ToggleBottomNavigationMenu))

                ]
            ]

        ]
    ]
