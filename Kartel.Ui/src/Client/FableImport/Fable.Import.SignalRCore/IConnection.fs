namespace Fable.Import.SignalRCore
// ts2fable 0.6.1
module rec IConnection =

    open System
    open Fable.Core
    open Fable.Core.JS
    open ITransport
    open Error

    type [<AllowNullLiteral>] IConnection =
        abstract features: obj option
        abstract start: transferFormat: TransferFormat -> Promise<unit>
        abstract send: data: U2<string, ArrayBuffer> -> Promise<unit>
        abstract stop: ?error: Error -> Promise<unit>
        abstract onreceive: (U2<string, ArrayBuffer> -> unit) with get, set
        abstract onclose: (Error -> unit) with get, set
