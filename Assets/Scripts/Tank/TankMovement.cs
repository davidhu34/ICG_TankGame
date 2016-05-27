using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;         
    public float m_Speed = 3f;            
    public float m_TurnSpeed = 90f;       
    public AudioSource m_MovementAudio;    
    public AudioClip m_EngineIdling;       
    public AudioClip m_EngineDriving;      
    public float m_PitchRange = 0.1f;

    private string m_MovementAxisName;     
    private string m_TurnAxisName;
    private float m_MovementInputValue;
    private float m_TurnInputValue;
	private float m_MousePan;

	private Rigidbody m_Rigidbody;
	private Transform m_GunTransform;

	private float m_OriginalPitch;


    private void Awake()
    {
        m_Rigidbody = GetComponentInChildren<Rigidbody>();
		m_GunTransform = GameObject.Find("TankTop").transform;
	}

    private void OnEnable ()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable ()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        m_OriginalPitch = m_MovementAudio.pitch;
    }

    private void Update()
    {
		m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
		m_TurnInputValue = Input.GetAxis(m_TurnAxisName);
		m_MousePan = Input.GetAxis("Mouse X");
		EngineAudio();
    }

    private void EngineAudio()
    {
        if (Mathf.Abs(m_MovementInputValue) > 0.1f || Mathf.Abs(m_TurnInputValue) > 0.1f) {
			if (m_MovementAudio.clip == m_EngineIdling)
			{
				m_MovementAudio.clip = m_EngineDriving;
				m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
				m_MovementAudio.Play();
			}
        } else {
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
		Aim();
    }

    private void Move()
	{
		Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
		m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
	}


    private void Turn()
    {
		float degree = m_TurnInputValue * m_TurnSpeed * Time.deltaTime * 0.4f;
		Quaternion turnRotation = Quaternion.Euler(0f, degree, 0f);
		m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
		m_GunTransform.Rotate(new Vector3(0f, -degree, 0f));
	}

	private void Aim()
	{
		float pan = 0f;
		if (m_MousePan > 0) pan = 0.4f;
		else if (m_MousePan < 0) pan = -0.4f;
		m_GunTransform.Rotate(new Vector3(0f, pan * m_TurnSpeed * Time.deltaTime, 0f));
	}
}