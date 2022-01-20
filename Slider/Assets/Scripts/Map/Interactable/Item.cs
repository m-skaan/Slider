using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour 
{

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D myCollider;

    // animation
    
    [SerializeField] private float pickUpDuration;
    [SerializeField] private AnimationCurve xPickUpMotion;
    [SerializeField] private AnimationCurve yPickUpMotion;


    private void Awake() 
    {
        
    }


    public void PickUpItem(Transform pickLocation, System.Action callback=null) // pickLocation may be moving
    {
        StartCoroutine(AnimatePickUp(pickLocation, callback));
    }

    public void DropItem(Vector3 dropLocation) 
    {
        StartCoroutine(AnimateDrop(dropLocation));
    }


    private IEnumerator AnimatePickUp(Transform target, System.Action callback=null)
    {
        float t = 0;

        Vector3 start = new Vector3(transform.position.x, transform.position.y);

        while (t < pickUpDuration)
        {
            float x = xPickUpMotion.Evaluate(t / pickUpDuration);
            float y = yPickUpMotion.Evaluate(t / pickUpDuration);
            Vector3 pos = new Vector3(Mathf.Lerp(start.x, target.transform.position.x, x),
                                      Mathf.Lerp(start.y, target.transform.position.y, y));
            
            spriteRenderer.transform.position = pos;

            yield return null;
            t += Time.deltaTime;
        }

        transform.position = target.position;
        spriteRenderer.transform.position = target.position;
        myCollider.enabled = false;
        callback();
    }

    private IEnumerator AnimateDrop(Vector3 target)
    {
        float t = pickUpDuration;

        Vector3 start = new Vector3(transform.position.x, transform.position.y);
        transform.position = target;
        myCollider.enabled = true;

        while (t >= 0)
        {
            float x = xPickUpMotion.Evaluate(t / pickUpDuration);
            float y = yPickUpMotion.Evaluate(t / pickUpDuration);
            Vector3 pos = new Vector3(Mathf.Lerp(target.x, start.x, x),
                                      Mathf.Lerp(target.y, start.y, y));
            
            spriteRenderer.transform.position = pos;

            yield return null;
            t -= Time.deltaTime;
        }

        transform.position = target;
        spriteRenderer.transform.position = target;
    }
}