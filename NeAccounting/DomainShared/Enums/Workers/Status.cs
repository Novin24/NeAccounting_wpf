namespace DomainShared.Enums
{
    /// <summary>
    /// وضعیت کارگر
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// همه
        /// </summary>
        All,
        /// <summary>
        /// درحال کار
        /// </summary>
        InWork,
        /// <summary>
        /// تسویه و اتمام کار
        /// </summary>
        Settlement,
        /// <summary>
        /// اخراج
        /// </summary>
        FiredUp
    }
}