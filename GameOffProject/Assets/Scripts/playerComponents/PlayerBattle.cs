using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattle : MonoBehaviour
{

    enum State
    {
        Preparation,
        PlayerTurn,
        EnemyTurn,
        Death,
        Win
    }

    public GameObject enemy;
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject gamObjectsInScene;

    [SerializeField]
    float maxHealth;
    float curHealth;
    
    PlayerStatus playerStatus;

    State mCurState;


    // Start is called before the first frame update
    void Start()
    {
        // initialize player status
        curHealth = maxHealth;
        mCurState = State.PlayerTurn;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth();
        UpdateCurState();
    }

    void UpdateCurState()
    {
        switch (mCurState)
        {
            case State.Preparation:
                UpdatePlayerTurn();
                break;
            case State.PlayerTurn:
                UpdatePlayerTurn();
                break;
            case State.EnemyTurn:
                UpdateEnemyTurn();
                break;
            case State.Death:
                UpdatePlayerDeath();
                break;
            case State.Win:
                UpdateWin();
                break;
        }
    }

    void UpdatePlayerTurn()
    {
        if(curHealth <= 0.0f)
        {
            mCurState = State.Death;
        }
    }

    void UpdateEnemyTurn()
    {
        if(curHealth <= 0.0f)
        {
            mCurState = State.Death;
        }
    }

    void UpdatePlayerDeath()
    {
        gamObjectsInScene.SetActive(true);
        gameObject.SetActive(false);
    }

    void UpdateHealth()
    {
        healthBar.GetComponent<Slider>().value = curHealth / maxHealth;
    }

    void RegenerateHealth(float heal)
    {
        curHealth = Mathf.Min(curHealth + heal, maxHealth);
    }

    void TakeDamage(float damage)
    {
        curHealth -= damage;
    }

    void UpdateWin()
    {
        gamObjectsInScene.SetActive(true);
        gameObject.SetActive(false);
    }
}