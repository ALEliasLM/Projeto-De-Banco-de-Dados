using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSelector : MonoBehaviour
{
    Image image;

    [SerializeField] Sprite[] sprites;
    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = sprites[Random.Range(0, sprites.Length-1)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
