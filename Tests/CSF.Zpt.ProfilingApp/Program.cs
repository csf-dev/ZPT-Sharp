//
// Program.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2018 Craig Fowler
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.IO;
using CSF.Screenplay.Reporting;
using CSF.Screenplay.Reporting.Html.Tests.Autofixture;
using CSF.Screenplay.Reporting.Models;
using Moq;
using Ploeh.AutoFixture;

namespace CSF.Zpt.ProfilingApp
{
  class MainClass
  {
    public static void Main(string[] args)
    {
      var doc = GetDocument();
      var model = GetModel();
      var result = doc.Render(model);

      Console.WriteLine(result);
    }

    static ReportDocument GetModel()
    {
      var report = GetReport();
      var formatter = GetObjectFormatter();
      return new ReportDocument(report, formatter);
    }

    static IZptDocument GetDocument()
    {
      var appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
      var appDir = Path.GetDirectoryName(appPath);
      var viewPath = Path.Combine(appDir, "ScreenplayView.pt");
      var viewFile = new FileInfo(viewPath);

      IZptDocumentFactory docFactory = new ZptDocumentFactory();
      return docFactory.CreateDocument(viewFile);
    }

    static Report GetReport()
    {
      var fixture = new Fixture();
      new ReportCustomisation().Customize(fixture);
      return fixture.Create<Report>();
    }

    static IObjectFormattingService GetObjectFormatter()
    {
      return Mock.Of<IObjectFormattingService>(x => x.Format(It.IsAny<object>()) == "Formatted object"
                                               && x.HasExplicitSupport(It.IsAny<object>()) == true);
    }
  }
}
