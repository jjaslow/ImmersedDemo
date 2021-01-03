using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRUiKits.Utils
{
    public class CardListManager : MonoBehaviour
    {
        public Transform listContent;
        public GameObject itemTemplate;
        [HideInInspector]
        public List<Card> cardList = new List<Card>(); // Used for data population;
        List<CardItem> cardItems = new List<CardItem>();
        [HideInInspector]
        public Card selectedCard;

        void Awake()
        {
            itemTemplate.SetActive(false);
            PopulateList();
        }

        void SetSelectedCard(Card card) {
            selectedCard = card;
        }

        public void Reset()
        {
            foreach (CardItem _item in cardItems)
            {
                Util.SafeDestroyGameObject(_item);
            }
            cardItems.Clear();
        }

        public void PopulateList()
        {
            for (int i = 0; i < cardList.Count; i++)
            {
                AddCardItem (cardList[i]);
            }
        }

        public void AddCardItem(Card card) {
            CardItem _item = Instantiate(itemTemplate, listContent).GetComponent<CardItem>();
            _item.Card = card;
            _item.gameObject.SetActive(true);
            cardItems.Add(_item);
            _item.OnCardClicked += SetSelectedCard;
        }
    }
}