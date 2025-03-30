namespace Doryu.JBSave
{
    public interface ISavable<T>
    {
        /// <summary>
        /// Called when classdata is loaded
        /// </summary>
        public void OnLoadData(T classData);
        /// <summary>
        /// Called when classdata is saved
        /// </summary>
        public void OnSaveData(string savedFileName);
        /// <summary>
        /// Called when classdata is saved
        /// </summary>
        public void ResetData();
    }
}
