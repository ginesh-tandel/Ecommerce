namespace Ordering.Application.Exceptions
{
    public class OrderNotFoundException : ApplicationException
    {
        public OrderNotFoundException() { }
        public OrderNotFoundException(string name, object key) : base($"Entity {name} - {key} is not found") { }
    }
}
