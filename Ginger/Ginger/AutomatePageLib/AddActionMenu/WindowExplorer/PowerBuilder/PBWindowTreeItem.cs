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

using System;
using System.Windows.Controls;
using Amdocs.Ginger.Common.Enums;
using Amdocs.Ginger.Common.UIElement;
using GingerWPF.UserControlsLib.UCTreeView;

namespace Ginger.Drivers.PowerBuilder
{
    public class PBWindowTreeItem : PBControlTreeItemBase, ITreeViewItem
    {        
        Object ITreeViewItem.NodeObject()
        {
            return base.UIAElementInfo;
        }

        StackPanel ITreeViewItem.Header()
        {
            return TreeViewUtils.CreateItemHeader(UIAElementInfo.ElementTitle, ElementInfo.GetElementTypeImage(eElementType.Window));
        }
        
        bool ITreeViewItem.IsExpandable()
        {
            return true;
        }

        Page ITreeViewItem.EditPage(Amdocs.Ginger.Common.Context mContext)
        {
            //TODO: create Edit page for Window, with action of Swithc Window
            return null;
        }

        ContextMenu ITreeViewItem.Menu()
        {
            return null;
        }

        void ITreeViewItem.SetTools(ITreeView TV)
        {
            TV.AddToolbarTool(eImageType.Refresh, "Refresh", TV.Tree.RefreshSelectedTreeNodeChildrens);
        }
    }
}
