using MyServicesLibrary.Controls.CheckedTree;

namespace MyServicesLibrary.Controls.FilterPanelCheckedTree;
public class TreeFilterCollection
{
    public List<TreeNode> FilterCollection { get; set; }
    public string Title { get; set; }
    public TreeInitType InitType { get; set; }
}