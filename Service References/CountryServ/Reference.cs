﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ElectrosLtdApplication.CountryServ {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="CountryServ.ICountryService")]
    public interface ICountryService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICountryService/GetAllCountries", ReplyAction="http://tempuri.org/ICountryService/GetAllCountriesResponse")]
        Common.Country[] GetAllCountries();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICountryServiceChannel : ElectrosLtdApplication.CountryServ.ICountryService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CountryServiceClient : System.ServiceModel.ClientBase<ElectrosLtdApplication.CountryServ.ICountryService>, ElectrosLtdApplication.CountryServ.ICountryService {
        
        public CountryServiceClient() {
        }
        
        public CountryServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CountryServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CountryServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CountryServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Common.Country[] GetAllCountries() {
            return base.Channel.GetAllCountries();
        }
    }
}
