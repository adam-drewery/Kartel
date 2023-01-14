module Layout.Types

type PersistentMenu = {
    IsActive: bool
}

type LeftNavigationMenu = {
    IsActive: bool
}

type BottomNavigationMenu = {
    IsActive: bool
}

type Model = {
    PersistentMenu: PersistentMenu
    LeftNavigationMenu: LeftNavigationMenu
    BottomNavigationMenu: BottomNavigationMenu
}

// [<RequireQualifiedAccess>]
type Msg =
| TogglePersistentMenu
| ClosePersistentMenu
| ToggleLeftNavigationMenu
| CloseLeftNavigationMenu
| ToggleBottomNavigationMenu
| CloseBottomNavigationMenu