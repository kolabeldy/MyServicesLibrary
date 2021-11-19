using System.Collections.Generic;
using System.Windows;

namespace MyServicesLibrary.Controls.CheckedTree;
public class TreeNode : DependencyObject, IParent<object>
{
    public string? Name { get; set; }
    public int Id { get; set; }
    public List<TreeItem>? TreeNodeItems { get; set; }

    IEnumerable<object>? IParent<object>.GetChildren()
    {
        return TreeNodeItems;
    }
}

public class TreeItem : DependencyObject
{
    public int Id { get; set; }
    public string? Name { get; set; }
}