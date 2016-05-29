using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public float m_Speed = 3f;
    public float m_TurnSpeed = 90f;
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;
    public float m_PitchRange = 0.1f;

    private string m_MovementAxisName;
    private string m_TurnAxisName;
    private string m_AimAxisName;
    private float m_MovementInputValue;
    private float m_TurnInputValue;
    private float m_AimInputValue;
    private float m_MousePan;

    private Rigidbody m_Rigidbody;
    private Transform m_GunTransform;

    private float m_OriginalPitch;


    private void Awake()
    {
        m_Rigidbody = GetComponentInChildren<Rigidbody>();
        m_GunTransform = GameObject.Find("TankTop").transform;
    }

    private void OnEnable()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        m_MovementAxisName = "Vertical1";
        m_TurnAxisName = "Horizontal1";
        m_AimAxisName = "Horizontal2";
        m_OriginalPitch = m_MovementAudio.pitch;
    }

    private void Update()
    {
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);
        m_AimInputValue = Input.GetAxis(m_AimAxisName);
        m_MousePan = Input.GetAxis("Mouse X");
        EngineAudio();
    }

    private void EngineAudio()
    {
        if (Mathf.Abs(m_MovementInputValue) > 0.1f || Mathf.Abs(m_TurnInputValue) > 0.1f)
        {
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
    }

    private void Move()
    {
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }

    private void Turn()
    {
        float turnDeg = m_TurnInputValue * m_TurnSpeed * Time.deltaTime * 0.4f;
        float aimDeg = m_AimInputValue * m_TurnSpeed * Time.deltaTime * 0.4f;
        Quaternion turnRotation = Quaternion.Euler(0f, turnDeg, 0f);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
        m_GunTransform.Rotate(new Vector3(0f, aimDeg - turnDeg, 0f));
    }
}