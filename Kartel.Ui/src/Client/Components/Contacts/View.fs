module Components.Contacts

open Elmish
open Fable.React
open Feliz
open Theme.Color
open Theme.Font
open Shared

type Model = {
    AvailableContacts: Contact list
    SelectedContact: Contact option
}

type ExternalMsg =
| Noop
| SelectContact of Contact

type Msg =
| Noop
| ExternalMsg of ExternalMsg
// | SelectContact of Contact

// let initContacts = [
//     { Name = "Dave D" }
//     { Name = "Bob C" }
//     { Name = "Fred D" }
//     { Name = "Nick C" }
//     { Name = "Jon F" }
// ]

let initModelPartial = {
    AvailableContacts = []
    SelectedContact = None
}

let init (initContacts: Contact list) : Model * Cmd<Msg> =
    let initModel = { initModelPartial with AvailableContacts = initContacts }
    initModel, Cmd.none

let update (msg : Msg) (model : Model) : Model * Cmd<Msg> * ExternalMsg =
    match model, msg with
    | _, ExternalMsg externalMsg ->
        model, Cmd.none, externalMsg
    // | _, SelectContact contact ->
    //     let nextModel = { currentModel with SelectedContact = Some contact }
    //     nextModel, Cmd.none, ExternalMsg.Noop
    | _, Noop ->
        model, Cmd.none, ExternalMsg.Noop

let view (availableContacts: Contact list) (dispatch: Msg -> unit) =
    availableContacts
    |> List.mapi (
        fun i contact ->
            Elements.Contacts.contactsSection
                (sprintf "left-nav-contact-%i" i)
                contact
                (fun _ -> dispatch (ExternalMsg (SelectContact contact)))
    )