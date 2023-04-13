
using UnityEngine;

public class PowerPellete : Pellete
{
    public float duration = 8f;

    protected override void Eat()
    {
        FindObjectOfType<GameManager>().PowerPelletEaten(this);
    }
}
