using System.Diagnostics;
using Prism.Mvvm;

namespace PromptNote.Models
{
    public class TextWrapper : BindableBase
    {
        private string title;
        private string version = string.Empty;

        public TextWrapper()
        {
            Title = "PromptNote";

            SetVersion();
            AddDebugMark();
        }

        private string Title
        {
            get => string.IsNullOrWhiteSpace(Version)
                ? title
                : title + " version : " + Version;
            set => SetProperty(ref title, value);
        }

        private string Version { get => version; set => SetProperty(ref version, value); }

        public override string ToString()
        {
            return Title;
        }

        [Conditional("RELEASE")]
        private void SetVersion()
        {
            const int major = 1;
            const int minor = 8;
            const int patch = 2;
            const string date = "20250116";
            const string suffix = "a";

            Version = $"{major}.{minor}.{patch} ({date}{suffix})";
        }

        [Conditional("DEBUG")]
        private void AddDebugMark()
        {
            Title += " (Debug)";
        }
    }
}