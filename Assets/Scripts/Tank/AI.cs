using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour
{
    public float m_Speed = 3f;
	public float m_TurnSpeed = 90f;
	public AudioSource m_MovementAudio;
	public AudioClip m_EngineIdling;
	public AudioClip m_EngineDriving;
	public float m_PitchRange = 0.1f;

	private Rigidbody m_Rigidbody;
	private Transform m_GunTransform;

	private float updateInterval;
	private float m_MoveDir;
	private float m_TurnDir;
	private float m_AimDir;
	private float m_MoveTimer;
	private float m_TurnTimer;

	private float m_OriginalPitch;


	private void Awake()
	{
		m_Rigidbody = GetComponentInChildren<Rigidbody>();
		m_GunTransform = GameObject.Find("TankTop").transform;
	}

	private void OnEnable()
	{
		m_Rigidbody.isKinematic = false;
	}

	private void OnDisable()
	{
		m_Rigidbody.isKinematic = true;
	}

	private void Start()
	{
		m_OriginalPitch = m_MovementAudio.pitch;
	}

	private void Update()
	{

		updateInterval = Time.deltaTime;
		m_MoveTimer -= updateInterval;
		m_TurnTimer -= updateInterval;
		if (m_MoveTimer < 0f)
		{
			m_MoveTimer = Mathf.Round(Random.Range(1f, 5f));
			m_MoveDir = randomDir();
		}
		if (m_TurnTimer < 0f) {
			m_TurnTimer = Mathf.Round(Random.Range(1f, 5f));
			m_TurnDir = randomDir();
		}
		EngineAudio();
	}

	void OnCollisionEnter (Collision col)
	{
		m_MoveDir *= -1;
		m_MoveTimer = Mathf.Round(Random.Range(3f, 5f));
	}

	private float randomDir()
	{
		float random = Random.Range(0f, 1f);
		if (random < 0.4f)
		{
			return 1f;
		}
		else if (random < 0.8f)
		{
			return -1f;
		}
		else return 0f;
	}

	private void EngineAudio()
	{
		if (Mathf.Abs(m_MoveDir) > 0.1f || Mathf.Abs(m_TurnDir) > 0.1f)
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
		Vector3 movement = transform.forward * m_MoveDir * m_Speed * Time.deltaTime;
		m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
	}

	private void Turn()
	{
		float turnDeg = m_TurnDir * m_TurnSpeed * Time.deltaTime * 0.4f;
		float aimDeg = m_AimDir * m_TurnSpeed * Time.deltaTime * 0.4f;
		Quaternion turnRotation = Quaternion.Euler(0f, turnDeg, 0f);
		m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
		m_GunTransform.Rotate(new Vector3(0f, aimDeg - turnDeg, 0f));
	}
}