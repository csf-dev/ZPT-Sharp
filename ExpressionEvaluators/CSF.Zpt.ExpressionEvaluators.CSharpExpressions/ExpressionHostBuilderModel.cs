using System;
using System.Collections.Generic;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  [Serializable]
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

    public string[] PropertyNames
    {
      get;
      private set;
    }

    #endregion

    #region constructor

    public ExpressionHostBuilderModel(int id, string text, string[] properties)
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

