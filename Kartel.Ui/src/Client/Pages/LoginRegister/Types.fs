module Pages.LoginRegister.Types

type Username = string
type Password = string

type Model = {
    UsernameField: Username
    PasswordField: Password
}

type ExternalMsg =
| Noop
| NavigateToLoggedInPage of Username
| NavigateToGameStartPage

type Msg =
| AttemptLogin
| UpdateUsernameField of Username
| UpdatePasswordField of Password
| LoginSuccessful of Username
| LoginFailed of exn
