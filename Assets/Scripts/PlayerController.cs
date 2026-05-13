using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    [Header("Jump Setting")] 
    public float jumpHeight = 2f;
    public float jumpDuration = 0.5f;
    
    private SpriteRenderer spriteRenderer;
    private bool isJumping = false;
    private float jumpTimer = 0f;
    private Vector3 startPosition;
    private bool isMovingRight = false;
    
    [Header("Animation Controller")]
    public RuntimeAnimatorController idleController;
    public RuntimeAnimatorController jumpController;
    public RuntimeAnimatorController runController;
    public RuntimeAnimatorController crouchController;
    
    private PolygonCollider2D polygonCollider;

    private  Animator animator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        polygonCollider = GetComponent<PolygonCollider2D>(); // 추가
        animator.runtimeAnimatorController = idleController;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveDirection = Vector2.zero;
        

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            StartJump();
        }

        if (isJumping)
        {
            UpdateJump();
        }
        else
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                animator.runtimeAnimatorController = crouchController;
                Destroy(polygonCollider); // 이전 콜라이더 데이터 잔상 제거를 위해 재배치하는 것이 안전합니다
                polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
            }
            else
            {
                animator.runtimeAnimatorController = idleController;
                Destroy(polygonCollider);
                polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
            }
        }
        
        moveDirection = moveDirection.normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        
    }

    void StartJump()
    {
        isJumping = true;
        jumpTimer = 0f;
        startPosition = transform.position;
        
        animator.runtimeAnimatorController = jumpController;
    }

    void UpdateJump()
    {
        jumpTimer += Time.deltaTime;
        float progress = jumpTimer / jumpDuration;

        if (progress >= 1f)
        {
            transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);
            isJumping = false;
            animator.runtimeAnimatorController = idleController;
        }
        else
        {
            float height = Mathf.Sin(progress * Mathf.PI) * jumpHeight;
            transform.position = new Vector3(startPosition.x, startPosition.y + height, startPosition.z);
        }
    }
}
