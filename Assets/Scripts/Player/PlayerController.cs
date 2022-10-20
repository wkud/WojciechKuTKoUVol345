using System;
using System.Collections.Generic;
using TKOU.SimAI.Highlights;
using TKOU.SimAI.Levels;
using TKOU.SimAI.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TKOU.SimAI
{
    /// <summary>
    /// Handles the player and his input.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput playerInput;

        private new IAmCamera camera;
        private IAmEntity selectedEntity;
        private IAmEntity hoveredEntity;
        private IAmEntity underCursorEntity;
        
        private bool isContextPressed = false;

        private const float moveScale = 0.04f;
        private const float zoomScale = 5f;

        private Dictionary<Type, System.Action<IAmEntity>> typeToSelectionHandler;

        private bool shouldUpdateEntityUnderCursor = false;

        private BuildHandler buildHandler;

        [SerializeField]
        private HighlightController highlightController;

        [SerializeField]
        private UIBuildingController uiBuildingController;

        [SerializeField]
        private UICashController uiCashController;
        
        public event System.Action<IAmEntity> OnHoverEntityE;
        public event System.Action<IAmEntity> OnSelectEntityE;

        private void Awake()
        {
            //Initialize input.
            playerInput.moveMouseInput.action.performed += PlayerInput_OnMoveMouse;
            playerInput.moveCameraInput.action.performed += PlayerInput_OnMoveCamera;
            playerInput.contextInput.action.performed += PlayerInput_OnContext;
            playerInput.returnInput.action.performed += PlayerInput_OnReturn;
            playerInput.selectInput.action.performed += PlayerInput_OnSelect;
            playerInput.zoomCameraInput.action.performed += PlayerInput_OnZoomCamera;

            playerInput.moveMouseInput.action.Enable();
            playerInput.moveCameraInput.action.Enable();
            playerInput.contextInput.action.Enable();
            playerInput.returnInput.action.Enable();
            playerInput.selectInput.action.Enable();
            playerInput.zoomCameraInput.action.Enable();

            //Initialize logic handling.

            typeToSelectionHandler = new Dictionary<Type, System.Action<IAmEntity>>();

            typeToSelectionHandler.Add(typeof(TileEntity), OnSelectEntity_TileEntity);
            typeToSelectionHandler.Add(typeof(BuildingEntity), OnSelectEntity_BuildingEntity);

            buildHandler = new BuildHandler(uiCashController);
        }

        private void OnDestroy()
        {
            playerInput.moveMouseInput.action.performed -= PlayerInput_OnMoveMouse;
            playerInput.moveCameraInput.action.performed -= PlayerInput_OnMoveCamera;
            playerInput.contextInput.action.performed -= PlayerInput_OnContext;
            playerInput.returnInput.action.performed -= PlayerInput_OnReturn;
            playerInput.selectInput.action.performed -= PlayerInput_OnSelect;
            playerInput.zoomCameraInput.action.performed -= PlayerInput_OnZoomCamera;
        }

        private void Update()
        {
            if (shouldUpdateEntityUnderCursor)
            {
                UpdateEntityUnderCursor();
                UpdateHoveredEntity();
                UpdateBuildHandler();
                shouldUpdateEntityUnderCursor = false;
            }
        }

        public void Initialize(IAmCamera camera)
        {
            uiBuildingController.buildSelectionTarget = buildHandler;
            this.camera = camera;
        }

        private void UpdateEntityUnderCursor()
        {
            Ray ray = camera.Camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit[] hits = Physics.RaycastAll(ray);

            underCursorEntity = null;

            if (hits.Length == 0)
            {
                return;
            }

            for (int i = 0; i < hits.Length; i++)
            {
                underCursorEntity = hits[i].collider.GetComponentInChildren<IAmEntity>();

                if (underCursorEntity != null)
                {
                    return;
                }

                Rigidbody body = hits[i].rigidbody;

                if (body == null)
                {
                    continue;
                }

                underCursorEntity = body.GetComponentInChildren<IAmEntity>();

                if (underCursorEntity != null)
                {
                    return;
                }
            }
        }

        private void UpdateHoveredEntity()
        {
            HoverEntity(underCursorEntity);
        }

        private void UpdateBuildHandler()
        {
            if (underCursorEntity == null)
            {
                return;
            }

            buildHandler.BuildTarget = underCursorEntity;
        }

        private void SelectEntity(IAmEntity entity)
        {
            if (selectedEntity == entity)
            {
                return;
            }

            selectedEntity = entity;

            if (selectedEntity == null)
            {
                return;
            }

            if (typeToSelectionHandler.TryGetValue(selectedEntity.GetType(), out System.Action<IAmEntity> logicAction))
            {
                logicAction(selectedEntity);
            }
            else
            {
                Debug.LogError($"Unhandled entity pressed: {selectedEntity.GetType()}");
            }
        }

        private void HoverEntity(IAmEntity entity)
        {
            if(hoveredEntity == entity)
            {
                return;
            }

            hoveredEntity = entity;

            highlightController.ClearAllHighlights();

            if(hoveredEntity != null)
            {
                highlightController.Highlight(entity);
            }

            OnHoverEntityE?.Invoke(hoveredEntity);

            Debug.Log($"Entity hovered: {hoveredEntity}");
        }

        private void PlayerInput_OnZoomCamera(InputAction.CallbackContext obj)
        {

            if (obj.phase != InputActionPhase.Performed)
            {
                return;
            }

            float zoom = obj.ReadValue<float>() * zoomScale * Time.deltaTime; ;

            camera?.Zoom(zoom);

            shouldUpdateEntityUnderCursor = true;
        }

        private void PlayerInput_OnSelect(InputAction.CallbackContext obj)
        {
            if (obj.phase != InputActionPhase.Performed)
            {
                return;
            }

            if (obj.ReadValueAsButton())
            {
                if(buildHandler.BuildSelection != null)
                {
                    buildHandler.AttemptToBuildSelection();
                }
                else
                {
                    SelectEntity(hoveredEntity);
                }

            }
        }

        private void PlayerInput_OnReturn(InputAction.CallbackContext obj)
        {
            if(buildHandler.BuildSelection != null)
            {
                buildHandler.BuildSelection = null;
            }
        }

        private void PlayerInput_OnContext(InputAction.CallbackContext obj)
        {

            if(obj.phase != InputActionPhase.Performed)
            {
                return;
            }

            isContextPressed = obj.ReadValueAsButton();
        }

        private void PlayerInput_OnMoveCamera(InputAction.CallbackContext obj)
        {
            if (obj.phase != InputActionPhase.Performed)
            {
                return;
            }

            if (!isContextPressed)
            {
                return;
            }

            Vector2 delta = obj.ReadValue<Vector2>() * moveScale;

            camera?.MoveBy(-delta);

            shouldUpdateEntityUnderCursor = true;
        }

        private void PlayerInput_OnMoveMouse(InputAction.CallbackContext obj)
        {

            if (obj.phase != InputActionPhase.Performed)
            {
                return;
            }

            Vector2 delta = obj.ReadValue<Vector2>() * moveScale;

            shouldUpdateEntityUnderCursor = true;
        }

        private void OnSelectEntity_TileEntity(IAmEntity tileEntity)
        {
            TileEntity entity = (TileEntity)tileEntity;

            OnSelectEntityE?.Invoke(tileEntity);
            Debug.Log($"Tile pressed! {entity}");
        }

        private void OnSelectEntity_BuildingEntity(IAmEntity buildingEntity)
        {
            BuildingEntity entity = (BuildingEntity)buildingEntity;

            OnSelectEntityE?.Invoke(buildingEntity);
            Debug.Log($"Building pressed! {entity}");
        }
    }
}
