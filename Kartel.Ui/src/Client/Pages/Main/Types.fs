module Pages.Main.Types

open Elmish
open Fable.React
open Fable.React.Props
open Feliz
open Theme.Color
open Theme.Font
open Shared
open Browser.Types
// open Layout.Types

type ViewContext =
| ContactsViewContext
| JobsViewContext
| GlobeViewContext


type LeftNavigationModel =
| ContactsPanel of Components.Contacts.Model
| ExamplePanel of Components.Contacts.Model

type ViewContextModel =
| Contacts of Pages.Contacts.View.Model
| Jobs of Pages.Jobs.View.Model
| Globe of Pages.Globe.View.Model

type Model = {
    CurrentViewContext: ViewContext
    ViewContextModel: ViewContextModel
    LayoutModel: Layout.Types.Model
    GameHub: Components.GameHub.Model
    SelectedContact: Contact option
    AvailableContacts: Contact list
    LeftNavigationModel: LeftNavigationModel
}

type ViewContextMsg =
| ContactsViewContextMsg of Pages.Contacts.View.Msg
| JobsViewContextMsg of Pages.Jobs.View.Msg
| GlobeViewContextMsg of Pages.Globe.View.Msg

type LeftNavigationMessage =
| ContactsPanelMsg of Components.Contacts.Msg

type Msg =
| Noop
| ViewContextMsg of ViewContextMsg
| SwitchViewContext of ViewContext
| LayoutMsg of Layout.Types.Msg
| GameHubMsg of Components.GameHub.Msg
| LeftNavigationMsg of LeftNavigationMessage

type TopPersistentMenuItemData<'a> = {
    Label: string
    OnClick: 'a -> unit
}
