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

    public DeckDrawSettings NumberSettings = new DeckDrawSettings();

    [Serializable]
    public struct DeckDrawSettings
    {
        public NumberDrawnSettings PlusTwo;
        public NumberDrawnSettings PlusOne;
        public NumberDrawnSettings Zero;
        public NumberDrawnSettings MinusOne;
        public NumberDrawnSettings MinusTwo;
    }

    [Serializable]
    public struct NumberDrawnSettings
    {
        public int[] NumbersToAdd;
        public int[] NumbersToRemove;
    }

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

        OnCardDrawn(savedBeachCard, beachDeck);
        OnCardDrawn(savedSuburbCard, suburbDeck);
        OnCardDrawn(savedCityCard, cityDeck);

        string deckLog = "";
        for (int i = 0; i < beachDeck.currentDeck.Count; i++)
        {
            deckLog += beachDeck.currentDeck[i] + " ";
        }

        Debug.Log(deckLog);

        OnMarketUpdated?.Invoke();
        
        // Log the results for debugging
        Debug.Log($"Market Update: Beach Level = {beachPriceLevel}, Suburb Level = {suburbPriceLevel}, City Level = {cityPriceLevel}");
    }

    private void AdjustPriceLevel(ref int priceLevel, int cardValue)
    {
        priceLevel = Mathf.Clamp(priceLevel + cardValue, minPriceLevel, maxPriceLevel);
    }

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
        return location switch
        {
            Location.Beach => savedBeachCard,
            Location.City => savedCityCard,
            Location.Suburbs => savedSuburbCard,
            _ => throw new ArgumentOutOfRangeException(nameof(location), location, null)
        };
    }

    public float GetMultiplier(Location location)
    {
        return IntToFloatMapper.GetMultiplier(GetCurrentMarketForce(location));
    }

    private void OnCardDrawn(int cardValue, DeckManager deck)
    {
        if (cardValue == -2) 
        {
            for (int i = 0; i < NumberSettings.MinusTwo.NumbersToAdd.Length; i++)
            {
                deck.AddCard(NumberSettings.MinusTwo.NumbersToAdd[i]);
            }
            for (int i = 0; i < NumberSettings.MinusTwo.NumbersToRemove.Length; i++)
            {
                deck.RemoveCard(NumberSettings.MinusTwo.NumbersToRemove[i]);
            }
        }
        if (cardValue == -1)
        {
            for (int i = 0; i < NumberSettings.MinusOne.NumbersToAdd.Length; i++)
            {
                deck.AddCard(NumberSettings.MinusOne.NumbersToAdd[i]);
            }
            for (int i = 0; i < NumberSettings.MinusOne.NumbersToRemove.Length; i++)
            {
                deck.RemoveCard(NumberSettings.MinusOne.NumbersToRemove[i]);

            }
        }
        if (cardValue == 0)
        {
            for (int i = 0; i < NumberSettings.Zero.NumbersToAdd.Length; i++)
            {
                deck.AddCard(NumberSettings.Zero.NumbersToAdd[i]);
            }
            for (int i = 0; i < NumberSettings.Zero.NumbersToRemove.Length; i++)
            {
                deck.RemoveCard(NumberSettings.Zero.NumbersToRemove[i]);
            }
        }
        if (cardValue == 1)
        {
            for (int i = 0; i < NumberSettings.PlusOne.NumbersToAdd.Length; i++)
            {
                deck.AddCard(NumberSettings.PlusOne.NumbersToAdd[i]);
            }
            for (int i = 0; i < NumberSettings.PlusOne.NumbersToRemove.Length; i++)
            {
                deck.RemoveCard(NumberSettings.PlusOne.NumbersToRemove[i]);
            }
        }
        if (cardValue == 2)
        {
            for (int i = 0; i < NumberSettings.PlusTwo.NumbersToAdd.Length; i++)
            {
                deck.AddCard(NumberSettings.PlusTwo.NumbersToAdd[i]);
            }
            for (int i = 0; i < NumberSettings.PlusTwo.NumbersToRemove.Length; i++)
            {
                deck.RemoveCard(NumberSettings.PlusTwo.NumbersToRemove[i]);
            }
        }
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
        public List<int> currentDeck;
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