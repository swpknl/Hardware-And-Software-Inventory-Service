namespace PopulateWMIInfo.Rules
{
    using System.Management;

    using Constants;

    using PopulateWMIInfo.Contracts;

    using ReportToRestEndpoint.Contracts;

    /// <summary>
    /// The software licensing service.
    /// </summary>
    public class SoftwareLicensingService : IWmiInfo
    {
        private ManagementObjectSearcher searcher;
        
        private string oa3XOriginalProductKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoftwareLicensingService"/> class.
        /// </summary>
        public SoftwareLicensingService()
        {
            this.searcher = new ManagementObjectSearcher(
                WmiConstants.WmiMS409Namespace,
                "SELECT * FROM SoftwareLicensingService");
        }

        /// <summary>
        /// The get WMI info.
        /// </summary>
        public void GetWMIInfo()
        {
            this.oa3XOriginalProductKey = this.GetValue();
        }

        /// <summary>
        /// The report WMI info.
        /// </summary>
        /// <param name="visitor">
        /// The visitor.
        /// </param>
        public void ReportWMIInfo(IVisitor visitor)
        {
            
        }

        public void CheckForHardwareChanges()
        {
            var tempValue = this.GetValue();
            var changedHardwareValue = tempValue.Equals(
                this.oa3XOriginalProductKey,
                System.StringComparison.OrdinalIgnoreCase)
                                           ? string.Empty
                                           : tempValue;
        }

        private string GetValue()
        {
            var tempValue = string.Empty;
            foreach (var queryObject in this.searcher.Get())
            {
                tempValue = queryObject[WmiConstants.OA3xOriginalProductKey].ToString();
            }

            return tempValue;
        }
    }
}
