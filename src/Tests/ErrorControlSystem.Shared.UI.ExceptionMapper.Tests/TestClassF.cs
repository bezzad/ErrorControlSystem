namespace ErrorControlSystem.Shared.UI.ExceptionMapper.Tests
{
    public static class TestClassF
    {
        public static int F1(string value)
        {
            return F2(int.Parse(value), 0xB);
        }


        public static int F2(int a, int b)
        {
            //
            // Throw new divide by zero exception for test
            return b / a;
        }
    }
}
