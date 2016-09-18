namespace PopulateWMIInfo.Rules
{
    using System.Management;

    using Constants;

    using PopulateWMIInfo.Contracts;

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
            foreach (var queryObject in this.searcher.Get())
            {
                this.oa3XOriginalProductKey = queryObject[WmiConstants.OA3xOriginalProductKey].ToString();
            }
        }

        /// <summary>
        /// The report WMI info.
        /// </summary>
        public void ReportWMIInfo()
        {
            
        }
    }
}
