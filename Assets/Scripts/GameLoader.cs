using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneToLoad;

    [Header("References")]
    public DeviceMananager inputManager;
    public Transform screenCoverup;

    [Header("Coverup Settings")]
    public Vector3 endpoint = Vector3.zero;
    public float slideDuration = 1.0f;
    public AnimationCurve slideCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [SerializeField] private bool manualMode = false;
    private bool active = false;
    private bool isLoading = false;

    private Vector3 coverupStartPos;

    void Start()
    {
        if (inputManager == null)
        {
            manualMode = true;
            Debug.LogWarning("[GameLoader] No DeviceManager found. Using manual mode (Space key).");
        }

        if (screenCoverup != null)
            coverupStartPos = screenCoverup.position;

        StartCoroutine(ActivateDelay());
    }

    void Update()
    {
        if (!active || isLoading)
            return;

        IEnumerator coroutine = LoadSceneCoroutine();
        if (screenCoverup != null)
            coroutine = LoadSceneWithCoverupCoroutine();

        if (manualMode && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(coroutine);
        }

        if (!manualMode && inputManager != null && inputManager.InputReady())
        {
            StartCoroutine(coroutine);
        }
    }

    public void LoadScene()
    {
        StartCoroutine(LoadSceneCoroutine());
    }

    private IEnumerator LoadSceneCoroutine()
    {
        SceneManager.LoadScene(sceneToLoad);
        yield return null;
    }

    private IEnumerator LoadSceneWithCoverupCoroutine()
    {
        isLoading = true;

        // Animate screen coverup first
        if (screenCoverup != null)
            yield return StartCoroutine(SlideCoverup());

        // Load the scene after animation completes
        Debug.Log("[GameLoader] Loading scene: " + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }

    private IEnumerator SlideCoverup()
    {
        float elapsed = 0f;

        Vector3 start = coverupStartPos;
        Vector3 end = endpoint;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = slideCurve.Evaluate(Mathf.Clamp01(elapsed / slideDuration));
            screenCoverup.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        screenCoverup.position = end;
    }

    private IEnumerator ActivateDelay()
    {
        yield return new WaitForSeconds(0.5f);
        active = true;
    }
}
