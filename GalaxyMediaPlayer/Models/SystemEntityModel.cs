using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GalaxyMediaPlayer
{
    /// <summary>
    /// This class model is used for EntityListBoxItem
    /// Which mainly consists of folder and media file
    /// </summary>
    public class SystemEntityModel: INotifyPropertyChanged
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
            IsSelected = false;
        }

        public string Name { get; set; }
        public EntityType Type { get; set; }
        public string Path { get; set; }
        public string DateCreated { get; set; }
        public long Size { get; set; }
        public string Extension { get; set; }

        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set 
            { 
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public enum EntityType
    {
        Folder,
        Image,
        Video,
        Music
    }
}
