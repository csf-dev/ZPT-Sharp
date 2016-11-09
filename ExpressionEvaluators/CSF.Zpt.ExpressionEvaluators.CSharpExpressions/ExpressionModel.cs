using System;
using System.Collections.Generic;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  public class ExpressionModel
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

    public string[] PropertyNames
    {
      get;
      private set;
    }

    #endregion

    #region methods

    public string GetClassName()
    {
      return String.Format("ExpressionHost_{0}", ExpressionId);
    }

    #endregion

    #region constructor

    public ExpressionModel(int id, string text, string[] properties)
    {
      if(text == null)
      {
        throw new ArgumentNullException(nameof(text));
      }
      if(properties == null)
      {
        throw new ArgumentNullException(nameof(properties));
      }

      ExpressionId = id;
      ExpressionText = text;
      PropertyNames = properties;
    }

    #endregion
  }
}

