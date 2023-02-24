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

//#region License
///*
//Copyright © 2014-2023 European Support Limited

//Licensed under the Apache License, Version 2.0 (the "License")
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at 

//http://www.apache.org/licenses/LICENSE-2.0 

//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS, 
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
//See the License for the specific language governing permissions and 
//limitations under the License. 
//*/
//#endregion

using Amdocs.Ginger.Common;
using GingerCore.ALM.RQM;
using Ginger;
using System;
using System.Collections.Generic;
using GingerCore.Activities;
using TDAPIOLELib;
using AlmDataContractsStd.Enums;
using GingerCore.ALM.QC;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.IO.Compression;
using Newtonsoft.Json;
using GingerCore.External;
using Amdocs.Ginger.Repository;
using Amdocs.Ginger.Common.InterfacesLib;
using System.Linq;
using GingerCoreNET.SolutionRepositoryLib.RepositoryObjectsLib.PlatformsLib;
using GingerCoreNET.ALMLib;

namespace GingerCore.ALM
{
    public class RQMCore : ALMCore
    {
        public static string ConfigPackageFolderPath { set; get; }
        public static string ALMProjectGroupName { get; set; }
        public static string ALMProjectGuid { get; set; }

        public override bool ConnectALMServer()
        {
            return RQMConnect.Instance.ConnectRQMServer();
        }

        public override bool ConnectALMProject()
        {
            return RQMConnect.Instance.SetRQMProjectFullDetails();
        }

        public override void DisconnectALMServer()
        {
            RQMConnect.Instance.DisconnectRQMServer();
        }

        public override bool DisconnectALMProjectStayLoggedIn()
        {
            return RQMConnect.Instance.DisconnectALMProjectStayLoggedIn();
        }

        public override bool IsServerConnected()
        {
            return RQMConnect.Instance.IsServerConnected();
        }

        public override List<string> GetALMDomains()
        {
            return RQMConnect.Instance.GetRQMDomains();
        }

        public override Dictionary<string, string> GetALMDomainProjects(string ALMDomain)
        {
            //set the domain of the default ALM
            DefaultAlmConfig.ALMDomain = ALMDomain;
            return RQMConnect.Instance.GetRQMDomainProjects();
        }

        public override bool ExportExecutionDetailsToALM(BusinessFlow bizFlow, ref string result, bool exectutedFromAutomateTab = false, PublishToALMConfig publishToALMConfig = null)
        {
            return ExportToRQM.Instance.ExportExecutionDetailsToRQM(bizFlow, ref result, exectutedFromAutomateTab, publishToALMConfig);
        }
        public bool ExportBfActivitiesGroupsToALM(BusinessFlow businessFlow, ObservableList<ActivitiesGroup> grdActivitiesGroups, ref string result)
        {
            return ExportToRQM.Instance.ExportBfActivitiesGroupsToALM(businessFlow, grdActivitiesGroups, ref result);
        }

        public bool ExportBusinessFlowToRQM(BusinessFlow businessFlow, ObservableList<ExternalItemFieldBase> ExternalItemsFields, ref string result)
        {
            return ExportToRQM.Instance.ExportBusinessFlowToRQM(businessFlow, ExternalItemsFields, ref result);
        }

        public override ObservableList<ExternalItemFieldBase> GetALMItemFields(BackgroundWorker bw, bool online, AlmDataContractsStd.Enums.ResourceType resourceType)
        {
            return UpdatedAlmFields(ImportFromRQM.GetALMItemFields(bw, online));
        }

        public override Dictionary<Guid, string> CreateNewALMDefects(Dictionary<Guid, Dictionary<string, string>> defectsForOpening, List<ExternalItemFieldBase> defectsFields, bool useREST)
        {
            return null;
        }

        public override ObservableList<ActivitiesGroup> GingerActivitiesGroupsRepo
        {
            get { return ImportFromRQM.GingerActivitiesGroupsRepo; }
            set { ImportFromRQM.GingerActivitiesGroupsRepo = value; }
        }

        public override ObservableList<Activity> GingerActivitiesRepo
        {
            get { return ImportFromRQM.GingerActivitiesRepo; }
            set { ImportFromRQM.GingerActivitiesRepo = value; }
        }

        public override ObservableList<ApplicationPlatform> ApplicationPlatforms
        {
            get { return ImportFromRQM.ApplicationPlatforms; }
            set { ImportFromRQM.ApplicationPlatforms = value; }
        }

        public override ALMIntegrationEnums.eALMType ALMType => ALMIntegrationEnums.eALMType.RQM;

