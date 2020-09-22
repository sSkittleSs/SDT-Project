﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SDT_Project.ServiceServer {
    using System.Runtime.Serialization;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DataTypes", Namespace="http://schemas.datacontract.org/2004/07/SDT_Project_Service")]
    public enum DataTypes : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        UserData = 0,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="MessageImportance", Namespace="http://schemas.datacontract.org/2004/07/SDT_Project_Service")]
    public enum MessageImportance : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Common = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Important = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SysInfo = 2,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceServer.IServiceServer", CallbackContract=typeof(SDT_Project.ServiceServer.IServiceServerCallback))]
    public interface IServiceServer {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceServer/Connect", ReplyAction="http://tempuri.org/IServiceServer/ConnectResponse")]
        uint Connect(string userName, string userPassword);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceServer/Connect", ReplyAction="http://tempuri.org/IServiceServer/ConnectResponse")]
        System.Threading.Tasks.Task<uint> ConnectAsync(string userName, string userPassword);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IServiceServer/Disconnect")]
        void Disconnect(uint id);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IServiceServer/Disconnect")]
        System.Threading.Tasks.Task DisconnectAsync(uint id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceServer/Registering", ReplyAction="http://tempuri.org/IServiceServer/RegisteringResponse")]
        uint Registering();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceServer/Registering", ReplyAction="http://tempuri.org/IServiceServer/RegisteringResponse")]
        System.Threading.Tasks.Task<uint> RegisteringAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceServer/GetUserData", ReplyAction="http://tempuri.org/IServiceServer/GetUserDataResponse")]
        string GetUserData(uint id, SDT_Project.ServiceServer.DataTypes dataType);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceServer/GetUserData", ReplyAction="http://tempuri.org/IServiceServer/GetUserDataResponse")]
        System.Threading.Tasks.Task<string> GetUserDataAsync(uint id, SDT_Project.ServiceServer.DataTypes dataType);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IServiceServer/ConsoleLog")]
        void ConsoleLog(string msg, SDT_Project.ServiceServer.MessageImportance importance);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IServiceServer/ConsoleLog")]
        System.Threading.Tasks.Task ConsoleLogAsync(string msg, SDT_Project.ServiceServer.MessageImportance importance);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceServerCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IServiceServer/DataCallback")]
        void DataCallback(string data);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceServerChannel : SDT_Project.ServiceServer.IServiceServer, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceServerClient : System.ServiceModel.DuplexClientBase<SDT_Project.ServiceServer.IServiceServer>, SDT_Project.ServiceServer.IServiceServer {
        
        public ServiceServerClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public ServiceServerClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public ServiceServerClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceServerClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceServerClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public uint Connect(string userName, string userPassword) {
            return base.Channel.Connect(userName, userPassword);
        }
        
        public System.Threading.Tasks.Task<uint> ConnectAsync(string userName, string userPassword) {
            return base.Channel.ConnectAsync(userName, userPassword);
        }
        
        public void Disconnect(uint id) {
            base.Channel.Disconnect(id);
        }
        
        public System.Threading.Tasks.Task DisconnectAsync(uint id) {
            return base.Channel.DisconnectAsync(id);
        }
        
        public uint Registering() {
            return base.Channel.Registering();
        }
        
        public System.Threading.Tasks.Task<uint> RegisteringAsync() {
            return base.Channel.RegisteringAsync();
        }
        
        public string GetUserData(uint id, SDT_Project.ServiceServer.DataTypes dataType) {
            return base.Channel.GetUserData(id, dataType);
        }
        
        public System.Threading.Tasks.Task<string> GetUserDataAsync(uint id, SDT_Project.ServiceServer.DataTypes dataType) {
            return base.Channel.GetUserDataAsync(id, dataType);
        }
        
        public void ConsoleLog(string msg, SDT_Project.ServiceServer.MessageImportance importance) {
            base.Channel.ConsoleLog(msg, importance);
        }
        
        public System.Threading.Tasks.Task ConsoleLogAsync(string msg, SDT_Project.ServiceServer.MessageImportance importance) {
            return base.Channel.ConsoleLogAsync(msg, importance);
        }
    }
}
