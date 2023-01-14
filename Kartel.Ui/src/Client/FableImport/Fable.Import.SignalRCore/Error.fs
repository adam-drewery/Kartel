namespace Fable.Import.SignalRCore

// ts2fable 0.6.1
module Error =

    open System

    type [<AllowNullLiteral>] Error =
        abstract name: string with get, set