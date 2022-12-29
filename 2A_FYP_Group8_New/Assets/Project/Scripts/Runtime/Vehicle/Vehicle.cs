using UnityEngine;

public class Vehicle : MonoBehaviour
{
    private readonly Observable<int> m_PropulsiveDirection = new();
    public Observable<int> PropulsiveDirection => m_PropulsiveDirection;

    private readonly Observable<float> m_SteeringWheelAngle = new();
    public Observable<float> SteeringWheelAngle => m_SteeringWheelAngle;

    private readonly Observable<WheelTorque> m_WheelTorque = new();
    public Observable<WheelTorque> WheelTorque => m_WheelTorque;

    private readonly Observable<float> m_Velocity = new();
    public Observable<float> Velocity => m_Velocity;

    private readonly Observable<float> m_UserSteering = new();
    public Observable<float> UserSteering => m_UserSteering;
}

public abstract class VehicleComponent : MonoBehaviour
{
    private Vehicle m_Vehicle;
    protected Vehicle Vehicle => m_Vehicle;

    protected virtual void Awake() => m_Vehicle = GetComponentInParent<Vehicle>();
}
