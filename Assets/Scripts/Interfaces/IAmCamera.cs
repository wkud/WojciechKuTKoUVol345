namespace TKOU.SimAI
{
    public interface IAmCamera : IAmMovable, IAmZoomable
    {
        UnityEngine.Camera Camera { get; }
    }
}
