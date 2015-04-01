using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ErrorHandlerEngine.ExceptionManager
{
    public class CodeScope
    {
        public string AssemblyName = String.Empty;
        public string ClassName = String.Empty;
        public string MethodName = String.Empty;

        public CodeScope(string assemblyName, string className, string methodName)
        {
            AssemblyName = assemblyName;
            ClassName = className;
            MethodName = methodName;
        }

        /// <summary>
        /// Is call method from this code place.
        /// </summary>
        /// <param name="frames">The frames of call methods or exception stackTrace frames.</param>
        /// <returns></returns>
        public bool IsCallFromThisPlace(IEnumerable<StackFrame> frames)
        {
            if (frames == null || !frames.Any()) return false;

            bool a = !string.IsNullOrEmpty(AssemblyName),
                c = !string.IsNullOrEmpty(ClassName),
                m = !string.IsNullOrEmpty(MethodName);

            if (a && c && m)
            {
                return frames.Any(x =>
                    RemoveExtension(x.GetMethod().Module.Name).Equals(AssemblyName, StringComparison.OrdinalIgnoreCase) &&
                    x.GetMethod().ReflectedType != null && x.GetMethod().ReflectedType.Name.Equals(ClassName, StringComparison.OrdinalIgnoreCase) &&
                    x.GetMethod().Name.Equals(MethodName, StringComparison.OrdinalIgnoreCase));
            }
            else if (a && c)
            {
                return frames.Any(x =>
                   RemoveExtension(x.GetMethod().Module.Name).Equals(AssemblyName, StringComparison.OrdinalIgnoreCase) &&
                   x.GetMethod().ReflectedType != null && x.GetMethod().ReflectedType.Name.Equals(ClassName, StringComparison.OrdinalIgnoreCase));
            }
            else if (a && m)
            {
                return frames.Any(x =>
                   RemoveExtension(x.GetMethod().Module.Name).Equals(AssemblyName, StringComparison.OrdinalIgnoreCase) &&
                   x.GetMethod().Name.Equals(MethodName, StringComparison.OrdinalIgnoreCase));
            }
            else if (a)
            {
                return frames.Any(x => RemoveExtension(x.GetMethod().Module.Name).Equals(AssemblyName, StringComparison.OrdinalIgnoreCase));
            }
            else if (c & m)
            {
                return frames.Any(x =>
                   x.GetMethod().ReflectedType != null && x.GetMethod().ReflectedType.Name.Equals(ClassName, StringComparison.OrdinalIgnoreCase) &&
                   x.GetMethod().Name.Equals(MethodName, StringComparison.OrdinalIgnoreCase));
            }
            else if (c)
            {
                return frames.Any(x => x.GetMethod().ReflectedType != null && x.GetMethod().ReflectedType.Name.Equals(ClassName, StringComparison.OrdinalIgnoreCase));
            }
            else if (m)
            {
                return frames.Any(x => x.GetMethod().Name.Equals(MethodName, StringComparison.OrdinalIgnoreCase));
            }

            return false;
        }

        private string RemoveExtension(string value)
        {
            int dotIndex = value.IndexOf('.');
            return value.Substring(0, dotIndex);
        }
    }
}
