namespace CharLS.Native
{
    /// <summary>
    /// Defines the Application profile identifier options that can be used in a SPIFF header v2, as defined in ISO/IEC 10918-3, F.1.2.
    /// </summary>
    public enum SpiffProfileId
    {
        /// <summary>
        /// No profile identified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Continuous-tone base profile (JPEG)
        /// </summary>
        ContinuousToneBase = 1,

        /// <summary>
        /// Continuous-tone progressive profile
        /// </summary>
        ContinuousToneProgressive = 2,

        /// <summary>
        /// Bi-level facsimile profile (MH, MR, MMR, JBIG)
        /// </summary>
        BiLevelFacsimile = 3,

        /// <summary>
        /// Continuous-tone facsimile profile (JPEG)
        /// </summary>
        ContinuousToneFacsimile = 4
    }
}