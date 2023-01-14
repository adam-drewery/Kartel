module Pages.GameStart.State

open Elmish
open Types

let initModel = []

let init () =
    initModel, Cmd.none

let update (msg: Msg) (model: Model) =
    match model, msg with
    | _, _ -> model, Cmd.none
