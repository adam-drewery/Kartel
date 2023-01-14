namespace Fable.Import.SignalRCore
// ts2fable 0.6.1
module rec IHubProtocol =

    open System
    open Fable.Core
    open Fable.Core.JS
    open ILogger
    open ITransport

    type [<RequireQualifiedAccess>] MessageType =
        | Invocation = 1
        | StreamItem = 2
        | Completion = 3
        | StreamInvocation = 4
        | CancelInvocation = 5
        | Ping = 6
        | Close = 7

    /// Defines a dictionary of string keys and string values representing headers attached to a Hub message.
    type [<AllowNullLiteral>] MessageHeaders =
        /// Gets or sets the header with the specified key.
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> string with get, set

    type HubMessage =
        U7<InvocationMessage, StreamInvocationMessage, StreamItemMessage, CompletionMessage, CancelInvocationMessage, PingMessage, CloseMessage>

    [<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module HubMessage =
        let ofInvocationMessage v: HubMessage = v |> U7.Case1
        let isInvocationMessage (v: HubMessage) = match v with U7.Case1 _ -> true | _ -> false
        let asInvocationMessage (v: HubMessage) = match v with U7.Case1 o -> Some o | _ -> None
        let ofStreamInvocationMessage v: HubMessage = v |> U7.Case2
        let isStreamInvocationMessage (v: HubMessage) = match v with U7.Case2 _ -> true | _ -> false
        let asStreamInvocationMessage (v: HubMessage) = match v with U7.Case2 o -> Some o | _ -> None
        let ofStreamItemMessage v: HubMessage = v |> U7.Case3
        let isStreamItemMessage (v: HubMessage) = match v with U7.Case3 _ -> true | _ -> false
        let asStreamItemMessage (v: HubMessage) = match v with U7.Case3 o -> Some o | _ -> None
        let ofCompletionMessage v: HubMessage = v |> U7.Case4
        let isCompletionMessage (v: HubMessage) = match v with U7.Case4 _ -> true | _ -> false
        let asCompletionMessage (v: HubMessage) = match v with U7.Case4 o -> Some o | _ -> None
        let ofCancelInvocationMessage v: HubMessage = v |> U7.Case5
        let isCancelInvocationMessage (v: HubMessage) = match v with U7.Case5 _ -> true | _ -> false
        let asCancelInvocationMessage (v: HubMessage) = match v with U7.Case5 o -> Some o | _ -> None
        let ofPingMessage v: HubMessage = v |> U7.Case6
        let isPingMessage (v: HubMessage) = match v with U7.Case6 _ -> true | _ -> false
        let asPingMessage (v: HubMessage) = match v with U7.Case6 o -> Some o | _ -> None
        let ofCloseMessage v: HubMessage = v |> U7.Case7
        let isCloseMessage (v: HubMessage) = match v with U7.Case7 _ -> true | _ -> false
        let asCloseMessage (v: HubMessage) = match v with U7.Case7 o -> Some o | _ -> None

    /// Defines properties common to all Hub messages.
    type [<AllowNullLiteral>] HubMessageBase =
        /// A {@link @aspnet/signalr.MessageType} value indicating the type of this message.
        abstract ``type``: MessageType

    /// Defines properties common to all Hub messages relating to a specific invocation.
    type [<AllowNullLiteral>] HubInvocationMessage =
        inherit HubMessageBase
        /// A {@link @aspnet/signalr.MessageHeaders} dictionary containing headers attached to the message.
        abstract headers: MessageHeaders option
        /// The ID of the invocation relating to this message.
        ///
        /// This is expected to be present for {@link @aspnet/signalr.StreamInvocationMessage} and {@link @aspnet/signalr.CompletionMessage}. It may
        /// be 'undefined' for an {@link @aspnet/signalr.InvocationMessage} if the sender does not expect a response.
        abstract invocationId: string option

    /// A hub message representing a non-streaming invocation.
    type [<AllowNullLiteral>] InvocationMessage =
        inherit HubInvocationMessage
        abstract ``type``: MessageType
        /// The target method name.
        abstract target: string
        /// The target method arguments.
        abstract arguments: ResizeArray<obj option>

    /// A hub message representing a streaming invocation.
    type [<AllowNullLiteral>] StreamInvocationMessage =
        inherit HubInvocationMessage
        abstract ``type``: MessageType
        /// The invocation ID.
        abstract invocationId: string
        /// The target method name.
        abstract target: string
        /// The target method arguments.
        abstract arguments: ResizeArray<obj option>

    /// A hub message representing a single item produced as part of a result stream.
    type [<AllowNullLiteral>] StreamItemMessage =
        inherit HubInvocationMessage
        abstract ``type``: MessageType
        /// The invocation ID.
        abstract invocationId: string
        /// The item produced by the server.
        abstract item: obj option

    /// A hub message representing the result of an invocation.
    type [<AllowNullLiteral>] CompletionMessage =
        inherit HubInvocationMessage
        abstract ``type``: MessageType
        /// The invocation ID.
        abstract invocationId: string
        /// The error produced by the invocation, if any.
        ///
        /// Either {@link @aspnet/signalr.CompletionMessage.error} or {@link @aspnet/signalr.CompletionMessage.result} must be defined, but not both.
        abstract error: string option
        /// The result produced by the invocation, if any.
        ///
        /// Either {@link @aspnet/signalr.CompletionMessage.error} or {@link @aspnet/signalr.CompletionMessage.result} must be defined, but not both.
        abstract result: obj option

    /// A hub message indicating that the sender is still active.
    type [<AllowNullLiteral>] PingMessage =
        inherit HubMessageBase
        abstract ``type``: MessageType

    /// A hub message indicating that the sender is closing the connection.
    ///
    /// If {@link @aspnet/signalr.CloseMessage.error} is defined, the sender is closing the connection due to an error.
    type [<AllowNullLiteral>] CloseMessage =
        inherit HubMessageBase
        abstract ``type``: MessageType
        /// The error that triggered the close, if any.
        ///
        /// If this property is undefined, the connection was closed normally and without error.
        abstract error: string option

    /// A hub message sent to request that a streaming invocation be canceled.
    type [<AllowNullLiteral>] CancelInvocationMessage =
        inherit HubInvocationMessage
        abstract ``type``: MessageType
        /// The invocation ID.
        abstract invocationId: string

    /// A protocol abstraction for communicating with SignalR Hubs.
    type [<AllowNullLiteral>] IHubProtocol =
        /// The name of the protocol. This is used by SignalR to resolve the protocol between the client and server.
        abstract name: string
        /// The version of the protocol.
        abstract version: float
        /// The {@link @aspnet/signalr.TransferFormat} of the protocol.
        abstract transferFormat: TransferFormat
        /// <summary>Creates an array of {@link @aspnet/signalr.HubMessage} objects from the specified serialized representation.
        ///
        /// If {@link @aspnet/signalr.IHubProtocol.transferFormat} is 'Text', the `input` parameter must be a string, otherwise it must be an ArrayBuffer.</summary>
        /// <param name="input">A string, or ArrayBuffer containing the serialized representation.</param>
        /// <param name="logger">A logger that will be used to log messages that occur during parsing.</param>
        abstract parseMessages: input: U2<string, ArrayBuffer> * logger: ILogger -> ResizeArray<HubMessage>
        /// <summary>Writes the specified {@link @aspnet/signalr.HubMessage} to a string or ArrayBuffer and returns it.
        ///
        /// If {@link @aspnet/signalr.IHubProtocol.transferFormat} is 'Text', the result of this method will be a string, otherwise it will be an ArrayBuffer.</summary>
        /// <param name="message">The message to write.</param>
        abstract writeMessage: message: HubMessage -> U2<string, ArrayBuffer>
