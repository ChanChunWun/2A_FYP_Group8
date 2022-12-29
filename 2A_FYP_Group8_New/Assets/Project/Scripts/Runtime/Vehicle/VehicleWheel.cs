using UnityEngine;

public class VehicleWheel : MonoBehaviour
{
    [SerializeField]
    [Range(0, 10)]
    private float m_CasterAngle = 7;
    [SerializeField]
    [Range(5, 15)]
    private float m_SteeringAxisInclination = 10;
    [SerializeField]
    [Range(-0.05f, 0.05f)]
    private float m_ScrubRadius = -0.02f;
    [SerializeField]
    [Range(0, 0.15f)]
    private float m_Trail = 0.07f;

    private WheelCollider m_WheelCollider;
    private float m_WheelRadius;
    private float m_WheelMultiplier = 1f;
    private Transform m_Visual;
    private Quaternion m_InitialLocalRotation;

    private Vector3 m_PositionVector;
    private Vector3 m_SteeringAxisPoint, m_SteeringAxis;
    private Vector3 m_TempVector = new Vector3(0f, 1f, 0f);
    private float m_ForwardRotation = 0f;

    void Awake()
    {
        m_WheelCollider = transform.GetComponentInParent<WheelCollider>();
        Debug.Assert(m_WheelCollider != null, "Unable to get WheelCollider component from parent");
    }

    void Start()
    {
        m_WheelRadius = m_WheelCollider.radius;
        m_WheelMultiplier = transform.InverseTransformPoint(m_WheelCollider.attachedRigidbody.transform.position).x >= 0 ? 1 : -1;

        m_Visual = transform.GetChild(0);
        m_InitialLocalRotation = m_Visual.localRotation;
    }

    void Update()
    {
        // Update position
        m_WheelCollider.GetWorldPose(out Vector3 wheelPos, out _);
        transform.position = wheelPos;
        m_PositionVector.y = transform.localPosition.y;
        transform.localPosition = m_PositionVector;

        // Set temporarily to nominal rotation
        m_Visual.localRotation = m_InitialLocalRotation;

        // Steering rotation
        m_SteeringAxisPoint = transform.TransformPoint(m_WheelMultiplier * m_ScrubRadius, -m_WheelRadius, m_Trail);
        m_TempVector.x = Mathf.Tan(m_WheelMultiplier * m_SteeringAxisInclination * Mathf.Deg2Rad);
        m_TempVector.z = Mathf.Tan(-m_CasterAngle * Mathf.Deg2Rad);
        m_SteeringAxis = (transform.TransformPoint(m_TempVector) - m_SteeringAxisPoint).normalized;
        m_Visual.Rotate(m_SteeringAxis, m_WheelCollider.steerAngle, Space.World);

        // Spinning rotation
        if (Mathf.Abs(m_WheelCollider.rpm) > 10 || !m_WheelCollider.isGrounded)
            m_ForwardRotation += m_WheelCollider.rpm * 6 * Time.deltaTime;
        else
        {
            float velocity = m_WheelCollider.attachedRigidbody.transform.InverseTransformVector(m_WheelCollider.attachedRigidbody.velocity).z;
            m_ForwardRotation += (velocity * Time.deltaTime) / (2 * Mathf.PI * m_WheelRadius) * 360f;
        }
        m_ForwardRotation %= 360f;
        m_Visual.Rotate(m_Visual.right, m_ForwardRotation, Space.World);
    }
}