        public override void SetALMConfigurations(string ALMServerUrl, bool UseRest, string ALMUserName, string ALMPassword,
                                                    string ALMDomain, string ALMProject, string ALMProjectKey, GingerCoreNET.ALMLib.ALMIntegrationEnums.eALMType almType,
                                                    string ALMConfigPackageFolderPath, bool ZephyrEntToken, GingerCoreNET.ALMLib.ALMIntegrationEnums.eTestingALMType testingALMType = GingerCoreNET.ALMLib.ALMIntegrationEnums.eTestingALMType.None)
        {
            GingerCoreNET.ALMLib.ALMConfig AlmConfig = amdocs.ginger.GingerCoreNET.WorkSpace.Instance.Solution.ALMConfigs.FirstOrDefault(x => x.AlmType == almType);
            GingerCoreNET.ALMLib.ALMConfig AlmCoreConfig = AlmConfigs.FirstOrDefault(x => x.AlmType == almType);

            GingerCoreNET.ALMLib.ALMUserConfig CurrentAlmUserConfigurations = amdocs.ginger.GingerCoreNET.WorkSpace.Instance.UserProfile.ALMUserConfigs.FirstOrDefault(x => x.AlmType == almType);

            //if not exist add otherwise update
            if (AlmConfig == null)
            {
                AlmConfig = new GingerCoreNET.ALMLib.ALMConfig();
                AlmConfigs.Add(AlmConfig);
            }
            AlmConfig.ALMServerURL = GetServerValueFromDict(GetDynamicServerConfigAndSetPaths());
            AlmConfig.UseRest = UseRest;
            AlmConfig.ALMUserName = CurrentAlmUserConfigurations.ALMUserName;
            AlmConfig.ALMPassword = CurrentAlmUserConfigurations.ALMPassword;
            AlmConfig.ALMDomain = ALMDomain;
            AlmConfig.ALMProjectName = ALMProject;
            AlmConfig.ALMProjectKey = ALMProjectKey;
            AlmConfig.AlmType = almType;
            AlmConfig.ALMConfigPackageFolderPath = amdocs.ginger.GingerCoreNET.WorkSpace.Instance.SolutionRepository.ConvertFullPathToBeRelative(ALMConfigPackageFolderPath);
            AlmConfig.JiraTestingALM = testingALMType;

            AlmCoreConfig = AlmConfig;
        }

        #region RQM Configurations Package
        private Dictionary<String, Object> GetDynamicServerConfigAndSetPaths()
        {
            if (IsConfigPackageExists())
            {
                try
                {
                    XmlDocument RQMSettingsXML = new XmlDocument();
                    string RQMSettingsXMLFilePath = Path.Combine(RQMCore.ConfigPackageFolderPath, "RQMSettings.xml");
                    RQMSettingsXML.Load(RQMSettingsXMLFilePath);

                    //Set Path of Export_Settings.xml file inside RQMSettings.xml file
                    XmlNode Export_SettingsFile_Location = RQMSettingsXML.SelectSingleNode("RQM/GeneralData/Export_SettingsFile_Location");
                    Export_SettingsFile_Location.InnerText = Path.Combine(RQMCore.ConfigPackageFolderPath, "RQM_Export");
                    RQMSettingsXML.Save(RQMSettingsXMLFilePath);

                    //Extract end return ServerURL value from RQM/GeneralData/ServerURL node
                    XmlNode ServerURLNode = RQMSettingsXML.SelectSingleNode("RQM/GeneralData/ServerURL");
                    string serverURL = ServerURLNode.InnerText;
                    Dictionary<String, Object> dictionary = new Dictionary<string, object>();
                    dictionary.Add("ServerURL", serverURL);
                    return dictionary;
                }
                catch (Exception e)
                {
                    Reporter.ToLog(eLogLevel.ERROR, "Error reading ALM RQMConfigPackage at: " + Path.Combine(RQMCore.ConfigPackageFolderPath, "RQMSettings.xml"), e);
                }
            }
            return new Dictionary<string, object>();
        }

        public bool IsConfigPackageExists()
        {
            string CurrRQMConfigPath = Path.Combine(ALMCore.SolutionFolder, "Configurations", "RQMServerConfigurationsPackage");

            if (Directory.Exists(CurrRQMConfigPath))
            {
                if (File.Exists(Path.Combine(CurrRQMConfigPath, "RQMSettings.xml")))
                {
                    RQMCore.ConfigPackageFolderPath = CurrRQMConfigPath;
                    ALMCore.DefaultAlmConfig.ALMConfigPackageFolderPath = CurrRQMConfigPath;
                    RQMConnect.Instance.CreateRQMRepository();
                    return true;
                }
                else
                {
                    //Missing RQMSettings.xml file
                    Reporter.ToLog(eLogLevel.WARN, "RQM Configuration package not exist in solution, RqmSettings.xml not exist at: " + Path.Combine(CurrRQMConfigPath, "RQMSettings.xml"));
                }
            }
            else
            {
                //Missing RQM Configurations Folder
                Reporter.ToLog(eLogLevel.WARN, "RQMServerConfigurationsPackage folder not exist at: " + CurrRQMConfigPath);
            }

            return false;
        }

        public bool ValidateConfigurationFile(string PackageFileName)
        {
            bool containRQMSettingsFile = false;
            using (FileStream configPackageZipFile = new FileStream(PackageFileName, FileMode.Open))
            {
                using (ZipArchive zipArchive = new ZipArchive(configPackageZipFile))
                {
                    foreach (ZipArchiveEntry entry in zipArchive.Entries)
                        if (entry.Name == "RQMSettings.xml")
                        {
                            containRQMSettingsFile = true;
                            break;
                        }
                }
            }
            return containRQMSettingsFile;
        }

        private string GetServerValueFromDict(Dictionary<string, object> dic)
        {
            if (dic.ContainsKey("ServerURL"))
                return (string)dic["ServerURL"];
            else return "";
        }
        #endregion

        public void UpdatedRQMTestInBF(ref BusinessFlow busFlow, RQMTestPlan testPlan, List<string> TCsIDs)
        {
            ImportFromRQM.UpdatedRQMTestInBF(ref busFlow, testPlan, TCsIDs);
        }

        public void UpdateBusinessFlow(ref BusinessFlow busFlow, RQMTestPlan testPlan)
        {
            ImportFromRQM.UpdateBusinessFlow(ref busFlow, testPlan);
        }
        public BusinessFlow ConvertRQMTestPlanToBF(RQMTestPlan testPlan)
        {
            return ImportFromRQM.ConvertRQMTestPlanToBF(testPlan);
        }
    }
}
