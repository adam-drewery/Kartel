module Components.GameHub

open Elmish
open Fable.Core
open Fable.Core.JS
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props
open Fable.Import
open System
module Hc = SignalRCore.HubConnection
module HcBuilder = SignalRCore.HubConnectionBuilder

let hashOn f x =  hash (f x) // TODO: move to 'helper'

// // Might be useful for managing set of connections, want something more idiomatic
// [<CustomEquality; CustomComparison>]
// type Connection =
//     {
//         Key: string
//         HubConnection: Hc.HubConnection
//     }

//     static member GetKey (conn: Connection) = conn.Key
//     override x.Equals y = Connection.GetKey x = Connection.GetKey (y :?> Connection)
//     override x.GetHashCode() = hashOn Connection.GetKey x

//     interface System.IComparable with
//         member x.CompareTo y = if Connection.GetKey x = Connection.GetKey (y :?> Connection) then 0 else 1

type Username = string

type Model = {
    Num: int
    // HubConnections: Collections.Set<Connection>
    HubConnection: Hc.HubConnection option
    Username: Username
    UsernameField: string
    MessageField: string
}

type Msg =
| Noop
| MakeConnection
| AddConnection of Hc.HubConnection
| RegisterUser
| UpdateMessageField of string
| UpdateUsernameField of Username
| SendMessageMsg
| SetUsername of Username

let init () =
    let initModel = {
        Num = 1;
        HubConnection = None
        // HubConnections = Collections.Set.empty
        Username = ""
        UsernameField = ""
        MessageField = "msg"
        }
    initModel, Cmd.none

let hubUrl = "/gamehub"
// importAll "@microsoft/signalr"
importAll "./../public/signalr.min.js"

[<Emit("new signalR.HubConnectionBuilder()")>]
let hcBuilder2: HcBuilder.HubConnectionBuilder = jsNative

let createHubConnection url : Hc.HubConnection =
    hcBuilder2
        .withUrl(url)
        .build()

let startSignalR2() =
    let connection = createHubConnection hubUrl

    connection.on(
        "ReceiveMessage",
        (fun user message ->
            Browser.Dom.console.log("Connection message received for from " + message)
            let encodedMsg = user + "wot" + message
            let newDiv = Browser.Dom.document.createElement("li")
            newDiv.textContent <- encodedMsg
            Browser.Dom.document.getElementById("messagesList").appendChild(newDiv) |> ignore
        )
    )

    let start =
        promise {
            try
                return! connection.start()
                Browser.Dom.console.log("connected")
            with | err ->
                Browser.Dom.console.log(err)
                setTimeout (fun () -> ()) 5000 |> ignore
                return ()
        }

    start, connection

    // let credentialsFalse = createObj [ "withCredentials" ==> false ]

    // connection?onclose(fun () ->
    //     async {
    //         return! start?(credentialsFalse)
    //     }
    // )

    // this is here to show an alternative to start, including the catch
    // connection?start(credentialsFalse)?catch(
    //     fun (err: obj) ->
    //         Browser.Dom.console.error(string err);
    // )

let Register (connection: Hc.HubConnection) username : Promise<unit> =
    promise {
        Browser.Dom.console.log("Connection register user " + username)
        // let connection = connections[key]
        // if connection.IsNone
        // then Browser.Dom.console.log("Connection not yet made")
        // else
        //     Browser.Dom.console.log("Connection located")
        //     // send message (async)
        return! connection.invoke("Register", unbox username)
    }

let SendMessage (connection: Hc.HubConnection) username message =
    promise {
        Browser.Dom.console.log("Connection send request")
        Browser.Dom.console.log("Connection located")
        // send message (async)
        return! connection?invoke("SendMessage", unbox [username; message])
    }

let update (msg: Msg) (model: Model) : Model * Cmd<Msg> =
    match model, msg with
    | _, RegisterUser ->
        let connection = model.HubConnection
        match connection with
        | None ->
            model, Cmd.none
        | Some conn ->
            let username = model.UsernameField
            let registerSignalRPromise = Register conn username
            model,
            Cmd.OfPromise.either
                (fun _ -> registerSignalRPromise)
                ()
                (fun _ -> Browser.Dom.console.log("started successfully"); SetUsername username)
                (fun _ -> Browser.Dom.console.log("failed"); Noop)

    | _, AddConnection conn ->
        let nextModel = { model with HubConnection = Some conn }
        nextModel, Cmd.none

    | _, SetUsername username ->
        let nextModel = { model with Username = username }
        nextModel, Cmd.none

    | model, SendMessageMsg when model.HubConnection.IsSome ->
        model,
        Cmd.OfPromise.perform
            (fun _ -> SendMessage model.HubConnection.Value model.Username model.MessageField)
            ()
            (fun _ -> Noop)

    | model, SendMessageMsg ->
        model, Cmd.none

    | _, UpdateUsernameField username ->
        let nextModel = { model with UsernameField = username }
        nextModel, Cmd.none

    | _, UpdateMessageField message ->
        let nextModel = { model with MessageField = message }
        nextModel, Cmd.none

    | _, MakeConnection ->
        let startSignalRPromise, conn = startSignalR2()
        model,
        Cmd.OfPromise.either
            (fun _ -> startSignalRPromise)
            ()
            (fun _ -> Browser.Dom.console.log("started successfully"); AddConnection conn)
            (fun _ -> Browser.Dom.console.log("failed"); Noop)

    | _, Noop ->
        model, Cmd.none

[<Emit("startSignalR()")>]
let startSignalR(): unit = jsNative

let view (model: Model) (dispatch: Msg -> unit) =
    div [] [
        button [ OnClick (fun _ -> dispatch MakeConnection) ] [ str "connect" ]
        div [ Id "messagesList" ] []
        button [ OnClick (fun _ -> startSignalR()) ] [ str "js msg" ]
        button [ OnClick (fun _ -> dispatch (RegisterUser)) ] [ str "register" ]
        input [
            Props.Type "text"
            OnChange (fun ev -> dispatch (UpdateUsernameField !!ev.target?value))
        ]
        input [
            Props.Type "text"
            Props.DefaultValue model.MessageField
            OnChange (fun ev -> dispatch (UpdateMessageField !!ev.target?value))
        ]
        button [ OnClick (fun _ -> dispatch (SendMessageMsg)) ] [ str "send message" ]
    ]
