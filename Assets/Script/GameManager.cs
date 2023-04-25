using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;
    public GameObject MenuUI;
    public static bool GameIsPaused = false;

    public TMP_Text highScoreT;
    public TMP_Text Score;
    public TMP_Text gameOver;
    public TMP_Text livesText;

    [SerializeField] private AudioSource intro;
    [SerializeField] private AudioSource eatPellets;
    [SerializeField] private AudioSource eatPPellets;
    [SerializeField] private AudioSource eatGhost;
    [SerializeField] private AudioSource death;
    [SerializeField] private AudioSource victory;

    public int ghostMultiplier { get; private set; } = 1;
    public int score { get; private set; }
    public int lives { get; private set; }

    private void Start()
    {
        intro.Play();
        NewGame();
        highScoreT.text = "HiScore:" + PlayerPrefs.GetInt("HiScore").ToString();
    }

    private void Update()
    {
        if (this.lives<=0 && Input.anyKeyDown)
        {
            NewGame();
        }
        Menu();
    }
    public void LoadData(GameData data) 
    {
        this.score = data.Score;
        this.lives = data.Lives;
        foreach (Transform pells in pellets)
        {
            if (data.pelPos.Contains(pells.transform.position))
            {
                pells.gameObject.SetActive(false);
            }
            else
            {
                pells.gameObject.SetActive(true);
            }
        }

    }
    public void SaveData(GameData data) 
    {
        data.Score = this.score;
        data.Lives = this.lives;
        foreach (Transform pells in pellets)
        {
            if (!pells.gameObject.activeSelf)
                data.pelPos.Add(pells.transform.position);
        }
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
        death.Play();
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
        if (score > PlayerPrefs.GetInt("HiScore" , 0))
        {
            PlayerPrefs.SetInt("HiScore", score);
            highScoreT.text = "HiScore:" +  score.ToString();
        }
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
            victory.Play();
            this.pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 5.0f);
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
}
