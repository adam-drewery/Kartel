namespace Fable.Import.SignalRCore
// ts2fable 0.6.1
module rec Stream =

    /// Defines the expected type for a receiver of results streamed by the server.
    type [<AllowNullLiteral>] IStreamSubscriber<'T> =
        /// A boolean that will be set by the {@link @aspnet/signalr.IStreamResult} when the stream is closed.
        abstract closed: bool option with get, set
        /// Called by the framework when a new item is available.
        abstract next: value: 'T -> unit
        /// Called by the framework when an error has occurred.
        ///
        /// After this method is called, no additional methods on the {@link @aspnet/signalr.IStreamSubscriber} will be called.
        abstract error: err: obj option -> unit
        /// Called by the framework when the end of the stream is reached.
        ///
        /// After this method is called, no additional methods on the {@link @aspnet/signalr.IStreamSubscriber} will be called.
        abstract complete: unit -> unit

    /// Defines the result of a streaming hub method.
    type [<AllowNullLiteral>] IStreamResult<'T> =
        /// Attaches a {@link @aspnet/signalr.IStreamSubscriber}, which will be invoked when new items are available from the stream.
        abstract subscribe: subscriber: IStreamSubscriber<'T> -> ISubscription<'T>

    /// An interface that allows an {@link @aspnet/signalr.IStreamSubscriber} to be disconnected from a stream.
    type [<AllowNullLiteral>] ISubscription<'T> =
        /// Disconnects the {@link @aspnet/signalr.IStreamSubscriber} associated with this subscription from the stream.
        abstract dispose: unit -> unit
