namespace Fable.Import.SignalRCore
// ts2fable 0.6.1
module rec ITransport =

    open System
    open Fable.Core
    open Fable.Core.JS
    open Fable.Import.SignalRCore.Error

    type [<RequireQualifiedAccess>] HttpTransportType =
        | None = 0
        | WebSockets = 1
        | ServerSentEvents = 2
        | LongPolling = 4

    type [<RequireQualifiedAccess>] TransferFormat =
        | Text = 1
        | Binary = 2

    /// An abstraction over the behavior of transports. This is designed to support the framework and not intended for use by applications.
    type [<AllowNullLiteral>] ITransport =
        abstract connect: url: string * transferFormat: TransferFormat -> Promise<unit>
        abstract send: data: obj option -> Promise<unit>
        abstract stop: unit -> Promise<unit>
        abstract onreceive: (U2<string, ArrayBuffer> -> unit) with get, set
        abstract onclose: (Error -> unit) with get, set
