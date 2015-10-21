using Scribe.Core.ConnectorApi;
using Scribe.Core.ConnectorApi.Actions;
using Scribe.Core.ConnectorApi.ConnectionUI;
using Scribe.Core.ConnectorApi.Logger;
using Scribe.Core.ConnectorApi.Query;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Xml;

namespace MelissaData.Connector.MDServices
{
    [ScribeConnector(
    ConnectorSettings.ConnectorTypeId,
    ConnectorSettings.Name,
    ConnectorSettings.Description,
    typeof(Connector),
    StandardConnectorSettings.SettingsUITypeName,
    StandardConnectorSettings.SettingsUIVersion,
    StandardConnectorSettings.ConnectionUITypeName,
    StandardConnectorSettings.ConnectionUIVersion,
    StandardConnectorSettings.XapFileName,
    new[] { "Scribe.IS2.Target", "Scribe.IS2.Source" },
    ConnectorSettings.SupportsCloud, ConnectorSettings.ConnectorVersion)]
    public class Connector : ISupportProcessNotifications, IConnector
    {
        private string licensekey;
        private string package;
        private string product;
        private int counter;
        private int check;
        private int row;
        private int IntCheck;
        private int IntGeo;
        private int verify;
        private int move;
        private string system;
        private int appendname;
        private int appendaddress;
        private int appendphone;
        private int appendemail;
        private int geo;
        private int GeoBasic;
        private Uri serviceUri;
        private ConcurrentQueue<DataEntity> bulkQueue = new ConcurrentQueue<DataEntity>();

        public bool IsConnected { get; set; }

        //-----------------------------------------------------------------------------------------------------
        public void ProcessEnded(Guid processId, bool success)
        {
            // this.IsConnected = false;
            if (this.geo > 0)
            {
                chargeGEO();
            }
            if (this.check > 0)
            {
                chargeCheck();
            }
            if (this.move > 0)
            {
                chargeMove();
            }
            if (this.verify > 0)
            {
                chargeVerify();
            }
            if ((this.appendaddress > 0) || (this.appendphone > 0) || (this.appendemail > 0) || (this.appendname > 0))
            {
                chargeAppend();
            }
            if (this.IntCheck > 0)
            {
                chargeIntCheck();
            }
            if (this.IntGeo > 0)
            {
                chargeIntGeo();
            }
            if (this.GeoBasic > 0)
            {
                chargeGeoCodeBasic();
            }
        }

        //-----------------------------------------------------------------------------------------------------
        public void ProcessStarted(Guid processId)
        {
        }

        //-----------------------------------------------------------------------------------------------------
        public Guid ConnectorTypeId
        {
            get { return new Guid(ConnectorSettings.ConnectorTypeId); }
        }

        private MetadataProvider metadataProvider;

        //-----------------------------------------------------------------------------------------------------
        public IMetadataProvider GetMetadataProvider()
        {
            this.metadataProvider = new MetadataProvider(serviceUri);
            return this.metadataProvider;
        }

        //-----------------------------------------------------------------------------------------------------
        public string PreConnect(IDictionary<string, string> properties)
        {
            var form = new FormDefinition
            {
                CompanyName = ("Melissa Data. Additional Information : http://www.melissadata.com/dqt/scribe.htm"),
                CryptoKey = "1",
                HelpUri = new Uri("http://wiki.melissadata.com/index.php?title=Data_Quality_Components_for_Scribesoft"),
                Entries =
                new Collection<EntryDefinition>
                {
                     new EntryDefinition
                    {
                    InputType = InputType.Text,
                    IsRequired = true,
                    Label = "Melissa Data License Key",
                    PropertyName = "licensekey"
                    },
                }
            };

            return form.Serialize();
        }

        //-----------------------------------------------------------------------------------------------------
        public static string CreateLicenseRequest(string s)
        {
            string UrlRequest = "https://token.melissadata.net/v3/WEB/service.svc/CheckCredits?L=" +
            HttpUtility.UrlEncode(s) + "&P=2700&K=pkgBorgCheck&q=1";
            return (UrlRequest);
        }

        //-----------------------------------------------------------------------------------------------------
        public string CheckInputSizeRequest(string s)
        {
            string UrlRequest = "https://token.melissadata.net/v3/WEB/service.svc/QueryCustomerInfo?L=" +
            HttpUtility.UrlEncode(s) + "&P=2700&K=pkgBorgCheck";
            return (UrlRequest);
        }

