using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Brick : MonoBehaviour
{
    private SpriteRenderer _sr;

    public int hitPoints = 1;
    public ParticleSystem destroyEffect;

    public static event Action<Brick> OnBrickDestruction;

    private void Awake()
    {
        this._sr = this.GetComponent<SpriteRenderer>();       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        ApplyCollisionLogic(ball);
    }

    private void ApplyCollisionLogic(Ball ball)
    {
        this.hitPoints--;

        if(this.hitPoints <= 0)
        {
            OnBrickDestruction?.Invoke(this);
            SpawnDestroyEffect();
            Destroy(this.gameObject);
        }
        else
        {
            this._sr.sprite = BricksManager.Instance.Sprites[this.hitPoints - 1];
        }
    }

    private void SpawnDestroyEffect()
    {
        Vector3 brickPosition = gameObject.transform.position;
        Vector3 spawnPosition = new Vector3(brickPosition.x, brickPosition.y, brickPosition.z - 0.2f);
        GameObject effect = Instantiate(destroyEffect.gameObject, spawnPosition, Quaternion.identity);

        MainModule mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = this._sr.color;
        Destroy(effect, destroyEffect.main.startLifetime.constant);
    }

    public void Init(Transform containerTransform, Sprite sprite, Color color, int brickHitPoints)
    {
        this.transform.SetParent(containerTransform);
        this._sr.sprite = sprite;
        this._sr.color = color;
        this.hitPoints = brickHitPoints;
    }
}
