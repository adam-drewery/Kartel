module Layout.State

open Types

let initModel = {
    PersistentMenu = { IsActive = true }
    LeftNavigationMenu = { IsActive = true }
    BottomNavigationMenu = { IsActive = true }
}

let init() =
    initModel

let update (msg: Msg) (model: Model) =

    match model, msg with
    | model, Msg.TogglePersistentMenu ->
        let nextModel = { model with PersistentMenu = { IsActive = not model.PersistentMenu.IsActive} }
        nextModel

    | model, Msg.ClosePersistentMenu ->
        let nextModel = { model with PersistentMenu = { IsActive = false } }
        nextModel

    | model, Msg.ToggleLeftNavigationMenu ->
        let nextModel = { model with LeftNavigationMenu = { IsActive = not model.LeftNavigationMenu.IsActive} }
        nextModel

    | model, Msg.CloseLeftNavigationMenu ->
        let nextModel = { model with LeftNavigationMenu = { IsActive = false } }
        nextModel

    | model, Msg.ToggleBottomNavigationMenu ->
        let nextModel = { model with BottomNavigationMenu = { IsActive = not model.BottomNavigationMenu.IsActive} }
        nextModel

    | model, Msg.CloseBottomNavigationMenu ->
        let nextModel = { model with BottomNavigationMenu = { IsActive = false } }
        nextModel