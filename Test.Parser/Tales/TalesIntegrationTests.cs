
using System;
using NUnit.Framework;
using CraigFowler.Web.ZPT.Tales;
using CraigFowler.Web.ZPT.Tales.Expressions;
using CraigFowler.Web.ZPT.Mocks;
using CraigFowler.Web.ZPT.Tales.Exceptions;

namespace Test.CraigFowler.Web.ZPT.Tales
{
  [TestFixture]
  [Category("Integration")]
  public class TalesIntegrationTests
  {
    #region fields
    
    private MockObject mock;
    private MockObjectWithConflict conflict;
    private MockWithoutIndexer noIndexer;
    
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
    
    protected MockWithoutIndexer MockWithoutIndexer
    {
      get {
        return noIndexer;
      }
      set {
        noIndexer = value;
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
      this.MockWithoutIndexer = new MockWithoutIndexer();
    }
    
    #endregion
    
    #region success tests
    
    [Test]
    public void TestParseSimplePathExpression()
    {
      TalesContext context;
      TalesExpression expression;
      object testObj;
      int testInt;
      
      this.Mock.InnerObject.IntegerValue = 2;
      
      context = new TalesContext();
      context.AddDefinition("mock", this.Mock);
      
      expression = context.CreateExpression("mock/InnerObject/IntegerValue");
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(Int32), testObj, "Test object is correct type");
      testInt = (int) testObj;
      Assert.AreEqual(2, testInt, "Test integer has correct value");
    }
    
    [Test]
    public void TestParseSimplePathExpressionWithVariableSubstitution()
    {
      TalesContext context;
      TalesExpression expression;
      object testObj;
      int testInt;
      
      this.Mock.InnerObject.IntegerValue = 2;
      
      context = new TalesContext();
      context.AddDefinition("mock", this.Mock);
      context.AddDefinition("myVar", "InnerObject");
      
      expression = context.CreateExpression("mock/?myVar/IntegerValue");
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(Int32), testObj, "Test object is correct type");
      testInt = (int) testObj;
      Assert.AreEqual(2, testInt, "Test integer has correct value");
    }
    
