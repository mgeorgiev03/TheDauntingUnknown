using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    public float maxHP;
    GameObject[] children;
    bool FreezJump = false;
    public SpriteRenderer sprite;
    public SpriteRenderer Knifesprite;
    public SpriteRenderer Gunsprite;
    public SpriteRenderer Shotgunsprite;
    public SpriteRenderer Staffsprite;
    public SpriteRenderer Bloodgunsprite;
    public CharacterController2D controller;
    public Animator animator;

    private HeathBar heathBar;
    public Transform spawnPoint;
    public static PlayerMovement instance;
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] public LayerMask platformLayerMask;
    Rigidbody2D m_Rigidbody2D;
    private bool m_Grounded;

    public float runSpeed = 40f;
    public int level;
    public float HP;
    private bool flag = true;
    [HideInInspector] public float horizontalMove = 0.0f;

    bool dash = false;
    int dashCount = 1;
    float DashCooldown;

    bool jump = false;
    public GameObject activeWeapon;
    #endregion

    #region Start, Update, FixedUpdate

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        heathBar = FindObjectOfType<HeathBar>();

        if (SceneManager.GetActiveScene().name != "MenuScene")
        {
            heathBar.SetMaxHealth(maxHP);
            spawnPoint = GameObject.FindGameObjectWithTag("spawnpoint").transform;
            transform.position = spawnPoint.position;
        }
        if (SceneManager.GetActiveScene().name == "MenuScene")
        {
            
        }
      
        children = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i).gameObject;
            if (children[i].activeInHierarchy)
                activeWeapon = children[i];
        }

        if (activeWeapon.CompareTag("Knife"))
        {
            GetComponent<Shooting>().enabled = false;
            GetComponent<MeleeAttack>().enabled = true;
        }
        else
        {
            GetComponent<Shooting>().enabled = true;
            GetComponent<MeleeAttack>().enabled = false;
        }
        
        if (SceneManager.GetActiveScene().name != "MenuScene")
            Respawn();
    }


    void Awake()
    {
        if (SceneManager.GetActiveScene().name != "MenuScene")
            heathBar = FindObjectOfType<HeathBar>();
        
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        if (SceneManager.GetActiveScene().name != "MenuScene")
            DontDestroyOnLoad(gameObject);
        
        level = SceneManager.GetActiveScene().buildIndex;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "MenuScene")
            heathBar.SetHealth(HP);

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));


        if (horizontalMove != 0 && IsGrounded())
        {
            if (flag)
                StartCoroutine(Walking());
        }

        if (m_Grounded && Input.GetKeyDown(KeyCode.Space))
        {
            //animator.SetBool("IsJumping", true);
            jump = true;
            if (FreezJump)
                jump = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("IsJumping");
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_Rigidbody2D.velocity.y / 5);
            m_Rigidbody2D.AddForce(new Vector2(0f, -2f), ForceMode2D.Impulse);
        }

        if (IsGrounded() && dashCount <= 1)
            dashCount++;

        if (Input.GetKeyDown(KeyCode.Mouse1) && dashCount != 0 && DashCooldown == 0f)
        {
            animator.SetTrigger("IsDashing");
            dash = true;
            dashCount--;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject.Find("LevelLoader 2").GetComponent<LevelLoader>().ReLoadLevel();
            HP = maxHP;
        }

        children = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i).gameObject;
            if (children[i].activeInHierarchy)
                activeWeapon = children[i];
        }

        if (activeWeapon != null)
        { 
            if (activeWeapon.CompareTag("Knife"))
            {
                GetComponent<Shooting>().enabled = false;
                GetComponent<MeleeAttack>().enabled = true;
            }
            else
            {
                GetComponent<Shooting>().enabled = true;
                GetComponent<MeleeAttack>().enabled = false;
            }
        }
    }

    void FixedUpdate()
    {
        m_Grounded = IsGrounded();
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash, dashCount, DashCooldown);
        jump = false;

        if (!dash)
        {
            DashCooldown -= Time.fixedDeltaTime;

            if (DashCooldown < 0)
                DashCooldown = 0f;

        }
        else if (dash)
            DashCooldown += Time.fixedDeltaTime * 25;

        dash = false;
        CheckWall();
    }

    public IEnumerator Walking()
    {
        FindObjectOfType<AudioManager>().Play("Footsteps");
        flag = false;
        yield return new WaitForSeconds(0.3f);
        flag = true;
    }

    #endregion

    private GameObject[] enemies;
    public void TakeDamage(float damage)
    {
        if (HP > 0)
        {
            StartCoroutine(FlashRed());
        }
        
        HP -= damage;
        
        if (HP <= 0)
        {
            GetComponent<Shooting>().enabled = false;
            GetComponent<MeleeAttack>().enabled = false;

            enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (var child in children)
                child.SetActive(false);
            
            sprite.color = Color.white;

            foreach ( var enemy in enemies)
                Destroy(enemy);
            
            m_Rigidbody2D.gravityScale = 5;
            m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;

            FreezJump = true;
            animator.SetBool("IsDead", true);
            
        }
    }

    public void HealPlayer()
    {
        if (HP < 5)
            HP++;
    }

    public void SlowDownPlayer()
    {
        runSpeed /= 2;
        Invoke("SpeedUpPlayer", 2f);
    }

    public void SpeedUpPlayer()
    {
        runSpeed *= 2;
    }

    /*public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }*/

    #region IsGrounded

    private bool IsGrounded()
    {
        float extra = .05f;

        RaycastHit2D raycast = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down,
            boxCollider.bounds.extents.y + extra, platformLayerMask);

        return raycast.collider != null;
    }
    #endregion

    #region CheckWall

    private void CheckWall()
    {
        if (LeftCheckWallAhead() || RightCheckWallAhead())
            m_Rigidbody2D.gravityScale = 20;
        else
            m_Rigidbody2D.gravityScale = 7;
    }

    private bool LeftCheckWallAhead()
    {
        float extra = .05f;

        RaycastHit2D raycast = Physics2D.Raycast(boxCollider.bounds.center, Vector2.left, boxCollider.bounds.extents.x + extra, platformLayerMask);
        if (Input.GetButton("Jump"))
            return false;

        return raycast.collider != null;
    }

    private bool RightCheckWallAhead()
    {
        float extra = .05f;

        RaycastHit2D raycast = Physics2D.Raycast(boxCollider.bounds.center, Vector2.right, boxCollider.bounds.extents.x + extra, platformLayerMask);
        
        if (Input.GetButton("Jump"))
            return false;

        return raycast.collider != null;
    }
    #endregion

    public IEnumerator FlashRed()
    {
        if (HP != 0)
        {
            sprite.color = Color.red;
            Knifesprite.color = Color.red;
            Gunsprite.color = Color.red;
            Shotgunsprite.color = Color.red;
            Staffsprite.color = Color.red;
            Bloodgunsprite.color = Color.red;

            yield return new WaitForSeconds(0.1f);

            sprite.color = Color.white;
            Knifesprite.color = Color.white;
            Gunsprite.color = Color.white;
            Shotgunsprite.color = Color.white;
            Staffsprite.color = Color.white;
            Bloodgunsprite.color = Color.white;
        }
    }

    public void ReloadScene()
    {
        GameObject.Find("LevelLoader 2").GetComponent<LevelLoader>().ReLoadLevel();
        transform.position = spawnPoint.position;

        if (SceneManager.GetActiveScene().name == "Tutorial")
            Destroy(gameObject);
        
        HP = maxHP;
    }

    void Respawn()
    {
        GetComponent<Shooting>().enabled = true;
        GetComponent<MeleeAttack>().enabled = true;

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        activeWeapon.SetActive(true);

        m_Rigidbody2D.gravityScale = 1;
        m_Rigidbody2D.constraints = RigidbodyConstraints2D.None;
        m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        FreezJump = false;
        animator.SetBool("IsDead", false);
        HP = maxHP; 
        
        heathBar = FindObjectOfType<HeathBar>();
        heathBar.SetMaxHealth(HP);
    }
}