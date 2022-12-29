using System;
using System.Collections.Generic;
using UnityEngine;

public class VehicleDynamics : VehicleComponent
{
    [SerializeField]
    private Rigidbody m_VehicleRigidbody;
    [SerializeField]
    private Wheel m_WheelFL;
    [SerializeField]
    private Wheel m_WheelFR;
    [SerializeField]
    private Wheel m_WheelRL;
    [SerializeField]
    private Wheel m_WheelRR;

    [SerializeField]
    private float m_OperatingAngle;

    [SerializeField]
    private bool m_AntiRoll;
    [SerializeField]
    [Range(0, 1)]
    private float m_AntiRollStrength;

    [SerializeField]
    private bool m_AntiLockBraking;

    // Steer
    private float m_TargetSteerAngle;
    private float m_CurrentSteerAngle;

    // Chassis
    private float m_WheelBase;
    private float m_Track;
    private float m_OriginalWheelRadius;

    // Rack
    private const float k_MaxRackPosition = 0.093f;
    private float m_CurrentRackPosition;

    // Drivetrain
    private float m_PropulsiveDir;
    private WheelTorque m_WheelTorque = new();

    // Anti-roll bar
    private float m_AntiRollStiffness;
    private float m_TravelLeft;
    private float m_TravelRight;
    private float m_AntiRollForce;

    // Anti-brake system
    private readonly Dictionary<WheelCollider, float> m_ABSFactor = new();

    private Action<WheelTorque> m_WheelTorqueAction;
    private Action<int> m_PropulsiveDirAction;
    private Action<float> m_UserSteeringInputAction;

    protected override void Awake()
    {
        base.Awake();
        m_WheelBase = (m_WheelFL.wheelRoot.position - m_WheelRL.wheelRoot.position).magnitude;
        m_Track = (m_WheelFL.wheelRoot.position - m_WheelFR.wheelRoot.position).magnitude;
    }

    void Start()
    {
        m_OriginalWheelRadius = m_WheelFL.collider.radius;

        m_ABSFactor[m_WheelFL.collider] = 1;
        m_ABSFactor[m_WheelFR.collider] = 1;
        m_ABSFactor[m_WheelRL.collider] = 1;
        m_ABSFactor[m_WheelRR.collider] = 1;

        m_WheelFR.collider.ConfigureVehicleSubsteps(1f, 75, 50);
    }

    void OnEnable()
    {
        if (m_WheelTorqueAction == null)
            m_WheelTorqueAction = (v) => m_WheelTorque = v;

        Vehicle.WheelTorque.SubscribeImmediate(m_WheelTorqueAction);

        if (m_PropulsiveDirAction == null)
            m_PropulsiveDirAction = (v) => m_PropulsiveDir = v;

        Vehicle.PropulsiveDirection.SubscribeImmediate(m_PropulsiveDirAction);

        if (m_UserSteeringInputAction == null)
            m_UserSteeringInputAction = (v) => m_TargetSteerAngle = v;

        Vehicle.UserSteering.SubscribeImmediate(m_UserSteeringInputAction);
    }

    void OnDisable()
    {
        if (m_WheelTorqueAction != null)
            Vehicle.WheelTorque.Unsubscribe(m_WheelTorqueAction);
        if (m_PropulsiveDirAction != null)
            Vehicle.PropulsiveDirection.Unsubscribe(m_PropulsiveDirAction);
        if (m_UserSteeringInputAction != null)
            Vehicle.UserSteering.Unsubscribe(m_UserSteeringInputAction);
    }

    void FixedUpdate()
    {
        SetWheelTorque(m_WheelFL.collider, m_WheelTorque.fL);
        SetWheelTorque(m_WheelFR.collider, m_WheelTorque.fR);
        SetWheelTorque(m_WheelRL.collider, m_WheelTorque.rL);
        SetWheelTorque(m_WheelRR.collider, m_WheelTorque.rR);
        ApplyRackForce();

        // Anti-roll
        if (m_AntiRoll)
        {
            m_AntiRollStiffness = m_WheelFL.collider.suspensionSpring.spring * m_AntiRollStrength;

            // Front axle
            m_TravelLeft = GetSuspensionTravel(m_WheelFL.collider);
            m_TravelRight = GetSuspensionTravel(m_WheelFR.collider);
            m_AntiRollForce = (m_TravelLeft - m_TravelRight) * m_AntiRollStiffness;

            if (m_WheelFL.collider.GetGroundHit(out _))
                m_VehicleRigidbody.AddForceAtPosition(m_WheelFL.collider.transform.up * -m_AntiRollForce, m_WheelFL.collider.transform.position);
            if (m_WheelFR.collider.GetGroundHit(out _))
                m_VehicleRigidbody.AddForceAtPosition(m_WheelFR.collider.transform.up * m_AntiRollForce, m_WheelFR.collider.transform.position);

            // Rear axle
            m_TravelLeft = GetSuspensionTravel(m_WheelRL.collider);
            m_TravelRight = GetSuspensionTravel(m_WheelRR.collider);
            m_AntiRollForce = (m_TravelLeft - m_TravelRight) * m_AntiRollStiffness;
            if (m_WheelRL.collider.GetGroundHit(out _))
                m_VehicleRigidbody.AddForceAtPosition(m_WheelRL.collider.transform.up * -m_AntiRollForce, m_WheelRL.collider.transform.position);
            if (m_WheelRR.collider.GetGroundHit(out _))
                m_VehicleRigidbody.AddForceAtPosition(m_WheelRR.collider.transform.up * m_AntiRollForce, m_WheelRR.collider.transform.position);
        }

        // Update data
        Vehicle.SteeringWheelAngle.SetValue(m_CurrentSteerAngle * m_OperatingAngle);
    }

