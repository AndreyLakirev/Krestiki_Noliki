namespace Network
{
    public interface IConsumer<in T>
    {
        void Consume(T message);
    }
}