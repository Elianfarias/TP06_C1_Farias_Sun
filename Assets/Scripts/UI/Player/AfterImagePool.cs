using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Gameplay.Player;

[DisallowMultipleComponent]
public class AfterImagePool : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;

    [Header("Pool")]
    [SerializeField] private int poolSize = 20;
    [SerializeField] private Transform poolParent;

    [Header("Afterimage Settings")]
    [SerializeField] private float afterImageMinDistance = 0.25f;
    [SerializeField] private float afterImageLifeTime = 0.45f;
    [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    [SerializeField] private Color afterImageColor = new(1f, 1f, 1f, 0.9f);
    [SerializeField] private float afterImageScaleMultiplier = 1f;

    private Queue<SpriteRenderer> pool = new();
    private Coroutine _afterImageRoutine;
    private Vector3 _lastAfterImagePos;

    private void Awake()
    {
        playerMovement.onDashCD += PlayerMovement_onDashCD;

        for (int i = 0; i < poolSize; i++)
            pool.Enqueue(CreateNewSpriteRenderer());
    }

    private void OnDestroy()
    {
        playerMovement.onDashCD -= PlayerMovement_onDashCD;
    }

    private SpriteRenderer CreateNewSpriteRenderer()
    {
        GameObject go = new("AfterImage");
        go.transform.SetParent(poolParent);
        var sr = go.AddComponent<SpriteRenderer>();
        sr.enabled = false;

        return sr;
    }

    private void PlayerMovement_onDashCD(float dashDuration)
    {
        if (_afterImageRoutine != null)
            StopCoroutine(_afterImageRoutine);

        _afterImageRoutine = StartCoroutine(SpawnAfterImagesRoutine(dashDuration));
    }

    private IEnumerator SpawnAfterImagesRoutine(float dashDuration)
    {
        _lastAfterImagePos = playerSpriteRenderer.transform.position;
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            if (Vector3.Distance(playerSpriteRenderer.transform.position, _lastAfterImagePos) >= afterImageMinDistance)
            {
                SpawnAfterImage();
                _lastAfterImagePos = playerSpriteRenderer.transform.position;
            }

            yield return null;
        }
    }

    private void SpawnAfterImage()
    {
        if (playerSpriteRenderer == null) return;

        SpriteRenderer sr = pool.Count > 0 ? pool.Dequeue() : CreateNewSpriteRenderer();
        sr.enabled = true;
        sr.sprite = playerSpriteRenderer.sprite;
        sr.flipX = playerSpriteRenderer.flipX;
        sr.sortingLayerID = playerSpriteRenderer.sortingLayerID;
        sr.sortingOrder = playerSpriteRenderer.sortingOrder - 1;
        sr.transform.position = playerSpriteRenderer.transform.position;
        sr.transform.rotation = playerSpriteRenderer.transform.rotation;
        sr.transform.localScale = playerSpriteRenderer.transform.localScale * afterImageScaleMultiplier;
        sr.color = afterImageColor;

        StartCoroutine(FadeAndReturn(sr, afterImageLifeTime));
    }

    private IEnumerator FadeAndReturn(SpriteRenderer sr, float lifeTime)
    {
        float time = 0f;
        Color start = sr.color;

        while (time < lifeTime)
        {
            time += Time.deltaTime;
            float norm = Mathf.Clamp01(time / lifeTime);
            float alpha = fadeCurve.Evaluate(norm);
            sr.color = new Color(start.r, start.g, start.b, start.a * alpha);
            yield return null;
        }

        sr.enabled = false;
        pool.Enqueue(sr);
    }
}
