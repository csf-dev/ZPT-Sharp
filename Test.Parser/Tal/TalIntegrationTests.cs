using System;
using NUnit.Framework;
using System.IO;
using System.Collections.Generic;
using CraigFowler.Web.ZPT.Tal;
using System.Configuration;
using System.Xml;
using System.Text;
using CraigFowler.Web.ZPT.Mocks;

namespace Test.CraigFowler.Web.ZPT.Tal
{
	[TestFixture]
	[Description("These are a batch of tests imported from the Zope source code.")]
	[Category("Integration")]
	public class TalIntegrationTests
	{
		[Test]
		public void TestRenderDocuments()
		{
			List<FileInfo> inputFiles, outputFiles;
			
			GetTestFiles(out inputFiles, out outputFiles);
			
			for(int i = 0; i < inputFiles.Count; i++)
			{
				TalDocument document = new TalDocument();
				string
					inputFilename = inputFiles[i].FullName,
					outputFilename = outputFiles[i].FullName,
					expectedOutput,
					renderedOutput = null;
				StringBuilder renderingBuilder;
				MockObject mock = new MockObject(true);
				
				// Load the input document and also the expected output
				document.Load(inputFilename);
				expectedOutput = File.ReadAllText(outputFilename);
				
				// Configure the mock object
				document.TalesContext.AddDefinition("mock", mock);
				mock["first"] = "First test";
				mock["second"] = "Second test";
				mock["third"] = "Third test";
				mock["fourth"] = "Fourth test";
				mock.BooleanValue = false;
				
				try
				{
					renderingBuilder = new StringBuilder();
					
					using(TextWriter writer = new StringWriter(renderingBuilder))
					{
						using(XmlWriter xmlWriter = new XmlTextWriter(writer))
						{
							xmlWriter.Settings.Indent = true;
							xmlWriter.Settings.IndentChars = "  ";
							xmlWriter.Settings.NewLineChars = "\n";
							
							document.Render(xmlWriter);
						}
					}
					
					renderedOutput = renderingBuilder.ToString();
				}
				catch(Exception ex)
				{
					Console.WriteLine (ex.ToString());
					Assert.Fail(String.Format("Encountered an exception whilst rendering file '{0}'.", inputFilename));
				}
				
				if(renderedOutput != expectedOutput)
				{
					Console.WriteLine ("Expected:\n{0}\n\nActual:\n{1}", expectedOutput, renderedOutput);
				}
				
				Assert.AreEqual(expectedOutput,
				                renderedOutput,
				                String.Format("Test rendering of '{0}' matches '{1}'", inputFilename, outputFilename));
			}
		}
		
		[Test]
		public void TestLoadDocuments()
		{
			List<FileInfo> inputFiles, outputFiles;
			
			GetTestFiles(out inputFiles, out outputFiles);
			
			for(int i = 0; i < inputFiles.Count; i++)
			{
				TalDocument document = new TalDocument();
				string inputFilename = inputFiles[i].FullName;
				document.Load(inputFilename);
				Assert.IsNotNull(document);
			}
		}
		
		[Test]
		[Description("This test checks the integrity of the XML document - for some reason the div element seems to " +
								 "have fewer children than it should?")]
		public void TestDocumentIntegrity()
		{
			string testFilename;
			TalDocument talDoc = new TalDocument();
			XmlDocument xmlDoc = new XmlDocument();
			XmlElement node;
			
			if(ConfigurationManager.AppSettings["test-data-path"] == null)
			{
				throw new InvalidOperationException("Configuration location is null.");
			}
			
			testFilename = Path.Combine(ConfigurationManager.AppSettings["test-data-path"],
			                            "input/testTalDocumentWithMockObject.xhtml");
			
			if(!File.Exists(testFilename))
			{
				throw new FileNotFoundException("The test file was not found", testFilename);
			}
			
			xmlDoc.Load(testFilename);
			
			node = (XmlElement) xmlDoc.GetElementsByTagName("div")[0];
			
			if(node == null)
			{
				throw new InvalidOperationException("The target XML node is null.");
			}
			
			Assert.AreEqual(5, node.ChildNodes.Count, "Correct number of child nodes - XML document");
			
			talDoc.Load(testFilename);
			node = (XmlElement) talDoc.GetElementsByTagName("div")[0];
			
			if(node == null)
			{
				throw new InvalidOperationException("The target TAL node is null.");
			}
			
			Assert.AreEqual(5, node.ChildNodes.Count, "Correct number of child nodes - TAL document");
		}
		
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
		/// <para>
		/// Overloaded.  Gets two collections of files containing test TAL documents and the expected renderings of
		/// those documents.
		/// </para>
		/// </summary>
		/// <param name="inputFiles">
		/// A <see cref="List<FileInfo>"/>
		/// </param>
		/// <param name="outputFiles">
		/// A <see cref="List<FileInfo>"/>
		/// </param>
		public void GetTestFiles(out List<FileInfo> inputFiles, out List<FileInfo> outputFiles)
		{
			inputFiles = GetTestFiles("input");
			outputFiles = GetTestFiles("output");
		}
		
		#endregion
	}
}

