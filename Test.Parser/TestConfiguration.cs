//  
//  TestConfiguration.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2011 Craig Fowler
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;

namespace CraigFowler.Web.ZPT
{
  public class TestConfiguration
  {
    #region properties
    
    /// <summary>
    /// <para>Read-only.  Gets the root path to the test data.</para>
    /// </summary>
    public string TestDataRoot
    {
      get {
        return AppSettings["test-data-path"];
      }
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>
    /// Gets a <see cref="DirectoryInfo"/> that represents the <see cref="TestDataRoot"/>.  The returned instance is
    /// also checked to ensure that it exists.
    /// </para>
    /// </summary>
    /// <returns>
    /// A <see cref="DirectoryInfo"/>
    /// </returns>
    public DirectoryInfo GetTestDataDirectoryInfo()
    {
      DirectoryInfo output;
      
      if(String.IsNullOrEmpty(this.TestDataRoot))
      {
        throw new InvalidOperationException("Test data root is null or empty (missing or malformed config file?).");
      }
      
      output = new DirectoryInfo(this.TestDataRoot);
      
      if(!output.Exists)
      {
        string message = "The test data root was not found.";
        DirectoryNotFoundException ex = new DirectoryNotFoundException(message);
        ex.Data["Directory"] = output.FullName;
        
        throw ex;
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Creates a new <see cref="ZptDocumentCollection"/> from the test filesystem.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="ZptDocumentCollection"/>
    /// </returns>
    public ZptDocumentCollection GetTestDocuments()
    {
      DirectoryInfo basePath, testPath;
      
      basePath = GetTestDataDirectoryInfo();
      testPath = new DirectoryInfo(Path.Combine(basePath.FullName, "Document Root"));
      
      return ZptDocumentCollection.CreateFromFilesystem(testPath);
    }
    
    #endregion
    
    #region static properties
    
    /// <summary>
    /// <para>Read-only.  Gets the <c>AppSettings</c> collection of configuration items.</para>
    /// </summary>
    protected static NameValueCollection AppSettings
    {
      get;
      private set;
    }
    
    #endregion
    
    #region constructors
    
    /// <summary>
    /// <para>
    /// Static constructor sets the <see cref="AppSettings"/> section using <see cref="ConfigurationManager"/>.
    /// </para>
    /// </summary>
    static TestConfiguration ()
    {
      AppSettings = ConfigurationManager.AppSettings;
    }
    
    #endregion
  }
}

