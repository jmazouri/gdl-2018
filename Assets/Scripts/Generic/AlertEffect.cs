using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlertEffect : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _tmp;

    private float _lifetime;

    public void SetText(string text)
    {
        _tmp.SetText(text);
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = 
            Vector3.Lerp(transform.position, transform.position + new Vector3(0, Time.deltaTime), Time.deltaTime * 8f);

        _lifetime += Time.deltaTime * 1.5f;

        _tmp.color = new Color(_tmp.color.r, _tmp.color.g, _tmp.color.b, 1 - _lifetime);

        if (_lifetime > 1)
        {
            Destroy(gameObject);
        }
	}
}
