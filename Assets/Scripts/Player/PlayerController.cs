using NUnit.Framework.Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class PlayerController : MonoBehaviour
{
    ///--------------------------- <summary>
    /// Movement + camera
    /// </summary>
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Camera")]
    public float mouseSensitivity = 100f;

    private CharacterController characterController;
    private Transform playerCameraTransform;
    private PlayerInputs inputActions;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float xRotation = 0f;
    ///---------------------------
    /// Object Interactable
    /// </summary>
    [Header("Interaction")]
    public float interactDistance = 3f;
    public LayerMask interactLayerMask;
    private Camera playerCamera;
    private GameObject item;


    ///---------------------------
    /// Object Interactable -> Mask
    /// </summary>
    [Header("Outliner")]
    public Material materialOutliner;

    private GameObject lastHighlightedObject = null;

    public bool canMove = true;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputActions = new PlayerInputs();
        playerCamera = GetComponentInChildren<Camera>();
        inputActions.Player.Interact.performed += ctx => InteractButton();
    }

    void Start()
    {
        playerCameraTransform = GetComponentInChildren<Camera>().transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnEnable()
    {
        inputActions.Enable();

        // Subscribe to Move action events
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // Subscribe to Look action events
        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
    
    private void Update()
    {
        if (canMove)
        {
            HandleMovement();
            HandleCameraRotation();
            CheckForInteractableObject();
        }
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void HandleCameraRotation()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void InteractButton()
    {
        if (item == null)
        {
            Ray ray = playerCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayerMask))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    item = hit.transform.gameObject;
                    if (item.GetComponent<ActionDoor>() != null )
                    {
                        if (!item.GetComponent<ActionDoor>().usableOneTime)
                        {
                            interactable.Interact(playerCameraTransform);
                        }
                    }
                    else 
                    {
                        interactable.Interact(playerCameraTransform);
                    }

                    if (item.GetComponent<TakeItem>() == null)
                    {
                        item = null;
                    }
                }
            }
        }
        else
        {
            if(item.GetComponent<TakeItem>() != null) {
                if (item.tag == "Note")
                {
                    Debug.Log("Player Note");
                    item.GetComponent<TakeItem>().Drop();
                    item = null;
                }
                else
                {
                    Ray ray = playerCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

                    if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayerMask))
                    {
                        if (hit.collider.TryGetComponent(out IInteractable interactable))
                        {
                            GameObject item2 = hit.transform.gameObject;
                            if (item2.GetComponent<ActionDoor>() && item2.GetComponent<ActionDoor>().usableOneTime
                                && item2.GetComponent<ActionDoor>().idUsable == item.GetComponent<TakeItem>().id)
                            {
                                interactable.Interact(playerCameraTransform);
                                item.GetComponent<TakeItem>().DropDefinitive();
                                item = null;
                            }
                            else if (item2.GetComponent<ActionDoor>() && !item2.GetComponent<ActionDoor>().usableOneTime)
                            {
                                interactable.Interact(playerCameraTransform);
                            }
                            else if (item2.GetComponent<TakeItem>())
                            {
                                item.GetComponent<TakeItem>().Drop();
                                interactable.Interact(playerCameraTransform);
                                item = item2;
                            }
                        }
                        else
                        {
                            item.GetComponent<TakeItem>().Drop();
                            item = null;
                        }
                    }
                    else
                    {
                        item.GetComponent<TakeItem>().Drop();
                        item = null;
                    }
                }
                
            }
        }
    }
    void CheckForInteractableObject()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactLayerMask))
        {
            float distanceToObject = Vector3.Distance(playerCameraTransform.position, hit.point);
            
            if (distanceToObject > 0.5f)
            {
                GameObject objectRenderer = hit.collider.gameObject;
                if (objectRenderer != null)
                {
                    // Si hay un objeto interactuable, resaltar su borde con un shader
                    if (lastHighlightedObject != objectRenderer)
                    {

                        /*Material[] materials = new Material[2];
                        materials[0] = objectRenderer.GetComponent<Renderer>().materials[0];
                        materials[1] = materialOutliner;
                        objectRenderer.GetComponent<Renderer>().materials = materials;*/
                        Material[] materials = objectRenderer.GetComponent<Renderer>().materials;
                        materials[1].SetFloat("_Alpha", 1f);
                        //objectRenderer.GetComponent<Renderer>().materials = materials;
                        //
                        //Revisar bug OutlinerEncendido al dejar de mirar
                        if(lastHighlightedObject != null)
                        {
                            Material[] materials2 = lastHighlightedObject.GetComponent<Renderer>().materials;
                            materials2[1].SetFloat("_Alpha", 0f);
                            lastHighlightedObject.GetComponent<Renderer>().materials = materials;
                        }
                        //
                        lastHighlightedObject = objectRenderer;

                    }
                }
            }
                
        }
        else
        {
            // Si no hay objeto en el rango, desactivamos el resaltado
            if (lastHighlightedObject != null)
            {
                // Restauramos el material original
                //Debug.Log("Reset Ant Color -> " + lastHighlightedObject.GetComponent<Renderer>().materials[1].name);
                Material[] materials = lastHighlightedObject.GetComponent<Renderer>().materials;
                materials[1].SetFloat("_Alpha", 0f);
                lastHighlightedObject.GetComponent<Renderer>().materials = materials;
                //Debug.Log("Reset Des Color -> " + lastHighlightedObject.GetComponent<Renderer>().materials[1].name);
                lastHighlightedObject = null;
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Damage")
        {
            GetComponent<CharacterController>().enabled = false;
            //transform.position = new Vector3(-9f, 3f, 3f);
            transform.position = new Vector3(-38f, 3f, -3f);
            GetComponent<CharacterController>().enabled = true;
        }
    }

}
