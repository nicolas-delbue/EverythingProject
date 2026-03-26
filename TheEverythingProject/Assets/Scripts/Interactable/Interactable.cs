using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private bool _isInitialized; //Will have a use when level item manager exists

    [SerializeField]
    protected float _interactTime = 3;
    [SerializeField]
    protected string _interactableName = "NA";
    [SerializeField]
    protected string _interactionType = "NA";
    public float InteractTime => _interactTime;
    public string InteractName => _interactableName;
    public string InteractType => _interactionType;
    public bool canInteract = true;

    public virtual void Initialize()
    {
        _isInitialized = true;
    }
    public virtual void OnDestroy()
    {
        Dispose();
    }
    public virtual void Dispose()
    {
        _isInitialized = false;
    }
    public virtual void Start()
    {
        Initialize();
    }
    public void Interact(PlayerInteraction interactor)
    {
        HandleInteraction(interactor);
    }

    public virtual void Update()
    {

    }

    protected abstract void HandleInteraction(PlayerInteraction interactor);
}
