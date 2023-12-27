namespace WebApp.TestAutomation.CustomException
{
    public class NoDriverFound : Exception
    {
        /// <summary>
        /// This is a custom exception class for NoDriverFound.
        /// </summary>
        /// <param name="message"></param>
        public NoDriverFound(string message) : base(message)
        {

        }
    }
}
