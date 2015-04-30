using System;

namespace ErrorControlSystem.Shared.UI.ExceptionMapper.Tests
{
    public static class TestClassE
    {
        public static string E1()
        {
            try
            {
                return TestClassF.F1("0").ToString();
            }
            catch (Exception exp)
            {
                throw new InvalidCastException("Cannot to get integer value", exp);
            }
        }
    }
}
