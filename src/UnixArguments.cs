using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CraigFowler
{
  public sealed class UnixArguments
  {
    private bool parsed;
    private Dictionary<string, ParameterDefinition> definitions;
    private Dictionary<string, List<Parameter>> parameters;
    private List<string> actions;
    private List<string> unsupported;
    
    private const string NOT_PARSED = "No arguments have been parsed yet";
    private const string ALREADY_PARSED = "Arguments have already been parsed";
    
    public UnixArguments()
    {
      // Constructor
      definitions = new Dictionary<string, ParameterDefinition>();
      actions = new List<string>();
      unsupported = new List<string>();
      parameters = new Dictionary<string, List<Parameter>>();
      parsed = false;
    }
    
    public string[] Actions
    {
      get {
        if(parsed)
          return actions.ToArray();
        else
          throw new InvalidOperationException(NOT_PARSED);
      }
    }
    
    public string[] Unrecognised
    {
      get {
        if(parsed)
          return unsupported.ToArray();
        else
          throw new InvalidOperationException(NOT_PARSED);
      }
    }
    
    public Parameter[] this[string key]
    {
      get {
        if(parsed)
          return parameters.ContainsKey(key) ? parameters[key].ToArray() : null;
        else
          throw new InvalidOperationException(NOT_PARSED);
      }
    }
    
    public bool Contains(string key)
    {
      if(parsed)
        return parameters.ContainsKey(key);
      else
        throw new InvalidOperationException(NOT_PARSED);
    }
    
    public void Parse(string[] commandLine)
    {
      if(parsed)
        throw new InvalidOperationException(ALREADY_PARSED);
      else
      {
        parseArguments(commandLine);
        parsed = true;
      }
    }
    
    public void RegisterParameter(string name)
    {
      registerParam(name, new string[0], false);
    }
    
    public void RegisterParameter(string name, string alias)
    {
      registerParam(name, new string[1] { alias }, false);
    }
    
    public void RegisterParameter(string name, bool takesVal)
    {
      registerParam(name, new string[0], takesVal);
    }
    
    public void RegisterParameter(string name, string[] aliases)
    {
      registerParam(name, aliases, false);
    }
    
    public void RegisterParameter(string name, string alias, bool takesVal)
    {
      registerParam(name, new string[1] { alias }, takesVal);
    }
    
    public void RegisterParameter(string name, string[] aliases, bool takesVal)
    {
      registerParam(name, aliases, takesVal);
    }
    
    private void registerParam(string name, string[] aliases, bool takesValue)
    {
      string exMsg = "Parameter name and any aliases must not be null or empty";
      
      if(String.IsNullOrEmpty(name))
        throw new ArgumentOutOfRangeException(exMsg);
      else
        definitions[name] = new ParameterDefinition(name, takesValue);
      
      foreach(string alias in aliases)
      {
        if(String.IsNullOrEmpty(alias))
          throw new ArgumentOutOfRangeException(exMsg);
        else
          definitions[alias] = new ParameterDefinition(name, takesValue);
      }
    }
    
    private void parseArguments(string[] args)
    {
      bool expectingValue = false;
      string current = null;
#if DEBUG
      string debugOut;
#endif
      string val;
      char[] shortChars;
      Regex matchLong = new Regex(@"^--([a-zA-Z0-9]+[a-zA-Z0-9_-]*)$",
                                  RegexOptions.Compiled);
      Regex matchShort = new Regex(@"^-([a-zA-Z0-9]+)$",
                                   RegexOptions.Compiled);
      Regex quoteRemover = new Regex(@"^['""]?([^'""]+)['""]?$",
                                     RegexOptions.Compiled);
      
      foreach(string argument in args)
      {
        if(matchLong.Match(argument).Success)
        {
          // Then we have found a long parameter or switch
          if(expectingValue && current != null)
          {
            current = storeWithValue(current, null, ref expectingValue);
          }
          else
          {
            current = null;
            expectingValue = false;
          }
          current = matchLong.Match(argument).Groups[1].Value;
#if DEBUG
          debugOut = "Matched a long argument start.  Argument name is '{0}'";
          Console.WriteLine(debugOut, current);
#endif
          current = tryStoreValue(current, ref expectingValue);
        }
        else if(matchShort.Match(argument).Success)
        {
          // Then we have found a short parameter or switch
          if(expectingValue && current != null)
          {
            current = storeWithValue(current, null, ref expectingValue);
          }
          else
          {
            current = null;
            expectingValue = false;
          }
          shortChars = matchShort.Match(argument).Groups[1].Value.ToCharArray();
          foreach(char character in shortChars)
          {
#if DEBUG
            debugOut = "Matched a short argument.  Argument name is '{0}'";
            Console.WriteLine(debugOut, character);
#endif
            if(expectingValue && current != null)
            {
              current = storeWithValue(current, null, ref expectingValue);
            }
            else
            {
              current = null;
              expectingValue = false;
            }
            current = character.ToString();
            current = tryStoreValue(current, ref expectingValue);
          }
        }
        else
        {
#if DEBUG
          debugOut = "Matched lone string: '{0}'.  Expected: {1}, alias: '{2}'";
          Console.WriteLine(debugOut, argument, expectingValue, current);
#endif
          // Then we have found a string on its own
          val = quoteRemover.Match(argument).Groups[1].Value;
          if(expectingValue && current != null)
          {
            current = storeWithValue(current, val, ref expectingValue);
          }
          else
          {
            actions.Add(val);
            current = null;
            expectingValue = false;
          }
        }
      }
    }
    
    private string tryStoreValue(string alias, ref bool expected)
    {
      string output;
      
      if(definitions.ContainsKey(alias))
      {
        if(definitions[alias].TakesValue)
        {
#if DEBUG
          Console.WriteLine("Found alias {0}, and we are expecting a value",
                            alias);
#endif
          expected = true;
          output = alias;
        }
        else
        {
#if DEBUG
          Console.WriteLine("Found alias {0}, and we are NOT expecting a value",
                            alias);
#endif
          expected = false;
          output = storeNonValue(alias);
        }
      }
      else
      {
        unsupported.Add(alias);
        expected = false;
        output = null;
      }
      
      return output;
    }
    
    /* If we were expecting a value and instead we come across a new parameter
     * then we store the old parameter with a null value
     */
    private string storeWithValue(string key, string val, ref bool expecting)
    {
      Parameter p;
      
      p = new Parameter(true);
      p.AliasUsed = key;
      p.Name = definitions[key].Name;
      p.Value = val;
      
      if(parameters.ContainsKey(p.Name) == false)
        parameters.Add(p.Name, new List<Parameter>());
      parameters[p.Name].Add(p);
      
      expecting = false;
      return null;
    }
    
    private string storeNonValue(string key)
    {
      Parameter p;
      
      p = new Parameter(false);
      p.AliasUsed = key;
      p.Name = definitions[key].Name;
      
      if(parameters.ContainsKey(p.Name) == false)
        parameters.Add(p.Name, new List<Parameter>());
      parameters[p.Name].Add(p);
      
      return null;
    }
    
    public struct ParameterDefinition
    {
      public string Name;
      public bool TakesValue;
      
      public ParameterDefinition(string name, bool takesValue)
      {
        Name = name;
        TakesValue = takesValue;
      }
    }
    
    public class Parameter
    {
      private bool takesValue;
      private string val;
      private string name;
      private string alias;
      
      public string Name
      {
        get {
          return name;
        }
        set {
          name = value;
        }
      }
      public string AliasUsed
      {
        get {
          return alias;
        }
        set {
          alias = value;
        }
      }
      
      public string Value
      {
        get {
          return takesValue ? val : null;
        }
        set {
          val = value;
        }
      }
      
      public Parameter(bool takesVal)
      {
        takesValue = takesVal;
        val = "";
        name = "";
        alias = "";
      }
    }
  }
}