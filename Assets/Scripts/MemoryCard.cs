using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCard : MonoBehaviour
{
    // Variable is private to this class and visible in the Inspector
    [SerializeField] private GameObject cardBack;
    [SerializeField] private SceneController controller;
    // [SerializeField] private Sprite image;

    // Added getter function (an idiom common in languages like C# and Java)
    private int m_id;
    public int id
    {
        get { return m_id; }
    }

    // Public method that other scripts can use to pass new sprites to this object
    public void SetCard(int id, Sprite image)
    {
        m_id = id;
        // SpriteRenderer code line just like in the deleted example code
        GetComponent<SpriteRenderer>().sprite = image;
    }

    /*
    private void Start()
    {
        // Set the sprite for this SpriteRenderer component;
        // GetComponent<SpriteRenderer>().sprite = image;
    }
    */

    // MonoBehaviour function that is called when GameObject is clicked
    public void OnMouseDown()
    {
        // Check the controller's canReveal property to make sure only two cards are revealed at a time
        if(cardBack.activeSelf && controller.canReveal)
        {
            // Set the object to inactive/invisible
            cardBack.SetActive(false);
            // Notify the controller when this card is revealed
            controller.CardRevealed(this);
            //Debug.Log(this.id);
        }
    }

    // A public method so that SceneController can hide the card again (by turning card_back back on)
    public void Unreveal()
    {
        cardBack.SetActive(true);
    }
}
