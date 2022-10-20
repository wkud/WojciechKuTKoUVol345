using UnityEngine;

namespace TKOU.SimAI
{
    public interface IAmMovable
    {
        public void MoveBy(Vector2 delta);

        public void MoveTo(Vector2 position);
    }
}
