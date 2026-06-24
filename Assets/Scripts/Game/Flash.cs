using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material flashMat;
    [SerializeField] private float duration;

    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Coroutine flashRoutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }

    // Update is called once per frame
    public void SimpleFlash()
    {
        Debug.Log("Flash!");
        if(flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.material = flashMat;
        yield return new WaitForSeconds(duration);
        spriteRenderer.material = originalMaterial;
        flashRoutine = null;
    }
}
