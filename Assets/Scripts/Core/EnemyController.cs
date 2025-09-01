using UnityEngine;
using System.Collections;
using System;

public class EnemyController : MonoBehaviour
{

    [Header("State Materials")]
    [Tooltip("Material for enemy when crushable.")]
    [SerializeField] private Material timedMaterial;
    [Tooltip("Material for when permanent.")]
    [SerializeField] private Material permanentMaterial;

    [Header("Timers")]
    [Tooltip("Time to crush before permanent.")]
    [SerializeField] private float timeToPermanent = 15f;

    private Renderer objectRenderer;
    private bool isPermanent = false;
    private Coroutine flashCoroutine;

    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        GetComponent<BoxCollider>().isTrigger = true; // Ensure trigger so is crushable
    }

    private void OnEnable()
    {
        GameManager.OnStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnStateChanged -= HandleStateChanged;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(BecomePermanentRoutine());
        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private void HandleStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.GameOver)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator BecomePermanentRoutine()
    {
        yield return new WaitForSeconds(timeToPermanent);

        isPermanent = true;

        // Stop flashing
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        objectRenderer.material = permanentMaterial;

        // Make solid for collision
        GetComponent<BoxCollider>().isTrigger = false;
    }

    private IEnumerator FlashRoutine()
    {
        while (true)
        {
            objectRenderer.material = permanentMaterial;
            yield return new WaitForSeconds(0.5f);
            objectRenderer.material = timedMaterial;
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isPermanent && collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.EndGame();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!isPermanent && collider.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
