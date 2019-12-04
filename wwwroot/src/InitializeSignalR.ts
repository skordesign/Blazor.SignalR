import { HubConnectionManager } from './HubConnectionManager';

namespace SignalR {
  const skorBlazor: string = 'SkorBlazor';
  // define what this extension adds to the window object inside BlazorExtensions
  const extensionObject = {
    SignalR: new HubConnectionManager()
  };

  export function initialize(): void {
    if (typeof window !== 'undefined' && !window[skorBlazor]) {
      // when the library is loaded in a browser via a <script> element, make the
      // following APIs available in global scope for invocation from JS
      window[skorBlazor] = {
        ...extensionObject
      };
    } else {
      window[skorBlazor] = {
        ...window[skorBlazor],
        ...extensionObject
      };
    }
  }
}

SignalR.initialize();
