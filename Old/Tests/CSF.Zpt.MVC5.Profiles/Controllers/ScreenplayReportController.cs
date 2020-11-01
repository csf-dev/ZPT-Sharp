using System.Web.Mvc;
using CSF.Screenplay.Reporting;
using CSF.Screenplay.Reporting.Html.Tests.Autofixture;
using CSF.Screenplay.Reporting.Models;
using Moq;
using Ploeh.AutoFixture;

namespace CSF.Zpt.MVC5.Profiles.Controllers
{
  public class ScreenplayReportController : Controller
  {
    public ActionResult Index()
    {
      var report = GetReport();
      var formatter = GetObjectFormatter();

      var model = new ReportDocument(report, formatter);

      return View(model);
    }

    Report GetReport()
    {
      var fixture = new Fixture();
      new ReportCustomisation().Customize(fixture);
      return fixture.Create<Report>();
    }

    IObjectFormattingService GetObjectFormatter()
    {
      return Mock.Of<IObjectFormattingService>(x => x.Format(It.IsAny<object>()) == "Formatted object"
                                                    && x.HasExplicitSupport(It.IsAny<object>()) == true);
    }
  }
}
