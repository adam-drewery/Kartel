module Components.PlayerAttributes

open Fable
open Fable.Core
open Fable.React
open Feliz
open Theme.Color

let fieldValueSection fieldValue =
    Html.div [
        prop.style [
            style.color Color.custom6
            style.backgroundColor "rgba(200,200,200,0.1)"
            style.paddingRight (length.px 30)
            style.paddingLeft (length.px 10)
            style.paddingBottom (length.px 10)
            style.paddingTop (length.px 10)
        ]
        prop.children [ str fieldValue ]
    ]

let labelSection labelText =
    Html.div [
        prop.style [
            style.color Color.custom5
            style.backgroundColor "rgba(200,200,200,0.2)"
            style.paddingRight (length.px 30)
            style.paddingLeft (length.px 10)
            style.paddingBottom (length.px 10)
            style.paddingTop (length.px 10)
        ]
        prop.children [ str labelText ]
    ]

let field label fieldValue =
    Html.div [
        prop.style [
            style.display.grid
            style.custom("grid-template-columns", "repeat(2, 1fr)")
            style.custom("grid-template-rows", "auto")
            // style.custom("row-gap", "2px")
            style.custom("grid-column-gap", "2px")
            style.paddingBottom (length.px 2)
        ]
        prop.children [
            labelSection label
            fieldValueSection fieldValue
        ]
    ]

let view () =
    Html.div [
        prop.style [
            // style.display.flex
            // style.flexDirection.row
        ]
        prop.children [
            field "Health" "80%"
            field "Cash" "£40"
            field "Bank" "£1,267"
        ]

    ]