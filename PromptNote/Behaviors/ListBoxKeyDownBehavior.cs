using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;
using PromptNote.Models;
using PromptNote.ViewModels;

namespace PromptNote.Behaviors
{
    public class ListBoxKeyDownBehavior : Behavior<ListBox>
    {
        private static int lastItemHeight;
        private ScrollViewer scrollViewer;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.KeyDown += OnKeyDown;
            AssociatedObject.SelectionChanged += OnSelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.KeyDown -= OnKeyDown;
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
        }

        private static ScrollViewer GetScrollViewer(DependencyObject obj)
        {
            if (obj == null)
            {
                return null;
            }

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);

                if (child is ScrollViewer scrollViewer)
                {
                    return scrollViewer;
                }

                var childScrollViewer = GetScrollViewer(child);
                if (childScrollViewer != null)
                {
                    return childScrollViewer;
                }
            }

            return null;
        }

        private static int GetVisibleItemCount(ListBox listBox)
        {
            var scrollViewer = GetScrollViewer(listBox);
            if (scrollViewer == null)
            {
                return 0;
            }

            // アイテムコンテナの高さを取得
            var itemContainerGenerator = listBox.ItemContainerGenerator;
            if (itemContainerGenerator.Status != GeneratorStatus.ContainersGenerated || listBox.Items.Count == 0)
            {
                return 0;
            }

            if (itemContainerGenerator.ContainerFromIndex(0) is not FrameworkElement firstItem)
            {
                return lastItemHeight;
            }

            // アイテムの高さ
            var itemHeight = firstItem.ActualHeight;

            // ScrollViewer のビューポート高さから表示可能なアイテム数を計算
            var viewportHeight = scrollViewer.ActualHeight;
            lastItemHeight = (int)(viewportHeight / itemHeight);

            return lastItemHeight;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            var listBox = sender as ListBox;

            var isShiftPressed = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
            var isControlPressed = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;

            if (listBox == null)
            {
                return;
            }

            if (listBox.DataContext is not MainWindowViewModel vm)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.D:
                    if (!isControlPressed)
                    {
                        return;
                    }

                    scrollViewer ??= GetScrollViewer(listBox);
                    if (scrollViewer != null)
                    {
                        // scrollViewer.PageDown, PageUp は以下のような動作をする。
                        // 選択中のアイテムが表示領域の一番下（上）になるようにスクロールする。
                        // そのため、ページダウンに際してスクロールの開始位置を調節するためこのメソッドを使用している。インデックスはこのメソッドでは変化しない。
                        scrollViewer.PageUp();
                        var visibleItemCount = GetVisibleItemCount(listBox);
                        vm.PromptsViewModel.CursorManager.MoveCursor(visibleItemCount);
                        e.Handled = true;
                    }

                    break;

                case Key.U:
                    if (!isControlPressed)
                    {
                        return;
                    }

                    scrollViewer ??= GetScrollViewer(listBox);
                    if (scrollViewer != null)
                    {
                        scrollViewer.PageDown();
                        var visibleItemCount = GetVisibleItemCount(listBox);
                        vm.PromptsViewModel.CursorManager.MoveCursor(-visibleItemCount);
                        e.Handled = true;
                    }

                    break;

                case Key.G:
                    if (isShiftPressed)
                    {
                        vm.PromptsViewModel.CursorManager.MoveCursorToBottom();
                        break;
                    }

                    vm.PromptsViewModel.CursorManager.MoveCursorToTop();
                    break;

                case Key.J:
                    if (isControlPressed)
                    {
                        return;
                    }

                    if (isShiftPressed && listBox.SelectedIndex < listBox.Items.Count - 1)
                    {
                        var index = listBox.SelectedIndex;
                        var item = listBox.SelectedItem as Prompt;
                        if (listBox.ItemsSource is ObservableCollection<Prompt> items)
                        {
                            items.RemoveAt(index);
                            items.Insert(index + 1, item);
                            listBox.SelectedIndex = index + 1;
                            listBox.SelectedItem = item;

                            // vm.ReIndex(items);
                        }

                        break;
                    }

                    vm.PromptsViewModel.CursorManager.MoveCursorDown();
                    break;

                case Key.K:
                    if (isShiftPressed && listBox.SelectedIndex > 0)
                    {
                        var index = listBox.SelectedIndex;
                        var item = listBox.SelectedItem as Prompt;
                        if (listBox.ItemsSource is ObservableCollection<Prompt> items)
                        {
                            items.RemoveAt(index);
                            items.Insert(index - 1, item);
                            listBox.SelectedIndex = index - 1;
                            listBox.SelectedItem = item;

                            // vm.ReIndex(items);
                        }

                        break;
                    }

                    vm.PromptsViewModel.CursorManager.MoveCursorUp();
                    break;

                case Key.I:
                    if (listBox.SelectedIndex >= 0)
                    {
                        if (listBox.SelectedItem is Prompt item)
                        {
                            item.ContainsOutput = !item.ContainsOutput;
                        }
                    }

                    break;

                case Key.Delete:
                    if (listBox.SelectedIndex >= 0)
                    {
                        var index = listBox.SelectedIndex;
                        if (listBox.ItemsSource is ObservableCollection<Prompt> items)
                        {
                            items.RemoveAt(index);
                            if (items.Count > 0)
                            {
                                if (index >= items.Count)
                                {
                                    index = items.Count - 1;
                                }

                                listBox.SelectedIndex = index;
                                listBox.SelectedItem = items[index];
                            }
                        }
                    }

                    break;
            }

            if (listBox.SelectedItem != null)
            {
                listBox.ScrollIntoView(listBox.SelectedItem);
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ListBox listBox || listBox.Items.Count == 0)
            {
                return;
            }

            if (listBox.SelectedIndex < 0)
            {
                return;
            }

            var item = listBox.Items[listBox.SelectedIndex];

            if (item == null)
            {
                return;
            }

            listBox.ScrollIntoView(item);
        }
    }
}