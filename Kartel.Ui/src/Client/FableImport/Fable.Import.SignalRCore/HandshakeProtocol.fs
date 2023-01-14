namespace Fable.Import.SignalRCore
// ts2fable 0.6.1
module rec HandshakeProtocol =

    open System
    open Fable.Core

    type [<AllowNullLiteral>] HandshakeRequestMessage =
        abstract protocol: string
        abstract version: float

    type [<AllowNullLiteral>] HandshakeResponseMessage =
        abstract error: string

    type [<AllowNullLiteral>] HandshakeProtocol =
        abstract writeHandshakeRequest: handshakeRequest: HandshakeRequestMessage -> string
        abstract parseHandshakeResponse: data: obj option -> obj option * HandshakeResponseMessage

    type [<AllowNullLiteral>] HandshakeProtocolStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> HandshakeProtocol
