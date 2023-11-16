using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class PokemonUi : MonoBehaviour
{
    [Header("Api End Point")]
    [SerializeField] private string endPoint;

    [Header("Pokemon Data")]
    [SerializeField] private Pokemons pokemons;

    [Header("Pokemon UI")]
    [SerializeField] private RectTransform parent;
    [SerializeField] private List<PokemonPanel> pokemonPanels;
    [SerializeField] private GameObject pokemonPanelPrefab;
    [SerializeField] private Vector2 sizeOfPanel;

    // Start is called before the first frame update
    void Start()
    {
        GetAllPokemonData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GetAllPokemonData()
    {
        pokemons = new Pokemons();

        ApiManager.GetAPIData?.Invoke(endPoint, ProcessAllPokemonApiResponse);
    }

    private void ProcessAllPokemonApiResponse(string response)
    {
        pokemons = JsonConvert.DeserializeObject<Pokemons>(response);

        for (int i = 0; i < pokemons.results.Count; i++)
        {
            GameObject obj = Instantiate(pokemonPanelPrefab, transform.position, Quaternion.identity);
            obj.SetActive(true);
            obj.transform.SetParent(parent);

            obj.GetComponent<PokemonPanel>().SetUp(pokemons.results[i]);
        }

        parent.sizeDelta = new Vector2(parent.sizeDelta.x, (sizeOfPanel.y + 25) * (pokemons.results.Count * 0.5f + 0));
    }
}
