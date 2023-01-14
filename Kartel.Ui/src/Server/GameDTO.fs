namespace Kartel.Ui

open Kartel
open Kartel.Environment.Topography
open Kartel.Service.Client
open System.Threading.Tasks
open Shared
open FSharp.Control.Tasks.V2
open System.Threading

open System
open System.Linq
open System.Threading
open System.Threading.Tasks
open System.Timers
open Kartel.Environment.Topography
open Kartel.Extensions

module GameDTO =

    let UpdateSemaphore : SemaphoreSlim = new SemaphoreSlim(1, 1)

    // public static StatusBar StatusBar { get; } = new StatusBar();

    let GameInstance : Game =
        Game(
            new PropertyMarketClient()
        )
            // new EstateService(),
            // new GoogleGeocodingService(),
            // new GoogleDirectionService(),
            // new SocialEventsService()
        // )

    let Player =
        task {
            let location = GameInstance
                    // .Locations.OfType<City>()
                    // .Single(fun city -> city.Name = "Leeds" )
            //  .OfType<City>().Single(fun c -> c.Name = "Leeds")
            return! Player.New(location)
        } |> Async.AwaitTask |> Async.RunSynchronously

    let mutable whatever = 0

    let Game =

        GameInstance |> ignore
        // GameInstance.Exception.Add(fun exn -> printfn "%A" exn)
        GameInstance.Characters.Add(Player)
        GameInstance.Clock.MinimumTickSpeed <- 50.
        GameInstance.Clock.SpeedFactor <- float32 1
        // GameInstance.Clock.Tick.Add(fun eventArgs -> StatusBar.Render(0) )
        if whatever = 0 then GameInstance.Clock.Start() else () |> ignore
        whatever <- 1
        GameInstance,Player

        // new MainMenu(null).Render();

    let getInitGame() : Task<GameShared> = task {
        let game,player = Game
        // let gameClock = game.Clock.Time.ToLongTimeString()
        // let playerName = "thing"
        let playerName = player.Name
        // let res = sprintf "%s and name is %s" gameClock playerName
        return { Value = playerName }
    }

    let getCurrentTime() : Task<GameShared> = task {
        return { Value = "eeeeerm" }
    }
    // GameInstance.Clock.Time.ToLongTimeString()