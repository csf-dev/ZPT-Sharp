using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Default implementation of <see cref="ISourceInfoFactory"/>.
  /// </summary>
  public class SourceInfoFactory : ISourceInfoFactory
  {
    /// <summary>
    /// Creates and returns a <see cref="ISourceInfo"/> instance based on the given type name and information.
    /// </summary>
    /// <returns>A source info instance.</returns>
    /// <param name="typeAQN">The Assembly-qualified name for the <see cref="ISourceInfo"/> type.</param>
    /// <param name="info">The string representation of the source info.</param>
    public ISourceInfo CreateSourceInfo(string typeAQN, string info)
    {
      if(typeAQN == null)
      {
        throw new ArgumentNullException(nameof(typeAQN));
      }

      var desiredType = Type.GetType(typeAQN);
      if(desiredType == null)
      {
        string message = String.Format(Resources.ExceptionMessages.SourceInfoTypeMustExistFormat,
                                       typeAQN,
                                       typeof(ISourceInfo).Name);
        throw new ArgumentException(message, nameof(typeAQN));
      }

      return (ISourceInfo) Activator.CreateInstance(desiredType, new object[] { info });
    }
  }
}

