using System;
using System.Collections.Generic;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  public class ExpressionHostBuilderModel
  {
    #region properties

    public int ExpressionId
    {
      get;
      private set;
    }

    public string ExpressionText
    {
      get;
      private set;
    }

    public IEnumerable<string> PropertyNames
    {
      get;
      private set;
    }

    #endregion

    #region constructor

    public ExpressionHostBuilderModel(int id, string text, IEnumerable<string> properties)
    {
      if(text == null)
      {
        throw new ArgumentNullException(nameof(text));
      }
      if(properties == null)
      {
        throw new ArgumentNullException(nameof(properties));
      }

      Id = id;
      ExpressionText = text;
      PropertyNames = properties;
    }

    #endregion
  }
}

