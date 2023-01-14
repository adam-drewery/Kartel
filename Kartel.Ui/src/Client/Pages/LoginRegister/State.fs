module Pages.LoginRegister.State

open Elmish
open Types
open Fable.Core

let initModel : Model = {
    UsernameField = "uname"
    PasswordField = "pword"
}

let init () =
    // initModel, Cmd.none, Noop
    initModel, Cmd.ofMsg AttemptLogin, Noop // Turnds off auth

let attemptLogin (username: string) (password: string) () =
    promise {
        return username
    }

let update (msg: Msg) (model: Model) =
    // match externalMsg with
    // | NavigateToLoggedInPage ->
    //     model, Cmd.none
    // | _ ->
        match model, msg with
        | _, LoginSuccessful username ->
            model, Cmd.none, NavigateToLoggedInPage username
        | _, LoginFailed error ->
            Browser.Dom.console.log("login failed with error: " + error.Message)
            model, Cmd.none, Noop
        | _, AttemptLogin ->
            model,
            Cmd.OfPromise.either (attemptLogin model.UsernameField model.PasswordField) () LoginSuccessful LoginFailed,
            Noop
        | _, UpdateUsernameField username ->
            { model with UsernameField = username }, Cmd.none, Noop
        | _, UpdatePasswordField password ->
            { model with PasswordField = password }, Cmd.none, Noop

    // | _, _ -> model, Cmd.none
