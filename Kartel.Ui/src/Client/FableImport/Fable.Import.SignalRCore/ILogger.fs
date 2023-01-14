namespace Fable.Import.SignalRCore

// ts2fable 0.6.1
module rec ILogger =

    open System
    open Fable.Core
    open Fable.Core.JS


    type [<RequireQualifiedAccess>] LogLevel =
        | Trace = 0
        | Debug = 1
        | Information = 2
        | Warning = 3
        | Error = 4
        | Critical = 5
        | None = 6

    /// An abstraction that provides a sink for diagnostic messages.
    type [<AllowNullLiteral>] ILogger =
        /// <summary>Called by the framework to emit a diagnostic message.</summary>
        /// <param name="logLevel">The severity level of the message.</param>
        /// <param name="message">The message.</param>
        abstract log: logLevel: LogLevel * message: string -> unit
