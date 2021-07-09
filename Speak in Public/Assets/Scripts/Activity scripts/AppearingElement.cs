using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearingElement : MonoBehaviour
{
    [SerializeField] private float refAngle = 80f;
    [SerializeField] private Vector3 leftPos;
    [SerializeField] private Vector3 rightPos;

    private Vector3 _fwdDirection;
    private Vector3 _rightDirection;
    private Vector3 _leftDirection;

    private Camera _cam;
    private Renderer _mesh;
    private bool _enabled;

    void Awake()
    {
        _cam = Camera.main;
        _fwdDirection = _cam.transform.forward;
    }

    // Start is called before the first frame update
    void Start()
    {
        _enabled = false;
        _mesh = GetComponent<Renderer>();

        _rightDirection = rightPos - _cam.transform.position;
        _leftDirection = leftPos - _cam.transform.position;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            ChangeState();
    }

    private void ChangeState()
    {
        if(!_enabled)
        {
            float angle = Vector3.SignedAngle(_fwdDirection, _cam.transform.forward, Vector3.up);
            print(angle);

            if(angle > 0)
                transform.position = leftPos;
            else
                transform.position = rightPos;
            
            _mesh.enabled = true;
        }
        else
            StartCoroutine(ChangeStateCoroutine());

         _enabled = !_enabled;
    }    

    private IEnumerator ChangeStateCoroutine()
    {
        Vector3 vec;

        if(transform.position == rightPos)
            vec = _rightDirection;
        else
            vec = _leftDirection;
        
        float angle;

        do
        {
            yield return null;
            
            angle = Mathf.Abs(Vector3.SignedAngle(vec, _cam.transform.forward, Vector3.up));
            
        }while(angle < refAngle);

        _mesh.enabled = false;
    }
}
