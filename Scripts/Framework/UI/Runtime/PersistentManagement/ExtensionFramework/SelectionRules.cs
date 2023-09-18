namespace Framework.UI.Runtime.PersistentManagement.ExtensionFramework
{
    /// <summary>
    /// The selection rules used to UQuery.
    /// </summary>
    [System.Serializable]
    public struct SelectionRules
    {
        /// <summary>
        /// Selects VisualElements by name (id) when specified. 
        /// </summary>
        public string name;
            
        /// <summary>
        /// Selects VisualElements by class name when specified. 
        /// </summary>
        public string[] classes;
    }
}