        //-----------------------------------------------------------------------------------------------------
        public void Connect(IDictionary<string, string> properties)
        {
            this.IsConnected = false;
            this.licensekey = properties["licensekey"];
            this.package = "";
            this.product = "";
            this.system = "";
            this.check = 0;
            this.IntCheck = 0;
            this.IntGeo = 0;
            this.move = 0;
            this.verify = 0;
            this.row = 0;
            this.counter = 0;
            this.appendname = 0;
            this.appendaddress = 0;
            this.appendphone = 0;
            this.appendemail = 0;
            this.geo = 0;
            this.GeoBasic = 0;
            this.serviceUri = new Uri(CreateLicenseRequest(licensekey));
            HttpWebRequest myHttpWebRequest = null;
            HttpWebResponse myHttpWebResponse = null;
            XmlDocument myXMLDocument = null;
            XmlTextReader myXMLReader = null;
            myHttpWebRequest = WebRequest.Create(serviceUri) as HttpWebRequest;
            myHttpWebRequest.Timeout = 60000;
            myHttpWebRequest.ReadWriteTimeout = 60000;
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.ContentType = "application/xml; charset=utf-8";
            myHttpWebRequest.Accept = "application/xml";
            myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

            myXMLDocument = new XmlDocument();

            myXMLReader = new XmlTextReader(myHttpWebResponse.GetResponseStream());
            myXMLDocument.Load(myXMLReader);
            XmlNode requestNode = myXMLDocument.DocumentElement.ChildNodes[0];
            if (requestNode.InnerText.Contains("TS01") || requestNode.InnerText.Contains("TS02"))
            {
                if (requestNode.InnerText.Contains("TS02"))
                {
                    this.system = "credit";
                    Logger.Write(Logger.Severity.Info, "Credential Type", "Using Credit System");
                }
                if (requestNode.InnerText.Contains("TS01"))
                {
                    this.system = "subscription";
                    Logger.Write(Logger.Severity.Info, "Credential Type", "Using Subscription System");
                }
                this.IsConnected = true;
                Logger.Write(Logger.Severity.Info, "Connection Results", "Successful");
            }
            else
            {
                if (requestNode.InnerText.Contains("SE01"))
                {
                    this.IsConnected = false;
                    Logger.Write(Logger.Severity.Info, "Connection Results", "SE01 - Internal error.");
                    throw new System.ArgumentException("Connection Results", "SE01 - Internal error.");
                }
                if (requestNode.InnerText.Contains("GE01"))
                {
                    this.IsConnected = false;
                    Logger.Write(Logger.Severity.Info, "Connection Results", "GE01 - Invalid or missing input parameter.");
                    throw new System.ArgumentException("Connection Results", "GE01 - Invalid or missing input parameter.");
                }
                if (requestNode.InnerText.Contains("TE01"))
                {
                    this.IsConnected = false;
                    Logger.Write(Logger.Severity.Info, "Connection Results", "TE01 - Missing customer Id.");
                    throw new System.ArgumentException("Connection Results", "TE01 - Missing customer Id.");
                }
                if (requestNode.InnerText.Contains("TE02"))
                {
                    this.IsConnected = false;
                    Logger.Write(Logger.Severity.Info, "Connection Results", "TE02 - Missing customer Id.");
                    throw new System.ArgumentException("Connection Results", "TE02 - Missing customer Id.");
                }
                if (requestNode.InnerText.Contains("TE03"))
                {
                    this.IsConnected = false;
                    Logger.Write(Logger.Severity.Info, "Connection Results", "TE03 - Customer not found.");
                    throw new System.ArgumentException("Connection Results", "TE03 - Customer not found.");
                }
                if (requestNode.InnerText.Contains("TE04"))
                {
                    this.IsConnected = false;
                    Logger.Write(Logger.Severity.Info, "Connection Results", "TE04 -  The customer is not subscribed to the specified package.");
                    throw new System.ArgumentException("Connection Results", "TE04 -  The customer is not subscribed to the specified package.");
                }
                if (requestNode.InnerText.Contains("TE05"))
                {
                    this.IsConnected = false;
                    Logger.Write(Logger.Severity.Info, "Connection Results", "TE05 - The customer does not have enough credits. Please purchase more credits at http://www.melissadata.com/dqt/credits.htm ");
                    throw new System.ArgumentException("Connection Results", "TE05 - The customer does not have enough credits. Please purchase more credits at http://www.melissadata.com/dqt/credits.htm");
                }
                if (requestNode.InnerText.Contains("TE06"))
                {
                    this.IsConnected = false;
                    Logger.Write(Logger.Severity.Info, "Connection Results", "TE06 - The specified product Id is invalid.");
                    throw new System.ArgumentException("Connection Results", "TE06 - The specified product Id is invalid.");
                }
                if (requestNode.InnerText.Contains(""))
                {
                    this.IsConnected = false;
                    Logger.Write(Logger.Severity.Info, "Connection Results", "Check Connection to Internet.");
                    throw new System.ArgumentException("Connection Results", "Connection Results - Check Connection to Internet.");
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------
        public void Disconnect()
        {
            this.IsConnected = false;
        }

        //-----------------------------------------------------------------------------------------------------
        public MethodResult ExecuteMethod(MethodInput input)
        {
            throw new NotImplementedException();
        }

        //-----------------------------------------------------------------------------------------------------
        public IEnumerable<DataEntity> ExecuteQuery(Query query)
        {
            if ((query.RootEntity.ObjectDefinitionFullName == "BulkPersonator") || query.RootEntity.ObjectDefinitionFullName == "BulkGlobal")
            {
                DataEntity temp;
                while (bulkQueue.TryDequeue(out temp))
                {
                    yield return temp;
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------
        public string RequestToken(string license, string productnum, string packages, string quantity)
        {
            string UrlRequest = "https://token.melissadata.net/v3/WEB/service.svc/CheckCredits?L=" +
            HttpUtility.UrlEncode(license) + "&P=" + productnum + "&K=" + packages + "&Q=" + quantity;
            return (UrlRequest);
        }

        //-----------------------------------------------------------------------------------------------------
        public void chargeGEO()
        {
            System.ServiceModel.EndpointAddress myEndpointAddress = new System.ServiceModel.EndpointAddress("https://token.melissadata.net/v3/SOAP/Service.svc");

            System.ServiceModel.BasicHttpBinding myBinding = new System.ServiceModel.BasicHttpBinding();
            myBinding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;

            SOAP2.ServiceClient client = new SOAP2.ServiceClient(myBinding, myEndpointAddress);

            SOAP2.ConsumeCreditsExRequest request = new SOAP2.ConsumeCreditsExRequest();
            request.License = licensekey;
            request.Source = "2611";
            request.TotalProductRecords = 1;
            request.ConsumeRecord = new SOAP2.ConsumeCreditsRecord[1];
            request.ConsumeRecord[0] = new SOAP2.ConsumeCreditsRecord();
            request.ConsumeRecord[0].Package = "pkgBorgGeoPoint";
            request.ConsumeRecord[0].Product = "2703";

            request.ConsumeRecord[0].Quantity = this.geo;
            client.ConsumeCreditsEx(request);

            if (this.system.Contains("credit"))
            {
                int geoCredit = (Convert.ToInt32(this.geo) * 5);
                string sgeoCredit = geoCredit.ToString();
                Logger.Write(Logger.Severity.Info, "Total Charged GeoCoding/GeoPoints - Consumed Credit: ", sgeoCredit);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        public void chargeCheck()
        {
            System.ServiceModel.EndpointAddress myEndpointAddress = new System.ServiceModel.EndpointAddress("https://token.melissadata.net/v3/SOAP/Service.svc");

            System.ServiceModel.BasicHttpBinding myBinding = new System.ServiceModel.BasicHttpBinding();
            myBinding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;

            SOAP2.ServiceClient client = new SOAP2.ServiceClient(myBinding, myEndpointAddress);
            SOAP2.ConsumeCreditsExRequest request = new SOAP2.ConsumeCreditsExRequest();
            request.License = licensekey;
            request.Source = "2611";
            request.TotalProductRecords = 1;
            request.ConsumeRecord = new SOAP2.ConsumeCreditsRecord[1];
            request.ConsumeRecord[0] = new SOAP2.ConsumeCreditsRecord();
            request.ConsumeRecord[0].Package = "pkgBorgCheck";
            request.ConsumeRecord[0].Product = "2700";
            request.ConsumeRecord[0].Quantity = this.check;
            client.ConsumeCreditsEx(request);

            if (this.system.Contains("credit"))
            {
                int checkCredit = (Convert.ToInt32(this.check) * 1);
                string scheckCredit = checkCredit.ToString();
                Logger.Write(Logger.Severity.Info, "Total Charged Check - Consumed Credit: ", scheckCredit);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        public void chargeVerify()
        {
            System.ServiceModel.EndpointAddress myEndpointAddress = new System.ServiceModel.EndpointAddress("https://token.melissadata.net/v3/SOAP/Service.svc");

            System.ServiceModel.BasicHttpBinding myBinding = new System.ServiceModel.BasicHttpBinding();
            myBinding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;

            SOAP2.ServiceClient client = new SOAP2.ServiceClient(myBinding, myEndpointAddress);

            SOAP2.ConsumeCreditsExRequest request = new SOAP2.ConsumeCreditsExRequest();
            request.License = licensekey;
            request.Source = "2611";
            request.TotalProductRecords = 1;
            request.ConsumeRecord = new SOAP2.ConsumeCreditsRecord[1];
            request.ConsumeRecord[0] = new SOAP2.ConsumeCreditsRecord();
            request.ConsumeRecord[0].Package = "pkgBorgVerify";
            request.ConsumeRecord[0].Product = "2701";
            request.ConsumeRecord[0].Quantity = this.verify;
            client.ConsumeCreditsEx(request);

            if (this.system.Contains("credit"))
            {
                int verifyCredit = (Convert.ToInt32(this.verify) * 1);
                string sverifyCredit = verifyCredit.ToString();
                Logger.Write(Logger.Severity.Info, "Total Charged Verify - Consumed Credit: ", sverifyCredit);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        public void chargeMove()
        {
            System.ServiceModel.EndpointAddress myEndpointAddress = new System.ServiceModel.EndpointAddress("https://token.melissadata.net/v3/SOAP/Service.svc");

            System.ServiceModel.BasicHttpBinding myBinding = new System.ServiceModel.BasicHttpBinding();
            myBinding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;

            SOAP2.ServiceClient client = new SOAP2.ServiceClient(myBinding, myEndpointAddress);

            SOAP2.ConsumeCreditsExRequest request = new SOAP2.ConsumeCreditsExRequest();
            request.License = licensekey;
            request.Source = "2611";
            request.ConsumeRecord = new SOAP2.ConsumeCreditsRecord[1];
            request.ConsumeRecord[0] = new SOAP2.ConsumeCreditsRecord();
            request.ConsumeRecord[0].Package = "pkgBorgMoveUpdate";
            request.ConsumeRecord[0].Product = "2702";
            request.ConsumeRecord[0].Quantity = this.move;
            client.ConsumeCreditsEx(request);

            if (this.system.Contains("credit"))
            {
                int moveCredit = (Convert.ToInt32(this.move) * 5);
                string smoveCredit = moveCredit.ToString();
                Logger.Write(Logger.Severity.Info, "Total Charged Move - Consumed Credit: ", smoveCredit);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        public void chargeAppend()
        {
            System.ServiceModel.EndpointAddress myEndpointAddress = new System.ServiceModel.EndpointAddress("https://token.melissadata.net/v3/SOAP/Service.svc");

            System.ServiceModel.BasicHttpBinding myBinding = new System.ServiceModel.BasicHttpBinding();
            myBinding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;

            SOAP2.ServiceClient client = new SOAP2.ServiceClient(myBinding, myEndpointAddress);

            SOAP2.ConsumeCreditsExRequest request = new SOAP2.ConsumeCreditsExRequest();

            request.License = licensekey;
            request.Source = "2611";
            request.ConsumeRecord = new SOAP2.ConsumeCreditsRecord[4];
            request.ConsumeRecord[0] = new SOAP2.ConsumeCreditsRecord();
            request.ConsumeRecord[0].Package = "pkgBorgAppend";
            request.ConsumeRecord[0].Product = "2704";
            request.ConsumeRecord[0].Quantity = this.appendname;
            request.ConsumeRecord[1] = new SOAP2.ConsumeCreditsRecord();
            request.ConsumeRecord[1].Package = "pkgBorgAppend";
            request.ConsumeRecord[1].Product = "2705";
            request.ConsumeRecord[1].Quantity = this.appendaddress;
            request.ConsumeRecord[2] = new SOAP2.ConsumeCreditsRecord();
            request.ConsumeRecord[2].Package = "pkgBorgAppend";
            request.ConsumeRecord[2].Product = "2706";
            request.ConsumeRecord[2].Quantity = this.appendphone;
            request.ConsumeRecord[3] = new SOAP2.ConsumeCreditsRecord();
            request.ConsumeRecord[3].Package = "pkgBorgAppend";
            request.ConsumeRecord[3].Product = "2707";
            request.ConsumeRecord[3].Quantity = this.appendemail;
            client.ConsumeCreditsEx(request);

            if (this.system.Contains("credit"))
            {
                int appendCredit = (Convert.ToInt32(this.appendname) * 3) + (Convert.ToInt32(this.appendaddress) * 5) + (Convert.ToInt32(this.appendphone) * 5) + (Convert.ToInt32(this.appendemail) * 5);
                string sappendCredit = appendCredit.ToString();
                Logger.Write(Logger.Severity.Info, "Total Charged Append - Consumed Credit: ", sappendCredit);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        public void chargeIntCheck()
        {
            System.ServiceModel.EndpointAddress myEndpointAddress = new System.ServiceModel.EndpointAddress("https://token.melissadata.net/v3/SOAP/Service.svc");

            System.ServiceModel.BasicHttpBinding myBinding = new System.ServiceModel.BasicHttpBinding();
            myBinding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;

            SOAP2.ServiceClient client = new SOAP2.ServiceClient(myBinding, myEndpointAddress);
            SOAP2.ConsumeCreditsExRequest request = new SOAP2.ConsumeCreditsExRequest();
            request.License = licensekey;
            request.Source = "2611";
            request.TotalProductRecords = 1;
            request.ConsumeRecord = new SOAP2.ConsumeCreditsRecord[1];
            request.ConsumeRecord[0] = new SOAP2.ConsumeCreditsRecord();
            request.ConsumeRecord[0].Package = "pkgIntAddressCheck";
            request.ConsumeRecord[0].Product = "2708";
            request.ConsumeRecord[0].Quantity = this.IntCheck;
            client.ConsumeCreditsEx(request);

            if (this.system.Contains("credit"))
            {
                int IntCheckCredit = (Convert.ToInt32(this.IntCheck) * 20);
                string sIntCheckCredit = IntCheckCredit.ToString();
                Logger.Write(Logger.Severity.Info, "Total Charged Int Check - Consumed Credit: ", sIntCheckCredit);
            }
        }

        public void chargeGeoCodeBasic()
        {
            System.ServiceModel.EndpointAddress myEndpointAddress = new System.ServiceModel.EndpointAddress("https://token.melissadata.net/v3/SOAP/Service.svc");

            System.ServiceModel.BasicHttpBinding myBinding = new System.ServiceModel.BasicHttpBinding();
            myBinding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;

            SOAP2.ServiceClient client = new SOAP2.ServiceClient(myBinding, myEndpointAddress);
            SOAP2.ConsumeCreditsExRequest request = new SOAP2.ConsumeCreditsExRequest();
            request.License = licensekey;
            request.Source = "2611";
            request.TotalProductRecords = 1;
            request.ConsumeRecord = new SOAP2.ConsumeCreditsRecord[1];
            request.ConsumeRecord[0] = new SOAP2.ConsumeCreditsRecord();
            request.ConsumeRecord[0].Package = "pkgBorgGeoCode";
            request.ConsumeRecord[0].Product = "2734";
            request.ConsumeRecord[0].Quantity = this.GeoBasic;
            client.ConsumeCreditsEx(request);

            if (this.system.Contains("credit"))
            {
                int geoCredit = (Convert.ToInt32(this.GeoBasic) * 1);
                string sgeoCredit = geoCredit.ToString();
                Logger.Write(Logger.Severity.Info, "Total Charged Geocode Basic - Consumed Credit: ", sgeoCredit);
            }
        }

        public void chargeIntGeo()
        {
            System.ServiceModel.EndpointAddress myEndpointAddress = new System.ServiceModel.EndpointAddress("https://token.melissadata.net/v3/SOAP/Service.svc");

            System.ServiceModel.BasicHttpBinding myBinding = new System.ServiceModel.BasicHttpBinding();
            myBinding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;

            SOAP2.ServiceClient client = new SOAP2.ServiceClient(myBinding, myEndpointAddress);
            SOAP2.ConsumeCreditsExRequest request = new SOAP2.ConsumeCreditsExRequest();
            request.License = licensekey;
            request.Source = "2611";
            request.TotalProductRecords = 1;
            request.ConsumeRecord = new SOAP2.ConsumeCreditsRecord[1];
            request.ConsumeRecord[0] = new SOAP2.ConsumeCreditsRecord();
            request.ConsumeRecord[0].Package = "pkgIntGeoCode";
            request.ConsumeRecord[0].Product = "2709";
            request.ConsumeRecord[0].Quantity = this.IntGeo;
            client.ConsumeCreditsEx(request);

            if (this.system.Contains("credit"))
            {
                int IntGeoCredit = (Convert.ToInt32(this.IntGeo) * 5);
                string sIntGeoCredit = IntGeoCredit.ToString();
                Logger.Write(Logger.Severity.Info, "Total Charged Int Geo - Consumed Credit: ", sIntGeoCredit);
            }
        }

        public OperationResult ExecuteOperation(OperationInput input)
        {
            this.product = "2700,2701,2702,2703,2704,2705,2706,2707,2708,2709,2734";
            this.package = "pkgBorgCheck,pkgBorgVerify,pkgBorgMoveUpdate,pkgBorgGeoPoint,pkgBorgAppend,pkgBorgAppend,pkgBorgAppend,pkgBorgAppend,pkgBorgGeoCode,pkgIntAddressCheck,pkgIntGeoCode";

            if (this.system.Contains("credit"))
            {
                this.serviceUri = new Uri(CheckInputSizeRequest(licensekey));
                HttpWebRequest mykHttpWebRequest = null;
                HttpWebResponse mykHttpWebResponse = null;
                XmlDocument mykXMLDocument = null;
                XmlTextReader mykXMLReader = null;
                mykHttpWebRequest = WebRequest.Create(serviceUri) as HttpWebRequest;
                mykHttpWebRequest.Timeout = 60000;
                mykHttpWebRequest.ReadWriteTimeout = 60000;
                mykHttpWebRequest.Method = "GET";
                mykHttpWebRequest.ContentType = "application/xml; charset=utf-8";
                mykHttpWebRequest.Accept = "application/xml";

                bool calloutSuccess = false;
                while (!calloutSuccess)
                {
                    try
                    {
                        mykHttpWebResponse = (HttpWebResponse)mykHttpWebRequest.GetResponse();
                        calloutSuccess = true;
                    }
                    catch (WebException e)
                    {
                        calloutSuccess = false;
                    }
                }

                mykXMLDocument = new XmlDocument();
                mykXMLReader = new XmlTextReader(mykHttpWebResponse.GetResponseStream());
                mykXMLDocument.Load(mykXMLReader);
                XmlNode krequestNode = mykXMLDocument.DocumentElement.ChildNodes[2];
                int creditbalance;

                try
                {
                    if (input.Input[0].ObjectDefinitionFullName.Contains("Personator"))
                    {
                        this.row++;
                        this.counter++;
                        creditbalance = int.Parse(krequestNode.InnerText);
                    }
                    else
                    {
                        this.row++;
                        this.counter = this.counter + 20;
                        creditbalance = int.Parse(krequestNode.InnerText);
                    }
                }
                catch
                {
                    Logger.Write(Logger.Severity.Info, "Data Results", "Empty List");
                    Scribe.Core.ConnectorApi.Exceptions.InvalidExecuteOperationException.ReferenceEquals("Data Results", "Empty List").ToString();
                    throw new System.ArgumentException("Data Results", "Empty List");
                }

                if (creditbalance > this.counter)
                {
                    this.IsConnected = true;
                    Logger.Write(Logger.Severity.Info, "Credit Check Good", "Successful! Credit Balance: " + creditbalance + "On row: " + this.row);
                }
                else
                {
                    if (this.geo > 0)
                    {
                        chargeGEO();
                    }
                    if (this.IntGeo > 0)
                    {
                        chargeIntGeo();
                    }
                    if (this.IntCheck > 0)
                    {
                        chargeIntCheck();
                    }
                    if (this.check > 0)
                    {
                        chargeCheck();
                    }
                    if (this.move > 0)
                    {
                        chargeMove();
                    }
                    if (this.verify > 0)
                    {
                        chargeVerify();
                    }
                    if (this.GeoBasic > 0)
                    {
                        chargeGeoCodeBasic();
                    }
                    if ((this.appendaddress > 0) || (this.appendphone > 0) || (this.appendemail > 0) || (this.appendname > 0))
                    {
                        chargeAppend();
                    }

                    Logger.Write(Logger.Severity.Info, "Process Exception", " Not enough Credits for row: " + this.row + " Estimated Balance: " + (creditbalance - this.counter) + ", Please purchase more credits at http://www.melissadata.com/dqt/credits.htm");
                    this.IsConnected = false;
                    throw new Scribe.Core.ConnectorApi.Exceptions.FatalErrorException("Processing Exception: Not enough Credits for row: " + this.row + " Estimated Balance: " + (creditbalance - this.counter) + ", Please purchase more credits at http://www.melissadata.com/dqt/credits.htm");
                }
            }

            ErrorResult[] ErrorResultArray = new ErrorResult[input.Input.Length];
            int[] ObjectsAffectedArray = new int[input.Input.Length];
            bool[] SuccessArray = new bool[input.Input.Length];
            var outputEntityArray = new DataEntity[input.Input.Length];

            if (input.Input[0].ObjectDefinitionFullName.Contains("Personator") || input.Input[0].ObjectDefinitionFullName.Contains("BulkPersonator"))
            {
                System.ServiceModel.BasicHttpBinding myBinding = new System.ServiceModel.BasicHttpBinding();
                myBinding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                myBinding.MaxReceivedMessageSize = 900000000;
                System.ServiceModel.EndpointAddress myEndpointAddress = new System.ServiceModel.EndpointAddress("https://personator.melissadata.net/v3/SOAP/ContactVerify");
                SOAP.ServicemdContactVerifySOAPClient client = new SOAP.ServicemdContactVerifySOAPClient(myBinding, myEndpointAddress);
                SOAP.Request request = new SOAP.Request();
                request.CustomerID = licensekey;
                int numLoops = 0;
                if ((input.Input.Length % 100) == 0)
                {
                    numLoops = (int)(input.Input.Length / 100);
                }
                else
                {
                    numLoops = (int)(input.Input.Length / 100) + 1;
                }

                for (int i = 0; i < numLoops; i++)
                {
                    DataEntity data = null;
                    int floorDiv = (int)(input.Input.Length / 100);
                    int innerCount = 0;
                    request.Records = new SOAP.RequestRecord[((i < floorDiv) ? 100 : input.Input.Length % 100)];

                    for (int j = 0; j < ((i < floorDiv) ? 100 : input.Input.Length % 100); j++)
                    {
                        request.Records[j] = new SOAP.RequestRecord();
                        data = input.Input[innerCount];

                        if (data.Properties.ContainsKey("Input_RecordID"))
                        {
                            if (data.Properties["Input_RecordID"] == null)
                            {
                                request.Records[j].RecordID = "";
                            }
                            else
                            {
                                request.Records[j].RecordID = data.Properties["Input_RecordID"].ToString();
                            }
                        }
                        if (data.Properties.ContainsKey("Input_City"))
                        {
                            if (data.Properties["Input_City"] == null)
                            {
                                request.Records[j].City = "";
                            }
                            else
                            {
                                request.Records[j].City = data.Properties["Input_City"].ToString();
                            }
                        }
                        if (data.Properties.ContainsKey("Input_State"))
                        {
                            if (data.Properties["Input_State"] == null)
                            {
                                request.Records[j].State = "";
                            }
                            else
                            {
                                request.Records[j].State = data.Properties["Input_State"].ToString();
                            }
                        }
                        if (data.Properties.ContainsKey("Input_PostalCode"))
                        {
                            if (data.Properties["Input_PostalCode"] == null)
                            {
                                request.Records[j].PostalCode = "";
                            }
                            else
                            {
                                request.Records[j].PostalCode = data.Properties["Input_PostalCode"].ToString();
                            }
                        }

                        if ((data.Properties.ContainsKey("Setting_Columns")))
                        {
                            string Columns = data.Properties["Setting_Columns"].ToString();
                            string[] cols;
                            char[] charcolSeparators = new char[] { ',' };
                            cols = Columns.Split(charcolSeparators, 8, StringSplitOptions.RemoveEmptyEntries);
                            for (int c = 0; c < cols.Length; c++)
                            {
                                if ((data.Properties.ContainsKey("Setting_Columns")) && !cols[c].ToString().Trim().ToLower().Equals("grpaddressdetails") && (!cols[c].ToString().Trim().ToLower().Equals("grpcensus")) && (!cols[c].ToString().Trim().ToLower().Equals("grpgeocode")) && (!cols[c].ToString().Trim().ToLower().Equals("grpnamedetails")) && (!cols[c].ToString().Trim().ToLower().Equals("grpparsedaddress")) && (!cols[c].ToString().Trim().ToLower().Equals("grpparsedemail")) && (!cols[c].ToString().Trim().ToLower().Equals("grpparsedphone")) && (!cols[c].ToString().Trim().ToLower().Equals("grpall")) && (!cols[c].ToString().Trim().ToLower().Equals("grpdemographicbasic")))
                                {
                                    throw new Scribe.Core.ConnectorApi.Exceptions.InvalidExecuteOperationException("Invalid Action, action must be either ({blank}, GrpAddressDetails, GrpCensus, GrpGeocode, GrpNameDetails, GrpParsedAddress, GrpParsedEmail, GrpParsedPhone, GrpAll");
                                }
                                else
                                {
                                    request.Columns = (!data.Properties.ContainsKey("Setting_Columns") ? "" : data.Properties["Setting_Columns"].ToString());
                                }
                            }
                        }

                        if (data.Properties.ContainsKey("Setting_Actions"))
                        {
                            if (data.Properties["Setting_Actions"] == null)
                            {
                                request.Actions = "check";
                            }
                            else
                            {
                                request.Actions = data.Properties["Setting_Actions"].ToString();
                            }
                        }
                        else
                        {
                            request.Actions = "check";
                        }

                        if ((data.Properties.ContainsKey("Setting_Actions")))
                        {
                            string Actions = data.Properties["Setting_Actions"].ToString();
                            string[] act;
                            char[] charSeparators = new char[] { ',' };
                            act = Actions.Split(charSeparators, 4, StringSplitOptions.RemoveEmptyEntries);
                            for (int k = 0; k < act.Length; k++)
                            {
                                if ((data.Properties.ContainsKey("Setting_Actions")) && !act[k].ToString().Trim().ToLower().Equals("append") && (!act[k].ToString().Trim().ToLower().Equals("check")) && (!act[k].ToString().Trim().ToLower().Equals("verify")) && (!act[k].ToString().Trim().ToLower().Equals("move")))
                                {
                                    throw new Scribe.Core.ConnectorApi.Exceptions.InvalidExecuteOperationException("Invalid Action, action must be either (Check, Verify, Append, Move");
                                }
                                else
                                {
                                    request.Actions = (!data.Properties.ContainsKey("Setting_Actions") ? "Check" : data.Properties["Setting_Actions"].ToString());
                                }
                            }
                        }

                        if ((data.Properties.ContainsKey("Setting_AdvancedAddressCorrection")) && (!data.Properties["Setting_AdvancedAddressCorrection"].ToString().Trim().ToLower().Equals("on") && (!data.Properties["Setting_AdvancedAddressCorrection"].ToString().Trim().ToLower().Equals("off"))))
                        {
                            throw new Scribe.Core.ConnectorApi.Exceptions.InvalidExecuteOperationException("Invalid AdvancedAddressCorrection Property, action must be either (on, off");
                        }
                        else
                        {
                            request.Options = request.Options + "AdvancedAddressCorrection:" + (!data.Properties.ContainsKey("Setting_AdvancedAddressCorrection") ? "off;" : data.Properties["Setting_AdvancedAddressCorrection"].ToString() + ";");
                        }

                        if ((data.Properties.ContainsKey("Setting_CentricHint")) && (!data.Properties["Setting_CentricHint"].ToString().Trim().ToLower().Equals("auto")) && (!data.Properties["Setting_CentricHint"].ToString().Trim().ToLower().Equals("address")) && (!data.Properties["Setting_CentricHint"].ToString().Trim().ToLower().Equals("phone")) && (!data.Properties["Setting_CentricHint"].ToString().Trim().ToLower().Equals("email")))
                        {
                            throw new Scribe.Core.ConnectorApi.Exceptions.InvalidExecuteOperationException("Invalid CentricHint Property, action must be either (auto, address, phone, email");
                        }
                        else
                        {
                            request.Options = request.Options + "CentricHint:" + (!data.Properties.ContainsKey("Setting_CentricHint") ? "off;" : data.Properties["Setting_CentricHint"].ToString() + ";");
                        }
                        if ((data.Properties.ContainsKey("Setting_Append")) && (!data.Properties["Setting_Append"].ToString().Trim().ToLower().Equals("blank")) && (!data.Properties["Setting_Append"].ToString().Trim().ToLower().Equals("checkerror")) && (!data.Properties["Setting_Append"].ToString().Trim().ToLower().Equals("always")))
                        {
                            throw new Scribe.Core.ConnectorApi.Exceptions.InvalidExecuteOperationException("Invalid Append Property, action must be either (checkerror, blank, always");
                        }
                        else
                        {
                            request.Options = request.Options + "Append:" + (!data.Properties.ContainsKey("Setting_Append") ? "Always;" : data.Properties["Setting_Append"].ToString() + ";");
                        }

                        if ((data.Properties.ContainsKey("Setting_UsePreferredCity")) && (!data.Properties["Setting_UsePreferredCity"].ToString().Trim().ToLower().Equals("on")) && (!data.Properties["Setting_UsePreferredCity"].ToString().Trim().ToLower().Equals("off")))
                        {
                            throw new Scribe.Core.ConnectorApi.Exceptions.InvalidExecuteOperationException("Invalid Append Property, action must be either (off, on)");
                        }
                        else
                        {
                            request.Options = request.Options + "UsePreferredCity:" + (!data.Properties.ContainsKey("Setting_UsePreferredCity") ? "off;" : data.Properties["Setting_UsePreferredCity"].ToString() + ";");
                        }

                        if (data.Properties.ContainsKey("Input_AddressLine2"))
                        {
                            if (data.Properties["Input_AddressLine2"] == null)
                            {
                                request.Records[j].AddressLine2 = "";
                            }
                            else
                            {
                                request.Records[j].AddressLine2 = data.Properties["Input_AddressLine2"].ToString();
                            }
                        }
                        if (data.Properties.ContainsKey("Input_Country"))
                        {
                            if (data.Properties["Input_Country"] == null)
                            {
                                request.Records[j].Country = "";
                            }
                            else
                            {
                                request.Records[j].Country = data.Properties["Input_Country"].ToString();
                            }
                        }
                        if (data.Properties.ContainsKey("Input_CompanyName"))
                        {
                            if (data.Properties["Input_CompanyName"] == null)
                            {
                                request.Records[j].CompanyName = "";
                            }
                            else
                            {
                                request.Records[j].CompanyName = data.Properties["Input_CompanyName"].ToString();
                            }
                        }
                        if (data.Properties.ContainsKey("Input_FullName"))
                        {
                            if (data.Properties["Input_FullName"] == null)
                            {
                                request.Records[j].FullName = "";
                            }
                            else
                            {
                                request.Records[j].FullName = data.Properties["Input_FullName"].ToString();
                            }
                        }
                        if (data.Properties.ContainsKey("Input_LastName"))
                        {
                            if (data.Properties["Input_LastName"] == null)
                            {
                                request.Records[j].LastName = "";
                            }
                            else
                            {
                                request.Records[j].LastName = data.Properties["Input_LastName"].ToString();
                            }
                        }
                        if (data.Properties.ContainsKey("Input_PhoneNumber"))
                        {
                            if (data.Properties["Input_PhoneNumber"] == null)
                            {
                                request.Records[j].PhoneNumber = "";
                            }
                            else
                            {
                                request.Records[j].PhoneNumber = data.Properties["Input_PhoneNumber"].ToString();
                            }
                        }

                        if (data.Properties.ContainsKey("Input_AddressLine1"))
                        {
                            if (data.Properties["Input_AddressLine1"] == null)
                            {
                                request.Records[j].AddressLine1 = "";
                            }
                            else
                            {
                                request.Records[j].City = request.Records[j].City != null ? request.Records[j].City.Trim() : "";
                                request.Records[j].State = request.Records[j].State != null ? request.Records[j].State.Trim() : "";
                                request.Records[j].PostalCode = request.Records[j].PostalCode != null ? request.Records[j].PostalCode.Trim() : "";
                                request.Records[j].FreeForm = request.Records[j].FreeForm != null ? request.Records[j].FreeForm.Trim() : "";

                                if (request.Records[j].City == "" && request.Records[j].State == "" && request.Records[j].PostalCode == "" && request.Records[j].FreeForm == "")
                                {
                                    request.Records[j].AddressLine1 = "";
                                    request.Records[j].FreeForm = data.Properties["Input_AddressLine1"].ToString();
                                }
                                else
                                {
                                    request.Records[j].AddressLine1 = data.Properties["Input_AddressLine1"].ToString();
                                }
                            }
                        }

                        if (data.Properties.ContainsKey("Input_FreeForm"))
                        {
                            if (data.Properties["Input_FreeForm"] == null)
                            {
                                request.Records[j].FreeForm = "";
                            }
                            else
                            {
                                request.Records[j].AddressLine1 = "";
                                request.Records[j].FreeForm = data.Properties["Input_FreeForm"].ToString();
                            }
                        }
                        if (data.Properties.ContainsKey("Input_EmailAddress"))
                        {
                            if (data.Properties["Input_EmailAddress"] == null)
                            {
                                request.Records[j].EmailAddress = "";
                            }
                            else
                            {
                                request.Records[j].EmailAddress = data.Properties["Input_EmailAddress"].ToString();
                            }
                        }

                        if (data.Properties.ContainsKey("Setting_Columns"))
                        {
                            if (data.Properties["Setting_Columns"] == null)
                            {
                                request.Columns = "";
                            }
                            else
                            {
                                request.Columns = data.Properties["Setting_Columns"].ToString();
                            }
                        }

                        if (request.Records[j].PostalCode == "" && request.Records[j].State == "" && request.Records[j].City == "" && request.Records[j].FreeForm == "")
                        {
                            throw new Scribe.Core.ConnectorApi.Exceptions.InvalidExecuteOperationException("City, State or Zipcode or Freeform required.");
                        }

                        innerCount++;
                    }

                    this.serviceUri = new Uri(RequestToken(licensekey, this.product, this.package, ((i < floorDiv) ? 100 : input.Input.Length % 100).ToString()));
                    HttpWebRequest myeHttpWebRequest = null;
                    HttpWebResponse myeHttpWebResponse = null;
                    XmlDocument myXMLDocument = null;
                    XmlTextReader myXMLReader = null;
                    myeHttpWebRequest = WebRequest.Create(serviceUri) as HttpWebRequest;
                    myeHttpWebRequest.Timeout = 60000;
                    myeHttpWebRequest.ReadWriteTimeout = 60000;
                    myeHttpWebRequest.Method = "GET";
                    myeHttpWebRequest.ContentType = "application/xml; charset=utf-8";
                    myeHttpWebRequest.Accept = "application/xml";

                    bool success = false;
                    while (!success)
                    {
                        try
                        {
                            myeHttpWebResponse = (HttpWebResponse)myeHttpWebRequest.GetResponse();
                            success = true;
                        }
                        catch (WebException e)
                        {
                            success = false;
                        }
                    }

                    myXMLDocument = new XmlDocument();

                    myXMLReader = new XmlTextReader(myeHttpWebResponse.GetResponseStream());
                    myXMLDocument.Load(myXMLReader);
                    XmlNode resultNode = myXMLDocument.DocumentElement.ChildNodes[0];
                    XmlNode tokenNode = myXMLDocument.DocumentElement.ChildNodes[1];
                    if (resultNode.InnerText.Contains("TS01") || resultNode.InnerText.Contains("TS02"))
                    {
                        request.CustomerID = tokenNode.InnerText;
                        Logger.Write(Logger.Severity.Info, "Connection Results", "Token Successful");
                    }
                    else
                    {
                        Logger.Write(Logger.Severity.Info, "Connection Results", "Token Successful");
                    }
                    SOAP.Response response = null;

                    while (response == null)
                    {
                        try
                        {
                            response = client.doContactVerify(request);
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    for (int l = 0; l < response.Records.Length; l++)
                    {
                        List<PropertyInfo> responseProperties = response.Records[l].GetType().GetProperties().ToList();

                        var output = new DataEntity(input.Input[100 * i + l].ObjectDefinitionFullName);
                        if ((response.Records[l].Results.Contains("AS")))
                        {
                            this.check++;
                        }
                        if (response.Records[l].Results.Contains("GS05") || response.Records[l].Results.Contains("GS06"))
                        {
                            this.geo++;
                        }
                        if (response.Records[l].Results.Contains("GS01"))
                        {
                            this.GeoBasic++;
                        }
                        if (response.Records[l].Results.Contains("VR"))
                        {
                            this.verify++;
                        }
                        if (response.Records[l].Results.Contains("AS12"))
                        {
                            this.move++;
                        }
                        if (response.Records[l].Results.Contains("DA10"))
                        {
                            this.appendname++;
                        }
                        if (response.Records[l].Results.Contains("DA00"))
                        {
                            this.appendaddress++;
                        }
                        if (response.Records[l].Results.Contains("DA30"))
                        {
                            this.appendphone++;
                        }
                        if (response.Records[l].Results.Contains("DA40"))
                        {
                            this.appendemail++;
                        }
                        foreach (PropertyInfo property in responseProperties)
                        {
                            string getValue = property.GetValue(response.Records[l], null) != null ? property.GetValue(response.Records[l], null).ToString() : "";

                            output.Properties["MD_" + property.Name] = getValue;
                        }

                        outputEntityArray[i * 100 + l] = output;
                    }
                }

                ErrorResultArray = new ErrorResult[input.Input.Length];
                ObjectsAffectedArray = new int[input.Input.Length];
                SuccessArray = new bool[input.Input.Length];

                for (int x = 0; x < input.Input.Length; x++)
                {
                    ErrorResultArray[x] = new ErrorResult();
                    ObjectsAffectedArray[x] = 1;
                    SuccessArray[x] = true;
                }
            }

            if (input.Input[0].ObjectDefinitionFullName.Contains("Global_Address_Verification") || input.Input[0].ObjectDefinitionFullName.Contains("BulkGlobal"))
            {
                System.ServiceModel.BasicHttpBinding myBinding = new System.ServiceModel.BasicHttpBinding();
                myBinding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                myBinding.MaxReceivedMessageSize = 900000000;
                System.ServiceModel.EndpointAddress myEndpointAddress = new System.ServiceModel.EndpointAddress("https://address.melissadata.net/v3/SOAP/GlobalAddress");
                SOAP3.AddressCheckSoapClient client = new SOAP3.AddressCheckSoapClient(myBinding, myEndpointAddress);
                SOAP3.Request request = new SOAP3.Request();
                request.CustomerID = licensekey;
                int numLoops = 0;
                if ((input.Input.Length % 100) == 0)
                {
                    numLoops = (int)(input.Input.Length / 100);
                }
                else
                {
                    numLoops = (int)(input.Input.Length / 100) + 1;
                }

                for (int i = 0; i < numLoops; i++)
                {
                    DataEntity data = null;
                    int floorDiv = (int)(input.Input.Length / 100);
                    int innerCount = 0;
                    request.Records = new SOAP3.RequestRecord[((i < floorDiv) ? 100 : input.Input.Length % 100)];
                    for (int j = 0; j < ((i < floorDiv) ? 100 : input.Input.Length % 100); j++)
                    {
                        request.Records[j] = new SOAP3.RequestRecord();
                        data = input.Input[innerCount];

                        if (data.Properties.ContainsKey("Input_RecordID"))
                        {
                            if (data.Properties["Input_RecordID"] == null)
                            {
                                request.Records[j].RecordID = "";
                            }
                            else
                            {
                                request.Records[j].RecordID = data.Properties["Input_RecordID"].ToString();
                            }
                        }
                        if (data.Properties.ContainsKey("Input_Organization"))
                        {
                            if (data.Properties["Input_Organization"] == null)
                            {
                                request.Records[j].Organization = "";
                            }
                            else
                            {
                                request.Records[j].Organization = data.Properties["Input_Organization"].ToString();
                            }
                        }

                        if (data.Properties.ContainsKey("Input_AddressLine1"))
                        {
                            if (data.Properties["Input_AddressLine1"] == null)
                            {
                                request.Records[j].AddressLine1 = "";
                            }
                            else
                            {
                                request.Records[j].AddressLine1 = data.Properties["Input_AddressLine1"].ToString();
                            }
                        }
                        if (data.Properties.ContainsKey("Input_AddressLine2"))
                        {
                            if (data.Properties["Input_AddressLine2"] == null)
                            {
                                request.Records[j].AddressLine2 = "";
                            }
                            else
                            {
                                request.Records[j].AddressLine2 = data.Properties["Input_AddressLine2"].ToString();
                            }
                        }
                        if (data.Properties.ContainsKey("Input_AddressLine3"))
                        {
                            if (data.Properties["Input_AddressLine3"] == null)
                            {
                                request.Records[j].AddressLine3 = "";
                            }
                            else
                            {
                                request.Records[j].AddressLine3 = data.Properties["Input_AddressLine3"].ToString();
                            }
                        }

                        if (data.Properties.ContainsKey("Input_AddressLine4"))
                        {
                            if (data.Properties["Input_AddressLine4"] == null)
                            {
                                request.Records[j].AddressLine4 = "";
                            }
                            else
                            {
                                request.Records[j].AddressLine4 = data.Properties["Input_AddressLine4"].ToString();
                            }
                        }
                        if (data.Properties.ContainsKey("Input_AddressLine5"))
                        {
                            if (data.Properties["Input_AddressLine5"] == null)
                            {
                                request.Records[j].AddressLine5 = "";
                            }
                            else
                            {
                                request.Records[j].AddressLine5 = data.Properties["Input_AddressLine5"].ToString();
                            }
                        }
                        if (data.Properties.ContainsKey("Input_AddressLine6"))
                        {
                            if (data.Properties["Input_AddressLine6"] == null)
                            {
                                request.Records[j].AddressLine6 = "";
                            }
                            else
                            {
                                request.Records[j].AddressLine6 = data.Properties["Input_AddressLine6"].ToString();
                            }
                        }
                        if (data.Properties.ContainsKey("Input_AddressLine7"))
                        {
                            if (data.Properties["Input_AddressLine7"] == null)
                            {
                                request.Records[j].AddressLine7 = "";
                            }
                            else
                            {
                                request.Records[j].AddressLine7 = data.Properties["Input_AddressLine7"].ToString();
                            }
                        }
                        if (data.Properties.ContainsKey("Input_AddressLine8"))
                        {
                            if (data.Properties["Input_AddressLine8"] == null)
                            {
                                request.Records[j].AddressLine8 = "";
                            }
                            else
                            {
                                request.Records[j].AddressLine8 = data.Properties["Input_AddressLine8"].ToString();
                            }
                        }
                        if (data.Properties.ContainsKey("Input_DoubleDependentLocality"))
                        {
                            if (data.Properties["Input_DoubleDependentLocality"] == null)
                            {
                                request.Records[j].DoubleDependentLocality = "";
                            }
                            else
                            {
                                request.Records[j].DoubleDependentLocality = data.Properties["Input_DoubleDependentLocality"].ToString();
                            }
                        }
                        if (data.Properties.ContainsKey("Input_DependentLocality"))
                        {
                            if (data.Properties["Input_DependentLocality"] == null)
                            {
                                request.Records[j].DependentLocality = "";
                            }
                            else
                            {
                                request.Records[j].DependentLocality = data.Properties["Input_DependentLocality"].ToString();
                            }
                        }

                        if (data.Properties.ContainsKey("Input_Locality"))
                        {
                            if (data.Properties["Input_Locality"] == null)
                            {
                                request.Records[j].Locality = "";
                            }
                            else
                            {
                                request.Records[j].Locality = data.Properties["Input_Locality"].ToString();
                            }
                        }
                        if (data.Properties.ContainsKey("Input_SubAdministrativeArea"))
                        {
                            if (data.Properties["Input_SubAdministrativeArea"] == null)
                            {
                                request.Records[j].SubAdministrativeArea = "";
                            }
                            else
                            {
                                request.Records[j].SubAdministrativeArea = data.Properties["Input_SubAdministrativeArea"].ToString();
                            }
                        }

                        if (data.Properties.ContainsKey("Input_AdministrativeArea"))
                        {
                            if (data.Properties["Input_AdministrativeArea"] == null)
                            {
                                request.Records[j].AdministrativeArea = "";
                            }
                            else
                            {
                                request.Records[j].AdministrativeArea = data.Properties["Input_AdministrativeArea"].ToString();
                            }
                        }
                        if (data.Properties.ContainsKey("Input_PostalCode"))
                        {
                            if (data.Properties["Input_PostalCode"] == null)
                            {
                                request.Records[j].PostalCode = "";
                            }
                            else
                            {
                                request.Records[j].PostalCode = data.Properties["Input_PostalCode"].ToString();
                            }
                        }
                        if (data.Properties.ContainsKey("Input_SubNationalArea"))
                        {
                            if (data.Properties["Input_SubNationalArea"] == null)
                            {
                                request.Records[j].SubNationalArea = "";
                            }
                            else
                            {
                                request.Records[j].SubNationalArea = data.Properties["Input_SubNationalArea"].ToString();
                            }
                        }

                        if (data.Properties.ContainsKey("Input_Country"))
                        {
                            if (data.Properties["Input_Country"] == null)
                            {
                                request.Records[j].Country = "";
                            }
                            else
                            {
                                request.Records[j].Country = data.Properties["Input_Country"].ToString();
                            }
                        }

                        if (data.Properties.ContainsKey("Setting_DeliveryLines"))
                        {
                            if (data.Properties["Setting_DeliveryLines"] == null)
                            {
                                request.Records[j].Country = "";
                            }
                            else
                            {
                                request.Records[j].Country = data.Properties["Setting_DeliveryLines"].ToString();
                            }
                        }

                        if ((data.Properties.ContainsKey("Setting_DeliveryLines")) && (!data.Properties["Setting_DeliveryLines"].ToString().Trim().ToLower().Equals("on") && (!data.Properties["Setting_DeliveryLines"].ToString().Trim().ToLower().Equals("off"))))
                        {
                            throw new Scribe.Core.ConnectorApi.Exceptions.InvalidExecuteOperationException("Invalid DeliveryLines Property, action must be either (on, off");
                        }
                        else
                        {
                            request.Options = request.Options + "DeliveryLines:" + (!data.Properties.ContainsKey("Setting_DeliveryLines") ? "off;" : data.Properties["Setting_DeliveryLines"].ToString() + ";");
                        }

                        if ((data.Properties.ContainsKey("Setting_LineSeparator")) && (!data.Properties["Setting_LineSeparator"].ToString().Trim().ToLower().Equals("semicolon") && (!data.Properties["Setting_DeliveryLines"].ToString().Trim().ToLower().Equals("pipe") && (!data.Properties["Setting_LineSeparator"].ToString().Trim().ToLower().Equals("cr") && (!data.Properties["Setting_LineSeparator"].ToString().Trim().ToLower().Equals("lf") && (!data.Properties["Setting_LineSeparator"].ToString().Trim().ToLower().Equals("crlf") && (!data.Properties["Setting_LineSeparator"].ToString().Trim().ToLower().Equals("tab") && (!data.Properties["Setting_LineSeparator"].ToString().Trim().ToLower().Equals("br")))))))))
                        {
                            throw new Scribe.Core.ConnectorApi.Exceptions.InvalidExecuteOperationException("Invalid LineSeparator Property, action must be either (semicolon (default), pipe, cr, lf, crlf, tab, br (line break)");
                        }
                        else
                        {
                            request.Options = request.Options + "LineSeparator:" + (!data.Properties.ContainsKey("LineSeparator") ? "SEMICOLON;" : data.Properties["Setting_DeliveryLines"].ToString() + ";");
                        }

                        if ((data.Properties.ContainsKey("Setting_OutputScript")) && (!data.Properties["Setting_OutputScript"].ToString().Trim().ToLower().Equals("nochange") && (!data.Properties["Setting_OutputScript"].ToString().Trim().ToLower().Equals("latn")) && (!data.Properties["Setting_OutputScript"].ToString().Trim().ToLower().Equals("native"))))
                        {
                            throw new Scribe.Core.ConnectorApi.Exceptions.InvalidExecuteOperationException("Invalid LineSeparator Property, action must be either (nochange (default), latn, native");
                        }
                        else
                        {
                            request.Options = request.Options + "OutputScript:" + (!data.Properties.ContainsKey("Setting_OutputScript") ? "nochange;" : data.Properties["Setting_OutputScript"].ToString() + ";");
                        }

                        if ((data.Properties.ContainsKey("Setting_CountryOfOrigin")))
                        {
                            request.Options = request.Options + "CountryOfOrigin:" + data.Properties["Setting_CountryOfOrigin"].ToString() + ";";
                        }

                        innerCount++;
                    }
                    this.serviceUri = new Uri(RequestToken(licensekey, this.product, this.package, ((i < floorDiv) ? 100 : input.Input.Length % 100).ToString()));

                    HttpWebRequest myeHttpWebRequest = null;
                    HttpWebResponse myeHttpWebResponse = null;
                    XmlDocument myXMLDocument = null;
                    XmlTextReader myXMLReader = null;
                    myeHttpWebRequest = WebRequest.Create(serviceUri) as HttpWebRequest;
                    myeHttpWebRequest.Timeout = 60000;
                    myeHttpWebRequest.ReadWriteTimeout = 60000;
                    myeHttpWebRequest.Method = "GET";
                    myeHttpWebRequest.ContentType = "application/xml; charset=utf-8";
                    myeHttpWebRequest.Accept = "application/xml";

                    bool success = false;
                    while (!success)
                    {
                        try
                        {
                            myeHttpWebResponse = (HttpWebResponse)myeHttpWebRequest.GetResponse();
                            success = true;
                        }
                        catch (WebException e)
                        {
                            success = false;
                        }
                    }

                    myXMLDocument = new XmlDocument();

                    myXMLReader = new XmlTextReader(myeHttpWebResponse.GetResponseStream());
                    myXMLDocument.Load(myXMLReader);
                    XmlNode resultNode = myXMLDocument.DocumentElement.ChildNodes[0];
                    XmlNode tokenNode = myXMLDocument.DocumentElement.ChildNodes[1];
                    if (resultNode.InnerText.Contains("TS01") || resultNode.InnerText.Contains("TS02"))
                    {
                        request.CustomerID = tokenNode.InnerText;
                        Logger.Write(Logger.Severity.Info, "Connection Results", "Token Successful");
                    }

                    SOAP3.Response response = client.doGlobalAddress(request);

                    for (int l = 0; l < response.Records.Length; l++)
                    {
                        List<PropertyInfo> responseProperties = response.Records[l].GetType().GetProperties().ToList();

                        var output = new DataEntity(input.Input[100 * i + l].ObjectDefinitionFullName);

                        if ((response.Records[l].Results.Contains("AV")))
                        {
                            this.IntCheck++;
                        }
                        if (response.Records[l].Results.Contains("GS"))
                        {
                            this.IntGeo++;
                        }

                        foreach (PropertyInfo property in responseProperties)
                        {
                            string getValue = property.GetValue(response.Records[l], null) != null ? property.GetValue(response.Records[l], null).ToString() : "";

                            output.Properties["MD_" + property.Name] = getValue;
                        }

                        outputEntityArray[i * 100 + l] = output;
                    }
                }

                ErrorResultArray = new ErrorResult[input.Input.Length];
                ObjectsAffectedArray = new int[input.Input.Length];
                SuccessArray = new bool[input.Input.Length];

                for (int x = 0; x < input.Input.Length; x++)
                {
                    ErrorResultArray[x] = new ErrorResult();
                    ObjectsAffectedArray[x] = 1;
                    SuccessArray[x] = true;
                }
            }

            foreach (var entity in outputEntityArray)
            {
                bulkQueue.Enqueue(entity);
            }

            return new OperationResult(input.Input.Length)
            {
                ErrorInfo = ErrorResultArray,
                ObjectsAffected = ObjectsAffectedArray,
                Output = outputEntityArray,
                Success = SuccessArray
            };
        }
    }
}