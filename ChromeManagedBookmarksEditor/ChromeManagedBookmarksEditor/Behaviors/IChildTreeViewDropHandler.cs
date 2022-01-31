using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactions.DragAndDrop;
using ChromeManagedBookmarksEditor.Interfaces;
using ChromeManagedBookmarksEditor.Models;
using ChromeManagedBookmarksEditor.ViewModels;

namespace ChromeManagedBookmarksEditor.Behaviors
{
    public class IChildTreeViewDropHandler : DropHandlerBase
    {
        private bool Validate<T>(TreeView treeView, DragEventArgs e, object? sourceContext, object? targetContext, bool bExecute) where T : IChild
        {
            if (sourceContext is not T sourceNode
            || targetContext is not EditorViewModel vm
            || treeView.GetVisualAt(e.GetPosition(treeView)) is not IControl targetControl
            || targetControl.DataContext is not T targetNode)
            {
                return false;
            }

            var sourceParent = sourceNode.Parent;
            var targetParent = targetNode.Parent;

            var sourceChildren = sourceParent is Folder ? sourceParent.Children : vm.RootFolders[0].Children;
            var targetChildren = targetParent is Folder ? targetParent.Children : vm.RootFolders[0].Children;

            var sourceIndex = sourceChildren.IndexOf(sourceNode);
            var targetIndex = targetChildren.IndexOf(targetNode);

            if (sourceIndex < 0 || targetIndex < 0)
            {
                return false;
            }

            //TODO - finish drag drop handler, still a WIP

            return false;
        }

        public override bool Validate(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state)
        {
            if (e.Source is IControl && sender is TreeView treeView)
            {
                return Validate<IChild>(treeView, e, sourceContext, targetContext, false);
            }
            return false;
        }

        public override bool Execute(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state)
        {
            if (e.Source is IControl && sender is TreeView treeView)
            {
                return Validate<IChild>(treeView, e, sourceContext, targetContext, true);
            }
            return false;
        }
    }
}
