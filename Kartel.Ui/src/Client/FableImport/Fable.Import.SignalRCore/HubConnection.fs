namespace Fable.Import.SignalRCore
// ts2fable 0.6.1
module rec HubConnection =
    open System
    open Fable.Core
    open Fable.Core.JS
    open IConnection
    open ILogger
    open IHubProtocol
    open HandshakeProtocol
    open Stream
    open Error

    /// Represents a connection to a SignalR Hub.
    [<ImportAll("./../public/signalr.min.js")>]
    type [<AllowNullLiteral>] HubConnection =

        abstract connection: IConnection
        abstract logger: ILogger
        abstract protocol: IHubProtocol with get, set
        abstract handshakeProtocol: HandshakeProtocol with get, set
        abstract callbacks: obj with get, set
        abstract methods: obj with get, set
        abstract id: float with get, set
        abstract closedCallbacks: ResizeArray<(Error -> unit)> with get, set
        abstract timeoutHandle: obj option with get, set
        abstract receivedHandshakeResponse: bool with get, set
        /// The server timeout in milliseconds.
        ///
        /// If this timeout elapses without receiving any messages from the server, the connection will be terminated with an error.
        /// The default timeout value is 30,000 milliseconds (30 seconds).
        abstract serverTimeoutInMilliseconds: float with get, set
        /// Starts the connection.
        abstract start: unit -> Promise<unit>
        /// Stops the connection.
        abstract stop: unit -> Promise<unit>
        /// <summary>Invokes a streaming hub method on the server using the specified name and arguments.</summary>
        /// <param name="methodName">The name of the server method to invoke.</param>
        /// <param name="args">The arguments used to invoke the server method.</param>
        abstract stream: methodName: string * [<ParamArray>] args: ResizeArray<obj option> -> IStreamResult<'T>
        /// <summary>Invokes a hub method on the server using the specified name and arguments. Does not wait for a response from the receiver.
        ///
        /// The Promise returned by this method resolves when the client has sent the invocation to the server. The server may still
        /// be processing the invocation.</summary>
        /// <param name="methodName">The name of the server method to invoke.</param>
        /// <param name="args">The arguments used to invoke the server method.</param>
        abstract send: methodName: string * [<ParamArray>] args: ResizeArray<obj option> -> Promise<unit>
        /// <summary>Invokes a hub method on the server using the specified name and arguments.
        ///
        /// The Promise returned by this method resolves when the server indicates it has finished invoking the method. When the promise
        /// resolves, the server has finished invoking the method. If the server method returns a result, it is produced as the result of
        /// resolving the Promise.</summary>
        /// <param name="methodName">The name of the server method to invoke.</param>
        /// <param name="args">The arguments used to invoke the server method.</param>
        abstract invoke: methodName: string * [<ParamArray>] args: ResizeArray<obj option> -> Promise<'T>
        /// <summary>Registers a handler that will be invoked when the hub method with the specified method name is invoked.</summary>
        /// <param name="methodName">The name of the hub method to define.</param>
        /// <param name="newMethod">The handler that will be raised when the hub method is invoked.</param>
        abstract on: methodName: string * newMethod: ('usr -> 'msg -> unit) -> unit
        abstract on: methodName: string * newMethod: ('msg -> unit) -> unit
        /// <summary>Removes all handlers for the specified hub method.</summary>
        /// <param name="methodName">The name of the method to remove handlers for.</param>
        abstract off: methodName: string -> unit
        /// <summary>Removes the specified handler for the specified hub method.
        ///
        /// You must pass the exact same Function instance as was previously passed to {@link @aspnet/signalr.HubConnection.on}. Passing a different instance (even if the function
        /// body is the same) will not remove the handler.</summary>
        /// <param name="methodName">The name of the method to remove handlers for.</param>
        /// <param name="method">The handler to remove. This must be the same Function instance as the one passed to {</param>
        abstract off: methodName: string * ``method``: (ResizeArray<obj option> -> unit) -> unit
        abstract off: methodName: string * ?``method``: (ResizeArray<obj option> -> unit) -> unit
        /// <summary>Registers a handler that will be invoked when the connection is closed.</summary>
        /// <param name="callback">The handler that will be invoked when the connection is closed. Optionally receives a single argument containing the error that caused the connection to close (if any).</param>
        abstract onclose: callback: (Error -> unit) -> unit
        abstract processIncomingData: data: obj option -> unit
        abstract processHandshakeResponse: data: obj option -> obj option
        abstract configureTimeout: unit -> unit
        abstract serverTimeout: unit -> unit
        abstract invokeClientMethod: invocationMessage: InvocationMessage -> unit
        abstract connectionClosed: ?error: Error -> unit
        abstract cleanupTimeout: unit -> unit
        abstract createInvocation: methodName: string * args: ResizeArray<obj option> * nonblocking: bool -> InvocationMessage
        abstract createStreamInvocation: methodName: string * args: ResizeArray<obj option> -> StreamInvocationMessage
        abstract createCancelInvocation: id: string -> CancelInvocationMessage

    /// Represents a connection to a SignalR Hub.
    type [<AllowNullLiteral>] HubConnectionStatic =
        abstract create: connection: IConnection * logger: ILogger * protocol: IHubProtocol -> HubConnection
        [<Emit "new $0($1...)">] abstract Create: connection: IConnection * logger: ILogger * protocol: IHubProtocol -> HubConnection
