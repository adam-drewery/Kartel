namespace Fable.Import.SignalRCore

// ts2fable 0.6.1
module rec Errors =

    open System
    open Fable.Core
    open Fable.Core.JS
    open Fable

    type [<AllowNullLiteral>] Error =
        abstract name: string with get, set

    /// Error thrown when an HTTP request fails.
    type [<AllowNullLiteral>] HttpError =
        inherit Error
        abstract __proto__: Error with get, set
        /// The HTTP status code represented by this error.
        abstract statusCode: float with get, set

    /// Error thrown when an HTTP request fails.
    type [<AllowNullLiteral>] HttpErrorStatic =
        /// <summary>Constructs a new instance of {@link @aspnet/signalr.HttpError}.</summary>
        /// <param name="errorMessage">A descriptive error message.</param>
        /// <param name="statusCode">The HTTP status code represented by this error.</param>
        [<Emit "new $0($1...)">] abstract Create: errorMessage: string * statusCode: float -> HttpError

    /// Error thrown when a timeout elapses.
    type [<AllowNullLiteral>] TimeoutError =
        inherit Error
        abstract __proto__: Error with get, set

    /// Error thrown when a timeout elapses.
    type [<AllowNullLiteral>] TimeoutErrorStatic =
        /// <summary>Constructs a new instance of {@link @aspnet/signalr.TimeoutError}.</summary>
        /// <param name="errorMessage">A descriptive error message.</param>
        [<Emit "new $0($1...)">] abstract Create: errorMessage: string -> TimeoutError
