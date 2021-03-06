﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ElectrosLtdApplication.OrderServ {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="OrderServ.IOrderService")]
    public interface IOrderService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOrderService/GetOrderByPersonId", ReplyAction="http://tempuri.org/IOrderService/GetOrderByPersonIdResponse")]
        Common.Order GetOrderByPersonId(string username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOrderService/GetShippedOrdersByPersonId", ReplyAction="http://tempuri.org/IOrderService/GetShippedOrdersByPersonIdResponse")]
        Common.Order[] GetShippedOrdersByPersonId(string username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOrderService/FilterWithDate", ReplyAction="http://tempuri.org/IOrderService/FilterWithDateResponse")]
        Common.Order[] FilterWithDate(string username, System.DateTime startDate, System.DateTime endDate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOrderService/ConfirmOrder", ReplyAction="http://tempuri.org/IOrderService/ConfirmOrderResponse")]
        void ConfirmOrder(Common.Order order);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOrderService/UpdateOrder", ReplyAction="http://tempuri.org/IOrderService/UpdateOrderResponse")]
        void UpdateOrder(Common.Order o);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOrderService/GetOrderByOrderId", ReplyAction="http://tempuri.org/IOrderService/GetOrderByOrderIdResponse")]
        Common.Order GetOrderByOrderId(int id);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IOrderServiceChannel : ElectrosLtdApplication.OrderServ.IOrderService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class OrderServiceClient : System.ServiceModel.ClientBase<ElectrosLtdApplication.OrderServ.IOrderService>, ElectrosLtdApplication.OrderServ.IOrderService {
        
        public OrderServiceClient() {
        }
        
        public OrderServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public OrderServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public OrderServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public OrderServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Common.Order GetOrderByPersonId(string username) {
            return base.Channel.GetOrderByPersonId(username);
        }
        
        public Common.Order[] GetShippedOrdersByPersonId(string username) {
            return base.Channel.GetShippedOrdersByPersonId(username);
        }
        
        public Common.Order[] FilterWithDate(string username, System.DateTime startDate, System.DateTime endDate) {
            return base.Channel.FilterWithDate(username, startDate, endDate);
        }
        
        public void ConfirmOrder(Common.Order order) {
            base.Channel.ConfirmOrder(order);
        }
        
        public void UpdateOrder(Common.Order o) {
            base.Channel.UpdateOrder(o);
        }
        
        public Common.Order GetOrderByOrderId(int id) {
            return base.Channel.GetOrderByOrderId(id);
        }
    }
}
