using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpriteConfig
{
    public Sprite sprite;
    public int[] indexes;
}

[CreateAssetMenu(fileName = "SpritesConfig", menuName = "ScriptableObjects/SpritesConfig")]
public class SpritesConfig : ScriptableObject
{
    [SerializeField] private List<SpriteConfig> list;
    public List<SpriteConfig> List => list;
}
