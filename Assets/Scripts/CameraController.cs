using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _movSpeed = 0.01f;
    private float _sensitivity = 3f;
    private Vector2 _currentRotation;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null))
        { //Input.GetMouseButton(0)
            Cursor.lockState = CursorLockMode.Locked;
            _currentRotation.x += Input.GetAxis("Mouse X") * _sensitivity;
            _currentRotation.y -= Input.GetAxis("Mouse Y") * _sensitivity;
            _currentRotation.x = Mathf.Repeat(_currentRotation.x, 360);
            _currentRotation.y = Mathf.Clamp(_currentRotation.y, -95, 95);
            //x and y needed to be switched in the quat. can't be bothered to figure out why
            Camera.main.transform.rotation = Quaternion.Euler(_currentRotation.y, _currentRotation.x, 0);
            Camera.main.transform.parent.transform.rotation = Quaternion.Euler(0,_currentRotation.x, 0);
            float xAxisValue = Input.GetAxis("Horizontal") * _movSpeed;
            float zAxisValue = Input.GetAxis("Vertical") * _movSpeed;
            float yAxisValue = Input.GetAxis("Fly Height") * _movSpeed;
            //Again, x and y are weird. I switched it in the variables this time, which is sloppy and inconsistent.
            Camera.main.transform.parent.transform.Translate(new Vector3(0, yAxisValue, 0), Space.World);
            Camera.main.transform.parent.transform.Translate(new Vector3(xAxisValue, 0, zAxisValue));


        }
        else
        {
            Cursor.lockState = CursorLockMode.None;

        }
    }
}
