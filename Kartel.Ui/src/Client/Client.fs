module Client

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props
open Fable.Core.JsInterop
open Shared
open Browser.Types
open System

type Player = {
    Name: string
}

type Page =
| MainPage of Pages.Main.Types.Model
| LoginRegisterPage of Pages.LoginRegister.Types.Model

type Model = {
    Page: Page
    LoggedInPlayer: Player option
}

type MsgResponse = {
    Ok: bool
}

type ExternalMsg =
| LoginRegisterPageExternalMsg of Pages.LoginRegister.Types.ExternalMsg

type Msg =
| Noop
| MainPageMsg of Pages.Main.Types.Msg
| LoginRegisterPageMsg of Pages.LoginRegister.Types.Msg

let init () : Model * Cmd<Msg> =
    // let initMainPageModel, initMainPageCmd = Pages.Main.init()
    let initLoginRegisterPageModel, initLoginRegisterPageCmd, externalMsg = Pages.LoginRegister.State.init()
    let initialModel = {
        LoggedInPlayer = None
        Page = LoginRegisterPage initLoginRegisterPageModel
    }

    initialModel,
    Cmd.batch [
        // Cmd.map (LoginRegisterPageExternalMsg externalMsg)
        Cmd.map LoginRegisterPageMsg initLoginRegisterPageCmd
    ]


let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match currentModel.LoggedInPlayer.IsNone with
    | true ->
        // INSECURE AREA
        match currentModel.Page, msg with
        | LoginRegisterPage subModel, LoginRegisterPageMsg loginRegisterPageMsg ->
            let nextSubModel, nextSubCmd, externalMessage = Pages.LoginRegister.State.update loginRegisterPageMsg subModel
            match externalMessage with
            | Pages.LoginRegister.Types.ExternalMsg.NavigateToLoggedInPage username ->
                let nextSubModel, nextSubCmd = Pages.Main.State.init()
                let nextModel = { currentModel with Page = MainPage nextSubModel; LoggedInPlayer = Some { Name = username } }
                nextModel, Cmd.map MainPageMsg nextSubCmd
            | _ ->
                let nextModel = { currentModel with Page = LoginRegisterPage nextSubModel }
                nextModel, Cmd.map LoginRegisterPageMsg nextSubCmd

        | _ ->
            let nextSubModel, nextSubCmd, externalMsg = Pages.LoginRegister.State.init()
            let nextModel = { currentModel with Page = LoginRegisterPage nextSubModel }
            nextModel, Cmd.map LoginRegisterPageMsg nextSubCmd

    | false ->
        // SECURE AREA
        match currentModel.Page, msg with
        | MainPage subModel, MainPageMsg subMsg ->
            // match subModel, subMsg with
            // | _, MainPageMsg subMsg ->
                let nextSubModel, nextSubCmd = Pages.Main.State.update subMsg subModel
                let nextModel = { currentModel with Page = MainPage nextSubModel }
                nextModel, Cmd.map MainPageMsg nextSubCmd
        | _, MainPageMsg mainPageMsg ->
            Browser.Dom.console.log("this shouldn't really happen")
            currentModel, Cmd.none
        | LoginRegisterPage subModel, LoginRegisterPageMsg subMsg ->
            let nextSubModel, nextSubCmd, externalMsg = Pages.LoginRegister.State.update subMsg subModel
            match externalMsg with
            | Pages.LoginRegister.Types.ExternalMsg.NavigateToLoggedInPage username ->
                let nextSubModel, nextSubCmd = Pages.Main.State.init()
                let nextModel = { currentModel with Page = MainPage nextSubModel; LoggedInPlayer = Some { Name = username } }
                nextModel, Cmd.map MainPageMsg nextSubCmd
            | message ->
                Browser.Dom.console.log("hit wot" + (sprintf "%A" message))
                currentModel, Cmd.none
               // Browser.Dom.console.log("hit loginregisterpagemsg externalmsg")
        | _, LoginRegisterPageMsg subMsg ->
            Browser.Dom.console.log("hit loginregisterpagemsg externalmsg")
            currentModel, Cmd.none
        | _, Noop ->
            currentModel, Cmd.none

let button txt onClick =
    button [ OnClick onClick ] [ str txt ]

let view (model : Model) (dispatch : Msg -> unit) =
    match model.Page with
    | MainPage mainPageModel ->
        Pages.Main.View.view mainPageModel (MainPageMsg >> dispatch)
    | LoginRegisterPage loginRegisterPageModel ->
        Pages.LoginRegister.View.view loginRegisterPageModel (LoginRegisterPageMsg >> dispatch)

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactBatched "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run