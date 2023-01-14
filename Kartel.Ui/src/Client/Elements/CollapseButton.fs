module Elements.CollapseButton

open Fable
open Fable.Core
open Feliz
open Theme.Color

type CollapseButtonPosition =
| TopLeft
| BottomLeft

let view (iconClassName: string) (position: CollapseButtonPosition) onClick =
    let circleDimension = length.px 35
    Html.div [
        prop.className "hover-opacity-up"
        prop.id "woah"
        prop.onClick onClick
        prop.style [
            style.height circleDimension
            style.width circleDimension
            style.borderRadius circleDimension
            style.backgroundColor Color.custom2
            style.display.flex
            style.alignItems.center
            style.color Color.custom6
            style.custom("justifyContent","center")
            style.position.absolute
            (if position = TopLeft then style.top 10 else style.bottom 10)
            style.left 10
            style.zIndex 1000
        ]
        prop.children [
            Html.i [
                prop.style [ style.fontSize (length.rem 1.0) ]
                prop.className iconClassName
            ]
        ]
    ]