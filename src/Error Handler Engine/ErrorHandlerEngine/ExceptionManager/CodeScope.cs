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

            if (!string.IsNullOrEmpty(AssemblyName))
            {
                frames = frames.Where(x =>
                            RemoveExtension(x.GetMethod().Module.Name)
                                .Equals(AssemblyName, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(ClassName))
            {
                frames = frames.Where(x =>
                            x.GetMethod().ReflectedType != null &&
                            x.GetMethod().ReflectedType.Name.Equals(ClassName, StringComparison.OrdinalIgnoreCase));
            }
            
            if (!string.IsNullOrEmpty(MethodName))
            {
                frames = frames.Where(x =>
                           x.GetMethod().Name.Equals(MethodName, StringComparison.OrdinalIgnoreCase));
            }

            return frames.Any();
        }

        private string RemoveExtension(string value)
        {
            int dotIndex = value.IndexOf('.');
            return value.Substring(0, dotIndex);
        }
    }
}
