using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTile : MonoBehaviour {

    public int hitPoints;
    private SpriteRenderer sprite;
    public bool isLightUp = false;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    //private void Update()
    //{
    //    if(hitPoints <= 0)
    //    {
    //        Destroy(this.gameObject);
    //    }
    //}

    //public void DestroyTile()
    //{
    //    Destroy(this.gameObject);
    //}

    //public void TakeDamage(int damage)
    //{
    //    Destroy(this.gameObject);
    //    //hitPoints -= damage;
    //    //LightUp();
    //}

    public void LightUp()
    {
        hitPoints -= 1;
        Debug.Log("ka");
        isLightUp = true;
        Color color = sprite.color;
        float newAlpha = color.a * 2f;
        sprite.color = new Color(color.r, color.g, color.b, newAlpha);
    }

}
