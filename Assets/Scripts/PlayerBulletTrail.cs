using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletTrail : MonoBehaviour
{
    public float lifetime = .5f;

    Transform target;
    private bool finished = false;
    private bool halfway = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(Transform target)
    {
        this.target = target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            return;
        }

        if (halfway == false)
        {
            var ba = target.position - transform.position;
            transform.position = transform.position + (.3f * ba);
            Vector2 randomOffset2 = Random.insideUnitCircle * .1f;
            Vector3 randomOffset = new Vector3(randomOffset2.x, randomOffset2.y, 0);
            transform.position += randomOffset;
            halfway = true;
        }
        else if (halfway && !finished)
        {
            transform.position = target.position;
            finished = true;
            Destroy(gameObject, lifetime);
        }
    }
}
