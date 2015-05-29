using System;
using System.Threading.Tasks;

namespace ErrorControlSystem.Shared.UI.ExceptionMapper.Tests
{
    public static class TestClassD
    {
        public static void D1()
        {
            try
            {
                var value = Task.Run(() => TestClassE.E1()).Result;
            }
            catch (Exception exp)
            {
                throw new AggregateException(exp);
            }
        }
    }
}
