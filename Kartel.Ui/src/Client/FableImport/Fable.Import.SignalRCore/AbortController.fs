namespace Fable.Import.SignalRCore

// ts2fable 0.6.1
module rec AbortController =

    open Fable.Core

    type [<AllowNullLiteral>] AbortController =
        inherit AbortSignal
        abstract isAborted: bool with get, set
        abstract onabort: (unit -> unit) with get, set
        abstract abort: unit -> unit

    type [<AllowNullLiteral>] AbortControllerStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> AbortController

    /// Represents a signal that can be monitored to determine if a request has been aborted.
    type [<AllowNullLiteral>] AbortSignal =
        /// Indicates if the request has been aborted.
        abstract aborted: bool with get, set
        /// Set this to a handler that will be invoked when the request is aborted.
        abstract onabort: (unit -> unit) with get, set
