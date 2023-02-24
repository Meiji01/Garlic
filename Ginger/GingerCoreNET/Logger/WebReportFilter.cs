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
using System.Collections.Generic;
using System.Text;

namespace Amdocs.Ginger.CoreNET.Logger
{
    public class WebReportFilter
    {
        public ReportLevelType ReportLevel { set; get; }
        public string Guid { set; get; }

    }

    public enum ReportLevelType
    {
        RunSet = 1,
        Runner = 2,
        BusinessFlow = 3,
        ActivityGroup = 4,
        Activity = 5,
        Action = 6
    }
}
