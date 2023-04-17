using UnityEngine;

public class GhostEye : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }
    public Movement movement { get; private set; }
    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;

    private void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.movement = GetComponent<Movement>();
    }

    private void Update()
    {
        if(this.movement.direction == Vector2.up)
        {
            this.spriteRenderer.sprite = up;
        }
        if (this.movement.direction == Vector2.down)
        {
            this.spriteRenderer.sprite = down;
        }
        if (this.movement.direction == Vector2.left)
        {
            this.spriteRenderer.sprite = left;
        }
        if (this.movement.direction == Vector2.right)
        {
            this.spriteRenderer.sprite = right;
        }
    }
}
