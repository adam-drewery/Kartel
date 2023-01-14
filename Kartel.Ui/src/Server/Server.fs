namespace Kartel.Ui

open System
open System.IO
open System.Threading.Tasks

open Microsoft.AspNetCore
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.SignalR
open Microsoft.AspNetCore.Cors.Infrastructure

open FSharp.Control.Tasks.V2
open Giraffe
open Shared
open GameDTO

open Microsoft.WindowsAzure.Storage

module Server =
    let tryGetEnv = System.Environment.GetEnvironmentVariable >> function null | "" -> None | x -> Some x

    let publicPath = tryGetEnv "public_path" |> Option.defaultValue "../Client/public" |> Path.GetFullPath
    let storageAccount = tryGetEnv "STORAGE_CONNECTIONSTRING" |> Option.defaultValue "UseDevelopmentStorage=true" |> CloudStorageAccount.Parse
    let port =
        "SERVER_PORT"
        |> tryGetEnv |> Option.map uint16 |> Option.defaultValue 8085us

    let getInitCounter() : Task<Counter> = task { return { Value = 42 } }

    let apiInit =
        fun next ctx ->
            task {
                let! counter = getInitCounter()
                return! json counter next ctx
            }

    let apiGame =
        fun next ctx ->
            task {
                // let! game = GameDTO.a()
                let! game = GameDTO.getInitGame()
                return! json game.Value next ctx
            }

    let postSendGameMessage =
        fun next (ctx: HttpContext) ->
            task {
                // let postBody = ctx.Request.Body |> readAllBytes |> Encoding.ASCII.GetString
                let! data = ctx.BindJsonAsync<SigRData>()
                // Decode.Auto.fromString<SigRData>(postBody)
                let b = ctx.RequestServices.GetService<IHubContext<Hubs.ChatHub>>()
                let! c = b.Clients.All.SendAsync ("ReceiveMessage", data.userInput, data.messageInput )
                return! next ctx
            }

    let webApp =
        choose [
            GET  >=> route "/api/init" >=> apiInit
            GET  >=> route "/api/game" >=> apiGame
            POST >=> route "/api/sendGameMessage" >=> postSendGameMessage
        ]

    // let webApp = router {


    //     // SEE HERE FOR HELP: https://dusted.codes/giraffe-110-more-routing-handlers-better-model-binding-and-brand-new-model-validation-api
    //     post "/api/sendGameMessage" (fun next ctx ->
    //         task {
    //             // let postBody = ctx.Request.Body |> readAllBytes |> Encoding.ASCII.GetString
    //             let! data = ctx.BindJsonAsync<SigRData>()
    //             // Decode.Auto.fromString<SigRData>(postBody)
    //             let b = ctx.RequestServices.GetService<IHubContext<Hubs.ChatHub>>()
    //             let! c = b.Clients.All.SendAsync ("ReceiveMessage", data.userInput, data.messageInput )
    //             return! next ctx
    //         })
    // }
    let configureSignalR (services:IServiceCollection) =
        services.AddSignalR().Services

    // let UseSignalR (config: IApplicationBuilder) =
    //     config.UseSignalR(fun routes -> routes.MapHub<Hubs.ChatHub>(PathString("/chatHub")) |> ignore)

    let configureCors (builder : CorsPolicyBuilder) =
        builder
               .WithOrigins([|"http://localhost:8080"; "http://localhost:8085"; "http://127.0.0.1:8085"; "http://0.0.0.0:8085";|])
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials()
               |> ignore

    let configureApp (app : IApplicationBuilder) =
        app.UseDefaultFiles()
            .UseCors(configureCors)
            .UseRouting()
            .UseEndpoints(
                fun ep ->
                    ep.MapHub<Hubs.ChatHub>("/chatHub") |> ignore
            )
            .UseStaticFiles()
            .UseGiraffe webApp

    let configureServices (services : IServiceCollection) =
        services.AddCors() |> ignore
        services.AddGiraffe() |> ignore
        services.AddRouting() |> ignore
        services.AddSignalR() |> ignore
        services.AddSingleton<Giraffe.Serialization.Json.IJsonSerializer>(Thoth.Json.Giraffe.ThothSerializer()) |> ignore
        tryGetEnv "APPINSIGHTS_INSTRUMENTATIONKEY" |> Option.iter (services.AddApplicationInsightsTelemetry >> ignore)

    WebHostBuilder()
        .UseKestrel()
        .UseWebRoot(publicPath)
        .UseContentRoot(publicPath)
        .ConfigureServices(configureServices)
        .Configure(Action<IApplicationBuilder> configureApp)
        .UseUrls("http://0.0.0.0:" + port.ToString() + "/")
        .Build()
        .Run()
