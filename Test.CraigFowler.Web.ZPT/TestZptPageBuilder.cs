//  
//  TestZptPageBuilder.cs
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
using NUnit.Framework;
using CraigFowler.Web.ZPT;
using System.Collections.Generic;
using System.IO;
using System.Configuration;

namespace Test.CraigFowler.Web.ZPT
{
  [TestFixture]
  public class TestZptPageBuilder
  {
    #region tests
    
    [Test]
    public void TestBuildPages()
    {
      DirectoryInfo testDataLocation;
      string
        testDataRoot = ConfigurationManager.AppSettings["test-data-path"],
        documentsSubdirectory = "page-build";
      
      testDataLocation = new DirectoryInfo(Path.Combine(testDataRoot, documentsSubdirectory));
      
      ZptPageBuilder builder = new ZptPageBuilder(testDataLocation.FullName);
      
      builder.BuildPages();
    }
    
    #endregion
    
    #region supporting methods
    
    /// <summary>
    /// <para>Overloaded.  Gets a collection of the files within the given test data path.</para>
    /// </summary>
    /// <param name="directoryPath">
    /// A <see cref="System.String"/>
    /// </param>
    /// <returns>
    /// A <see cref="List<FileInfo>"/>
    /// </returns>
    public List<FileInfo> GetTestFiles(string path)
    {
      string
        basePath = ConfigurationManager.AppSettings["test-data-path"],
        directoryPath;
      string[] filenames;
      List<FileInfo> output;
      
      if(!Directory.Exists(basePath))
      {
        throw new DirectoryNotFoundException("Test data base path not found.");
      }
      
      directoryPath = Path.Combine(basePath, path);
      
      if(!Directory.Exists(directoryPath))
      {
        throw new DirectoryNotFoundException("Test data subdirectory not found.");
      }
      
      filenames = Directory.GetFiles(directoryPath);
      Array.Sort(filenames);
      output = new List<FileInfo>();
      
      foreach(string filename in  filenames)
      {
        output.Add(new FileInfo(Path.Combine(directoryPath, filename)));
      }
      
      return output;
    }
    
    #endregion
  }
}

