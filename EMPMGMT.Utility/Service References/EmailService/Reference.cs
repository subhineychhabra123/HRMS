//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EMPMGMT.Utility.EmailService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="EmailService.IService1")]
    public interface IService1 {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SendEmail", ReplyAction="http://tempuri.org/IService1/SendEmailResponse")]
        bool SendEmail(string ToEmail, string Subject, string MessageBody, bool IsBodyHtml, string DisplayName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SendEmail", ReplyAction="http://tempuri.org/IService1/SendEmailResponse")]
        System.Threading.Tasks.Task<bool> SendEmailAsync(string ToEmail, string Subject, string MessageBody, bool IsBodyHtml, string DisplayName);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IService1Channel : EMPMGMT.Utility.EmailService.IService1, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class Service1Client : System.ServiceModel.ClientBase<EMPMGMT.Utility.EmailService.IService1>, EMPMGMT.Utility.EmailService.IService1 {
        
        public Service1Client() {
        }
        
        public Service1Client(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public Service1Client(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool SendEmail(string ToEmail, string Subject, string MessageBody, bool IsBodyHtml, string DisplayName) {
            return base.Channel.SendEmail(ToEmail, Subject, MessageBody, IsBodyHtml, DisplayName);
        }
        
        public System.Threading.Tasks.Task<bool> SendEmailAsync(string ToEmail, string Subject, string MessageBody, bool IsBodyHtml, string DisplayName) {
            return base.Channel.SendEmailAsync(ToEmail, Subject, MessageBody, IsBodyHtml, DisplayName);
        }
    }
}
