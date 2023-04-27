using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOperations:MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject CamPivot;
    [SerializeField] float CamSpeed;
    
    private void Update()
    {
        Vector3 CalcPlayerPos = new Vector3(player.transform.position.x, player.transform.position.y, -100); 
        CamPivot.transform.position = Vector3.Lerp(CamPivot.transform.position, CalcPlayerPos, CamSpeed * Time.deltaTime);
    }
    public void ShakeCam(float _duration, float strength)
    {
        StartCoroutine(ShakeOP(_duration,strength));
    }
    IEnumerator ShakeOP(float duration,float magnitude)
    {
        Vector3 InitialPos = transform.localPosition;
        float _time = 0f;
        while (_time < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition =new Vector3(x, y, InitialPos.z);
            _time += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = InitialPos;
    }
}
