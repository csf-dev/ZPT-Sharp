
using System;
using System.Reflection;
using NUnit.Framework;
using CraigFowler.Web.ZPT.Mocks;

namespace Test.CraigFowler.Web.ZPT.Mocks
{
  [TestFixture]
  public class TestMockObject
  {
    [Test]
    [Category("Information")]
    public void TestGetMembers()
    {
      MockObject mock = new MockObject(true);
      
      foreach(MemberInfo member in mock.GetType().GetMembers())
      {
        Console.WriteLine ("Member '{0}' is a '{1}'", member.Name, member.MemberType.ToString());
      }
    }
    
    [Test]
    [Category("Information")]
    public void TestGetIndexer()
    {
      MockObject mock = new MockObject(true);
      PropertyInfo property = (PropertyInfo) mock.GetType().GetMember("Item")[0];
      
      Console.WriteLine ("Property type: {0}", property.PropertyType);
      Console.WriteLine ("Number of parameters: {0}", property.GetGetMethod().GetParameters().Length);
    }
  }
}
