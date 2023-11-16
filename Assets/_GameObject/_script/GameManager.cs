using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private Canvas pokemonMemuCanvas;
    [SerializeField] private Canvas gameplayCanvas;
    [SerializeField] private Button gotoPokemonGameButton;
    [SerializeField] private Button gotoAstreoidGameButton;

    [Header("Player And Asteroid")]
    [SerializeField] private Player player;
    [SerializeField] private AsteroisSpawner asteriodSpawner;

    #region SingleTon
    public static GameManager Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        gotoPokemonGameButton.onClick.AddListener(EnablePokemonMenu);
        gotoAstreoidGameButton.onClick.AddListener(EnableAsteroidGame);

        EnablePokemonMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnablePokemonMenu()
    {
        pokemonMemuCanvas.enabled = true;
        gameplayCanvas.enabled = false;
    }

    public void EnableAsteroidGame()
    {
        pokemonMemuCanvas.enabled = false;
        gameplayCanvas.enabled = true;

        if (!player.gameObject.activeSelf)
        {
            player.gameObject.SetActive(true);
        }

        player.SetUp();
        asteriodSpawner.SetUp();
    }
}