    [Test]
    public void TestParseTwoPathExpressions()
    {
      TalesContext context;
      TalesExpression expression;
      object testObj;
      int testInt;
      
      this.Mock.InnerObject.IntegerValue = 2;
      
      context = new TalesContext();
      context.AddDefinition("mock", this.Mock);
      
      expression = context.CreateExpression("zzinvalid/bar | mock/InnerObject/IntegerValue");
      
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
      context.AddDefinition("mock", this.Mock);
      
      expression = context.CreateExpression("mock/inner/IntegerValue");
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(Int32), testObj, "Test object is correct type");
      testInt = (int) testObj;
      Assert.AreEqual(2, testInt, "Test integer has correct value");
    }
    
    [Test]
    public void TestParsePathExpressionFromIndexer()
    {
      TalesContext context;
      TalesExpression expression;
      object testObj;
      string testString;
      
      context = new TalesContext();
      context.AddDefinition("mock", this.Mock);
      
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
      context.AddDefinition("mock", this.MockWithConflict);
      
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
      context.AddDefinition("mock", this.Mock);
      
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
      context.AddDefinition("mock", this.Mock);
      
      expression = context.CreateExpression("mock/inner");
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(MockObject), testObj, "Test object is correct type");
      innerObj = (MockObject) testObj;
      Assert.AreEqual(20, innerObj.IntegerValue, "Test object is correct");
    }
    
    [Test]
    public void TestSimpleStringExpression()
    {
      TalesContext context;
      TalesExpression expression;
      object testObj;
      string stringObj;
      
      context = new TalesContext();
      
      expression = context.CreateExpression("string:Hello Craig, how are you?");
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(string), testObj, "Test object is correct type");
      stringObj = (string) testObj;
      Assert.AreEqual("Hello Craig, how are you?", stringObj, "Test object is correct");
    }
    
    [Test]
    public void TestStringExpressionWithReplacement()
    {
      TalesContext context;
      TalesExpression expression;
      object testObj;
      string stringObj;
      
      context = new TalesContext();
      context.AddDefinition("mock", this.Mock);
      
      this.Mock["name"] = "Craig";
      this.Mock["weather"] = "sunny!";
      
      expression = context.CreateExpression("string:Hello ${mock/name}, how are you?  The weather is $mock/weather");
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(string), testObj, "Test object is correct type");
      stringObj = (string) testObj;
      Assert.AreEqual("Hello Craig, how are you?  The weather is sunny!", stringObj, "Test object is correct");
    }
    
    [Test]
    public void TestParsePathExpressionFromUnambiguousIndexer()
    {
      TalesContext context;
      TalesExpression expression;
      object testObj;
      string testString;
      
      context = new TalesContext();
      context.AddDefinition("mock", this.Mock);
      
      expression = context.CreateExpression("mock/unambiguous/baz");
      
      try
      {
        testObj = expression.GetValue();
      }
      catch(TraversalException ex)
      {
        Console.WriteLine (ex);
        
        foreach(TalesException attempt in ex.Attempts.Values)
        {
          Console.WriteLine (attempt.ToString());
          Console.WriteLine (attempt.Data["path"].ToString());
        }
        
        throw;
      }
      
      Assert.IsInstanceOfType(typeof(String), testObj, "Test object is correct type");
      testString = (string) testObj;
      Assert.AreEqual("sample", testString, "Test string has correct value");
    }
    
    [Test]
    public void TestEmptyPathString()
    {
      TalesContext context;
      TalesExpression expression;
      object testObj;
      
      context = new TalesContext();
      expression = context.CreateExpression("path:");
      
      testObj = expression.GetValue();
      
      Assert.IsNull(testObj, "The test object should come out null - an empty path string means a null return");
    }
    
    [Test]
    public void TestEmptyExpressionString()
    {
      TalesContext context;
      TalesExpression expression;
      object testObj;
      
      context = new TalesContext();
      expression = context.CreateExpression("");
      
      testObj = expression.GetValue();
      
      Assert.IsNull(testObj, "The test object should come out null - an empty path string means a null return");
    }
    
    [Test]
    public void TestEmptyStringExpression()
    {
      TalesContext context;
      TalesExpression expression;
      object testObj;
      string stringObj;
      
      context = new TalesContext();
      expression = context.CreateExpression("string:");
      
      testObj = expression.GetValue();
      
      Assert.IsNotNull(testObj, "Test object is not null");
      Assert.IsInstanceOfType(typeof(string), testObj, "Test object is correct type");
      stringObj = (string) testObj;
      Assert.AreEqual(String.Empty, stringObj, "String has correct value");
    }
    
    #endregion
    
    #region failure tests
    
    [Test]
    [ExpectedException(ExceptionType = typeof(TraversalException),
                       ExpectedMessage = "Could not traverse any of the given paths.")]
    public void TestDuplicateAlias()
    {
      TalesContext context;
      TalesExpression expression;
      object testObj;
      
      context = new TalesContext();
      context.AddDefinition("mock", this.MockWithConflict);
      
      expression = context.CreateExpression("mock/Duplicate");
      
      testObj = expression.GetValue();
      
      Console.WriteLine ("TestDuplicateAlias has failed, the object it found was: {0}", testObj);
      
      Assert.Fail("If we reach this point then the test failed");
      Assert.IsNull(testObj, "Not really a test, just prevents a compiler warning");
    }
    
    [Test]
    [ExpectedException(ExceptionType = typeof(TraversalException))]
    public void TestNonExistantRootObject()
    {
      TalesContext context;
      TalesExpression expression;
      object testObj;
      
      context = new TalesContext();
      expression = context.CreateExpression("foo/bar");
      
      testObj = expression.GetValue();
      Assert.Fail("If the test reaches this point then we failed");
      Assert.IsNull(testObj, "Not a real test, but prevents a compiler warning");
    }
    
    [Test]
    [ExpectedException(ExceptionType = typeof(TraversalException))]
    public void TestParseTwoInvalidPathExpressions()
    {
      TalesContext context;
      TalesExpression expression;
      object testObj;
      
      this.Mock.InnerObject.IntegerValue = 2;
      
      context = new TalesContext();
      context.AddDefinition("mock", this.Mock);
      
      expression = context.CreateExpression("zzinvalid/bar | foo/baz");
      
      try
      {
        testObj = expression.GetValue();
      }
      catch(TraversalException ex)
      {
        Assert.AreEqual(2, ex.Attempts.Count, "Correct number of attempts made");
        Assert.IsInstanceOfType(typeof(RootObjectNotFoundException),
                                ex.Attempts[new TalesPath("zzinvalid/bar")],
                                "First failed attempt is correct type");
        Assert.IsTrue(ex.PermanentError, "This problem is permanent because it relates to the root of the context");
        throw;
      }
      
      Assert.Fail("If the test reaches this point then we failed");
      Assert.IsNull(testObj, "Not a real test, but prevents a compiler warning");
    }
    
    [Test]
    [ExpectedException(ExceptionType = typeof(TraversalException))]
    public void TestParseTwoInvalidPathExpressionsNonPermanent()
    {
      TalesContext context;
      TalesExpression expression;
      object testObj;
      
      this.Mock.InnerObject.IntegerValue = 2;
      
      context = new TalesContext();
      context.AddDefinition("mock", this.Mock);
      
      expression = context.CreateExpression("zzinvalid/bar | mock/nonexistant/reallynonexistant");
      
      try
      {
        testObj = expression.GetValue();
      }
      catch(TraversalException ex)
      {
        Assert.AreEqual(2, ex.Attempts.Count, "Correct number of attempts made");
        Assert.IsInstanceOfType(typeof(PathException),
                                ex.Attempts[new TalesPath("mock/nonexistant/reallynonexistant")],
                                "Second failed attempt is correct type");
        Assert.IsFalse(ex.PermanentError, "This problem is not permanent because it's a dictionary.");
        throw;
      }
      
      Console.WriteLine ("TestParseTwoInvalidPathExpressionsNonPermanent failed: '{0}' was found.", testObj);
      
      Assert.Fail("If the test reaches this point then we failed");
      Assert.IsNull(testObj, "Not a real test, but prevents a compiler warning");
    }
    
    [Test]
    [ExpectedException(ExceptionType = typeof(TraversalException))]
    public void TestParseTwoInvalidPathExpressionsPermanent()
    {
      TalesContext context;
      TalesExpression expression;
      object testObj;
      
      this.Mock.InnerObject.IntegerValue = 2;
      
      context = new TalesContext();
      context.AddDefinition("mock", this.MockWithoutIndexer);
      
      expression = context.CreateExpression("zzinvalid/bar | mock/invalid");
      
      try
      {
        testObj = expression.GetValue();
      }
      catch(TraversalException ex)
      {
        Assert.AreEqual(2, ex.Attempts.Count, "Correct number of attempts made");
        Assert.IsInstanceOfType(typeof(PathInvalidException),
                                ex.Attempts[new TalesPath("mock/invalid")],
                                "Second failed attempt is correct type");
        Assert.IsTrue(ex.PermanentError, "Permanent because there was no way that the given member could exist.");
        throw;
      }
      
      Assert.Fail("If the test reaches this point then we failed");
      Assert.IsNull(testObj, "Not a real test, but prevents a compiler warning");
    }
    
    #endregion
  }
}
