//  
//  MetalIntegrationTests.cs
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
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using CraigFowler.Web.ZPT;
using CraigFowler.Web.ZPT.Metal.Exceptions;
using CraigFowler.Web.ZPT.Mocks;
using CraigFowler.Web.ZPT.Tales;
using CraigFowler.Web.ZPT.Tales.Exceptions;
using NUnit.Framework;

namespace Test.CraigFowler.Web.ZPT.Metal
{
  [TestFixture]
  [Category("Integration")]
  public class MetalIntegrationTests
  {
    #region properties
    
    public ZptDocumentCollection Documents {
      get;
      private set;
    }
    
    public List<FileInfo> InputFiles {
      get;
      private set;
    }
    
    public List<FileInfo> OutputFiles {
      get;
      private set;
    }
    
    #endregion
    
    #region set up
    
    [TestFixtureSetUp]
    public void FixtureSetUp()
    {
      /* Create a ZptDocumentCollection that contains all of the input files, so that documents can refer to
       * each other.
       */
      DirectoryInfo basePath = new DirectoryInfo(Path.Combine(ConfigurationManager.AppSettings["test-data-path"],
                                                              "input"));
      if(!basePath.Exists)
      {
        throw new DirectoryNotFoundException(String.Format("Test data base path: '{0}' not found.",
                                                           basePath.FullName));
      }
      
      this.Documents = ZptDocumentCollection.CreateFromFilesystem(basePath, "*");
      
      // Get file info relating to all of the input and output files.
      this.InputFiles = GetTestFiles("input");
      this.OutputFiles = GetTestFiles("output");
    }
    
    #endregion
    
    #region tests
    
    [Test]
    public void TestRenderDocuments()
    {
//      bool failureFound = false;
      
      for(int i = 0; i < this.InputFiles.Count; i++)
      {
//        Console.WriteLine ("Testing document {0}", this.InputFiles[i].FullName);
        
        // Load the input document as well as the expected output
        IZptDocument document = ZptDocument.DocumentFactory(this.InputFiles[i]);
        string expectedOutput = File.ReadAllText(this.OutputFiles[i].FullName);
        ITemplateDocument template = document.GetTemplateDocument();
        string documentOutput;
        
        template.TalesContext.AddDefinition("mock", GetMockObject());
        template.TalesContext.AddDefinition("documents", this.Documents);
        
        try
        {
          documentOutput = template.Render();
        }
        catch(TraversalException ex)
        {
          foreach(TalesPath path in ex.Attempts.Keys)
          {
            Console.Error.WriteLine("Whilst processing {2}\n{0}: {1}",
                                    path.ToString(),
                                    ex.Attempts[path].Message,
                                    this.InputFiles[i].FullName);
          }
          throw;
        }
        catch(MetalException ex)
        {
          Console.Error.WriteLine("Whilst processing {0}\n{1}", this.InputFiles[i].FullName, ex.Message);
          throw;
        }
        
        try
        {
          Assert.AreEqual(expectedOutput, documentOutput, String.Format("Rendering test: {0}",
                                                                        this.InputFiles[i].FullName));
        }
        catch(AssertionException)
        {
          Console.WriteLine ("Rendered output:\n{0}\n\nExpected:\n{1}", documentOutput, expectedOutput);
          throw;
        }
      }
        
        // This is some more verbose debugging code that can be used to handle more complex failure scenarios.
//        try
//        {
//          documentOutput = template.Render();
//          
//          if(documentOutput != expectedOutput)
//          {
//            Console.WriteLine ("\n---\n{0}\nDid not render as expected\n---", this.OutputFiles[i].FullName);
//            Console.WriteLine ("Expected:\n{0}\nActual:\n{1}", expectedOutput, documentOutput);
//            failureFound = true;
//          }
//        }
//        catch(Exception ex)
//        {
//          Console.WriteLine ("\n---\n{0}\nAn exception was encountered\n---", this.OutputFiles[i].FullName);
//          Console.WriteLine (ex.ToString());
//          foreach(object key in ex.Data.Keys)
//          {
//            if((ex.Data[key] is Dictionary<string, MetalElement>) && key.ToString() == "Slots available")
//            {
//              Console.WriteLine ("** Available slots");
//              int dataIndex = 0;
//              
//              foreach(string item in ((Dictionary<string, MetalElement>) ex.Data[key]).Keys)
//              {
//                Console.WriteLine ("{0,-20}[{2}] : {1}", key.ToString(), item, dataIndex++);
//              }
//            }
//            else if(ex.Data[key] is IEnumerable)
//            {
//              int dataIndex = 0;
//              
//              foreach(object item in (IEnumerable) ex.Data[key])
//              {
//                Console.WriteLine ("{0,-20}[{2}] : {1}", key.ToString(), item.ToString(), dataIndex++);
//              }
//            }
//            else
//            {
//              Console.WriteLine ("{0,-20} : {1}", key.ToString(), ex.Data[key].ToString());
//            }
//          }
//          failureFound = true;
//        }
//      }
//      
//      if(failureFound)
//      {
//        Assert.Fail("One or more template documents did not render as expected.  Review the console output for filenames of failures.");
//      }
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
    
    /// <summary>
    /// <para>Creates and returns a sample mock object.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="MockObject"/>
    /// </returns>
    public MockObject GetMockObject()
    {
      MockObject output = new MockObject(true);
      
      output["first"] = "First test";
      output["second"] = "Second test";
      output["third"] = "Third test";
      output["fourth"] = "Fourth test";
      output.BooleanValue = false;
      
      return output;
    }
    
    #endregion
  }
}

