using UnityEngine;
using UnityEngine.InputSystem;
using ActionMapType = InputReader.ActionMapType;

public class PassengerPlayerController : MonoBehaviour
{
    public float LookSensitivity = 1f;
    public float PitchLimit = 89f;

    [SerializeField]
    private Transform m_CameraRoot;

    [SerializeField]
    private PlayerTurretController m_TurretController;

    private InputReader m_InputReader = default;
    private Vector2 m_Look;
    private float m_CameraPitch;

    private void Awake()
    {
        m_InputReader = ScriptableObject.CreateInstance<InputReader>();
        m_InputReader.GameInput.devices = new[] { Mouse.current };
    }

    private void Start()
        => m_InputReader.EnableInput(ActionMapType.Passenger);

    private void OnEnable()
    {
        m_InputReader.PassengerLookEvent += OnLook;
        m_InputReader.PassengerReadyWeaponEvent += OnReadyWeapon;
        m_InputReader.PassengerFireWeaponEvent += OnFireWeapon;
    }

    private void OnDisable()
    {
        m_InputReader.PassengerLookEvent -= OnLook;
        m_InputReader.PassengerReadyWeaponEvent -= OnReadyWeapon;
        m_InputReader.PassengerFireWeaponEvent -= OnFireWeapon;
    }

    private void LateUpdate()
    {
        // Camera Look
        m_CameraPitch += m_Look.y * LookSensitivity;
        m_CameraPitch = ClampAngle(m_CameraPitch, -PitchLimit, PitchLimit);
        m_CameraRoot.transform.localRotation = Quaternion.Euler(m_CameraPitch, 0f, 0f);
        transform.Rotate(Vector3.up * (m_Look.x * LookSensitivity));
    }

    private void OnLook(Vector2 value) => m_Look = value;

    private void OnReadyWeapon(bool value) => m_TurretController.SetUsing(value);

    private void OnFireWeapon(bool value) => m_TurretController.Fire(value);

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f)
            lfAngle += 360f;
        if (lfAngle > 360f)
            lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
