using System;
using System.Threading.Tasks;

namespace ErrorControlSystem.Shared.UI.ExceptionMapper.Tests
{
    public static class TestClassC
    {
        public static void C1()
        {
            try
            {
                TestClassD.D1();
            }
            catch (Exception exp)
            {
                throw new TaskSchedulerException(exp);
            }
        }
    }
}
