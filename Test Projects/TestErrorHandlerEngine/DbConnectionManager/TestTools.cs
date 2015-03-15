using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestErrorHandlerEngine.DbConnectionManager
{
    public static class TestTools
    {
        public static void ExpectException<T>(Action action) where T : Exception
        {
            try
            {
                action();
                Assert.Fail("Expected exception " + typeof(T));
            }
            catch (T)
            {
                // Expected
            }
        }

        public static void ExceptException<T>(Action action) where T : Exception
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (!(ex is T))
                    Assert.Fail("{0} occurred", ex.GetType());
            }
        }
    }
}
