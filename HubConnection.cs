using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blazor.SignalR
{
    public class HubConnection : IDisposable
    {
        private const string ON_METHOD = "Blazor.SignalR.On";
        private const string ON_CLOSE_METHOD = "Blazor.SignalR.OnClose";
        private const string OFF_METHOD = "Blazor.SignalR.Off";
        private const string CREATE_CONNECTION_METHOD = "Blazor.SignalR.CreateConnection";
        private const string REMOVE_CONNECTION_METHOD = "Blazor.SignalR.RemoveConnection";
        private const string START_CONNECTION_METHOD = "Blazor.SignalR.StartConnection";
        private const string STOP_CONNECTION_METHOD = "Blazor.SignalR.StopConnection";
        private const string INVOKE_ASYNC_METHOD = "Blazor.SignalR.InvokeAsync";
        private const string INVOKE_WITH_RESULT_ASYNC_METHOD = "Blazor.SignalR.InvokeWithResultAsync";

        internal HttpConnectionOptions Options { get; }
        internal string InternalConnectionId { get; }

        private Dictionary<string, Dictionary<string, HubMethodCallback>> _callbacks = new Dictionary<string, Dictionary<string, HubMethodCallback>>();

        private HubCloseCallback _closeCallback;
        private readonly IJSRuntime jSRuntime;
        public HubConnection(IJSRuntime jSRuntime, HttpConnectionOptions options)
        {
            this.jSRuntime = jSRuntime;
            this.Options = options;
            this.InternalConnectionId = Guid.NewGuid().ToString();
            this.jSRuntime.InvokeSync<object>(CREATE_CONNECTION_METHOD,
                this.InternalConnectionId,
                DotNetObjectReference.Create(this.Options));
        }


        public ValueTask<object> StartAsync() => this.jSRuntime.InvokeAsync<object>(START_CONNECTION_METHOD, this.InternalConnectionId);
        public ValueTask<object> StopAsync() => this.jSRuntime.InvokeAsync<object>(STOP_CONNECTION_METHOD, this.InternalConnectionId);

        public IDisposable On<TResult1>(string methodName, Func<TResult1, Task> handler)
            => On<TResult1, object, object, object, object, object, object, object, object, object>(methodName,
                (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => handler(t1));

        public IDisposable On<TResult1, TResult2>(string methodName, Func<TResult1, TResult2, Task> handler)
            => On<TResult1, TResult2, object, object, object, object, object, object, object, object>(methodName,
                (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => handler(t1, t2));

        public IDisposable On<TResult1, TResult2, TResult3>(string methodName, Func<TResult1, TResult2, TResult3, Task> handler)
            => On<TResult1, TResult2, TResult3, object, object, object, object, object, object, object>(methodName,
                (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => handler(t1, t2, t3));

        public IDisposable On<TResult1, TResult2, TResult3, TResult4>(string methodName, Func<TResult1, TResult2, TResult3, TResult4, Task> handler)
            => On<TResult1, TResult2, TResult3, TResult4, object, object, object, object, object, object>(methodName,
                (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => handler(t1, t2, t3, t4));

        public IDisposable On<TResult1, TResult2, TResult3, TResult4, TResult5>(string methodName, Func<TResult1, TResult2, TResult3, TResult4, TResult5, Task> handler)
            => On<TResult1, TResult2, TResult3, TResult4, TResult5, object, object, object, object, object>(methodName,
                (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => handler(t1, t2, t3, t4, t5));

        public IDisposable On<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(string methodName,
            Func<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, Task> handler)
            => On<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, object, object, object, object>(methodName,
                (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => handler(t1, t2, t3, t4, t5, t6));

        public IDisposable On<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(string methodName,
            Func<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, Task> handler)
            => On<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, object, object, object>(methodName,
                (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => handler(t1, t2, t3, t4, t5, t6, t7));

        public IDisposable On<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>(string methodName,
            Func<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, Task> handler)
            => On<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, object, object>(methodName,
                (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => handler(t1, t2, t3, t4, t5, t6, t7, t8));

        public IDisposable On<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>(string methodName,
            Func<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, Task> handler)
            => On<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, object>(methodName,
                (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => handler(t1, t2, t3, t4, t5, t6, t7, t8, t9));

        public IDisposable On<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>(string methodName,
            Func<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10, Task> handler)
        {
            if (string.IsNullOrEmpty(methodName)) throw new ArgumentNullException(nameof(methodName));
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            var callbackId = Guid.NewGuid().ToString();

            var callback = new HubMethodCallback(callbackId, methodName, this,
                 (payloads) =>
                 {
                     TResult1 t1 = default;
                     TResult2 t2 = default;
                     TResult3 t3 = default;
                     TResult4 t4 = default;
                     TResult5 t5 = default;
                     TResult6 t6 = default;
                     TResult7 t7 = default;
                     TResult8 t8 = default;
                     TResult9 t9 = default;
                     TResult10 t10 = default;

                     if (payloads.Length > 0)
                     {
                         t1 = JsonSerializer.Deserialize<TResult1>(payloads[0]);
                     }
                     if (payloads.Length > 1)
                     {
                         t2 = JsonSerializer.Deserialize<TResult2>(payloads[1]);
                     }
                     if (payloads.Length > 2)
                     {
                         t3 = JsonSerializer.Deserialize<TResult3>(payloads[2]);
                     }
                     if (payloads.Length > 3)
                     {
                         t4 = JsonSerializer.Deserialize<TResult4>(payloads[3]);
                     }
                     if (payloads.Length > 4)
                     {
                         t5 = JsonSerializer.Deserialize<TResult5>(payloads[4]);
                     }
                     if (payloads.Length > 5)
                     {
                         t6 = JsonSerializer.Deserialize<TResult6>(payloads[5]);
                     }
                     if (payloads.Length > 6)
                     {
                         t7 = JsonSerializer.Deserialize<TResult7>(payloads[6]);
                     }
                     if (payloads.Length > 7)
                     {
                         t8 = JsonSerializer.Deserialize<TResult8>(payloads[7]);
                     }
                     if (payloads.Length > 8)
                     {
                         t9 = JsonSerializer.Deserialize<TResult9>(payloads[8]);
                     }
                     if (payloads.Length > 9)
                     {
                         t10 = JsonSerializer.Deserialize<TResult10>(payloads[9]);
                     }

                     return handler(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                 }
            );

            RegisterHandle(methodName, callback);

            return callback;
        }

        internal void RegisterHandle(string methodName, HubMethodCallback callback)
        {
            if (this._callbacks.TryGetValue(methodName, out var methodHandlers))
            {
                methodHandlers[callback.Id] = callback;
            }
            else
            {
                this._callbacks[methodName] = new Dictionary<string, HubMethodCallback>
                {
                    { callback.Id, callback }
                };
            }

            this.jSRuntime.InvokeSync<object>(ON_METHOD, this.InternalConnectionId, DotNetObjectReference.Create(callback));
        }

        internal void RemoveHandle(string methodName, string callbackId)
        {
            if (this._callbacks.TryGetValue(methodName, out var callbacks))
            {
                if (callbacks.TryGetValue(callbackId, out var callback))
                {
                    this.jSRuntime.InvokeSync<object>(OFF_METHOD, this.InternalConnectionId, methodName, callbackId);
                    //HubConnectionManager.Off(this.InternalConnectionId, handle.Item1);
                    callbacks.Remove(callbackId);

                    if (callbacks.Count == 0)
                    {
                        this._callbacks.Remove(methodName);
                    }
                }
            }
        }

        public void OnClose(Func<Exception, Task> callback)
        {
            this._closeCallback = new HubCloseCallback(callback);
            this.jSRuntime.InvokeSync<object>(ON_CLOSE_METHOD,
                this.InternalConnectionId,
                DotNetObjectReference.Create(this._closeCallback));
        }

        public ValueTask<object> InvokeAsync(string methodName, params object[] args) =>
            this.jSRuntime.InvokeAsync<object>(INVOKE_ASYNC_METHOD, this.InternalConnectionId, methodName, args);

        public ValueTask<TResult> InvokeAsync<TResult>(string methodName, params object[] args) =>
            this.jSRuntime.InvokeAsync<TResult>(INVOKE_WITH_RESULT_ASYNC_METHOD, this.InternalConnectionId, methodName, args);

        public void Dispose() => this.jSRuntime.InvokeSync<object>(REMOVE_CONNECTION_METHOD, this.InternalConnectionId);
    }
}
