namespace PopulateWMIInfo.Contracts
{
    using System.Collections.Generic;

    using ReportToRestEndpoint.Contracts;

    /// <summary>
    /// The WmiInfo interface.
    /// </summary>
    public interface IWmiInfo
    {
        /// <summary>
        /// The get WMI info method.
        /// </summary>
        void GetWMIInfo();

        /// <summary>
        /// The report WMI info method.
        /// </summary>
        void ReportWMIInfo(IVisitor visitor);

        /// <summary>
        /// The check for hardware changes method.
        /// </summary>
        void CheckForHardwareChanges(IVisitor visitor);
    }
}
