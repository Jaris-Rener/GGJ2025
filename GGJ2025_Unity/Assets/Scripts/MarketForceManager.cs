using System;
using System.Collections.Generic;
using UnityEngine;

public class MarketForceManager : Singleton<MarketForceManager>
{
    public event Action OnMarketUpdated;
    
    public int beachPriceLevel = 0;
    public int suburbPriceLevel = 0;
    public int cityPriceLevel = 0;

    public int maxPriceLevel = 2;
    public int minPriceLevel = -2;

    public bool removeDrawnCardsFromDecks = false;

    private readonly List<int> defaultDeck = new List<int> { +2, +1, 0, -1, -2 };

    private DeckManager beachDeck;
    private DeckManager suburbDeck;
    private DeckManager cityDeck;

    public int savedBeachCard;
    public int savedSuburbCard;
    public int savedCityCard;

    private void OnEnable()
    {
        GlobalStepManager.OnStep += UpdateMarket;
        InitializeDecks();
    }

    private void OnDisable()
    {
        GlobalStepManager.OnStep -= UpdateMarket;
    }

    private void InitializeDecks()
    {
        beachDeck = new DeckManager(defaultDeck, removeDrawnCardsFromDecks);
        suburbDeck = new DeckManager(defaultDeck, removeDrawnCardsFromDecks);
        cityDeck = new DeckManager(defaultDeck, removeDrawnCardsFromDecks);

        // Initialize saved cards with an initial draw
        savedBeachCard = beachDeck.DrawCard();
        savedSuburbCard = suburbDeck.DrawCard();
        savedCityCard = cityDeck.DrawCard();
    }

    private void UpdateMarket()
    {
        // Apply saved cards to adjust price levels
        AdjustPriceLevel(ref beachPriceLevel, savedBeachCard);
        AdjustPriceLevel(ref suburbPriceLevel, savedSuburbCard);
        AdjustPriceLevel(ref cityPriceLevel, savedCityCard);

        // Draw new cards and replace the saved ones
        savedBeachCard = beachDeck.DrawCard();
        savedSuburbCard = suburbDeck.DrawCard();
        savedCityCard = cityDeck.DrawCard();

        OnCardDrawn(savedBeachCard, beachDeck); // Trigger the callback
        OnCardDrawn(savedSuburbCard, suburbDeck); // Trigger the callback
        OnCardDrawn(savedCityCard, cityDeck); // Trigger the callback

        OnMarketUpdated?.Invoke();
        
        // Log the results for debugging
        Debug.Log($"Market Update: Beach Level = {beachPriceLevel}, Suburb Level = {suburbPriceLevel}, City Level = {cityPriceLevel}");
    }

    private void AdjustPriceLevel(ref int priceLevel, int cardValue)
    {
        priceLevel = Mathf.Clamp(priceLevel + cardValue, minPriceLevel, maxPriceLevel);
    }

<<<<<<< Updated upstream
    public int GetCurrentMarketForce(Location location)
    {
        return location switch
        {
            Location.Beach => beachPriceLevel,
            Location.City => cityPriceLevel,
            Location.Suburbs => suburbPriceLevel,
            _ => throw new ArgumentOutOfRangeException(nameof(location), location, null)
        };
    }

    public int GetMarketDirection(Location location)
    {
        return GetNextMarketForce(location) - GetCurrentMarketForce(location);
    }

    private int GetNextMarketForce(Location location)
    {
        return 0;
    }

    public float GetMultiplier(Location location)
    {
        return IntToFloatMapper.GetMultiplier(GetCurrentMarketForce(location));
    }
}
=======
    private void OnCardDrawn(int cardValue, DeckManager deck)
    {
        // Callback function for when a card is drawn (extendable for custom behavior)
        Debug.Log($"Card Drawn: {cardValue}");
    }

    // Expose deck functions
    public void ResetDecks()
    {
        beachDeck.ResetDeck();
        suburbDeck.ResetDeck();
        cityDeck.ResetDeck();
    }

    public void AddCardToDeck(string deckName, int value)
    {
        GetDeck(deckName)?.AddCard(value);
    }

    public bool RemoveCardFromDeck(string deckName, int value)
    {
        return GetDeck(deckName)?.RemoveCard(value) ?? false;
    }

    private DeckManager GetDeck(string deckName)
    {
        return deckName.ToLower() switch
        {
            "beach" => beachDeck,
            "suburb" => suburbDeck,
            "city" => cityDeck,
            _ => null
        };
    }

    private class DeckManager
    {
        private readonly List<int> defaultDeck;
        private List<int> currentDeck;
        private readonly bool removeDrawnCards;

        public DeckManager(List<int> defaultDeck, bool removeDrawnCards)
        {
            this.defaultDeck = new List<int>(defaultDeck);
            this.removeDrawnCards = removeDrawnCards;
            ResetDeck();
        }

        public int DrawCard()
        {
            if (currentDeck.Count == 0)
            {
                ResetDeck();
            }

            int index = UnityEngine.Random.Range(0, currentDeck.Count);
            int cardValue = currentDeck[index];
            if (removeDrawnCards)
            {
                RemoveCard(cardValue);
            }
            return cardValue;
        }

        public void ResetDeck()
        {
            currentDeck = new List<int>(defaultDeck);
        }

        public void AddCard(int value)
        {
            currentDeck.Add(value);
        }

        public bool RemoveCard(int value)
        {
            int defaultCount = CountOccurrences(defaultDeck, value);
            int currentCount = CountOccurrences(currentDeck, value);

            if (currentCount > defaultCount)
            {
                Debug.Log("Removed Card");
                return currentDeck.Remove(value);
            }

            Debug.Log("No Card Removed");
            return false;
        }

        private int CountOccurrences(List<int> list, int value)
        {
            int count = 0;
            foreach (int item in list)
            {
                if (item == value)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
>>>>>>> Stashed changes
