using UnityEngine;
using VRUiKits.Utils;

public class ExampleDynamicCardList : MonoBehaviour
{
    public CardListManager clm;
    void Start()
    {
        UpdateList();
    }
    public void UpdateList()
    {
        clm.Reset();
        // If you need to clear the cardlist, and repopulate the list,
        // call clm.cardList.Clear(); If you want to append to the list,
        // don't call it.
        clm.cardList.Clear();
        for (int i = 0; i <= 5; i++)
        {
            Card card = new Card();
            card.title = "Test " + i.ToString();
            clm.cardList.Add(card);
        }
        // PopulateList will draw the UI element in the cardlist.
        clm.PopulateList();
    }
}
