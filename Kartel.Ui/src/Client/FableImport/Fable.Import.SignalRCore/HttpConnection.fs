namespace Fable.Import.SignalRCore
// ts2fable 0.6.1
module rec HttpConnection =

    open System
    open Fable.Core
    open Fable.Core.JS
    open ITransport
    open IConnection
    open ILogger
    open HttpClient
    open IHttpConnectionOptions
    open Error

    type ConnectionState =
        obj

    type [<AllowNullLiteral>] INegotiateResponse =
        abstract connectionId: string option with get, set
        abstract availableTransports: ResizeArray<IAvailableTransport> option with get, set
        abstract url: string option with get, set
        abstract accessToken: string option with get, set

    type [<AllowNullLiteral>] IAvailableTransport =
        abstract transport: obj with get, set
        abstract transferFormats: ResizeArray<obj> with get, set

    type [<AllowNullLiteral>] HttpConnection =
        inherit IConnection
        abstract connectionState: ConnectionState with get, set
        abstract baseUrl: string with get, set
        abstract httpClient: HttpClient
        abstract logger: ILogger
        abstract options: IHttpConnectionOptions
        abstract transport: ITransport with get, set
        abstract startPromise: Promise<unit> with get, set
        abstract stopError: Error option with get, set
        abstract accessTokenFactory: (unit -> U2<string, Promise<string>>) option with get, set
        abstract features: obj option
        abstract onreceive: (U2<string, ArrayBuffer> -> unit) with get, set
        abstract onclose: (Error -> unit) with get, set
        abstract start: unit -> Promise<unit>
        abstract start: transferFormat: TransferFormat -> Promise<unit>
        abstract start: ?transferFormat: TransferFormat -> Promise<unit>
        abstract send: data: U2<string, ArrayBuffer> -> Promise<unit>
        abstract stop: ?error: Error -> Promise<unit>
        abstract startInternal: transferFormat: TransferFormat -> Promise<unit>
        abstract getNegotiationResponse: url: string -> Promise<INegotiateResponse>
        abstract createConnectUrl: url: string * connectionId: string -> unit
        abstract createTransport: url: string * requestedTransport: U2<HttpTransportType, ITransport> * negotiateResponse: INegotiateResponse * requestedTransferFormat: TransferFormat -> Promise<unit>
        abstract constructTransport: transport: HttpTransportType -> unit
        abstract resolveTransport: endpoint: IAvailableTransport * requestedTransport: HttpTransportType * requestedTransferFormat: TransferFormat -> HttpTransportType option
        abstract isITransport: transport: obj option -> bool
        abstract changeState: from: ConnectionState * ``to``: ConnectionState -> bool
        abstract stopConnection: ?error: Error -> Promise<unit>
        abstract resolveUrl: url: string -> string
        abstract resolveNegotiateUrl: url: string -> string

    module HttpConnectionStatic =
        [<Emit "new $0($1...)">]
        let Create: url: string * options: IHttpConnectionOptions -> HttpConnection = jsNative
