using UnityEngine;
using GameSettings;
using Player.Modules;
using UI;
using SaveUtils;


namespace Player
{
    [SelectionBase]
    public class BasicPlayer : MonoBehaviour
    {
        [SerializeField] private PlayerMove _playerMove;
        [SerializeField] private PlayerInteract _playerInteract;

        public void OnValidate()
        {
            _playerMove ??= GetComponent<PlayerMove>();
            _playerInteract ??= GetComponent<PlayerInteract>();
        }

        public void Init(SaveLoadersManager saveLoadersManager, PlayerInputSystem inputSystem, PlayerConfig playerConfig,
            UIManager uiManager)
        {
            _playerMove.Init(saveLoadersManager, inputSystem, playerConfig);

            _playerInteract.Init(inputSystem, playerConfig, uiManager);
        }
    }
}