using UnityEngine;
using System.Collections;

public class GroundShockwaveController : MonoBehaviour
{
    [Tooltip("Final diameter shockwave will expand to.")]
    [SerializeField] private float maxScale = 100f;
    [Tooltip("Shockwave animation duration.")]
    [SerializeField] private float duration = 1.5f;

    private Renderer objectrenderer;
    private MaterialPropertyBlock propBlock;

    void Awake()
    {
        objectrenderer = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();
    }

    void Start()
    {
        StartCoroutine(ShockwaveRoutine());
    }

    private IEnumerator ShockwaveRoutine()
    {
        Debug.Log("Shockwave routine.");
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;

            //animate scale from small to large
            float currentScale = Mathf.Lerp(0f, maxScale, progress);
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);

            // Animate "_FadeAmount" property in shader
            objectrenderer.GetPropertyBlock(propBlock);
            propBlock.SetFloat("_FadeAmount", progress);
            objectrenderer.SetPropertyBlock(propBlock);

            if (elapsedTime == 0f) //only on first frame
            {
                Debug.Log("Renderer visible: " + objectrenderer.isVisible);
                Debug.Log("Renderer enabled: " + objectrenderer.enabled);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
