using System.Collections.Generic;

namespace MyServicesLibrary.Controls.CheckedTree;
interface IParent<T>
{
    IEnumerable<T> GetChildren();
}