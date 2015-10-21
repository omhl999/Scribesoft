using System;

namespace MelissaData.Connector.MDServices
{
    class ServiceContainer
    {
        private Uri serviceUri;
        public bool IgnoreMissingProperties { get; set; }
        public System.Net.NetworkCredential Credentials { get; set; }

        //-----------------------------------------------------------------------------------------------------
        public ServiceContainer(Uri serviceUri)
        {
            this.serviceUri = serviceUri;
        }
    }
}