using UnityEngine;



public class WallTransparency : MonoBehaviour

{

    [Header("Settings")]

    public bool shouldFade = false;

    public float fadeSpeed = 2.0f;



    private Material _materialInstance;

    private float _currentDissolve = 0f;



    // Using PropertyToID is faster than using strings every frame in Update

    private static readonly int DissolveProp = Shader.PropertyToID("_Dissolve");



    void Start()

    {

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();



        // This line BOTH gets the material AND creates a unique instance for this object

        _materialInstance = meshRenderer.material;



        // Initialize dissolve value

        if (_materialInstance.HasProperty(DissolveProp))

        {

            _currentDissolve = _materialInstance.GetFloat(DissolveProp);

        }

    }



    void Update()

    {

        float targetDissolve = shouldFade ? 1f : 0f;



        // Smoothly interpolate

        _currentDissolve = Mathf.MoveTowards(_currentDissolve, targetDissolve, Time.deltaTime * fadeSpeed);



        // Update the shader

        _materialInstance.SetFloat(DissolveProp, _currentDissolve);

    }



    void OnDestroy()

    {

        // Clean up to prevent memory leaks

        if (_materialInstance != null)

            Destroy(_materialInstance);

    }

}