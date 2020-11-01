using System;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Host
{
  internal partial class ExpressionHostTemplate
  {
    #region fields

    private ExpressionModel _model;

    #endregion

    #region methods

    protected IEnumerable<string> GetUsingNamespaces()
    {
      return _model.Specification.Namespaces
        .Where(x => !x.HasAlias)
        .Select(x => x.Namespace);
    }

    protected IEnumerable<Tuple<string,string>> GetAliasedUsingNamespaces()
    {
      return _model.Specification.Namespaces
        .Where(x => x.HasAlias)
        .Select(x => new Tuple<string,string>(x.Namespace, x.Alias));
    }

    protected IEnumerable<string> GetDynamicProperties()
    {
      return _model.Specification.Variables
        .Where(x => x.IsDynamicType)
        .Select(x => x.Name);
    }

    protected IEnumerable<Tuple<string,string>> GetTypedProperties()
    {
      return _model.Specification.Variables
        .Where(x => !x.IsDynamicType)
        .Select(x => new Tuple<string,string>(x.Name, x.TypeName));
    }

    protected string GetExpressionText()
    {
      return _model.Specification.Text;
    }

    protected string GetClassNamespace()
    {
      return _model.Namespace;
    }

    protected string GetClassName()
    {
      return _model.GetClassName();
    }

    #endregion

    #region constructor

    public ExpressionHostTemplate(ExpressionModel model)
    {
      if(model == null)
      {
        throw new ArgumentNullException(nameof(model));
      }

      _model = model;
    }

    #endregion
  }
}

