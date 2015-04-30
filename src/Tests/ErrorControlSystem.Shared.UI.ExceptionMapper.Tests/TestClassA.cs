using System;

namespace ErrorControlSystem.Shared.UI.ExceptionMapper.Tests
{
    public static class TestClassA
    {
        public static void A1()
        {
            try
            {
                TestClassB.B1();
            }
            catch (Exception exp)
            {
                throw new Exception("Test Inner Exception Thrown...", exp);
            }
        }
    }
}
