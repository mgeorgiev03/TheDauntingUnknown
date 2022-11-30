using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
	#region Variables
	[SerializeField] public float m_JumpForce = 2000f;
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
	[SerializeField] private bool m_AirControl = false;
	[SerializeField] private Collider2D BoxCollider2D;
	[SerializeField] private LayerMask platformLayerMask;
	[SerializeField] float speedOfDash = 10f;

	public Animator animator;
	private bool m_Grounded;
	private Rigidbody2D m_Rigidbody2D;
	public bool m_FacingRight = true;
	private Vector3 m_Velocity = Vector3.zero;

	bool isInvincible = false;
	bool isDodging = false;
	public float RepelForceX;
	public float RepelForceY;

	new private SpriteRenderer renderer;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }
	#endregion

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		renderer = GetComponent<SpriteRenderer>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		float extra = .05f;
		RaycastHit2D raycast = Physics2D.BoxCast(BoxCollider2D.bounds.center, BoxCollider2D.bounds.size, 0f, Vector2.down, extra, platformLayerMask);

		if (raycast.collider != null)
		{
			m_Grounded = true;
			if (wasGrounded)
				OnLandEvent.Invoke();
		}
	}

	#region Move; Flip
	public void Move(float move, bool jump, bool isDashing, int dashCount, float DashCooldown)
	{
		if (m_Grounded || m_AirControl)
		{
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
		}

		if (transform.position.x > Camera.main.ScreenToWorldPoint(Input.mousePosition).x && m_FacingRight)
			Flip();
		else if (transform.position.x < Camera.main.ScreenToWorldPoint(Input.mousePosition).x && !m_FacingRight)
			Flip();

		if (m_Grounded && jump)
		{
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}

		if (isDashing && dashCount > 0 && DashCooldown <= 0f)
		{
			if (m_FacingRight)
				//animator.SetBool("IsDashing", true);
				m_Rigidbody2D.velocity += Vector2.right * speedOfDash;
			else
				//animator.SetBool("IsDashing", true);
				m_Rigidbody2D.velocity += Vector2.left * speedOfDash;
				
			//Invoke("DashAnimDelay", 0.3f);
		}
	}

	private void Flip()
	{
		if (m_FacingRight)
			renderer.flipX = true;
		else
			renderer.flipX = false;

		m_FacingRight = !m_FacingRight;
	}
	#endregion

	#region OnCollisionEnter2D
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && !isInvincible)
		{
			isInvincible = true;
			Invoke("resetInvulnerability", 2);
			renderer.color = new Color(1f, 1f, 1f, 0.5f);

			if (m_FacingRight)
				m_Rigidbody2D.AddForce(new Vector2(RepelForceX, RepelForceY));

			else if (!m_FacingRight)
				m_Rigidbody2D.AddForce(new Vector2(-RepelForceX, RepelForceY));
		}
		else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && isDodging)
		{
		}
		else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && isInvincible)
		{
			if (m_FacingRight)
				m_Rigidbody2D.AddForce(new Vector2(RepelForceX, RepelForceY));

			else if (!m_FacingRight)
				m_Rigidbody2D.AddForce(new Vector2(-RepelForceX, RepelForceY));
		}

	}

	void resetInvulnerability()
	{
		renderer.color = new Color(1f, 1f, 1f, 1f);
		isInvincible = false;
		isDodging = false;
	}

	//void DashAnimDelay()
	//{
	//	animator.SetBool("IsDashing", false);
	//}
	#endregion
}


