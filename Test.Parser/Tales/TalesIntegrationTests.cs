
using System;
using NUnit.Framework;
using CraigFowler.Web.ZPT.Tales;
using CraigFowler.Web.ZPT.Tales.Expressions;
using CraigFowler.Web.ZPT.Mocks;

namespace Test.CraigFowler.Web.ZPT.Tales
{
  [TestFixture]
  [Category("Integration")]
  public class TalesIntegrationTests
  {
    #region fields
    
    private MockObject mock;
    private MockObjectWithConflict conflict;
    
    #endregion
    
    #region properties
    
    protected MockObject Mock
    {
      get {
        return mock;
      }
      set {
        mock = value;
      }
    }
    
    protected MockObjectWithConflict MockWithConflict
    {
      get {
        return conflict;
      }
      set {
        conflict = value;
      }
    }
    
    #endregion
    
    #region set up
    
    [SetUp]
    public void SetUpTests()
    {
      this.Mock = new MockObject(true);
      this.MockWithConflict = new MockObjectWithConflict();
    }
    
    #endregion
    
    #region tests
    
    [Test]
    public void TestParseSimplePathExpression()
    {
      TalesContext context;
      TalesExpression expression;
      object testObj;
      int testInt;
      
      this.Mock.InnerObject.IntegerValue = 2;
      
      context = new TalesContext();
      context.Aliases.Add("mock", this.Mock);
      
      expression = context.CreateExpression("mock/InnerObject/IntegerValue");
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(Int32), testObj, "Test object is correct type");
      testInt = (int) testObj;
      Assert.AreEqual(2, testInt, "Test integer has correct value");
    }
    
    [Test]
    public void TestParseSimplePathExpressionUsingAlias()
    {
      TalesContext context;
      TalesExpression expression;
      object testObj;
      int testInt;
      
      this.Mock.InnerObject.IntegerValue = 2;
      
      context = new TalesContext();
      context.Aliases.Add("mock", this.Mock);
      
      expression = context.CreateExpression("mock/inner/IntegerValue");
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(Int32), testObj, "Test object is correct type");
      testInt = (int) testObj;
      Assert.AreEqual(2, testInt, "Test integer has correct value");
    }
    
    [Test]
    [ExpectedException(ExceptionType = typeof(TalesException), ExpectedMessage = "Encountered duplicate TALES alias")]
    public void TestDuplicateAlias()
    {
      TalesContext context;
      TalesExpression expression;
      object testObj;
      
      context = new TalesContext();
      context.Aliases.Add("mock", this.MockWithConflict);
      
      expression = context.CreateExpression("mock/Duplicate");
      
      testObj = expression.GetValue();
      Assert.Fail("If we reach this point then the test failed");
      Assert.IsNull(testObj, "Not really a test, just prevents a compiler warning");
    }
    
    [Test]
    public void TestParsePathExpressionFromIndexer()
    {
      TalesContext context;
      TalesExpression expression;
      object testObj;
      string testString;
      
      context = new TalesContext();
      context.Aliases.Add("mock", this.Mock);
      
      expression = context.CreateExpression("mock/baz");
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(String), testObj, "Test object is correct type");
      testString = (string) testObj;
      Assert.AreEqual("sample", testString, "Test string has correct value");
    }
    
    [Test]
    public void TestAliasedMembersBeforeNormalMembers()
    {
      TalesContext context;
      TalesExpression expression;
      object testObj;
      string testString;
      
      context = new TalesContext();
      context.Aliases.Add("mock", this.MockWithConflict);
      
      expression = context.CreateExpression("mock/SomeProperty");
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(String), testObj, "Test object is correct type");
      testString = (string) testObj;
      Assert.AreEqual("bar", testString, "Test string has correct value");
    }
    
    [Test]
    public void TestNormalMemberBeforeIndexer()
    {
      TalesContext context;
      TalesExpression expression;
      object testObj;
      int testInteger;
      
      this.Mock["IntegerValue"] = "Some value";
      
      context = new TalesContext();
      context.Aliases.Add("mock", this.Mock);
      
      expression = context.CreateExpression("mock/IntegerValue");
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(Int32), testObj, "Test object is correct type");
      testInteger = (int) testObj;
      Assert.AreEqual(0, testInteger, "Test integer has correct value");
    }
    
    [Test]
    public void TestAliasBeforeIndexer()
    {
      TalesContext context;
      TalesExpression expression;
      object testObj;
      MockObject innerObj;
      
      this.Mock["inner"] = "Some value";
      this.Mock.InnerObject.IntegerValue = 20;
      
      context = new TalesContext();
      context.Aliases.Add("mock", this.Mock);
      
      expression = context.CreateExpression("mock/inner");
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(MockObject), testObj, "Test object is correct type");
      innerObj = (MockObject) testObj;
      Assert.AreEqual(20, innerObj.IntegerValue, "Test object is correct");
    }
    
    #endregion
  }
}
