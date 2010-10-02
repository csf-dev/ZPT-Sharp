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
			
			inputFiles = GetTestFiles("input");
			outputFiles = GetTestFiles("output");
			
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
				
				
				Assert.AreEqual(expectedOutput,
				                renderedOutput,
				                String.Format("Test rendering of '{0}' matches '{1}'",
				                              inputFilename,
				                              outputFilename));
			}
		}
		
		#region supporting methods
		
		/// <summary>
		/// <para>Gets a collection of the files within the given test data path.</para>
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

