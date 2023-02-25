using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn_characters : MonoBehaviour
{
    public List <GameObject> characters = new List <GameObject> ();
    public Transform current_character;

    private void Start ()
    {
        spawn_character ();
    }

    private void Update ()
    {
        if (transform.childCount  <= 0)
            spawn_character ();  
    }

    
    
    void spawn_character() // this spawns a character
    {
        
        int random_number;
        
        random_number = Random . Range (0, characters.Count) ;

        parent_prefab () ;
        set_parent () ;

        current_character.position = current_character . position + new Vector3 (0,1,0) ;


        Transform parent_prefab () => Instantiate (characters[random_number]) . transform ;
        void set_parent () => current_character.SetParent (transform) ;

        
    }
    
    

    public interface IInterface
    {
        void do_a_thing();
    }
}

/* if everything is in braille, would I structure my code doferently?
 * perhaps not.
 * 
 * currently spacing things out a little bit like: () and the . 
 * the => is not as ckear in braille 
 */