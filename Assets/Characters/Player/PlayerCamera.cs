using System.Linq;
using UnityEngine;
using Yarn.Unity;

public class PlayerCamera : MonoBehaviour
{
    public float CameraMinPosition;
    public float CameraMaxPosition;
    public float CameraMoveSpeed = 3.0f;

    private Player _activePlayer;
    private LevelInfo _levelInfo;
    private HUD HUD;
    private Vector3 _playerCameraOffset;
    private Camera _camera;
    private ScreenFader _screenFader;

    public void Awake()
    {
        _camera = GetComponent<Camera>();
        _activePlayer = GetActivePlayer();
        _playerCameraOffset = transform.position - _activePlayer.transform.position;
        _screenFader = GetComponent<ScreenFader>();
        HUD = GetComponentInChildren<HUD>();

        GetCameraPosition();

        Invoke(nameof(FadeCameraOut), 0.6f);
    }

    public Camera GetCamera()
    {
        return _camera;
    }

    public ScreenFader GetScreenFader()
    {
        return _screenFader;
    }

    [YarnCommand("fade_camera_out")]
    public void FadeCameraOut()
    {
        _screenFader.fromOutDelay = Constants.FadeDelay;
        _screenFader.fadeSpeed = Constants.FadeSpeed;
        _screenFader.fadeState = ScreenFader.FadeState.Out;
    }

    [YarnCommand("fade_camera_in")]
    public void FadeCameraIn()
    {
        _screenFader.fadeSpeed = Constants.FadeSpeed;
        _screenFader.fadeState = ScreenFader.FadeState.In;
    }

    public void FadeCameraOutShort()
    {
        _screenFader.fadeSpeed = Constants.FadeSpeedShort;
        _screenFader.fadeState = ScreenFader.FadeState.Out;
    }

    public void FadeCameraInShort()
    {
        _screenFader.fadeSpeed = Constants.FadeSpeedShort;
        _screenFader.fadeState = ScreenFader.FadeState.In;
    }

    public void LateUpdate()
    {
        if (_activePlayer == null) { return; }

        GetCameraPosition();
    }

    public void GetCameraPosition()
    {
        _playerCameraOffset = Vector3.Lerp(transform.position, _activePlayer.transform.position, CameraMoveSpeed * Time.deltaTime);
        _playerCameraOffset.x = Mathf.Clamp(_playerCameraOffset.x, CameraMinPosition, CameraMaxPosition);
        _playerCameraOffset.y = _activePlayer.transform.position.y + 2.2f;
        _playerCameraOffset.z = transform.position.z;

        transform.position = _playerCameraOffset;
    }

    public Player GetActivePlayer()
    {
        return FindObjectsOfType<Player>().Where(x => x.IsPlayerActive).First();
    }

    public void SwitchPlayer()
    {
        _activePlayer = GetActivePlayer();
        GetCameraPosition();
        HUD.ActivePlayer = _activePlayer;
        HUD.ChangeGUIColor();
    }
}

