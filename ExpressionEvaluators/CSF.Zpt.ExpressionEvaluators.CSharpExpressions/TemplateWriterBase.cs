using System;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  internal abstract class TemplateWriterBase
  {

    private global::System.Text.StringBuilder builder;

    private global::System.Collections.Generic.IDictionary<string, object> session;

    private global::System.CodeDom.Compiler.CompilerErrorCollection errors;

    private string currentIndent = string.Empty;

    private global::System.Collections.Generic.Stack<int> indents;

    private ToStringInstanceHelper _toStringHelper = new ToStringInstanceHelper();

    protected internal virtual global::System.Collections.Generic.IDictionary<string, object> Session {
      get {
        return this.session;
      }
      set {
        this.session = value;
      }
    }

    protected internal global::System.Text.StringBuilder GenerationEnvironment {
      get {
        if ((this.builder == null)) {
          this.builder = new global::System.Text.StringBuilder();
        }
        return this.builder;
      }
      set {
        this.builder = value;
      }
    }

    protected global::System.CodeDom.Compiler.CompilerErrorCollection Errors {
      get {
        if ((this.errors == null)) {
          this.errors = new global::System.CodeDom.Compiler.CompilerErrorCollection();
        }
        return this.errors;
      }
    }

    protected internal string CurrentIndent {
      get {
        return this.currentIndent;
      }
    }

    private global::System.Collections.Generic.Stack<int> Indents {
      get {
        if ((this.indents == null)) {
          this.indents = new global::System.Collections.Generic.Stack<int>();
        }
        return this.indents;
      }
    }

    protected internal ToStringInstanceHelper ToStringHelper {
      get {
        return this._toStringHelper;
      }
    }

    protected internal void Error(string message) {
      this.Errors.Add(new global::System.CodeDom.Compiler.CompilerError(null, -1, -1, null, message));
    }

    protected internal void Warning(string message) {
      global::System.CodeDom.Compiler.CompilerError val = new global::System.CodeDom.Compiler.CompilerError(null, -1, -1, null, message);
      val.IsWarning = true;
      this.Errors.Add(val);
    }

    protected internal string PopIndent() {
      if ((this.Indents.Count == 0)) {
        return string.Empty;
      }
      int lastPos = (this.currentIndent.Length - this.Indents.Pop());
      string last = this.currentIndent.Substring(lastPos);
      this.currentIndent = this.currentIndent.Substring(0, lastPos);
      return last;
    }

    protected internal void PushIndent(string indent) {
      this.Indents.Push(indent.Length);
      this.currentIndent = (this.currentIndent + indent);
    }

    protected internal void ClearIndent() {
      this.currentIndent = string.Empty;
      this.Indents.Clear();
    }

    protected internal void Write(string textToAppend) {
      this.GenerationEnvironment.Append(textToAppend);
    }

    protected internal void Write(string format, params object[] args) {
      this.GenerationEnvironment.AppendFormat(format, args);
    }

    protected internal void WriteLine(string textToAppend) {
      this.GenerationEnvironment.Append(this.currentIndent);
      this.GenerationEnvironment.AppendLine(textToAppend);
    }

    protected internal void WriteLine(string format, params object[] args) {
      this.GenerationEnvironment.Append(this.currentIndent);
      this.GenerationEnvironment.AppendFormat(format, args);
      this.GenerationEnvironment.AppendLine();
    }

    public abstract string TransformText();

    public virtual void Initialize()
    {
      // Intentional no-op.
    }

    internal class ToStringInstanceHelper {

      private global::System.IFormatProvider formatProvider = global::System.Globalization.CultureInfo.InvariantCulture;

      protected internal global::System.IFormatProvider FormatProvider {
        get {
          return this.formatProvider;
        }
        set {
          if ((value != null)) {
            this.formatProvider = value;
          }
        }
      }

      protected internal string ToStringWithCulture(object objectToConvert) {
        if ((objectToConvert == null)) {
          throw new global::System.ArgumentNullException("objectToConvert");
        }
        global::System.Type type = objectToConvert.GetType();
        global::System.Type iConvertibleType = typeof(global::System.IConvertible);
        if (iConvertibleType.IsAssignableFrom(type)) {
          return ((global::System.IConvertible)(objectToConvert)).ToString(this.formatProvider);
        }
        global::System.Reflection.MethodInfo methInfo = type.GetMethod("ToString", new global::System.Type[] {
          iConvertibleType});
        if ((methInfo != null)) {
          return ((string)(methInfo.Invoke(objectToConvert, new object[] {
            this.formatProvider})));
        }
        return objectToConvert.ToString();
      }
    }
  }

}

