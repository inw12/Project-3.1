using UnityEngine;
public class TrainingDummyHitFeedback : MonoBehaviour
{
    [SerializeField] private Transform model;

    [Header("Emission")]
    [SerializeField] private Color hitColor;
    [SerializeField] private float hitBrightness;
    [SerializeField] private float effectSpeed;
    private Color _defaultColor;
    private Material _material;

    [Header("Scale Pulse")]
    [SerializeField] private float pulseAmount = 0.15f;
    [SerializeField] private float growSpeed = 25f;
    private Vector3 _defaultScale;
    private Vector3 _scaleOffset;

    void Start()
    {
        // Emission
        if (model.TryGetComponent(out SkinnedMeshRenderer mesh))
        {
            _material = mesh.material;
        }
        _material.EnableKeyword("_EMISSION");
        _defaultColor = new(0, 0, 0);

        // Scale
        _defaultScale = transform.localScale;
    } 

    public void UpdateModel(float deltaTime)
    {
        // Emission Lerp
        Color current = _material.GetColor("_EmissionColor");
        Color next = Color.Lerp(current, _defaultColor, deltaTime * effectSpeed);
        _material.SetColor("_EmissionColor", next);

        // Return to normal scale
        model.localScale = _defaultScale + _scaleOffset;
        _scaleOffset = Vector3.Lerp(_scaleOffset, Vector3.zero, deltaTime * growSpeed);
    }

    public void TriggerHitFeedback()
    {
        _scaleOffset = Vector3.one * -pulseAmount;
        _material.SetColor("_EmissionColor", hitColor * hitBrightness);
    }
}
