﻿using System;

namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Exception raised when a constructed instance of <see cref="IBatchRenderingOptions"/> is invalid.
  /// </summary>
  [Serializable]
  public class InvalidBatchRenderingOptionsException : BatchRenderingException
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:InvalidBatchRenderingOptionsException"/> class
    /// </summary>
    public InvalidBatchRenderingOptionsException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.BatchRendering.InvalidBatchRenderingOptionsException"/> class.
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception.</param>
    /// <param name="fatalError">A value indicating the nature of a fatal error.</param>
    public InvalidBatchRenderingOptionsException(string message,
                                                 BatchRenderingFatalErrorType fatalError) : base(message, fatalError)
    {
      
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:InvalidBatchRenderingOptionsException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    public InvalidBatchRenderingOptionsException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:InvalidBatchRenderingOptionsException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
    public InvalidBatchRenderingOptionsException(string message, Exception inner) : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:InvalidBatchRenderingOptionsException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected InvalidBatchRenderingOptionsException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
  }
}
