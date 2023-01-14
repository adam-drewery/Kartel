module Pages.LoginRegister.View

open Fable
open Fable.Core
open Fable.Core.JsInterop
open Fable.React
open Feliz
open Elmish
open Types
open State

let usernameField (username: string) (onChange: Browser.Types.Event -> unit) =
    Html.input [
        prop.onChange onChange
        prop.withType "text"
        prop.style [
            style.backgroundColor "#343434"
            style.color "white"
            style.paddingTop 10
            style.paddingBottom 10
            style.paddingLeft 10
            style.paddingRight 10
            style.borderRadius 2
            style.fontSize (length.em 1.2)
        ]
        prop.defaultValue username
    ]

let passwordField (password: string) (onChange: Browser.Types.Event -> unit) =
    Html.input [
        prop.onChange onChange
        prop.style [
            style.backgroundColor "#343434"
            style.color "white"
            style.paddingTop 10
            style.paddingBottom 10
            style.paddingLeft 10
            style.paddingRight 10
            style.borderRadius 2
            style.fontSize (length.em 1.2)
        ]
        prop.defaultValue password
    ]


let loginButton onClick =
    Html.button [
        prop.onClick onClick
        prop.style [
            style.fontSize (length.em 1.2)
        ]
        prop.children [ str "Log in" ]
    ]

let view (model: Model) (dispatch: Msg -> unit) =
    Html.div [
        prop.style [
            style.height (length.perc 100)
            style.width (length.perc 100)
            style.display.flex
            style.alignItems.center
            style.custom("justify-content","center")
        ]
        prop.children [
            Html.div [
                prop.style [
                    style.height (length.px 200)
                    style.width (length.px 240)
                    style.display.flex
                    style.flexDirection.column
                    style.custom("justify-content","space-between")
                ]
                prop.children [
                    str "stuff"
                    usernameField "uname" (fun ev -> dispatch (UpdateUsernameField !!ev.target?value))
                    passwordField "pword" (fun ev -> dispatch (UpdatePasswordField !!ev.target?value))
                    loginButton (fun _ -> dispatch AttemptLogin)
                ]
            ]
        ]
    ]
