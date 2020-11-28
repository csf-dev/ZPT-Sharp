using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ZptSharp
{
    [TestFixture, Parallelizable]
    public class AllExceptionTypesTests
    {
        [Test]
        public void All_custom_exception_types_must_derive_from_ZptRenderingException([ValueSource(nameof(GetAllCustomExceptionTypes))] Type exceptionType)
        {
            Assert.That(typeof(ZptRenderingException).IsAssignableFrom(exceptionType),
                        Is.True,
                        $"The custom exception type {exceptionType.Name} must derive from {typeof(ZptRenderingException).Name}");
        }

        public static IEnumerable<Type> GetAllCustomExceptionTypes()
        {
            var assemblies = new[] {
                // The abstractions assembly
                typeof(ZptRenderingException).Assembly,

                // The impl assembly
                typeof(ZptFileRenderer).Assembly,
            };

            return (from assembly in assemblies
                    from type in assembly.GetExportedTypes()
                    where
                        typeof(Exception).IsAssignableFrom(type)
                        && type != typeof(ZptRenderingException)
                    select type)
                .ToList();
        }
    }
}
