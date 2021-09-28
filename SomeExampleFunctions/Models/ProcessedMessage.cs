namespace SomeExampleFunctions.Models
{
    public class ProcessedMessage: DispatchingMessage
    {
        public int Hash
        {
            get
            {
                return Secret?.GetHashCode() ?? -0;
            }
        }
    }
}
