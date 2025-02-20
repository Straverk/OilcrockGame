using UnityEngine;
using GameSettings;
using static SaveUtils.SaveLoadersManager;
using SaveUtils;


namespace Player.Modules
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMove : MonoBehaviour
    {
        //[Header("Base")]
        [SerializeField] private CharacterController _ñharacterController;

        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Transform _cameraTransform;


        private void OnValidate()
        {
            _ñharacterController ??= GetComponent<CharacterController>();

            _playerTransform ??= transform;
            _cameraTransform ??= GetComponentInChildren<Camera>().transform;
        }


        private struct Input
        {
            public Vector3 move;
            public Vector2 playerRotate;
            public Vector2 cameraRotate;
            public float sprintModifier;
            public float crouchModifier;
            public bool jump;
        };
        private Input _input;


        private SaveLoadersManager _saveLoadersManager;
        private PlayerInputSystem _playerInput;
        private PlayerConfig _playerConfig;


        private float _playerVelocityY = 0;
        private bool _groundedPlayer;
        private Vector3 _move;



        public void Init(SaveLoadersManager saveLoadersManager, PlayerInputSystem inputSystem, PlayerConfig playerCharacteristics)
        {
            _saveLoadersManager = saveLoadersManager;
            _playerInput = inputSystem;
            _playerConfig = playerCharacteristics;


            _input.sprintModifier = 1;
            _input.crouchModifier = 1;


            _playerInput.SetMoveHandler(MoveHandler);

            _playerInput.SetSprintHandler(x => _input.sprintModifier =
                Mathf.Lerp(1, _playerConfig.SprintMoveModifier, x));

            _playerInput.SetCrouchHandler(x => _input.crouchModifier =
                Mathf.Lerp(1, _playerConfig.CrouchMoveModifier, x));

            _playerInput.SetJumpHandler(x => _input.jump = x > 0);

            _playerInput.SetLookHandler(LookHandler);
        }


        private void Gravity()
        {
            _groundedPlayer = _ñharacterController.isGrounded;
            if (_groundedPlayer)
            {
                _playerVelocityY = _playerConfig.SurfacePull * Time.deltaTime;

                if (_input.jump)
                {
                    _playerVelocityY = _playerConfig.JumpStrength;

                    _input.jump = false;
                }
            }
            else
            {
                _playerVelocityY += _playerConfig.GravityForce * Time.deltaTime;
            }
        }

        private void Move()
        {
            if (_input.move != Vector3.zero)
            {

                // Rotate vector toward player derection
                _move = Quaternion.AngleAxis(
                    _playerTransform.localRotation.eulerAngles.y,
                    Vector3.up) * _input.move;

                _move *= _input.sprintModifier * _input.crouchModifier;
            }
            else
                _move = Vector3.zero;

            _move += Vector3.up * _playerVelocityY;

            _ñharacterController.Move(_move * Time.deltaTime);
        }

        private void Look()
        {
            _playerTransform.Rotate(_input.playerRotate * Time.deltaTime);

            var look = _input.cameraRotate * Time.deltaTime;


            if (_input.cameraRotate.magnitude == 0)
                return;


            var cameraRot = _cameraTransform.localRotation.eulerAngles.x;

            if (cameraRot > 180)
                cameraRot -= 360;

            look.x = Mathf.Clamp(
                look.x,
                cameraRot - _playerConfig.MinAngleDown,
                cameraRot - _playerConfig.MaxAngleUp);

            _cameraTransform.Rotate(-look);
        }



        public void Update()
        {
            if (_playerInput == null)
                return;

            Gravity();
            Move();
            Look();
            //Debug.Log(_ñharacterController.isGrounded);
        }


        public void MoveHandler(Vector2 move)
        {
            move *= _playerConfig.MoveSpeed;
            _input.move = new Vector3(move.x, 0, move.y);

            //Vector3. input.move
        }

        public void LookHandler(Vector2 look)
        {
            look *= _saveLoadersManager.SettingsSaveLoader.Sensitivity;
            // Only Y
            _input.playerRotate = new Vector2(0, look.x);

            _input.cameraRotate = new Vector2(look.y, 0);
        }
    }
}