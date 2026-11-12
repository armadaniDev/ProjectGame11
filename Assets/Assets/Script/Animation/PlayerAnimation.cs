using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator anim;
    SpriteRenderer sr;

    Vector2 lastPos;
    Vector2 velocity;
    Vector2 lastDir = Vector2.down;

    void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        lastPos = transform.position;
    }

    void FixedUpdate()
    {
        Vector2 pos = transform.position;
        velocity = (pos - lastPos) / Time.fixedDeltaTime;
        lastPos = pos;
    }

    void Update()
    {
        bool moving = velocity.sqrMagnitude > 0.01f;

        if (moving)
        {
            if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
                lastDir = new Vector2(Mathf.Sign(velocity.x), 0);
            else
                lastDir = new Vector2(0, Mathf.Sign(velocity.y));
        }

        anim.SetBool("isMoving", moving);
        anim.SetFloat("moveX", lastDir.x);
        anim.SetFloat("moveY", lastDir.y);

        if (lastDir.x != 0)
            sr.flipX = lastDir.x < 0;
    }
}