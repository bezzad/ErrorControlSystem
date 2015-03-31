using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorHandlerEngine.ExceptionManager
{
    public class CodePlace
    {
        public string AssemblyName = String.Empty;
        public string ClassName = String.Empty;
        public string MethodName = String.Empty;

        public CodePlace(string assemblyName, string className, string methodName)
        {
            AssemblyName = assemblyName;
            ClassName = className;
            MethodName = methodName;
        }

        /// <summary>
        /// Is call method from this code place.
        /// </summary>
        /// <param name="skipCalls">Number of the skip calls from top of the stack.</param>
        /// <returns></returns>
        public bool IsCallFromThisPlace(int skipCalls)
        {
            return IsCallFromThisPlace(frames: new StackTrace().GetFrames().Skip(skipCalls));
        }

        /// <summary>
        /// Is call method from this code place.
        /// </summary>
        /// <param name="frames">The frames of call methods or exception stackTrace frames.</param>
        /// <returns></returns>
        public bool IsCallFromThisPlace(IEnumerable<StackFrame> frames)
        {
            if (frames == null || !frames.Any()) return false;

            IEnumerable<StackFrame> lstFiltering = null;
            //
            // Filter by Assembly Names
            if (!string.IsNullOrEmpty(AssemblyName))
            {
                lstFiltering = frames.Where(
                    x => RemoveExtension(x.GetMethod().Module.Name).Equals(AssemblyName, StringComparison.OrdinalIgnoreCase));
            }
            //
            // Filter by Class Names
            if (!string.IsNullOrEmpty(ClassName))
            {
                if (lstFiltering != null) // Before Filtered by Assembly Name
                {
                    lstFiltering = lstFiltering.Where(
                        x =>
                        {
                            var declaringType = x.GetMethod().DeclaringType;
                            return declaringType != null && declaringType.Name.Equals(ClassName, StringComparison.OrdinalIgnoreCase);
                        });
                }
                else // Not Assembly Name
                {
                    lstFiltering = frames.Where(
                        x =>
                        {
                            var declaringType = x.GetMethod().DeclaringType;
                            return declaringType != null && declaringType.Name.Equals(ClassName, StringComparison.OrdinalIgnoreCase);
                        });
                }
            }
            //
            // Filter by Method Names
            if (!string.IsNullOrEmpty(MethodName))
            {
                if (lstFiltering != null) // Before Filtered by Assembly Name or Class Name
                {
                    lstFiltering = lstFiltering.Where(
                        x => x.GetMethod().Name.Equals(MethodName, StringComparison.OrdinalIgnoreCase));
                }
                else // Not any filter before
                {
                    lstFiltering = frames.Where(
                        x => x.GetMethod().Name.Equals(MethodName, StringComparison.OrdinalIgnoreCase));
                }
            }

            return lstFiltering != null && lstFiltering.Any();
        }

        private string RemoveExtension(string value)
        {
            int dotIndex = value.IndexOf('.');
            return value.Substring(0, dotIndex);
        }
    }
}
