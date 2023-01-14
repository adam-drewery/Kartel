// ts2fable 0.6.1
namespace Fable.Import.SignalRCore

module rec HubConnectionBuilder =
    open System
    open Fable.Core
    open ILogger
    open IHubProtocol
    open IHttpConnectionOptions
    open HubConnection
    open ITransport

    /// A builder for configuring {@link @aspnet/signalr.HubConnection} instances.
    // [<ImportAll("./../public/signalr.min.js")>]
    type [<AllowNullLiteral>] HubConnectionBuilder =
        abstract protocol: IHubProtocol with get, set
        abstract httpConnectionOptions: IHttpConnectionOptions with get, set
        abstract url: string with get, set
        abstract logger: ILogger with get, set
        /// <summary>Configures console logging for the {@link @aspnet/signalr.HubConnection}.</summary>
        /// <param name="logLevel">The minimum level of messages to log. Anything at this level, or a more severe level, will be logged.</param>
        abstract configureLogging: logLevel: LogLevel -> HubConnectionBuilder
        /// <summary>Configures custom logging for the {@link @aspnet/signalr.HubConnection}.</summary>
        /// <param name="logger">An object implementing the {</param>
        abstract configureLogging: logger: ILogger -> HubConnectionBuilder
        abstract configureLogging: logging: U2<LogLevel, ILogger> -> HubConnectionBuilder
        /// <summary>Configures the {@link @aspnet/signalr.HubConnection} to use HTTP-based transports to connect to the specified URL.
        ///
        /// The transport will be selected automatically based on what the server and client support.</summary>
        /// <param name="url">The URL the connection will use.</param>
        abstract withUrl: url: string -> HubConnectionBuilder
        /// <summary>Configures the {@link @aspnet/signalr.HubConnection} to use the specified HTTP-based transport to connect to the specified URL.</summary>
        /// <param name="url">The URL the connection will use.</param>
        /// <param name="transportType">The specific transport to use.</param>
        abstract withUrl: url: string * transportType: HttpTransportType -> HubConnectionBuilder
        /// <summary>Configures the {@link @aspnet/signalr.HubConnection} to use HTTP-based transports to connect to the specified URL.</summary>
        /// <param name="url">The URL the connection will use.</param>
        /// <param name="options">An options object used to configure the connection.</param>
        abstract withUrl: url: string * options: IHttpConnectionOptions -> HubConnectionBuilder
        abstract withUrl: url: string * ?transportTypeOrOptions: U2<IHttpConnectionOptions, HttpTransportType> -> HubConnectionBuilder
        /// <summary>Configures the {@link @aspnet/signalr.HubConnection} to use the specified Hub Protocol.</summary>
        /// <param name="protocol">The {</param>
        abstract withHubProtocol: protocol: IHubProtocol -> HubConnectionBuilder
        /// Creates a {@link @aspnet/signalr.HubConnection} from the configuration options specified in this builder.
        abstract build: unit -> HubConnection

    /// A builder for configuring {@link @aspnet/signalr.HubConnection} instances.
    module HubConnectionBuilderStatic =
        [<ImportAll("./../public/signalr.min.js")>]
        [<Emit "new $0($1...)">]
        let Create: unit -> HubConnectionBuilder = jsNative
