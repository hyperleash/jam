using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public int spellDamage = 10;

    protected void removeSpell(){
        Destroy(gameObject);
    }

    protected void enableCollider(){
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    protected void disableCollider(){
         gameObject.GetComponent<Collider2D>().enabled = false;
    }

    /*protected void OnCollisionEnter2D(Collision2D other){
        if(other.collider.tag == "enemy"){
            Debug.Log("Collision");
            other.gameObject.GetComponent<HealthBehaviour>().Health = other.gameObject.GetComponent<HealthBehaviour>().Health - spellDamage;
        }
    }*/

    protected void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "enemy"){
            Debug.Log("Collision");
            other.gameObject.GetComponent<HealthBehaviour>().Health = other.gameObject.GetComponent<HealthBehaviour>().Health - spellDamage;
        }
    }


}
