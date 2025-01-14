using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;
using PromptNote.ViewModels;

namespace PromptNote.Behaviors
{
    public class DragAndDropBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            // ファイルをドラッグしてきて、コントロール上に乗せた際の処理
            AssociatedObject.PreviewDragOver += AssociatedObject_PreviewDragOver;

            // ファイルをドロップした際の処理
            AssociatedObject.Drop += AssociatedObject_Drop;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewDragOver -= AssociatedObject_PreviewDragOver;
            AssociatedObject.Drop -= AssociatedObject_Drop;
        }

        private void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            // ファイルパスの一覧の配列
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (((ListBox)sender).DataContext is not MainWindowViewModel vm)
            {
                return;
            }

            if (files?.Length == 1 && File.Exists(files.First()))
            {
                if (vm.PromptGroupViewModel.SelectedItem != null)
                {
                    var p = files.FirstOrDefault();
                    var fileName = Path.GetFileName(p);
                    if (string.IsNullOrWhiteSpace(p) || string.IsNullOrWhiteSpace(fileName))
                    {
                        return;
                    }

                    var dirName = "sampleImages";
                    var dest = new FileInfo($"{dirName}\\{fileName}");

                    if (!Directory.Exists(dirName))
                    {
                        Directory.CreateDirectory(dirName);
                    }

                    File.Copy(p, dest.FullName, true);
                    vm.PromptGroupViewModel.SelectedItem.SampleImagePath = dest.FullName;
                    _ = vm.PromptGroupViewModel.SaveAsyncCommand.ExecuteAsync();
                }
            }
        }

        private void AssociatedObject_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
        }
    }
}