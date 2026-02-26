using UnityEngine;


public class WallTransparency : MonoBehaviour

{

    [Header("Settings")]

    public bool shouldFade = false;

    public float fadeSpeed = 2.0f;



    private Material _materialInstance;

    private float _currentDissolve = 0f;



    private static readonly int DissolveProp = Shader.PropertyToID("_Dissolve");



    void Start()

    {

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();



        _materialInstance = meshRenderer.material;



        if (_materialInstance.HasProperty(DissolveProp))

        {

            _currentDissolve = _materialInstance.GetFloat(DissolveProp);

        }

    }



    void Update()

    {

        float targetDissolve = shouldFade ? 1f : 0f;



        _currentDissolve = Mathf.MoveTowards(_currentDissolve, targetDissolve, Time.deltaTime * fadeSpeed);



        _materialInstance.SetFloat(DissolveProp, _currentDissolve);

    }



    void OnDestroy()

    {

        if (_materialInstance != null)

            Destroy(_materialInstance);

    }

}
