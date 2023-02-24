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
using Amdocs.Ginger.CoreNET.Run.SolutionCategory;
using Amdocs.Ginger.Repository;

namespace Amdocs.Ginger.Common.Repository.SolutionCategories
{
    public class SolutionCategoryDefinition : RepositoryItemBase
    {        
        public static object Solution
        {
            get;set;
        }

        public SolutionCategoryDefinition()
        {

        }

        public SolutionCategoryDefinition(eSolutionCategories category)
        {
            this.Category = category;
        }

        [IsSerializedForLocalRepository]
        public eSolutionCategories Category
        {
            get;
            set;
        }

        public override string ItemName
        {
            get
            {
                return this.Category.ToString();
            }
            set
            {
                this.Category = (eSolutionCategories)Enum.Parse(typeof(eSolutionCategories), value.ToString());
            }
        }
        
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public ObservableList<SolutionCategoryValue> CategoryOptionalValues { get; set; }

        private Guid mSelectedValueID;
        [IsSerializedForLocalRepository]
        public Guid SelectedValueID
        {
            get { return mSelectedValueID; }
            set
            {
                if (mSelectedValueID != value)
                {
                    mSelectedValueID = value;
                    OnPropertyChanged(nameof(SelectedValueID));
                }
            }
        }
    }
}
