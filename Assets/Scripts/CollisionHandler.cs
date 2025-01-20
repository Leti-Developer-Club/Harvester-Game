using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private BoxCollider2D bc;
    public ObjectPooling objectPooling;
    public ScoreHandler scoreHandler;
    public AudioScript audioManager;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Objects have entered the trigger zone: {collision.gameObject.name}");

        //check if the object has the Fruit tag and collided with the basket 
        if (collision.CompareTag("Fruit") && gameObject.CompareTag("Basket"))
        {
            scoreHandler.HandleBasketCollision(10);
            objectPooling.ReturnToPool(collision.gameObject);
            //scoreHandler.HandleBasketCollision(10);
            Debug.Log("Fruit collided by basket");
            audioManager.PlayFruitCollection(true);
        }
        //check if the object has the Enemy tag and collided with the Basket 
        else if (collision.CompareTag("Enemy") && gameObject.CompareTag("Basket"))
        {
            scoreHandler.HandleBasketCollision(-5);
            objectPooling.ReturnToPool(collision.gameObject);
            //scoreHandler.HandleBasketCollision(-5);
            Debug.Log("Enemy collided");
            audioManager.PlayEnemyCollection(true);

        }
        //check if the object collides with the bottom border
        else if (collision.CompareTag("Fruit") || collision.CompareTag("Enemy"))
        {
            if (gameObject.CompareTag("BottomBorder"))
            {
                objectPooling.ReturnToPool(collision.gameObject);
                Debug.Log("Object not collected: return to pool");
            }
        }
    }
}
