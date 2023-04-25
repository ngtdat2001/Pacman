using UnityEngine;

public class Ghost : MonoBehaviour, IDataPersistence
{
    public Movement movement { get; private set; }
    public GhostHome home { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostChase chase { get; private set; }
    public GhostFrightened frightened { get; private set; }

    public GhostBehavior initialBehavior;

    public Transform target;
     
    public int points = 200;

    private void Awake()
    {
        this.movement = GetComponent<Movement>();
        this.home = GetComponent<GhostHome>();
        this.scatter = GetComponent<GhostScatter>();
        this.chase = GetComponent<GhostChase>();
        this.frightened = GetComponent<GhostFrightened>();
    }

    private void Start()
    {
        ResetState();
    }

    public void LoadData(GameData data)
    {
        GameObject.Find("Ghost_Blinky").transform.position = data.blinkyPos;
        GameObject.Find("Ghost_Inky").transform.position = data.inkyPos;
        GameObject.Find("Ghost_Pinky").transform.position = data.pinkyPos;
        GameObject.Find("Ghost_Clyde").transform.position = data.clydePos;
    }
    public void SaveData(GameData data)
    {
        data.blinkyPos = GameObject.Find("Ghost_Blinky").transform.position;
        data.inkyPos = GameObject.Find("Ghost_Inky").transform.position;
        data.pinkyPos = GameObject.Find("Ghost_Pinky").transform.position;
        data.clydePos = GameObject.Find("Ghost_Clyde").transform.position;
    }

    public void ResetState()
    {
        this.gameObject.SetActive(true);
        this.movement.ResetState();

        this.frightened.Disable();
        this.chase.Disable();
        this.scatter.Enable();

        if(this.home != this.initialBehavior)
        {
            this.home.Disable();
        }

        if (this.initialBehavior != null)
        {
            this.initialBehavior.Enable();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (this.frightened.enabled)
            {
                FindObjectOfType<GameManager>().GhostEaten(this);
            }
            else
            {
                FindObjectOfType<GameManager>().PacmanEaten();
            }
        }
    }
}
