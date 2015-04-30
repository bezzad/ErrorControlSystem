using System;

namespace ErrorControlSystem.Shared.UI.ExceptionMapper.Tests
{
    public static class TestClassB
    {
        public static void B1()
        {
            try
            {
                TestClassC.C1();
            }
            catch (Exception exp)
            {
                throw new AggregateException(exp);
            }
        }
    }
}
