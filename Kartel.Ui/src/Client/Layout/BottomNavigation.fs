module Layout.BottomNavigation

open Fable.React
open Feliz
open Theme.Color
open Theme.Font

let panelHeight = 300
let tickerHeight = 35
let mainPaneHeight = panelHeight - tickerHeight

let ticker (content: ReactElement list) (onClick: Browser.Types.MouseEvent -> unit) =
    Html.div [
        prop.style [
            style.backgroundColor Color.custom2
            style.width (length.perc 100)
            style.height (length.px tickerHeight)
            style.paddingLeft (length.em 2)
            style.display.flex
            style.alignItems.center
            style.color Color.custom6
            style.fontFamily Font.First.regular
        ]
        prop.children
            (content@[
                Html.div [
                    prop.onClick onClick
                    prop.style [ style.color "white" ]
                    prop.children [
                        str "toggle"
                    ]
                ]
            ] |> List.ofSeq)
    ]

let mainPane (content: ReactElement list) =
    Html.div [
        prop.style [
            style.display.flex
            style.flexDirection.row
            style.height (length.px mainPaneHeight)
            style.width (length.perc 100)
        ]
        prop.children content
    ]

let mainPanel
        (tickerContent: ReactElement list)
        (mainPaneContent: ReactElement list)
        (isActive: bool)
        (toggleBottomNav: Browser.Types.MouseEvent -> unit)
        =
    Html.div [
        prop.className "bottom-navigation-panel"
        prop.style [
            style.backgroundColor Color.custom3
            style.height (length.px (if isActive then panelHeight else 0))
            style.width (length.perc 100)
            style.display.flex
            style.flexDirection.column
            style.alignItems.flexStart
            style.zIndex 1000 // Note: left-navigation is 999
            style.boxShadow (0, -4, 8, "rgba(0,0,0,0.2)")
        ]
        prop.children [
            ticker tickerContent (fun _ -> ())
            mainPane mainPaneContent
        ]
    ]