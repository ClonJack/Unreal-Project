using UnityEngine;

namespace UnrealTeam.SB.GamePlay.CharacterController.Views
{
    public class LinkPlayer : MonoBehaviour
    {
        [SerializeField] private CharacterView _characterView;
        [SerializeField] private CameraView _cameraView;

        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";
        
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            ///   _cameraView.SetFollowTransform(_characterView.CameraFollowPoint);
        }

        private void Update()
        {
           /* if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
*/
            HandleCharacterInput();
        }

        private void LateUpdate()
        {
         //   HandleCameraInput();
        }

        private void HandleCameraInput()
        {
            // Create the look input vector for the camera
            float mouseLookAxisUp = UnityEngine.Input.GetAxis(MouseYInput);
            float mouseLookAxisRight = UnityEngine.Input.GetAxis(MouseXInput);
            Vector2 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

        //    _cameraView.UpdateWithInput(lookInputVector);
        }

        private void HandleCharacterInput()
        {
          /*  var characterInputs = new PlayerCharacterInputs
            {
                // Build the CharacterInputs struct
                MoveAxisForward = UnityEngine.Input.GetAxisRaw(VerticalInput),
                MoveAxisRight = UnityEngine.Input.GetAxisRaw(HorizontalInput),
                CameraRotation = _cameraView.transform.rotation,
                JumpDown = UnityEngine.Input.GetKeyDown(KeyCode.Space),
                CrouchDown = UnityEngine.Input.GetKeyDown(KeyCode.C),
                CrouchUp = UnityEngine.Input.GetKeyUp(KeyCode.C)
            };

            // Apply inputs to character
            _characterView.SetInputs(ref characterInputs);*/
        }
    }
}