namespace MefContribDemo.Interception
{
    public interface IStartable
    {
        bool IsStarted { get; }

        void Start();
    }
}