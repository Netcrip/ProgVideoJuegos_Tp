using UnityEngine;
using System.Collections;

public class AnimationScript : MonoBehaviour {

	[SerializeField] float spedRotation;
    [SerializeField] float healAmount=20f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.Rotate(0f,0f, spedRotation * Time.deltaTime, Space.Self);
       
       
	}
    private void OnTriggerEnter(Collider other)
    {
        // Lógica para aplicar daño al enemigo
        PlayerRb isplayer = other.GetComponent<PlayerRb>();
        if (isplayer != null)
        {
            isplayer.heal(healAmount);
            Destroy(gameObject);
        }
    }
}
