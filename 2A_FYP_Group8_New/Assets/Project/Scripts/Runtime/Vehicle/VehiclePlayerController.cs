using UnityEngine;
using UnityEngine.InputSystem;
using ActionMapType = InputReader.ActionMapType;

public class VehiclePlayerController : VehicleComponent
{
    [SerializeField]
    private bool m_BrakeToReverse;
    [SerializeField]
    private AnimationCurve m_ForwardTorque = AnimationCurve.Constant(0, 50, 2700);
    [SerializeField]
    private AnimationCurve m_ReverseTorque = AnimationCurve.Linear(0, 2700, 15, 0);
    [SerializeField]
    private float m_ThrottleTime = 1.0f;
    [SerializeField]
    private float m_SteerTime = 0.6f;

    private InputReader m_InputReader = default;
    private WheelTorque m_WheelTorque = new();
    private float m_AccerateInput, m_BrakeInput, m_SteeringInput;
    private float m_Throttle, m_Brake, m_Steering;
    private float m_TotalTorque;
    private const float k_MaxBrakeTorque = 8000;

    protected override void Awake()
    {
        base.Awake();
        m_InputReader = ScriptableObject.CreateInstance<InputReader>();
        m_InputReader.GameInput.devices = new[] { Keyboard.current };
    }

    private void Start()
        => m_InputReader.EnableInput(ActionMapType.Driver);

    private void OnEnable()
    {
        m_InputReader.DriverAccerateEvent += OnAccerate;
        m_InputReader.DriverBrakeEvent += OnBrake;
        m_InputReader.DriverSteeringEvent += OnSteering;
    }

    private void OnDisable()
    {
        m_InputReader.DriverAccerateEvent -= OnAccerate;
        m_InputReader.DriverBrakeEvent -= OnBrake;
        m_InputReader.DriverSteeringEvent -= OnSteering;
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        GUILayout.Label("Input");
        GUILayout.Label("Throttle: " + m_AccerateInput);
        GUILayout.Label("Brake: " + m_BrakeInput);
        GUILayout.Label("Steering: " + m_SteeringInput);
        GUILayout.Label("Vehicle");
        GUILayout.Label("Throttle: " + m_Throttle);
        GUILayout.Label("Brake: " + m_Brake);
        GUILayout.Label("Steering: " + m_Steering);
        GUILayout.Label("Torque: " + m_TotalTorque);
    }
#endif

    private void Update()
    {
        // Steering
        m_Steering = ControlLerp(m_Steering, m_SteeringInput, m_SteerTime, false);
        Vehicle.UserSteering.SetValue(m_Steering);

        // Accerate
        m_Throttle = ControlLerp(m_Throttle, m_AccerateInput, m_ThrottleTime);

        // Brake
        m_Brake = ControlLerp(m_Brake, m_BrakeInput, m_ThrottleTime);

        // Apply
        if (Vehicle.PropulsiveDirection.Value == 1)
        {
            if (m_Throttle >= 0 && m_Brake <= 0 && Vehicle.Velocity.Value > -1.5f)
                m_TotalTorque = Mathf.Min(m_ForwardTorque.Evaluate(Mathf.Abs(Vehicle.Velocity.Value)), -1800 + 7900 * m_Throttle - 9500 * m_Throttle * m_Throttle + 9200 * m_Throttle * m_Throttle * m_Throttle);
            else
            {
                m_TotalTorque = -m_Brake * k_MaxBrakeTorque;
                if (Mathf.Abs(Vehicle.Velocity.Value) < 0.01f && m_BrakeToReverse && m_Brake >= 0)
                    Vehicle.PropulsiveDirection.SetValue(-1);
            }
        }
        else if (Vehicle.PropulsiveDirection.Value == -1)
        {
            if (m_Brake >= 0 && m_Throttle <= 0 && Vehicle.Velocity.Value < 1.5f)
                m_TotalTorque = Mathf.Min(m_ReverseTorque.Evaluate(Mathf.Abs(Vehicle.Velocity.Value)), -1800 + 7900 * m_Brake - 9500 * m_Brake * m_Brake + 9200 * m_Brake * m_Brake * m_Brake);
            else
            {
                m_TotalTorque = -m_Throttle * k_MaxBrakeTorque;
                if (Mathf.Abs(Vehicle.Velocity.Value) < 0.01f && m_Throttle >= 0)
                    Vehicle.PropulsiveDirection.SetValue(1);
            }
        }
        else
        {
            m_TotalTorque = -9000;
            if (Mathf.Abs(Vehicle.Velocity.Value) < 1f)
            {
                if (m_Throttle > 0)
                    Vehicle.PropulsiveDirection.SetValue(1);
                else if (m_Brake > 0 && m_BrakeToReverse)
                    Vehicle.PropulsiveDirection.SetValue(-1);
            }
        }

        // Set the torque values for the four wheels.
        m_WheelTorque.fL = 1.4f * m_TotalTorque / 4f;
        m_WheelTorque.fR = 1.4f * m_TotalTorque / 4f;
        m_WheelTorque.rL = 0.6f * m_TotalTorque / 4f;
        m_WheelTorque.rR = 0.6f * m_TotalTorque / 4f;

        // Update the wheel torque data item with the new values. This is accessible to other scripts, such as chassis dynamics.
        Vehicle.WheelTorque.SetValue(m_WheelTorque);
    }

    private float ControlLerp(float current, float target, float speed, bool clamp = true)
    {
        if (target > current)
        {
            current += Time.deltaTime / speed;
            if (target < current)
                current = target;
        }
        else if (target < current)
        {
            current -= Time.deltaTime / speed;
            if (target > current)
                current = target;
        }

        if (clamp)
            current = Mathf.Clamp01(current);

        return current;
    }

    private void OnAccerate(float value) => m_AccerateInput = value;
    private void OnBrake(float value) => m_BrakeInput = value;
    private void OnSteering(float value) => m_SteeringInput = value;
}
