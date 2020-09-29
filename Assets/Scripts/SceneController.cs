using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Values for how many grid spaces to make and how far apart to place them
    public const int gridRows = 2;
    public const int gridCols = 4;
    public const float offsetX = 2f;
    public const float offsetY = 2.5f;

    // Reference for the card in the scene
    [SerializeField] private MemoryCard originalCard;
    // An array for references to the sprite assets
    [SerializeField] private Sprite[] images;
    [SerializeField] private TextMesh scoreLabel;

    // Revealed cards
    private MemoryCard m_firstRevealed;
    private MemoryCard m_secondRevealed;

    private int m_score;

    // Start is called before the first frame update
    void Start()
    {
        // Position of the first card; all other cards will be offset from here
        Vector3 startPos = originalCard.transform.position;

        // Declare an integer array with a pair of IDs for all four card sprites
        int[] numbers = { 0, 0, 1, 1, 2, 2, 3, 3 };
        // Call a function that will shuffle the elements of the array
        numbers = ShuffleArray(numbers);

        // Nested loops to define both columns and rows of the grid
        for(int i = 0; i < gridCols; i++)
        {
            for(int j = 0; j < gridRows; j++)
            {
                // Container reference for either the original card or the copies
                MemoryCard card;
                if (i == 0 && j == 0)
                    card = originalCard;
                else
                    card = Instantiate(originalCard);

                // Counts index by grid: 0, 4, 1, 5, 2, 6, 3, 7
                // Each index corresponds to the shuffle array's index that contains
                // the card's ID number.                
                // CARD GRID
                // 0 | 1 | 2 | 3
                //---------------
                // 4 | 5 | 6 | 7
                int index = j * gridCols + i;
                // Retrieve IDs from the shuffled list instead
                int id = numbers[index];
                // Assigned the shuffle card symbol
                card.SetCard(id, images[id]);

                // Place the card into the correct area of the grid in the scene
                float posX = (offsetX * i) + startPos.x;
                float posY = - (offsetY * j) + startPos.y;
                // For 2D graphics, you only need to offset X and Y; keep Z the same
                card.transform.position = new Vector3(posX, posY, startPos.z);
            }
        }
    }

    // Function to shuffle the elements of the array (Knuth shuffle algorithm)
    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for(int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }
        return newArray;
    }

    public bool canReveal
    {
        // Getter function that returns false if there's already a second card revealed
        get { return m_secondRevealed == null; }
    }

    public void CardRevealed(MemoryCard card)
    {
        // Store card objects in one of the two card variables, depending on if the first variable is already occupied
        if (m_firstRevealed == null)
        {
            m_firstRevealed = card; 
        }
        else
        {
            m_secondRevealed = card;
            // Call the coroutine when both cards are revealed
            StartCoroutine(CheckMatch());
        }
    }

    // Problem: Cards that don't match are considered matched and vice versa
    private IEnumerator CheckMatch()
    {
        // Increment the score if the revealed cards have matching IDs
        if(m_firstRevealed.id == m_secondRevealed.id)
        {
            m_score++;
            // Text displayed is a property to set on text objects
            scoreLabel.text = "Score: " + m_score;
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            // Unreveal the cards
            m_firstRevealed.Unreveal();
            m_secondRevealed.Unreveal();
        }

        // Clear out the variables whether or not a match was made
        m_firstRevealed = null;
        m_secondRevealed = null;
    }

    // Reset the table (Gets called via SendMessage)
    public void Restart()
    {
        SceneManager.LoadScene("Scene");
    }
}
