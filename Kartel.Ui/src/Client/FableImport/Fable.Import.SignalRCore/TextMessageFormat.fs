namespace Fable.Import.SignalRCore
// ts2fable 0.6.1
module rec TextMessageFormat =

    open System
    open Fable.Core
    open Fable.Core.JS


    type [<AllowNullLiteral>] TextMessageFormat =
        abstract RecordSeparatorCode: obj with get, set
        abstract RecordSeparator: obj with get, set

    type [<AllowNullLiteral>] TextMessageFormatStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> TextMessageFormat
        abstract write: output: string -> string
        abstract parse: input: string -> ResizeArray<string>
