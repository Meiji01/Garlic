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

/*
 * Created by SharpDevelop.
 * User: Alan
 * Date: 6/8/2014
 * Time: 9:46 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace GingerSikuliStandard.sikuli_UTIL
{
	/// <summary>
	/// An exception thrown when a json_Result with a Result of FAIL is returned from the service, containing the error message.
	/// </summary>
	public class SikuliActionException : Exception
	{
		public SikuliActionException() : base()
		{
		}
		
		public SikuliActionException(ActionResult result, String message) : base("Result: " + result.ToString() + message)
		{
		}
	}
}
