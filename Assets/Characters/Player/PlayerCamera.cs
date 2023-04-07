using System.Linq;
using UnityEngine;
using Yarn.Unity;

public class PlayerCamera : MonoBehaviour
{
    public float CameraMinPosition;
    public float CameraMaxPosition;
    public float CameraMoveSpeed = 3.0f;
    private Player _activePlayer;

    private Camera _camera;

    private Vector3 _playerCameraOffset;
    private ScreenFader _screenFader;

    public void Start()
    {
        _camera = GetComponent<Camera>();
        _activePlayer = GetActivePlayer();
        _playerCameraOffset = transform.position - _activePlayer.transform.position;
        _screenFader = GetComponent<ScreenFader>();
        FadeCameraOut();
    }

    public Camera GetCamera()
    {
        return _camera;
    }

    [YarnCommand("fade_camera_out")]
    public void FadeCameraOut()
    {
        _screenFader.fadeState = ScreenFader.FadeState.Out;
        _screenFader.fromOutDelay = Constants.FadeDelay;
        _screenFader.fadeSpeed = Constants.FadeSpeed;
    }

    [YarnCommand("fade_camera_in")]
    public void FadeCameraIn()
    {
        _screenFader.fadeState = ScreenFader.FadeState.In;
        _screenFader.fadeSpeed = Constants.FadeSpeed;
    }

    public void LateUpdate()
    {
        if (_activePlayer == null) { return; }

        _playerCameraOffset = Vector3.Lerp(transform.position, _activePlayer.transform.position, CameraMoveSpeed * Time.deltaTime);
        _playerCameraOffset.x = Mathf.Clamp(_playerCameraOffset.x, CameraMinPosition, CameraMaxPosition);
        _playerCameraOffset.y = _activePlayer.transform.position.y + 2.5f;
        _playerCameraOffset.z = transform.position.z;

        transform.position = _playerCameraOffset;
    }

    public Player GetActivePlayer()
    {
        return FindObjectsOfType<Player>().Where(x => x.IsActive).First();
    }
}

