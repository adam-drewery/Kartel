module Layout.LeftNavigation

open Fable.React
open Feliz
open Theme.Color
open Theme.Font

let burgerButton =
    let circleDimension = length.px 35
    Html.div [
        prop.style [
            style.height circleDimension
            style.minWidth circleDimension
            style.borderRadius circleDimension
            style.backgroundColor Color.custom2
            style.display.flex
            style.alignItems.center
            style.color Color.custom6
            style.custom("justifyContent","center")
        ]
        prop.children [
            Html.i [
                prop.style [ style.fontSize (length.rem 1.0) ]
                prop.className "fa fa-bars"
            ]
        ]
    ]

let leftNavigationHeader (headerText: string) =
    let paddingVert = 10
    let paddingHorizontal = 10
    Html.div [
        prop.className "left-navigation-header"
        prop.style [
            style.backgroundColor Color.Light.custom3
            style.paddingTop paddingVert
            style.paddingBottom paddingVert
            style.paddingRight paddingHorizontal
            style.paddingLeft paddingHorizontal
            style.marginBottom 2
            style.fontFamily Font.First.semiBold
            style.fontSize (length.em 2.2)
            style.display.flex
            style.alignItems.center
            style.custom ("justifyContent","space-between")
            style.color Color.Dark.custom2
        ]
        prop.children [
            // burgerButton
            Html.div [
                prop.style [ style.width (length.perc 100); style.display.flex; style.custom("justifyContent","flex-start") ]
                prop.children [ str (sprintf "%s" headerText) ]
            ]
        ]
    ]


let menuContextButton backgroundColor =
    let circleDimension = length.px 30
    Html.div [
        prop.className "left-menu-context-button"
        prop.style [
            style.height circleDimension
            style.minWidth circleDimension
            // style.borderRadius circleDimension
            style.borderRadius (length.px 1)
            style.backgroundColor backgroundColor
            style.display.flex
            style.alignItems.center
            style.color Color.custom2
            style.custom("justifyContent","center")
            style.boxShadow (2, 2, 4, "rgba(0,0,0,0.2)")
        ]
        prop.children [
            Html.i [
                prop.style [ style.fontSize (length.rem 1.0) ]
                prop.className "fa fa-arrow-up"
            ]
        ]
    ]

let menuContextPane =
    let paddingVert = 10
    let paddingHorizontal = 10
    Html.div [
        // prop.className "left-navigation-header"
        prop.style [
            style.backgroundColor Color.custom2
            style.paddingTop paddingVert
            style.paddingBottom paddingVert
            style.paddingRight paddingHorizontal
            style.paddingLeft paddingHorizontal
            style.marginBottom 2
            style.fontFamily Font.First.semiBold
            style.fontSize (length.em 2.2)
            style.display.flex
            style.alignItems.center
            style.custom ("justifyContent","space-between")
            style.color "#121212"
        ]
        prop.children [
            // burgerButton
            Html.div [
                prop.style [ style.width (length.perc 100); style.display.flex; style.custom("justifyContent","space-between") ]
                prop.children [
                    menuContextButton Color.Bright.custom6
                    menuContextButton Color.Bright.custom3
                    menuContextButton Color.Bright.custom5
                    menuContextButton Color.Bright.custom4
                    menuContextButton Color.Bright.custom2
                ]
            ]
        ]
    ]

let leftNavContainer (isActive: bool) (content: ReactElement) =
    Html.div [
        prop.className (if isActive then "left-nav-container" else "left-nav-container-hidden")
        prop.style [
            // style.width
        ]
        prop.children [ content ]
    ]

let view (headerText: string) (isActive: bool) (content: ReactElement list) =
    leftNavContainer isActive (
        Html.div [
            prop.id "leftNavigation"
            prop.className "left-navigation-panel"
            prop.style [
                // style.width (length.perc 18)
                style.minWidth (length.px 220)
                style.maxWidth (length.px 280)
                style.minHeight (length.perc 100)
                style.cursor "default"
            ]
            prop.children [
                leftNavigationHeader headerText
                menuContextPane
                Html.div [
                    prop.style [ style.padding (length.perc 1) ]
                    prop.children content
                ]
            ]
        ]
    )