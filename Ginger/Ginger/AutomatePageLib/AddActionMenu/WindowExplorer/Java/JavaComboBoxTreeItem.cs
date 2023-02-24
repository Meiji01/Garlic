#region License
/*
Copyright © 2014-2023 European Support Limited

Licensed under the Apache License, Version 2.0 (the "License")
you may not use this file except in compliance with the License.
You may obtain a copy of the License at 

http://www.apache.org/licenses/LICENSE-2.0 

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS, 
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
See the License for the specific language governing permissions and 
limitations under the License. 
*/
#endregion

using Amdocs.Ginger.Common;
using System.Windows.Controls;
using GingerCore;
using GingerCore.Actions;
using GingerCore.Actions.Java;
using GingerWPF.UserControlsLib.UCTreeView;
using Amdocs.Ginger.Common.UIElement;

namespace Ginger.WindowExplorer.Java
{
    class JavaComboBoxTreeItem : JavaElementTreeItem, ITreeViewItem, IWindowExplorerTreeItem
    {
        StackPanel ITreeViewItem.Header()
        {
            return TreeViewUtils.CreateItemHeader(Name, ElementInfo.GetElementTypeImage(eElementType.ComboBox));
        }

        ObservableList<Act> IWindowExplorerTreeItem.GetElementActions()
        {
            ObservableList<Act> list = new ObservableList<Act>();

            list.Add(new ActJavaElement()
            {
                Description = "Set Selected Value " + Name,
                ControlAction = ActJavaElement.eControlAction.Select
            });

            list.Add(new ActJavaElement()
            {
                Description = "Get " + Name + " Value",
                ControlAction = ActJavaElement.eControlAction.GetValue
            });

            list.Add(new ActJavaElement()
            {
                Description = "Get IsEnabled Property " + Name,
                ControlAction = ActJavaElement.eControlAction.IsEnabled
            });

            list.Add(new ActJavaElement()
            {
                Description = "Select Value By Index",
                ControlAction = ActJavaElement.eControlAction.SelectByIndex,
                Value=0+""
            });

            list.Add(new ActJavaElement()
            {
                Description = "Get Value By Index",
                ControlAction = ActJavaElement.eControlAction.GetValueByIndex,
                Value = 0 + ""
            });

            list.Add(new ActJavaElement()
            {
                Description = "Get Item Count",
                ControlAction = ActJavaElement.eControlAction.GetItemCount,               
            });
            return list;
        }
    }
}
