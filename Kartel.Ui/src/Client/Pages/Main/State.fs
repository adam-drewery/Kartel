module Pages.Main.State

open Elmish
open Fable.React
open Fable.React.Props
open Feliz
open Theme.Color
open Theme.Font
open Shared
open Browser.Types
// open Layout.Types
open Types

let initContacts : Contact list = [
    { Name = "Dave D"; Location = (51.05, -0.09); ImgUrl = "assets/gangster1.png" }
    { Name = "Bob C"; Location = (51.25, -0.09); ImgUrl = "assets/gangster2.png" }
    { Name = "Lisa K"; Location = (51.06, -0.19); ImgUrl = "assets/gangster3.png" }
    { Name = "Nick C"; Location = (51.09, -0.39); ImgUrl = "assets/gangster2.png" }
    { Name = "Jon F"; Location = (51.15, -0.09); ImgUrl = "assets/gangster1.png" }
]

let init () : Model * Cmd<Msg> =
    let initGameHubModel, initGameHubCmd = Components.GameHub.init()
    let initViewContextModel, initViewContextCmd = Pages.Contacts.View.init initContacts
    let initLeftNavigationModel, initLeftNavigationCmd = Components.Contacts.init initContacts
    let initialModel = {
        CurrentViewContext = ContactsViewContext
        ViewContextModel = Contacts initViewContextModel
        LayoutModel = Layout.State.init()
        GameHub = initGameHubModel
        SelectedContact = None
        AvailableContacts = initContacts
        LeftNavigationModel = ContactsPanel initLeftNavigationModel
    }
    initialModel, Cmd.batch [
        Cmd.map (ContactsViewContextMsg >> ViewContextMsg) initViewContextCmd
        Cmd.map GameHubMsg initGameHubCmd
        Cmd.map (ContactsPanelMsg >> LeftNavigationMsg) initLeftNavigationCmd
    ]

let update (msg : Msg) (model : Model) : Model * Cmd<Msg> =
    match model, msg with
    | _, ViewContextMsg viewContextMsg ->

        match model.ViewContextModel, viewContextMsg with
        | Contacts subModel, ContactsViewContextMsg subMsg ->
            let nextSubModel, nextSubCmd, externalMsg = Pages.Contacts.View.update subMsg (subModel)
            match externalMsg with
            | Pages.Contacts.View.LayoutMsg msg ->
                model, Cmd.ofMsg (LayoutMsg msg)
            | Pages.Contacts.View.SetSelectedContact contact ->
                let nextModel = { model with SelectedContact = Some contact }
                nextModel, Cmd.map (ContactsViewContextMsg >> ViewContextMsg) nextSubCmd
            | _ ->
                let nextModel = { model with ViewContextModel = Contacts nextSubModel }
                nextModel, Cmd.map (ContactsViewContextMsg >> ViewContextMsg) nextSubCmd

        | Jobs subModel, JobsViewContextMsg subMsg ->
            let nextSubModel, nextSubCmd, externalMsg = Pages.Jobs.View.update subMsg (subModel)
            match externalMsg with
            | Pages.Jobs.View.LayoutMsg msg ->
                model, Cmd.ofMsg (LayoutMsg msg)
            | _ ->
                let nextModel = { model with ViewContextModel = Jobs nextSubModel }
                nextModel, Cmd.map (JobsViewContextMsg >> ViewContextMsg) nextSubCmd

        | Globe subModel, GlobeViewContextMsg subMsg ->
            let nextSubModel, nextSubCmd = Pages.Globe.View.update subMsg (subModel)
            let nextModel = { model with ViewContextModel = Globe nextSubModel }
            nextModel, Cmd.map (GlobeViewContextMsg >> ViewContextMsg) nextSubCmd

        | _, GlobeViewContextMsg subMsg -> model, Cmd.none
        | _, ContactsViewContextMsg subMsg -> model, Cmd.none
        | _, JobsViewContextMsg subMsg -> model, Cmd.none

    | _, SwitchViewContext viewContext ->
        if viewContext = model.CurrentViewContext
        then model, Cmd.ofMsg (LayoutMsg Layout.Types.Msg.ClosePersistentMenu)
        else
        match viewContext with
        | ContactsViewContext ->
            let nextSubModel, nextSubCmd = Pages.Contacts.View.init model.AvailableContacts
            let nextModel = { model with ViewContextModel = Contacts nextSubModel; CurrentViewContext = ContactsViewContext } // TODO: This is bad
            nextModel, Cmd.batch [
                Cmd.ofMsg (LayoutMsg Layout.Types.Msg.ClosePersistentMenu)
                Cmd.map (ContactsViewContextMsg >> ViewContextMsg) nextSubCmd ]

        | JobsViewContext ->
            let nextSubModel, nextSubCmd = Pages.Jobs.View.init()
            let nextModel = { model with ViewContextModel = Jobs nextSubModel; CurrentViewContext = JobsViewContext } // TODO: This is bad
            nextModel, Cmd.batch [
                Cmd.ofMsg (LayoutMsg Layout.Types.Msg.ClosePersistentMenu)
                Cmd.map (JobsViewContextMsg >> ViewContextMsg) nextSubCmd ]

        | GlobeViewContext ->
            let nextSubModel, nextSubCmd = Pages.Globe.View.init()
            let nextModel = { model with ViewContextModel = Globe nextSubModel; CurrentViewContext = GlobeViewContext } // TODO: This is bad
            nextModel, Cmd.batch [
                Cmd.ofMsg (LayoutMsg Layout.Types.Msg.ClosePersistentMenu)
                Cmd.map (GlobeViewContextMsg >> ViewContextMsg) nextSubCmd ]

    | _, LayoutMsg layoutMsg ->
        let nextLayoutModel = Layout.State.update layoutMsg model.LayoutModel
        let nextModel = { model with LayoutModel = nextLayoutModel }
        nextModel, Cmd.none

    | _, GameHubMsg subMsg ->
        let nextSubModel, nextSubCmd = Components.GameHub.update subMsg model.GameHub
        let nextModel = { model with GameHub = nextSubModel }
        nextModel, Cmd.map GameHubMsg nextSubCmd

    | model, LeftNavigationMsg leftNavMsg ->
        match model.LeftNavigationModel, leftNavMsg with
        | ContactsPanel subModel, ContactsPanelMsg subMsg ->
            let nextSubModel, nextSubCmd, externalMsg = Components.Contacts.update subMsg (subModel)
            match externalMsg with
            | Components.Contacts.SelectContact contact ->
                let nextModel = { model with SelectedContact = Some contact }
                nextModel, (Cmd.map (ContactsPanelMsg >> LeftNavigationMsg) nextSubCmd)
            | _ ->
                let nextModel = { model with LeftNavigationModel = ContactsPanel nextSubModel }
                nextModel, (Cmd.map (ContactsPanelMsg >> LeftNavigationMsg) nextSubCmd)
        | _ -> model, Cmd.none

    | _, Noop -> model, Cmd.none