    private void SetWheelTorque(WheelCollider wheelCollider, float requestedTorque)
    {
        if (wheelCollider == null)
            return;

        float propTorque, brake;
        if (requestedTorque >= 0)
        {
            brake = 0f;
            propTorque = Mathf.Abs(requestedTorque);
        }
        else
        {
            brake = Mathf.Abs(requestedTorque);
            propTorque = 0f;
        }

        wheelCollider.GetGroundHit(out WheelHit hit);

        if (m_AntiLockBraking)
        {
            float torqueStep = 2f * Time.fixedDeltaTime;
            if (Mathf.Abs(hit.forwardSlip) * Mathf.Clamp01(brake) > 1.1f * wheelCollider.forwardFriction.asymptoteSlip)
            {
                m_ABSFactor[wheelCollider] = wheelCollider.rpm < 0.01 ? // Wheel not spinning
                    Mathf.Min(Mathf.Clamp01(m_ABSFactor[wheelCollider] - torqueStep), 0.0f) :
                    Mathf.Clamp01(m_ABSFactor[wheelCollider] - 2f * torqueStep);
                brake *= m_ABSFactor[wheelCollider];
            }
            else
            {
                m_ABSFactor[wheelCollider] = Mathf.Clamp01(m_ABSFactor[wheelCollider] + 10 * torqueStep);
                brake *= m_ABSFactor[wheelCollider];
            }
        }

        // Apply final torques
        propTorque = m_PropulsiveDir == -1 ? -propTorque : propTorque;
        wheelCollider.motorTorque = propTorque;
        wheelCollider.brakeTorque = brake;

        if (brake > 0)
            wheelCollider.forceAppPointDistance = Mathf.Lerp(wheelCollider.forceAppPointDistance, m_OriginalWheelRadius * 0.8f, 5f * Time.fixedDeltaTime);
        else
            wheelCollider.forceAppPointDistance = wheelCollider.forceAppPointDistance = Mathf.Lerp(wheelCollider.forceAppPointDistance, m_OriginalWheelRadius, 5f * Time.fixedDeltaTime);
    }

    private float GetSuspensionTravel(WheelCollider wheel)
    {
        float travel = 1.0f;
        if (wheel.GetGroundHit(out WheelHit hit))
            travel = (-wheel.transform.InverseTransformPoint(hit.point).y - wheel.radius) / wheel.suspensionDistance;
        return travel;
    }

    private void ApplyRackForce()
    {
        if (m_WheelFL.collider == null || m_WheelFR.collider == null || m_WheelRL.collider == null || m_WheelRR.collider == null)
            return;

        m_CurrentRackPosition = m_TargetSteerAngle * k_MaxRackPosition;
        m_CurrentSteerAngle = 6.5f * m_CurrentRackPosition;

        // Ackermann steering 
        float rightAngle = Mathf.Atan(2f * m_WheelBase * Mathf.Sin(m_CurrentSteerAngle) / (2f * m_WheelBase * Mathf.Cos(m_CurrentSteerAngle) - m_Track * Mathf.Sin(m_CurrentSteerAngle)));
        float leftAngle = Mathf.Atan(2f * m_WheelBase * Mathf.Sin(m_CurrentSteerAngle) / (2f * m_WheelBase * Mathf.Cos(m_CurrentSteerAngle) + m_Track * Mathf.Sin(m_CurrentSteerAngle)));
        float ackermannRatio = Mathf.Min(0.58f * Mathf.Abs(m_CurrentSteerAngle) / 0.605f, 1f);
        rightAngle = (1 - ackermannRatio) * m_CurrentSteerAngle + ackermannRatio * rightAngle;
        leftAngle = (1 - ackermannRatio) * m_CurrentSteerAngle + ackermannRatio * leftAngle;

        m_WheelFL.collider.steerAngle = leftAngle * 180f / Mathf.PI;
        m_WheelFR.collider.steerAngle = rightAngle * 180f / Mathf.PI;
    }
    
    [Serializable]
    public struct Wheel
    {
        public Transform wheelRoot;
        public WheelCollider collider;

        [HideInInspector]
        public Quaternion originalRotation;
        [HideInInspector]
        public Vector3 originalPosition;
    }
}
