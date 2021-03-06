﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MelissaData.Connector.MDServices.SOAP2 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="urn:TokenServer", ConfigurationName="SOAP2.IService")]
    public interface IService {
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:TokenServer/IService/QueryVersionInformation", ReplyAction="urn:TokenServer/IService/QueryVersionInformationResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        MelissaData.Connector.MDServices.SOAP2.VersionInfoResponse QueryVersionInformation(string package);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:TokenServer/IService/QueryVersionUpdate", ReplyAction="urn:TokenServer/IService/QueryVersionUpdateResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        MelissaData.Connector.MDServices.SOAP2.VersionUpdateResponse QueryVersionUpdate(string package, string currentVersion);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:TokenServer/IService/RequestFreeToken", ReplyAction="urn:TokenServer/IService/RequestFreeTokenResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        MelissaData.Connector.MDServices.SOAP2.RequestTokenResponse RequestFreeToken(string authorization, int customerId, string package, string timespan);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:TokenServer/IService/RequestToken", ReplyAction="urn:TokenServer/IService/RequestTokenResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        MelissaData.Connector.MDServices.SOAP2.RequestTokenResponse RequestToken(string license, string package, string ipAddress);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:TokenServer/IService/CheckCredits", ReplyAction="urn:TokenServer/IService/CheckCreditsResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        MelissaData.Connector.MDServices.SOAP2.CheckCreditsResponse CheckCredits(string license, string products, string packages, long quantity);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:TokenServer/IService/QueryCustomerInfo", ReplyAction="urn:TokenServer/IService/QueryCustomerInfoResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        MelissaData.Connector.MDServices.SOAP2.QueryCustomerInfoResponse QueryCustomerInfo(string license, string products, string packages);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:TokenServer/IService/ConsumeCredits", ReplyAction="urn:TokenServer/IService/ConsumeCreditsResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        MelissaData.Connector.MDServices.SOAP2.ConsumeCreditsResponse ConsumeCredits(MelissaData.Connector.MDServices.SOAP2.ConsumeCreditsRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:TokenServer/IService/ConsumeCreditsEx", ReplyAction="urn:TokenServer/IService/ConsumeCreditsExResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        MelissaData.Connector.MDServices.SOAP2.ConsumeCreditsResponse ConsumeCreditsEx(MelissaData.Connector.MDServices.SOAP2.ConsumeCreditsExRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:TokenServer/IService/PurchaseCredits", ReplyAction="urn:TokenServer/IService/PurchaseCreditsResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        MelissaData.Connector.MDServices.SOAP2.PurchaseCreditsResponse PurchaseCredits(string authorization, int customerId, long quantity);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:TokenServer")]
    public partial class VersionInfoResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string resultField;
        
        private string versionNumberField;
        
        private string releaseDateField;
        
        private string downloadUrlField;
        
        private string changesUrlField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string Result {
            get {
                return this.resultField;
            }
            set {
                this.resultField = value;
                this.RaisePropertyChanged("Result");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string VersionNumber {
            get {
                return this.versionNumberField;
            }
            set {
                this.versionNumberField = value;
                this.RaisePropertyChanged("VersionNumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string ReleaseDate {
            get {
                return this.releaseDateField;
            }
            set {
                this.releaseDateField = value;
                this.RaisePropertyChanged("ReleaseDate");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string DownloadUrl {
            get {
                return this.downloadUrlField;
            }
            set {
                this.downloadUrlField = value;
                this.RaisePropertyChanged("DownloadUrl");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string ChangesUrl {
            get {
                return this.changesUrlField;
            }
            set {
                this.changesUrlField = value;
                this.RaisePropertyChanged("ChangesUrl");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:TokenServer")]
    public partial class PurchaseCreditsResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string resultField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string Result {
            get {
                return this.resultField;
            }
            set {
                this.resultField = value;
                this.RaisePropertyChanged("Result");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:TokenServer")]
    public partial class ConsumeCreditsExRequest : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string licenseField;
        
        private string sourceField;
        
        private int totalProductRecordsField;
        
        private ConsumeCreditsRecord[] consumeRecordField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string License {
            get {
                return this.licenseField;
            }
            set {
                this.licenseField = value;
                this.RaisePropertyChanged("License");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string Source {
            get {
                return this.sourceField;
            }
            set {
                this.sourceField = value;
                this.RaisePropertyChanged("Source");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public int TotalProductRecords {
            get {
                return this.totalProductRecordsField;
            }
            set {
                this.totalProductRecordsField = value;
                this.RaisePropertyChanged("TotalProductRecords");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ConsumeRecord", Order=3)]
        public ConsumeCreditsRecord[] ConsumeRecord {
            get {
                return this.consumeRecordField;
            }
            set {
                this.consumeRecordField = value;
                this.RaisePropertyChanged("ConsumeRecord");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:TokenServer")]
    public partial class ConsumeCreditsRecord : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string productField;
        
        private string packageField;
        
        private long quantityField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string Product {
            get {
                return this.productField;
            }
            set {
                this.productField = value;
                this.RaisePropertyChanged("Product");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string Package {
            get {
                return this.packageField;
            }
            set {
                this.packageField = value;
                this.RaisePropertyChanged("Package");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public long Quantity {
            get {
                return this.quantityField;
            }
            set {
                this.quantityField = value;
                this.RaisePropertyChanged("Quantity");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:TokenServer")]
    public partial class ConsumeCreditsResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string resultField;
        
        private decimal consumedCreditsField;
        
        private decimal creditBalanceField;
        
        private bool usingAutoRefillField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string Result {
            get {
                return this.resultField;
            }
            set {
                this.resultField = value;
                this.RaisePropertyChanged("Result");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public decimal ConsumedCredits {
            get {
                return this.consumedCreditsField;
            }
            set {
                this.consumedCreditsField = value;
                this.RaisePropertyChanged("ConsumedCredits");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public decimal CreditBalance {
            get {
                return this.creditBalanceField;
            }
            set {
                this.creditBalanceField = value;
                this.RaisePropertyChanged("CreditBalance");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public bool UsingAutoRefill {
            get {
                return this.usingAutoRefillField;
            }
            set {
                this.usingAutoRefillField = value;
                this.RaisePropertyChanged("UsingAutoRefill");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:TokenServer")]
    public partial class ConsumeCreditsRequest : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string licenseField;
        
        private int totalProductRecordsField;
        
        private ConsumeCreditsRecord[] consumeRecordField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string License {
            get {
                return this.licenseField;
            }
            set {
                this.licenseField = value;
                this.RaisePropertyChanged("License");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public int TotalProductRecords {
            get {
                return this.totalProductRecordsField;
            }
            set {
                this.totalProductRecordsField = value;
                this.RaisePropertyChanged("TotalProductRecords");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ConsumeRecord", Order=2)]
        public ConsumeCreditsRecord[] ConsumeRecord {
            get {
                return this.consumeRecordField;
            }
            set {
                this.consumeRecordField = value;
                this.RaisePropertyChanged("ConsumeRecord");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:TokenServer")]
    public partial class CustomerInfoPackageRecord : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string packageField;
        
        private string statusField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string Package {
            get {
                return this.packageField;
            }
            set {
                this.packageField = value;
                this.RaisePropertyChanged("Package");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string Status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
                this.RaisePropertyChanged("Status");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:TokenServer")]
    public partial class CustomerInfoProductRecord : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string productField;
        
        private string descriptionField;
        
        private decimal priceField;
        
        private decimal costField;
        
        private decimal probabilityOfSuccessField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string Product {
            get {
                return this.productField;
            }
            set {
                this.productField = value;
                this.RaisePropertyChanged("Product");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string Description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
                this.RaisePropertyChanged("Description");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public decimal Price {
            get {
                return this.priceField;
            }
            set {
                this.priceField = value;
                this.RaisePropertyChanged("Price");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public decimal Cost {
            get {
                return this.costField;
            }
            set {
                this.costField = value;
                this.RaisePropertyChanged("Cost");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public decimal ProbabilityOfSuccess {
            get {
                return this.probabilityOfSuccessField;
            }
            set {
                this.probabilityOfSuccessField = value;
                this.RaisePropertyChanged("ProbabilityOfSuccess");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:TokenServer")]
    public partial class QueryCustomerInfoResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string resultField;
        
        private string customerIdField;
        
        private long totalCreditsField;
        
        private int totalProductRecordsField;
        
        private int totalPackageRecordsField;
        
        private CustomerInfoProductRecord[] productRecordField;
        
        private CustomerInfoPackageRecord[] packageRecordField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string Result {
            get {
                return this.resultField;
            }
            set {
                this.resultField = value;
                this.RaisePropertyChanged("Result");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string CustomerId {
            get {
                return this.customerIdField;
            }
            set {
                this.customerIdField = value;
                this.RaisePropertyChanged("CustomerId");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public long TotalCredits {
            get {
                return this.totalCreditsField;
            }
            set {
                this.totalCreditsField = value;
                this.RaisePropertyChanged("TotalCredits");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public int TotalProductRecords {
            get {
                return this.totalProductRecordsField;
            }
            set {
                this.totalProductRecordsField = value;
                this.RaisePropertyChanged("TotalProductRecords");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public int TotalPackageRecords {
            get {
                return this.totalPackageRecordsField;
            }
            set {
                this.totalPackageRecordsField = value;
                this.RaisePropertyChanged("TotalPackageRecords");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ProductRecord", Order=5)]
        public CustomerInfoProductRecord[] ProductRecord {
            get {
                return this.productRecordField;
            }
            set {
                this.productRecordField = value;
                this.RaisePropertyChanged("ProductRecord");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PackageRecord", Order=6)]
        public CustomerInfoPackageRecord[] PackageRecord {
            get {
                return this.packageRecordField;
            }
            set {
                this.packageRecordField = value;
                this.RaisePropertyChanged("PackageRecord");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:TokenServer")]
    public partial class CheckCreditsResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string resultField;
        
        private string tokenField;
        
        private decimal estimatedCreditsField;
        
        private bool usingAutoRefillField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string Result {
            get {
                return this.resultField;
            }
            set {
                this.resultField = value;
                this.RaisePropertyChanged("Result");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string Token {
            get {
                return this.tokenField;
            }
            set {
                this.tokenField = value;
                this.RaisePropertyChanged("Token");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public decimal EstimatedCredits {
            get {
                return this.estimatedCreditsField;
            }
            set {
                this.estimatedCreditsField = value;
                this.RaisePropertyChanged("EstimatedCredits");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public bool UsingAutoRefill {
            get {
                return this.usingAutoRefillField;
            }
            set {
                this.usingAutoRefillField = value;
                this.RaisePropertyChanged("UsingAutoRefill");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:TokenServer")]
    public partial class RequestTokenResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string resultField;
        
        private string tokenField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string Result {
            get {
                return this.resultField;
            }
            set {
                this.resultField = value;
                this.RaisePropertyChanged("Result");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string Token {
            get {
                return this.tokenField;
            }
            set {
                this.tokenField = value;
                this.RaisePropertyChanged("Token");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:TokenServer")]
    public partial class VersionUpdateResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string resultField;
        
        private string versionNumberField;
        
        private string releaseDateField;
        
        private string downloadUrlField;
        
        private string changesUrlField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string Result {
            get {
                return this.resultField;
            }
            set {
                this.resultField = value;
                this.RaisePropertyChanged("Result");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string VersionNumber {
            get {
                return this.versionNumberField;
            }
            set {
                this.versionNumberField = value;
                this.RaisePropertyChanged("VersionNumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string ReleaseDate {
            get {
                return this.releaseDateField;
            }
            set {
                this.releaseDateField = value;
                this.RaisePropertyChanged("ReleaseDate");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string DownloadUrl {
            get {
                return this.downloadUrlField;
            }
            set {
                this.downloadUrlField = value;
                this.RaisePropertyChanged("DownloadUrl");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string ChangesUrl {
            get {
                return this.changesUrlField;
            }
            set {
                this.changesUrlField = value;
                this.RaisePropertyChanged("ChangesUrl");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceChannel : MelissaData.Connector.MDServices.SOAP2.IService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceClient : System.ServiceModel.ClientBase<MelissaData.Connector.MDServices.SOAP2.IService>, MelissaData.Connector.MDServices.SOAP2.IService {
        
        public ServiceClient() {
        }
        
        public ServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public MelissaData.Connector.MDServices.SOAP2.VersionInfoResponse QueryVersionInformation(string package) {
            return base.Channel.QueryVersionInformation(package);
        }
        
        public MelissaData.Connector.MDServices.SOAP2.VersionUpdateResponse QueryVersionUpdate(string package, string currentVersion) {
            return base.Channel.QueryVersionUpdate(package, currentVersion);
        }
        
        public MelissaData.Connector.MDServices.SOAP2.RequestTokenResponse RequestFreeToken(string authorization, int customerId, string package, string timespan) {
            return base.Channel.RequestFreeToken(authorization, customerId, package, timespan);
        }
        
        public MelissaData.Connector.MDServices.SOAP2.RequestTokenResponse RequestToken(string license, string package, string ipAddress) {
            return base.Channel.RequestToken(license, package, ipAddress);
        }
        
        public MelissaData.Connector.MDServices.SOAP2.CheckCreditsResponse CheckCredits(string license, string products, string packages, long quantity) {
            return base.Channel.CheckCredits(license, products, packages, quantity);
        }
        
        public MelissaData.Connector.MDServices.SOAP2.QueryCustomerInfoResponse QueryCustomerInfo(string license, string products, string packages) {
            return base.Channel.QueryCustomerInfo(license, products, packages);
        }
        
        public MelissaData.Connector.MDServices.SOAP2.ConsumeCreditsResponse ConsumeCredits(MelissaData.Connector.MDServices.SOAP2.ConsumeCreditsRequest request) {
            return base.Channel.ConsumeCredits(request);
        }
        
        public MelissaData.Connector.MDServices.SOAP2.ConsumeCreditsResponse ConsumeCreditsEx(MelissaData.Connector.MDServices.SOAP2.ConsumeCreditsExRequest request) {
            return base.Channel.ConsumeCreditsEx(request);
        }
        
        public MelissaData.Connector.MDServices.SOAP2.PurchaseCreditsResponse PurchaseCredits(string authorization, int customerId, long quantity) {
            return base.Channel.PurchaseCredits(authorization, customerId, quantity);
        }
    }
}
