using GameSettings;
using GameSettings.Configs;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private Player.BasicPlayer _playerManager;
    [SerializeField] private UI.UIManager _UIManager;
    [SerializeField] private DaySystem _dayManager;

    [Header("Confings")]
    [SerializeField] private PlayerConfig _playerCharacteristics;
    [SerializeField] private AnnumsConfig _annumsConfig;


    private T TryGetOneGameObject<T>() where T : Object
    {
        T[] obj = FindObjectsByType<T>(FindObjectsSortMode.None);

        if (obj.Length == 0)
        {
            Debug.LogErrorFormat("No {0} objects in scene", obj.GetType().FullName);
            return null;
        }

        if (obj.Length > 1)
            Debug.LogErrorFormat("The {0} objects has more than 1", obj.GetType().FullName);

        return obj[0];
    }

    public void OnValidate()
    {
        _playerManager ??= TryGetOneGameObject<Player.BasicPlayer>();
        _UIManager ??= TryGetOneGameObject<UI.UIManager>();
        _dayManager ??= TryGetOneGameObject<DaySystem>();

        if (_playerCharacteristics == null)
            Debug.LogError(
                "Please create PlayerCharacteristics setting with the Context Menu: " +
                "Right mouse button -> Create -> Setting -> Player Ñharacteristics " +
                "if it has not already been created, and drag the setting to the Bootstrapper.");

        if (_annumsConfig == null)
            Debug.LogError(
                "Please create SeasonSettings setting with the Context Menu: " +
                "Right mouse button -> Create -> Setting -> Season Settings " +
                "if it has not already been created, and drag the setting to the Bootstrapper.");
    }



    private SaveUtils.SaveLoadersManager _saveLoadersManager;
    private PlayerInputSystem _inputSystem;



    private void Awake()
    {
        (_saveLoadersManager = new()).Init();

        // UI Init
        _UIManager.Init();
        _UIManager.ShowPanel(UI.UIPanelsTypes.HUD);

        // Day Init
        _annumsConfig ??= new();
        _dayManager.Init(_saveLoadersManager, _annumsConfig);

        // Player Init
        (_inputSystem ??= new()).Init();
        _playerCharacteristics ??= new();
        _playerManager.Init(_saveLoadersManager, _inputSystem, _playerCharacteristics, _UIManager);




    }
}