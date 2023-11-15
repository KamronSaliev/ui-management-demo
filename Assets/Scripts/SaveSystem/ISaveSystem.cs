namespace UIManagementDemo.SaveSystem
{
    public interface ISaveSystem
    {
        void Save(SaveData data);

        SaveData Load();
    }
}