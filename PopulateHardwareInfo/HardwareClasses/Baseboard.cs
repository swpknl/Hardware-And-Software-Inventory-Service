namespace PopulateWMIInfo.HardwareClasses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management;

    using Constants;

    using Entities;

    using PopulateWMIInfo.Contracts;

    using Helpers;

    using Newtonsoft.Json.Linq;

    using ReportToRestEndpoint.Contracts;

    /// <summary>
    /// WMI Info for the baseboard.
    /// </summary>
    internal class Baseboard : IWmiInfo
    {
        private const string BaseBoardTableName = "base_board";

        private const string ClientBaseBoardTableName = "x_client_base_board";

        private ManagementObjectSearcher searcher;

        private int id;

        private List<BaseboardInfo> baseboardInfoList;

        /// <summary>
        /// Initializes a new instance of the <see cref="Baseboard"/> class.
        /// </summary>
        public Baseboard()
        {
            this.baseboardInfoList = new List<BaseboardInfo>();
            this.searcher = new ManagementObjectSearcher(
                WmiConstants.WmiRootNamespace,
                string.Format(
                    "SELECT {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8} FROM Win32_BaseBoard",
                    WmiConstants.Product,
                    WmiConstants.Manufacturer,
                    WmiConstants.HotSwappable,
                    WmiConstants.HostingBoard,
                    WmiConstants.Removable,
                    WmiConstants.Replaceable,
                    WmiConstants.RequiresDaughterBoard,
                    WmiConstants.Version,
                    WmiConstants.SerialNumber));
        }

        /// <summary>
        /// The get WMI info method.
        /// </summary>
        public void GetWMIInfo()
        {
            this.baseboardInfoList = this.GetValue();
        }
        
        /// <summary>
        /// The report WMI info method.
        /// </summary>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        public void ReportWMIInfo(IVisitor visitor)
        {
            var data = this.ConvertBaseBoardInfoToJson(this.baseboardInfoList);
            visitor.Visit(BaseBoardTableName, data, out id);
            data = this.ConvertClientBaseBoardInfoToJson(this.baseboardInfoList);
            int temp;
            visitor.Visit(ClientBaseBoardTableName, data, out temp);
        }

        /// <summary>
        /// The check for hardware changes.
        /// </summary>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        public void CheckForHardwareChanges(IVisitor visitor)
        {
            var changedBaseboardInfoList = new List<BaseboardInfo>();
            var tempBaseboardInfoList = this.GetValue();
            changedBaseboardInfoList = tempBaseboardInfoList.GetDifference<BaseboardInfo>(this.baseboardInfoList);
            if (changedBaseboardInfoList.Any())
            {
                var data = this.ConvertBaseBoardInfoToJson(changedBaseboardInfoList);
                visitor.Visit(BaseBoardTableName, data, out id);
                int temp;
                data = this.ConvertClientBaseBoardInfoToJson(changedBaseboardInfoList);
                visitor.Visit(ClientBaseBoardTableName, data, out temp);
            }
            
        }

        /// <summary>
        /// The get value.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<BaseboardInfo> GetValue()
        {
            var tempList = new List<BaseboardInfo>();
            try
            {
                foreach (var queryObj in this.searcher.Get())
                {
                    var baseboardInfo = new BaseboardInfo
                    {
                        Product = queryObj[WmiConstants.Product],
                        Manufacturer = queryObj[WmiConstants.Manufacturer],
                        HotSwappable = queryObj[WmiConstants.HotSwappable],
                        HostingBoard = queryObj[WmiConstants.HostingBoard],
                        Removable = queryObj[WmiConstants.Removable],
                        Replaceable = queryObj[WmiConstants.Replaceable],
                        RequiresDaughterBoard = queryObj[WmiConstants.RequiresDaughterBoard],
                        Version = queryObj[WmiConstants.Version],
                        SerialNumber = queryObj[WmiConstants.SerialNumber]
                    };
                    tempList.Add(baseboardInfo);
                }
            }
            catch (Exception)
            {
                throw;
            }
            

            return tempList;
        }

        /// <summary>
        /// The convert to JSON.
        /// </summary>
        /// <param name="inputList">
        /// The input List.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ConvertBaseBoardInfoToJson(List<BaseboardInfo> inputList)
        {
            var data = new JObject(
                new JProperty(
                    "resource",
                    new JArray(
                        from baseboardInfo in inputList
                        select
                            new JObject(
                            new JProperty("manufacturer", baseboardInfo.Manufacturer),
                            new JProperty(
                            "is_hot_swappable",
                            GenericExtensions.GetBooleanValue(baseboardInfo.HotSwappable)),
                            new JProperty(
                            "is_hosting_board",
                            GenericExtensions.GetBooleanValue(baseboardInfo.HostingBoard)),
                            new JProperty(
                            "is_removable",
                            GenericExtensions.GetBooleanValue(baseboardInfo.Removable)),
                            new JProperty(
                            "is_replaceable",
                            GenericExtensions.GetBooleanValue(baseboardInfo.Replaceable)),
                            new JProperty(
                            "requires_daugter_board",
                            GenericExtensions.GetBooleanValue(baseboardInfo.RequiresDaughterBoard)),
                            new JProperty("total_cpu_sockets", baseboardInfo.TotalCpuSockets)))));
            return data.ToString();
        }

        /// <summary>
        /// The convert client base board info to json.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ConvertClientBaseBoardInfoToJson(List<BaseboardInfo> inputList)
        {
            var data = new JObject(
                new JProperty(
                    "resource",
                    new JArray(
                        from baseboardInfo in inputList
                        select
                            new JObject(
                            new JProperty("version", baseboardInfo.Version),
                            new JProperty("serial_number", baseboardInfo.SerialNumber),
                            new JProperty("client_id", ClientId.Id),
                            new JProperty("base_board_id", this.id)))));
            return data.ToString();
        }
    }
}
