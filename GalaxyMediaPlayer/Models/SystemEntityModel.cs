namespace GalaxyMediaPlayer
{
    /// <summary>
    /// This class model is used for EntityListBoxItem
    /// Which mainly consists of folder and media file
    /// </summary>
    public class SystemEntityModel
    {
        public SystemEntityModel(
            string name,
            EntityType type,
            string path,
            string dateCreated,
            long size,
            string extension)
        {
            Name = name;
            Type = type;
            Path = path;
            DateCreated = dateCreated;
            Size = size;
            Extension = extension;
        }

        public string Name { get; set; }
        public EntityType Type { get; set; }
        public string Path { get; set; }
        public string DateCreated { get; set; }
        public long Size { get; set; }
        public string Extension { get; set; }
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
