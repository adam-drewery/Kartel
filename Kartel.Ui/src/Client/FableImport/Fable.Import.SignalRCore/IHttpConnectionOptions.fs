namespace Fable.Import.SignalRCore

// ts2fable 0.6.1
module rec IHttpConnectionOptions =

    open Fable.Core
    open Fable.Core.JS
    open ILogger
    open ITransport
    open HttpClient

    /// Options provided to the 'withUrl' method on {@link @aspnet/signalr.HubConnectionBuilder} to configure options for the HTTP-based transports.
    type [<AllowNullLiteral>] IHttpConnectionOptions =
        /// An {@link @aspnet/signalr.HttpClient} that will be used to make HTTP requests.
        abstract httpClient: HttpClient option with get, set
        /// An {@link @aspnet/signalr.HttpTransportType} value specifying the transport to use for the connection.
        abstract transport: U2<HttpTransportType, ITransport> option with get, set
        /// Configures the logger used for logging.
        ///
        /// Provide an {@link @aspnet/signalr.ILogger} instance, and log messages will be logged via that instance. Alternatively, provide a value from
        /// the {@link @aspnet/signalr.LogLevel} enumeration and a default logger which logs to the Console will be configured to log messages of the specified
        /// level (or higher).
        abstract logger: U2<ILogger, LogLevel> option with get, set
        /// A function that provides an access token required for HTTP Bearer authentication.
        abstract accessTokenFactory: unit -> U2<string, Promise<string>>
        /// A boolean indicating if message content should be logged.
        ///
        /// Message content can contain sensitive user data, so this is disabled by default.
        abstract logMessageContent: bool option with get, set
        /// A boolean indicating if negotiation should be skipped.
        ///
        /// Negotiation can only be skipped when the {@link @aspnet/signalr.IHttpConnectionOptions.transport} property is set to 'HttpTransportType.WebSockets'.
        abstract skipNegotiation: bool option with get, set
