﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 5.0.61118.0
// 
namespace Conquerors.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="", ConfigurationName="ServiceReference1.ControlModuleLoader")]
    public interface ControlModuleLoader {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="urn:ControlModuleLoader/GetModuleList", ReplyAction="urn:ControlModuleLoader/GetModuleListResponse")]
        System.IAsyncResult BeginGetModuleList(System.AsyncCallback callback, object asyncState);
        
        System.Collections.ObjectModel.ObservableCollection<string> EndGetModuleList(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ControlModuleLoaderChannel : Conquerors.ServiceReference1.ControlModuleLoader, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetModuleListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetModuleListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.Collections.ObjectModel.ObservableCollection<string> Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.Collections.ObjectModel.ObservableCollection<string>)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ControlModuleLoaderClient : System.ServiceModel.ClientBase<Conquerors.ServiceReference1.ControlModuleLoader>, Conquerors.ServiceReference1.ControlModuleLoader {
        
        private BeginOperationDelegate onBeginGetModuleListDelegate;
        
        private EndOperationDelegate onEndGetModuleListDelegate;
        
        private System.Threading.SendOrPostCallback onGetModuleListCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public ControlModuleLoaderClient() {
        }
        
        public ControlModuleLoaderClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ControlModuleLoaderClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ControlModuleLoaderClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ControlModuleLoaderClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<GetModuleListCompletedEventArgs> GetModuleListCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult Conquerors.ServiceReference1.ControlModuleLoader.BeginGetModuleList(System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetModuleList(callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Collections.ObjectModel.ObservableCollection<string> Conquerors.ServiceReference1.ControlModuleLoader.EndGetModuleList(System.IAsyncResult result) {
            return base.Channel.EndGetModuleList(result);
        }
        
        private System.IAsyncResult OnBeginGetModuleList(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((Conquerors.ServiceReference1.ControlModuleLoader)(this)).BeginGetModuleList(callback, asyncState);
        }
        
        private object[] OnEndGetModuleList(System.IAsyncResult result) {
            System.Collections.ObjectModel.ObservableCollection<string> retVal = ((Conquerors.ServiceReference1.ControlModuleLoader)(this)).EndGetModuleList(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetModuleListCompleted(object state) {
            if ((this.GetModuleListCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetModuleListCompleted(this, new GetModuleListCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetModuleListAsync() {
            this.GetModuleListAsync(null);
        }
        
        public void GetModuleListAsync(object userState) {
            if ((this.onBeginGetModuleListDelegate == null)) {
                this.onBeginGetModuleListDelegate = new BeginOperationDelegate(this.OnBeginGetModuleList);
            }
            if ((this.onEndGetModuleListDelegate == null)) {
                this.onEndGetModuleListDelegate = new EndOperationDelegate(this.OnEndGetModuleList);
            }
            if ((this.onGetModuleListCompletedDelegate == null)) {
                this.onGetModuleListCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetModuleListCompleted);
            }
            base.InvokeAsync(this.onBeginGetModuleListDelegate, null, this.onEndGetModuleListDelegate, this.onGetModuleListCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override Conquerors.ServiceReference1.ControlModuleLoader CreateChannel() {
            return new ControlModuleLoaderClientChannel(this);
        }
        
        private class ControlModuleLoaderClientChannel : ChannelBase<Conquerors.ServiceReference1.ControlModuleLoader>, Conquerors.ServiceReference1.ControlModuleLoader {
            
            public ControlModuleLoaderClientChannel(System.ServiceModel.ClientBase<Conquerors.ServiceReference1.ControlModuleLoader> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginGetModuleList(System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[0];
                System.IAsyncResult _result = base.BeginInvoke("GetModuleList", _args, callback, asyncState);
                return _result;
            }
            
            public System.Collections.ObjectModel.ObservableCollection<string> EndGetModuleList(System.IAsyncResult result) {
                object[] _args = new object[0];
                System.Collections.ObjectModel.ObservableCollection<string> _result = ((System.Collections.ObjectModel.ObservableCollection<string>)(base.EndInvoke("GetModuleList", _args, result)));
                return _result;
            }
        }
    }
}
