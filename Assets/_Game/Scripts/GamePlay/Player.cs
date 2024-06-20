using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Camera cam;
    public float rotateSpeed = 1f;
    public float dragSpeed = 1f;
    [SerializeField] private float smoothy = 0.8f;
    [SerializeField] Animator animator;
    private Vector3 targetPosition;
    private bool isLeftDragging;
    private bool isRightDragging;
    private bool isCanMove;
    private bool isCanFillColor;
    private float x;
    private float y;
    private const string VICTORY_ANIM = "Victory";
    public bool IsDragging;

    private Vector2 finger1Start, finger2Start;
    private Vector2 finger1Last, finger2Last;

    private void Start()
    {
        animator.enabled = false;
    }
    private void Update()
    {
        if (!GameManager.Ins.IsState(GameState.GamePlay)) return;

        //if (Input.touchCount == 1)
        //{
        //    Touch touch = Input.GetTouch(0);
        //    x = touch.position.x;
        //    y = touch.position.y;
        //    isLeftDragging = true;
        //}
        //else
        //if (Input.touchCount > 1)
        //{
        //    Touch touch1 = Input.GetTouch(0);
        //    Touch touch2 = Input.GetTouch(1);
        //    Vector2 middle = (touch1.position + touch2.position) / 2;
        //    x = middle.x;
        //    y = middle.y;
        //    isRightDragging = true;
        //    isLeftDragging = false;
        //}
        //else
        //{
        //    isRightDragging = false;
        //    isLeftDragging = false;
        //    isCanMove = true;
        //    isCanFillColor = true;
        //}
        if (Input.touchCount == 1 )
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    isLeftDragging = true;
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isLeftDragging = false;
                    isCanMove = true;
                    isCanFillColor = true;
                    break;

                case TouchPhase.Moved:
                    x = touch.deltaPosition.x;
                    y = touch.deltaPosition.y;
                    break;
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began)
            {
                finger1Start = touch1.position;
                finger1Last = touch1.position;
            }
            else if (touch2.phase == TouchPhase.Began)
            {
                finger2Start = touch2.position;
                finger2Last = touch2.position;
            }
            else if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
            {
                IsDragging = true;
                isLeftDragging = false;
                isRightDragging = true; 
                Vector2 finger1Move = touch1.deltaPosition;
                Vector2 finger2Move = touch2.deltaPosition;

                Vector2 averageMove = (finger1Move + finger2Move) * 0.5f;

                x = averageMove.x;
                y = averageMove.y;
            }
        }
        else
        {
            isLeftDragging = false;
            isRightDragging = false;    
        }
#if UNITY_EDITOR        
        if (Input.GetMouseButtonDown(0)) 
        { 
            isLeftDragging = true; 
        }
        if (Input.GetMouseButtonDown(1)) 
        { 
            isRightDragging = true; 
        }
        if (Input.GetMouseButtonUp(0))
        {
            isLeftDragging = false;
            isCanMove = true;
            isCanFillColor = true;
        }
        if (Input.GetMouseButtonUp(1)) 
        { 
            isRightDragging = false; 
        }

        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");
#endif
        if (isLeftDragging)
        {
            if (UIManager.Ins.GetUI<UIGameplay>().CheckInputOnUI()) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {

                Cube cube = hitInfo.collider.GetComponent<Cube>();
                if (cube != null && !cube.IsState(CubeState.Colored) && isCanFillColor)
                {
                    if (cube.GetColorID() == LevelManager.Ins.currentColor)
                    {
                        isCanMove = false;
                        LevelManager.Ins.OnFilledCube(cube);
                    }
                    else
                    {
                        if (isCanMove) isCanFillColor = false;
                    }

                }
                else
                {

                    if (isCanMove) {
                        isCanFillColor = false;
                        if ((Mathf.Abs(x) > 0.01f || Mathf.Abs(y) > 0.01f))
                        {

                            transform.Rotate(new Vector3(0, -x * rotateSpeed * Time.deltaTime, 0));
                            transform.Rotate(new Vector3(y * rotateSpeed * Time.deltaTime, 0, 0), Space.World);
                        }
                } }

                       
            }
            else
            {
                if (isCanMove)
                {
                    isCanFillColor = false;
                    if (Mathf.Abs(x) > 0.01f || Mathf.Abs(y) > 0.01f && isCanMove)
                    {
                        transform.Rotate(new Vector3(0, -x * rotateSpeed * Time.deltaTime, 0));
                        transform.Rotate(new Vector3(y * rotateSpeed * Time.deltaTime, 0, 0), Space.World);
                    }
                }
            }


        }

        if (isRightDragging && !CameraManager.Ins.IsZooming)
        {
            Vector3 screenPosition = cam.WorldToScreenPoint(transform.position);
            screenPosition.x = Mathf.Clamp(screenPosition.x, 0, Screen.width);
            screenPosition.y = Mathf.Clamp(screenPosition.y, 0, Screen.height);

            Vector3 moveDirection = new Vector3(x, y, 0f) * dragSpeed;
            Vector3 worldPosition = cam.ScreenToWorldPoint(screenPosition + moveDirection);

            transform.position = worldPosition;
           
        }


    }

    
    public void OnReset()
    {
        animator.enabled = false;
        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;  
        isLeftDragging= false;
        isRightDragging= false;
        isCanMove = false;
        isCanFillColor = false;
        this.gameObject.SetActive(false);   
    }
    public void PlayAnim()
    {
        animator.enabled = true;
        animator.SetTrigger(VICTORY_ANIM);   
    }
    public void MoveToStartPosition()
    {
        StartCoroutine(MovePlayer());
    }
    IEnumerator MovePlayer()
    {
        float moveDuration = 2f;
        Vector3 startPosition = transform.position; 
        Vector3 targetPosition = Vector3.zero;

        float timeElapsed = 0;

        while (timeElapsed < moveDuration)
        {

            transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / moveDuration);

            timeElapsed += Time.deltaTime;
            yield return null; 
        }

        transform.position = targetPosition;
    }
   
}
