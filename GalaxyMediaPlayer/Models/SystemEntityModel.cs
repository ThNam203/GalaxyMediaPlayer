namespace GalaxyMediaPlayer
{
    /// <summary>
    /// This class model is used for EntityListBoxItem
    /// Which mainly consists of folder and media file
    /// </summary>
    public class SystemEntityModel
    {
        // assuming it is a folder
        public SystemEntityModel()
        {
            entityName = "Unknown";
            entityType = EntityType.Folder;
            entityPath = "";
            imageSource = "folder.folder"; // use converter
        }
        public SystemEntityModel(string name)
        {
            entityName = name;
            entityType = EntityType.Folder;
            entityPath = "";
            imageSource = "folder.folder";
        }
        public SystemEntityModel(string name, EntityType type, string path, string imageSource)
        {
            entityName = name;
            entityType = type;
            entityPath = path;
            this.imageSource = imageSource;
        }

        public string entityName { get; set; }
        public EntityType entityType { get; set; }
        public string entityPath { get; set; }
        public string imageSource { get; set; }
    }

    public enum EntityType
    {
        Folder,
        Image,
        Video,
        Music,
        Document
    }
}
