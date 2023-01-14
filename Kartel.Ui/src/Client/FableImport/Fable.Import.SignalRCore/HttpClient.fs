namespace Fable.Import.SignalRCore
// ts2fable 0.6.1
module rec HttpClient =

    open System
    open Fable.Core
    open Fable.Core.JS
    open Fable
    open ILogger
    open AbortController
    open Errors
    open Browser.Types

    type XMLHttpRequestResponseType =
        | ArrayBuffer
        | Blob
        | Document
        | Json
        | Text
        | Empty

    /// Represents an HTTP request.
    type [<AllowNullLiteral>] HttpRequest =
        /// The HTTP method to use for the request.
        abstract ``method``: string option with get, set
        /// The URL for the request.
        abstract url: string option with get, set
        /// The body content for the request. May be a string or an ArrayBuffer (for binary data).
        abstract content: U2<string, ArrayBuffer> option with get, set
        /// An object describing headers to apply to the request.
        abstract headers: obj option with get, set
        /// The XMLHttpRequestResponseType to apply to the request.
        abstract responseType: XMLHttpRequestResponseType option with get, set
        /// An AbortSignal that can be monitored for cancellation.
        abstract abortSignal: AbortSignal option with get, set
        /// The time to wait for the request to complete before throwing a TimeoutError. Measured in milliseconds.
        abstract timeout: float option with get, set

    /// Represents an HTTP response.
    type [<AllowNullLiteral>] HttpResponse =
        interface end

    /// Represents an HTTP response.
    type [<AllowNullLiteral>] HttpResponseStatic =
        /// <summary>Constructs a new instance of {@link @aspnet/signalr.HttpResponse} with the specified status code.</summary>
        /// <param name="statusCode">The status code of the response.</param>
        [<Emit "new $0($1...)">] abstract Create: statusCode: float -> HttpResponse
        /// <summary>Constructs a new instance of {@link @aspnet/signalr.HttpResponse} with the specified status code and message.</summary>
        /// <param name="statusCode">The status code of the response.</param>
        /// <param name="statusText">The status message of the response.</param>
        [<Emit "new $0($1...)">] abstract Create: statusCode: float * statusText: string -> HttpResponse
        /// <summary>Constructs a new instance of {@link @aspnet/signalr.HttpResponse} with the specified status code, message and string content.</summary>
        /// <param name="statusCode">The status code of the response.</param>
        /// <param name="statusText">The status message of the response.</param>
        /// <param name="content">The content of the response.</param>
        [<Emit "new $0($1...)">] abstract Create: statusCode: float * statusText: string * content: string -> HttpResponse
        /// <summary>Constructs a new instance of {@link @aspnet/signalr.HttpResponse} with the specified status code, message and binary content.</summary>
        /// <param name="statusCode">The status code of the response.</param>
        /// <param name="statusText">The status message of the response.</param>
        /// <param name="content">The content of the response.</param>
        [<Emit "new $0($1...)">] abstract Create: statusCode: float * statusText: string * content: ArrayBuffer -> HttpResponse
        [<Emit "new $0($1...)">] abstract Create: statusCode: float * ?statusText: string * ?content: U2<string, ArrayBuffer> -> HttpResponse

    /// Abstraction over an HTTP client.
    ///
    /// This class provides an abstraction over an HTTP client so that a different implementation can be provided on different platforms.
    type [<AllowNullLiteral>] HttpClient =
        /// <summary>Issues an HTTP GET request to the specified URL, returning a Promise that resolves with an {@link @aspnet/signalr.HttpResponse} representing the result.</summary>
        /// <param name="url">The URL for the request.</param>
        abstract get: url: string -> Promise<HttpResponse>
        /// <summary>Issues an HTTP GET request to the specified URL, returning a Promise that resolves with an {@link @aspnet/signalr.HttpResponse} representing the result.</summary>
        /// <param name="url">The URL for the request.</param>
        /// <param name="options">Additional options to configure the request. The 'url' field in this object will be overridden by the url parameter.</param>
        abstract get: url: string * options: HttpRequest -> Promise<HttpResponse>
        abstract get: url: string * ?options: HttpRequest -> Promise<HttpResponse>
        /// <summary>Issues an HTTP POST request to the specified URL, returning a Promise that resolves with an {@link @aspnet/signalr.HttpResponse} representing the result.</summary>
        /// <param name="url">The URL for the request.</param>
        abstract post: url: string -> Promise<HttpResponse>
        /// <summary>Issues an HTTP POST request to the specified URL, returning a Promise that resolves with an {@link @aspnet/signalr.HttpResponse} representing the result.</summary>
        /// <param name="url">The URL for the request.</param>
        /// <param name="options">Additional options to configure the request. The 'url' field in this object will be overridden by the url parameter.</param>
        abstract post: url: string * options: HttpRequest -> Promise<HttpResponse>
        abstract post: url: string * ?options: HttpRequest -> Promise<HttpResponse>
        /// <summary>Issues an HTTP DELETE request to the specified URL, returning a Promise that resolves with an {@link @aspnet/signalr.HttpResponse} representing the result.</summary>
        /// <param name="url">The URL for the request.</param>
        abstract delete: url: string -> Promise<HttpResponse>
        /// <summary>Issues an HTTP DELETE request to the specified URL, returning a Promise that resolves with an {@link @aspnet/signalr.HttpResponse} representing the result.</summary>
        /// <param name="url">The URL for the request.</param>
        /// <param name="options">Additional options to configure the request. The 'url' field in this object will be overridden by the url parameter.</param>
        abstract delete: url: string * options: HttpRequest -> Promise<HttpResponse>
        abstract delete: url: string * ?options: HttpRequest -> Promise<HttpResponse>
        /// <summary>Issues an HTTP request to the specified URL, returning a {@link Promise} that resolves with an {@link @aspnet/signalr.HttpResponse} representing the result.</summary>
        /// <param name="request">An {</param>
        abstract send: request: HttpRequest -> Promise<HttpResponse>

    /// Abstraction over an HTTP client.
    ///
    /// This class provides an abstraction over an HTTP client so that a different implementation can be provided on different platforms.
    type [<AllowNullLiteral>] HttpClientStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> HttpClient

    /// Default implementation of {@link @aspnet/signalr.HttpClient}.
    type [<AllowNullLiteral>] DefaultHttpClient =
        inherit HttpClient
        abstract logger: ILogger
        abstract send: request: HttpRequest -> Promise<HttpResponse>

    /// Default implementation of {@link @aspnet/signalr.HttpClient}.
    type [<AllowNullLiteral>] DefaultHttpClientStatic =
        /// Creates a new instance of the {@link @aspnet/signalr.DefaultHttpClient}, using the provided {@link @aspnet/signalr.ILogger} to log messages.
        [<Emit "new $0($1...)">] abstract Create: logger: ILogger -> DefaultHttpClient
