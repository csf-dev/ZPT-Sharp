using System;
using System.Collections.Generic;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Class which defines the initial state of the ZPT model.
  /// </summary>
  public class InitialModelState
  {
    #region properties

    /// <summary>
    /// Gets the local definitions to be pre-loaded into the TAL model.
    /// </summary>
    /// <value>The TAL local definitions.</value>
    public IDictionary<string,object> TalLocalDefinitions
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the global definitions to be pre-loaded into the TAL model.
    /// </summary>
    /// <value>The TAL global definitions.</value>
    public IDictionary<string,object> TalGlobalDefinitions
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the keyword options to be pre-loaded into the TAL model.
    /// </summary>
    /// <value>The TAL keyword options.</value>
    public IDictionary<string,object> TalKeywordOptions
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the local definitions to be pre-loaded into the METAL model.
    /// </summary>
    /// <value>The METAL local definitions.</value>
    public IDictionary<string,object> MetalLocalDefinitions
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the global definitions to be pre-loaded into the METAL model.
    /// </summary>
    /// <value>The METAL global definitions.</value>
    public IDictionary<string,object> MetalGlobalDefinitions
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the keyword options to be pre-loaded into the METAL model.
    /// </summary>
    /// <value>The METAL keyword options.</value>
    public IDictionary<string,object> MetalKeywordOptions
    {
      get;
      private set;
    }

    #endregion

    #region methods

    /// <summary>
    /// Populates a METAL model from the state of the current instance.
    /// </summary>
    /// <param name="model">Model.</param>
    public void PopulateMetalModel(Model model)
    {
      if(model == null)
      {
        throw new ArgumentNullException(nameof(model));
      }

      foreach(string key in this.MetalLocalDefinitions.Keys)
      {
        model.AddLocal(key, this.MetalLocalDefinitions[key]);
      }
      foreach(string key in this.MetalGlobalDefinitions.Keys)
      {
        model.AddGlobal(key, this.MetalGlobalDefinitions[key]);
      }
    }

    /// <summary>
    /// Populates a TAL model from the state of the current instance.
    /// </summary>
    /// <param name="model">Model.</param>
    public void PopulateTalModel(Model model)
    {
      if(model == null)
      {
        throw new ArgumentNullException(nameof(model));
      }

      foreach(string key in this.TalLocalDefinitions.Keys)
      {
        model.AddLocal(key, this.TalLocalDefinitions[key]);
      }
      foreach(string key in this.TalGlobalDefinitions.Keys)
      {
        model.AddGlobal(key, this.TalGlobalDefinitions[key]);
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.InitialModelState"/> class.
    /// </summary>
    public InitialModelState()
    {
      this.TalLocalDefinitions = new Dictionary<string, object>();
      this.TalGlobalDefinitions = new Dictionary<string, object>();
      this.TalKeywordOptions = new Dictionary<string, object>();
      this.MetalLocalDefinitions = new Dictionary<string, object>();
      this.MetalGlobalDefinitions = new Dictionary<string, object>();
      this.MetalKeywordOptions = new Dictionary<string, object>();
    }

    #endregion
  }
}

