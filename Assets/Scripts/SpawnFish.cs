using UnityEngine;
using System.Collections;

public class SpawnFish : MonoBehaviour {

    public Rigidbody2D fish;
    public float spawnInterval;
    public Vector2 velocity;

	// Use this for initialization
	void Start () {
        StartCoroutine("FishTimer");
	}
    
    IEnumerator FishTimer()
    {
        while (true)
        {
            SpawnAFish();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnAFish()
    {
        var offset = Random.insideUnitSphere * 10;
        Rigidbody2D newFish = (Rigidbody2D)Instantiate(fish, transform.position + offset, transform.rotation);
        newFish.velocity = velocity;
    }
}
