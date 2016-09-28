namespace PopulateWMIInfo.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management;

    using Constants;

    using Entities;

    using Helpers;

    using Newtonsoft.Json.Linq;

    using PopulateWMIInfo.Contracts;

    using ReportToRestEndpoint.Contracts;

    /// <summary>
    /// The port connector information.
    /// </summary>
    public class PortConnector : IWmiInfo
    {
        private const string PortConnectorTableName = "port";

        private const string ClientPortConnectorTableName = "x_client_port";

        private ManagementObjectSearcher searcher;

        private List<PortConnectorInfo> portConnectorInfoList;

        /// <summary>
        /// Initializes a new instance of the <see cref="PortConnector"/> class.
        /// </summary>
        public PortConnector()
        {
            this.portConnectorInfoList = new List<PortConnectorInfo>();
            this.searcher = new ManagementObjectSearcher(
                WmiConstants.WmiRootNamespace,
                string.Format(
                    "SELECT {0}, {1}, {2} FROM Win32_PortConnector",
                    WmiConstants.Tag,
                    WmiConstants.InternalReferenceDesignator,
                    WmiConstants.ExternalReferenceDesignator));
        }

        /// <summary>
        /// The get WMI info.
        /// </summary>
        public void GetWMIInfo()
        {
            this.portConnectorInfoList = this.GetValue();
        }

        /// <summary>
        /// The report WMI info.
        /// </summary>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        public void ReportWMIInfo(IVisitor visitor)
        {
            var data = this.ConvertPortConnectorInfoToJson(this.portConnectorInfoList);
            visitor.Visit(PortConnectorTableName, data);
            data = this.ConvertClientPortConnectorInfoToJson(this.portConnectorInfoList);
            visitor.Visit(ClientPortConnectorTableName, data);
        }

        /// <summary>
        /// The check for hardware changes.
        /// </summary>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        public void CheckForHardwareChanges(IVisitor visitor)
        {
            var changedHardwareList = new List<PortConnectorInfo>();
            var tempList = this.GetValue();
            changedHardwareList = tempList.GetDifference(this.portConnectorInfoList);
            if (changedHardwareList.Any())
            {
                var data = this.ConvertPortConnectorInfoToJson(changedHardwareList);
                visitor.Visit(PortConnectorTableName, data);
                data = this.ConvertClientPortConnectorInfoToJson(changedHardwareList);
                visitor.Visit(ClientPortConnectorTableName, data);
            }
        }

        /// <summary>
        /// The get value.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<PortConnectorInfo> GetValue()
        {
            var tempList = new List<PortConnectorInfo>();
            try
            {
                foreach (var queryObject in this.searcher.Get())
                {
                    var portConnectorInfo = new PortConnectorInfo
                    {
                        Tag = queryObject[WmiConstants.Tag],
                        InternalReferenceDesignator = queryObject[WmiConstants.InternalReferenceDesignator],
                        ExternalReferenceDesignator = queryObject[WmiConstants.ExternalReferenceDesignator]
                    };
                    tempList.Add(portConnectorInfo);
                }
            }
            catch (Exception)
            {
                throw;
            }
            
            return tempList;
        }

        /// <summary>
        /// The convert port connector info to json.
        /// </summary>
        /// <param name="portConnectorInfoList">
        /// The port connector info list.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ConvertPortConnectorInfoToJson(List<PortConnectorInfo> portConnectorInfoList)
        {
            var data = new JObject(
                new JProperty(
                    "resources",
                    new JArray(
                        from portConnectorInfo in portConnectorInfoList
                        select
                            new JObject(
                            new JProperty("name", string.Empty),
                            new JProperty("internal_reference", portConnectorInfo.InternalReferenceDesignator)))));
            return data.ToString();
        }

        /// <summary>
        /// The convert client port connector info to json.
        /// </summary>
        /// <param name="portConnectorInfoList">
        /// The port connector info list.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ConvertClientPortConnectorInfoToJson(List<PortConnectorInfo> portConnectorInfoList)
        {
            var data = new JObject(
                new JProperty(
                    "resources",
                    new JArray(
                        from portConnectorInfo in portConnectorInfoList
                        select
                            new JObject(
                            new JProperty("external_reference", portConnectorInfo.ExternalReferenceDesignator)))));
            return data.ToString();
        }
    }
}
