namespace MiniTC.model {
    public class FileEntry {

        public static readonly FileEntry Parent = new("..", false);
        
        public string Name { get; }
        public bool IsDirectory { get; }

        public bool IsParent => ReferenceEquals(this, Parent);

        public bool IsFile => !IsDirectory && !IsParent; 

        public FileEntry(string name, bool isDirectory) {
            Name = name;
            IsDirectory = isDirectory;
        }

        public override bool Equals(object obj) {
            if (obj is not FileEntry o) return false;
            return Name == o.Name && IsDirectory == o.IsDirectory;
        }

        public override string ToString() {
            return (IsDirectory ? "<D> " : "") + Name;
        }
    }
}