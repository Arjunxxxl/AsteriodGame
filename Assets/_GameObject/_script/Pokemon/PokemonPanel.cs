using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PokemonPanel : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text pokemonName;

    public void SetUp(Pokemons.Data pokemonData)
    {
        pokemonName.text = pokemonData.name;

        GetPokemonSprite(pokemonData.url);
    }

    private void GetPokemonSprite(string url)
    {
        ApiManager.GetAPIData?.Invoke(url, ProcessAllPokemonSpriteApiResponse);
    }

    private void ProcessAllPokemonSpriteApiResponse(string response)
    {
        PokemonData data = JsonConvert.DeserializeObject<PokemonData>(response);

        StartCoroutine(LoadImage(data.sprites.front_default));
    }

    IEnumerator LoadImage(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), Vector2.one * 0.5f);
            icon.sprite = sprite;
        }
    }
}
