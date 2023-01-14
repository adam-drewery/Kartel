namespace Kartel.Ui

module Hubs =

    open Microsoft.AspNetCore.SignalR
    open FSharp.Control.Tasks.V2.ContextInsensitive

    type ChatHub() =
      inherit Hub()

      member x.SendMessage (user: string, msg: string)=
        task {
          do! x.Clients.All.SendAsync("ReceiveMessage", user, msg)
        }

      member x.SendGameMessage (user: string, msg: string) =
        task {
          do! x.Clients.All.SendAsync("ReceiveMessage", user, msg)
        }

    //     member x.SendMessage (user: string, msg: string) =
    // task {
    //   do! x.Clients.All.SendAsync("ReceiveMessage", user, msg)
    // }