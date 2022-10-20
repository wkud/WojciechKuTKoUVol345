using UnityEngine;
using UnityEngine.InputSystem;

namespace TKOU.SimAI
{
    [System.Serializable]
    public class PlayerInput
    {
        [Header("Input")]
        public InputActionReference moveMouseInput;
        public InputActionReference moveCameraInput;
        public InputActionReference contextInput;
        public InputActionReference returnInput;
        public InputActionReference selectInput;
        public InputActionReference zoomCameraInput;
    }
}
