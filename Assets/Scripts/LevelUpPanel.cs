using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpPanel : MonoBehaviour
{
    [SerializeField] private GameObject PlaceHolder1;
    [SerializeField] private GameObject PlaceHolder2;
    public List<GameObject> AbilityCards;

    private GameObject card1;
    private GameObject card2;
    private void OnEnable()
    {
        //choose random and showcase
        showCards();
    }

    private void showCards()
    {
        Time.timeScale = 0;
        //choose two random cards that are not chosen
        card1 = getUnSelectedCard();
        card2 = getUnSelectedCard();


        card1.transform.position = PlaceHolder1.transform.position;
        card2.transform.position = PlaceHolder2.transform.position;

        card1.SetActive(true);
        card2.SetActive(true);

        
    }

    private GameObject getUnSelectedCard()
    {
        int randomIndex = 0;
        for(int i =0; i < AbilityCards.Count; i++)
        {
            randomIndex = Random.Range(0, AbilityCards.Count);
            if (AbilityCards[randomIndex].gameObject.GetComponent<AbilityCard>().isActive() || AbilityCards[randomIndex] == card2 || AbilityCards[randomIndex] == card1)
            {
                continue;
            }
            else
            {
                break;
            }
        }
        

        return AbilityCards[randomIndex];
    }

    public void onSelect()
    {
        card1.SetActive(false);
        card2.SetActive(false);
        card1 = null;
        card2 = null;
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
