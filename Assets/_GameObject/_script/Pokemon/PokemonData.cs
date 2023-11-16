using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PokemonData
{
    public PokemonSprites sprites;
}

[System.Serializable]
public class PokemonSprites
{
    public string front_default;
}

[System.Serializable]
public class Pokemons
{
    [System.Serializable]
    public class Data
    {
        public string name;
        public string url;
    }

    public List<Data> results;
}