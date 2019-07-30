namespace CodeMagic.Core.Objects
{
    public enum UpdateOrder
    {
        /// <summary>
        /// For things that emit light and do other environment stuff
        /// </summary>
        Early = 0,

        /// <summary>
        /// For standard objects and creatures
        /// </summary>
        Medium = 1,

        /// <summary>
        /// For objects that should be updated after creatures update
        /// </summary>
        Late = 2
    }
}