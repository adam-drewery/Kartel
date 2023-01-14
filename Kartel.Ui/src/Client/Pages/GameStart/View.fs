module Pages.GameStart.View

open Fable
open Fable.Core
open Fable.React
open Feliz
open Elmish
open Types
open State

let passwordField (password: string) =
    Html.input [
        prop.style [
            style.backgroundColor "#343434"
        ]
        prop.value password
    ]

let usernameField (username: string) =
    Html.input [
        prop.style [
            style.backgroundColor "#343434"
        ]
        prop.value username
    ]

let view (model: Model) (dispatch: Msg -> unit) =
    Html.div [
        prop.children [
            str "stuff"
            usernameField ""
            passwordField ""
        ]
    ]
