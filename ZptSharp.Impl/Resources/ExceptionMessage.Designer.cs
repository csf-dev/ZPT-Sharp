﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZptSharp.Resources {
    using System;
    using System.Reflection;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ExceptionMessage {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExceptionMessage() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("ZptSharp.Resources.ExceptionMessage", typeof(ExceptionMessage).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static string CannotGetReaderWriterForFile {
            get {
                return ResourceManager.GetString("CannotGetReaderWriterForFile", resourceCulture);
            }
        }
        
        internal static string CannotConvertEvaluatedResult {
            get {
                return ResourceManager.GetString("CannotConvertEvaluatedResult", resourceCulture);
            }
        }
        
        internal static string MacroNotFound {
            get {
                return ResourceManager.GetString("MacroNotFound", resourceCulture);
            }
        }
        
        internal static string ExpressionTypeMustNotBeNullOrEmpty {
            get {
                return ResourceManager.GetString("ExpressionTypeMustNotBeNullOrEmpty", resourceCulture);
            }
        }
        
        internal static string NoEvaluatorForExpressionType {
            get {
                return ResourceManager.GetString("NoEvaluatorForExpressionType", resourceCulture);
            }
        }
        
        internal static string EvaluatorAlreadyRegistered {
            get {
                return ResourceManager.GetString("EvaluatorAlreadyRegistered", resourceCulture);
            }
        }
        
        internal static string EvaluatorTypeMustImplementInterface {
            get {
                return ResourceManager.GetString("EvaluatorTypeMustImplementInterface", resourceCulture);
            }
        }
        
        internal static string PathPartCannotBeEmpty {
            get {
                return ResourceManager.GetString("PathPartCannotBeEmpty", resourceCulture);
            }
        }
        
        internal static string AlternatePathExpressionCannotBeEmpty {
            get {
                return ResourceManager.GetString("AlternatePathExpressionCannotBeEmpty", resourceCulture);
            }
        }
        
        internal static string CannotParsePath {
            get {
                return ResourceManager.GetString("CannotParsePath", resourceCulture);
            }
        }
        
        internal static string InvalidVariableName {
            get {
                return ResourceManager.GetString("InvalidVariableName", resourceCulture);
            }
        }
        
        internal static string InvalidPathPart {
            get {
                return ResourceManager.GetString("InvalidPathPart", resourceCulture);
            }
        }
        
        internal static string CannotEvaluatePathExpressionAggregate {
            get {
                return ResourceManager.GetString("CannotEvaluatePathExpressionAggregate", resourceCulture);
            }
        }
        
        internal static string CannotEvaluatePathExpressionSingle {
            get {
                return ResourceManager.GetString("CannotEvaluatePathExpressionSingle", resourceCulture);
            }
        }
        
        internal static string CannotTraversePathPart {
            get {
                return ResourceManager.GetString("CannotTraversePathPart", resourceCulture);
            }
        }
    }
}
