namespace WebApp.TestAutomation.CustomException
{
    public class NoDriverFound : Exception
    {
        public NoDriverFound(string message) : base(message)
        {

        }
    }
}
