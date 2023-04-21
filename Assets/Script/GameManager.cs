using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;
    public GameObject MenuUI;
    public static bool GameIsPaused = false;
    public static bool PelletsIsEaten = false;

    public TMP_Text highScoreT;
    public TMP_Text Score;
    public TMP_Text gameOver;
    public TMP_Text livesText;

    [SerializeField] private AudioSource intro;
    [SerializeField] private AudioSource eatPellets;
    [SerializeField] private AudioSource eatPPellets;
    [SerializeField] private AudioSource eatGhost;
    [SerializeField] private AudioSource death;

    public int ghostMultiplier { get; private set; } = 1;
    public int score { get; private set; }
    public int highestScore { get; private set; }
    public int lives { get; private set; }

    Vector3 pacmanTransform;
    Vector3 pinkTransform;
    Vector3 redTransform;
    Vector3 blueTransform;
    Vector3 orangeTransform;

    int tempscore;
    int templives;


    private void Start()
    {
        intro.Play();
        NewGame();
    }

    private void Update()
    {
        if (this.lives<=0 && Input.anyKeyDown)
        {
            NewGame();
        }
        if (score > highestScore)
        {
            highestScore = score;
        }
        highScoreT.text = "Highest Score: " + highestScore.ToString();
        Menu();
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        gameOver.enabled = false;
        foreach(Transform pellet in this.pellets)
        {
            pellet.gameObject.SetActive(true);
        }
        ResetState();
    }

    private void ResetState()
    {
        ResetGhostMultiplier();
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].ResetState();
        }

        this.pacman.ResetState();
    }

    private void GameOver()
    {
        gameOver.enabled = true;
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);
        }

        this.pacman.gameObject.SetActive(false);
    }

    private void SetScore(int score)
    {
        Score.text = "Score: " + score.ToString();
        this.score = score;
    }

    private void SetLives(int lives)
    {
        livesText.text = "Lives: " + lives.ToString();
        this.lives = lives;
    }

    public void GhostEaten (Ghost ghost)
    {
        eatGhost.Play();
        int points = ghost.points * this.ghostMultiplier;
        SetScore(this.score + points);
        this.ghostMultiplier++;
    }

    public void PacmanEaten()
    {
        this.pacman.gameObject.SetActive(false);
        death.Play();
        SetLives(this.lives - 1);

        if (this.lives > 0)
        {
            Invoke(nameof(ResetState), 3.0f);
        }
        else
        {
            GameOver();
        }
    }

    public void PelletEaten(Pellete pellete)
    {
        eatPellets.Play();
        pellete.gameObject.SetActive(false);
        SetScore(this.score + pellete.points);
        if (!HasRemainingPellets())
        {
            this.pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3.0f);
        }
    }

    public void PowerPelletEaten(PowerPellete pellet)
    {
        eatPPellets.Play();
        for(int i =0; i<this.ghosts.Length; i++)
        {
            this.ghosts[i].frightened.Enable(pellet.duration);
        }
        PelletEaten(pellet);
        CancelInvoke();
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in this.pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }


    public void ExitGame()
    {
        Application.Quit();
    }

    public void Menu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Pause()
    {
        MenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    private void Resume()
    {
        MenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    public void Save()
    {
        GameObject pac = GameObject.Find("Pacman");
        GameObject pink = GameObject.Find("Ghost_Pinky");
        GameObject blue = GameObject.Find("Ghost_Inky");
        GameObject red = GameObject.Find("Ghost_Blinky");
        GameObject orange = GameObject.Find("Ghost_Clyde");
        pacmanTransform = pac.transform.position;
        pinkTransform = pink.transform.position;
        redTransform = red.transform.position;
        blueTransform = blue.transform.position;
        orangeTransform = orange.transform.position;
        tempscore = score;
        templives = lives;
        for (int i = 0; i < pellets.childCount; i++)
        {
            Transform pellet = pellets.GetChild(i);
            int isActive = pellet.gameObject.activeSelf ? 1 : 0;
            PlayerPrefs.SetInt("Pellet_" + i, isActive);
            PlayerPrefs.SetFloat("Pellet_" + i + "_x", pellet.position.x);
            PlayerPrefs.SetFloat("Pellet_" + i + "_y", pellet.position.y);
        }
    }
    public void Load()
    {
        for (int i = 0; i < pellets.childCount; i++)
        {
            int isActive = PlayerPrefs.GetInt("Pellet_" + i, 1);
            if (isActive == 0)
            {
                pellets.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                pellets.GetChild(i).gameObject.SetActive(true);
                float x = PlayerPrefs.GetFloat("Pellet_" + i + "_x");
                float y = PlayerPrefs.GetFloat("Pellet_" + i + "_y");
                pellets.GetChild(i).position = new Vector3(x, y, 0);
            }
        }
        GameObject pac = GameObject.Find("Pacman");
        GameObject pink = GameObject.Find("Ghost_Pinky");
        GameObject blue = GameObject.Find("Ghost_Inky");
        GameObject red = GameObject.Find("Ghost_Blinky");
        GameObject orange = GameObject.Find("Ghost_Clyde");
        pac.transform.position = pacmanTransform;
        pink.transform.position = pinkTransform;
        blue.transform.position = blueTransform;
        red.transform.position = redTransform;
        orange.transform.position = orangeTransform;
        score = tempscore;
        lives = templives;
    }
}
