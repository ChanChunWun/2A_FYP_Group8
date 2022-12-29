using UnityEngine;

public class VehicleSteeringWheel : VehicleComponent
{
    [SerializeField]
    private Transform m_Visual;

    private Quaternion m_OriginalRotation;

    private void Start() => m_OriginalRotation = m_Visual.localRotation;

    private void Update() => m_Visual.localRotation = m_OriginalRotation * 
        Quaternion.Euler(0f, 0f, -Vehicle.SteeringWheelAngle.Value);
}
