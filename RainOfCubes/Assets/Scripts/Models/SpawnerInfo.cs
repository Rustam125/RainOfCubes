namespace Models
{
    public class SpawnerInfo
    {
        public SpawnerInfo(string objectName, int spawnedObjects, int createdObjects, int activeObjects)
        {
            ObjectName = objectName;
            SpawnedObjects = spawnedObjects;
            CreatedObjects = createdObjects;
            ActiveObjects = activeObjects;
        }

        public string ObjectName { get; private set; }
        public int SpawnedObjects { get; private set; }
        public int CreatedObjects { get; private set; }
        public int ActiveObjects { get; private set; }

        public void IncreaseSpawnedObjects() => SpawnedObjects++;
        public void IncreaseCreatedObjects() => CreatedObjects++;
        public void SetCountActiveObjects(int count) => ActiveObjects = count;
    }
}