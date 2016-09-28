namespace PopulateWMIInfo.Contracts
{
    /// <summary>
    /// The PopulateHardwareInfo interface.
    /// </summary>
    public interface IPopulateWMIInfoFacade
    {
        /// <summary>
        /// Contract for populating hardware info.
        /// </summary>
        void PopulateWMIInfo();

        /// <summary>
        /// The check for hardware changes.
        /// </summary>
        void CheckForHardwareChanges();
    }
}
