namespace MiniTC.model {
    public class TCPanel {

        public string Path { get; set; }
        public FileEntry Selected { get; set; }

        public string FilePath => Selected is { IsParent: false } ? Path + Selected.Name : null;

        public override bool Equals(object obj) {
            if (obj is not TCPanel o) return false;
            return Path == o.Path && Selected == o.Selected;
        }

        public override string ToString() {
            return Path + (Selected?.Name ?? "");
        }
    }
}