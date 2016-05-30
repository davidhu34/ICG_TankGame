﻿using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;              // Used to identify the different players.
    public Rigidbody m_Shell;                   // Prefab of the shell.
    public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
    public Slider m_AimSlider;                  // A child of the tank that displays the current launch force.
	public Slider m_CannonCD;
	public Camera m_Camera;
	public AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
    public AudioClip m_ChargingClip;            // Audio that plays when each shot is charging up.
    public AudioClip m_FireClip;                // Audio that plays when each shot is fired.
    public float m_MinLaunchForce = 15f;        // The force given to the shell if the fire button is not held.
    public float m_MaxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time.
    public float m_MaxChargeTime = 0.75f;       // How long the shell can charge for before it is fired at max force.


    private string m_FireButton;                // The input axis that is used for launching shells.
	private string m_ToggleCamPos;
    private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.
    private float m_ChargeSpeed;                // How fast the launch force increases, based on the max charge time.
    private bool m_Fired;                       // Whether or not the shell has been launched with this button press.
	private float timer;
    private float Ttimer;
	private bool m_Zooming;
	private Vector3 m_ZoomIn;
	private Vector3 m_ZoomOut;

    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
		m_ZoomIn = new Vector3(0f, -1f, 5.8f);
		m_ZoomOut = new Vector3(0f, 1f, -5.8f);
		m_Camera.transform.position = new Vector3(0f, 3f, -6f);
		m_Zooming = false;
	}

    private void Start()
    {
		m_ToggleCamPos = "ToggleC";
        m_FireButton = "Fire" + m_PlayerNumber;
        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
        Ttimer = 0f;
    }


    private void Update()
    {
        Ttimer -= Time.deltaTime;
        timer -= Time.deltaTime;
        m_AimSlider.value = m_MinLaunchForce;
		if (m_Zooming)
		{
			if (Input.GetButton(m_ToggleCamPos) && Ttimer < 0)
			{
				m_Camera.transform.Translate(m_ZoomOut);
				m_Zooming = false;
                Ttimer = 1;
			}
			if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
			{
				m_CurrentLaunchForce = m_MaxLaunchForce;
				Fire();
			}
			else if (Input.GetButtonDown(m_FireButton) && timer < 0)
			{
				m_Fired = false;
				m_CurrentLaunchForce = m_MinLaunchForce;
				m_ShootingAudio.clip = m_ChargingClip;
				m_ShootingAudio.Play();
			}
			else if (Input.GetButton(m_FireButton) && !m_Fired)
			{
				m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
				m_AimSlider.value = m_CurrentLaunchForce;
			}
			else if (Input.GetButtonUp(m_FireButton) && !m_Fired)
			{
				Fire();
			}
		}
		else if (Input.GetButton(m_ToggleCamPos) && Ttimer < 0f)
		{
			m_Camera.transform.Translate(m_ZoomIn);
			m_Zooming = true;
            Ttimer = 1f;
		}
		m_CannonCD.value = timer;
    }


    private void Fire()
    {
        m_Fired = true;
		timer = 3;
		Rigidbody shellInstance =
            Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward; ;

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        m_CurrentLaunchForce = m_MinLaunchForce;
    }
}