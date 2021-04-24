using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    ItemController currentItem;
    [SerializeField]
    Camera playerCamera;
    [SerializeField]
    CinemachineFreeLook playerCinemachine;
    [SerializeField]
    TextMeshProUGUI gameplayText;
    [SerializeField]
    Color SelectedColor;
    [SerializeField]
    Color ProcessedColor;
    [SerializeField]
    float MoveForce;
    [SerializeField]
    float FloatForce;
    [SerializeField]
    float jumpChargeTime;
    [SerializeField]
    LayerMask propLayer;

    private ItemController selectedItem = null;
    private int botsRemaining;
    private Vector2 moveDeta;
    private bool jumpReady;

    public ItemController CurrentItem => currentItem;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        botsRemaining = FindObjectsOfType<BotController>().Length;
        gameplayText.text = "BOTS REMAINING :" + botsRemaining;
        
        currentItem.gameObject.layer = 2;
        jumpReady = true;

    }

    // Update is called once per frame
    void Update()
    {
        // Raycasting
        var ray = playerCamera.ScreenPointToRay(Input.mousePosition + new Vector3 (0, 100, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, propLayer))
        {
            if (selectedItem != null && selectedItem.gameObject != hit.collider.gameObject)
            {
                DeSelectItem();
            }
            selectedItem = hit.collider.GetComponent<ItemController>();
            selectedItem.outline.OutlineColor = SelectedColor;
            selectedItem.EnableOutline();
        }
        else if (selectedItem != null)
        {
            DeSelectItem();
        }

        currentItem.EnableOutline();

        // Movement
        Vector3 direction = new Vector3(moveDeta.x, 0f, moveDeta.y).normalized;

        if (direction.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.transform.eulerAngles.y;

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            currentItem.rigidbody.AddForce(moveDirection * MoveForce * Time.deltaTime, ForceMode.Force);
        }
    }

    private void DeSelectItem()
    {
        selectedItem.DisableOutline();
        selectedItem = null;
    }

    public void OnAbility(InputAction.CallbackContext value)
    {
        if (jumpReady)
        {
            currentItem.rigidbody.AddForce(Vector3.up * FloatForce, ForceMode.Force);
            StartCoroutine(RechargeJump());
        }

    }

    private IEnumerator RechargeJump()
    {
        jumpReady = false;
        yield return new WaitForSeconds(jumpChargeTime);
        jumpReady = true;
    }

    public void OnProcess(InputAction.CallbackContext value)
    {
        if (value.started && selectedItem != null)
        {
            currentItem.gameObject.layer = 8;
            playerCinemachine.Follow = selectedItem.transform;
            playerCinemachine.LookAt = selectedItem.transform;
            currentItem = selectedItem;
            currentItem.gameObject.layer = 2;
            selectedItem = null;
        }
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        moveDeta = value.ReadValue<Vector2>();
    }

    public void OnBotDestroyed()
    {
        botsRemaining--;
        gameplayText.text = "BOTS REMAINING :" + botsRemaining;

        if (botsRemaining <= 0)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }
}
