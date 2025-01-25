using NUnit.Framework;
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

    
    public List<int> randomizedStartingPrices = new List<int> { 2, 0, -1 };

    public List<int> randomizedInitialMarket = new List<int> { -1, 1, -1 };

    private DeckManager beachDeck;
    private DeckManager suburbDeck;
    private DeckManager cityDeck;

    public int savedBeachCard;
    public int savedSuburbCard;
    public int savedCityCard;

    public float[] intToFloatMap = { 0.5f, 0.75f, 1.0f, 1.5f, 2.0f };

    public DeckDrawSettings beachNumberSettings = new DeckDrawSettings();
    public DeckDrawSettings suburbNumberSettings = new DeckDrawSettings();
    public DeckDrawSettings cityNumberSettings = new DeckDrawSettings();
    

    [Serializable]
    public struct DeckDrawSettings
    {
        public NumberDrawnSettings PlusTwo;
        public NumberDrawnSettings PlusOne;
        public NumberDrawnSettings Zero;
        public NumberDrawnSettings MinusOne;
        public NumberDrawnSettings MinusTwo;
        public NumberDrawnSettings MaxOut;
        public NumberDrawnSettings BottomOut;
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

        BalanceRandomIndicies(randomizedStartingPrices);
        BalanceRandomIndicies(randomizedInitialMarket);

        // Initialize saved cards with a balanced random number
        savedBeachCard = randomizedInitialMarket[0];
        savedSuburbCard = randomizedInitialMarket[1];
        savedCityCard = randomizedInitialMarket[2];

        beachPriceLevel = randomizedStartingPrices[0];
        suburbPriceLevel = randomizedStartingPrices[1];
        cityPriceLevel = randomizedStartingPrices[2];
    }

    private void BalanceRandomIndicies(List<int> randomizedIndices)
    {
        for (int i = 0; i < randomizedIndices.Count; i++)
        {
            // Generate a random index within the list range
            int randomIndex = UnityEngine.Random.Range(0, randomizedIndices.Count);

            // Swap the current element with the element at the random index
            (randomizedIndices[i], randomizedIndices[randomIndex]) = (randomizedIndices[randomIndex], randomizedIndices[i]);
        }
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

        if (beachPriceLevel == maxPriceLevel && savedBeachCard > 0 || beachPriceLevel == minPriceLevel && savedBeachCard < 0)
        {
            savedBeachCard = 0;
            if (beachPriceLevel == maxPriceLevel)
            {
                for (int i = 0; i < beachNumberSettings.MaxOut.NumbersToAdd.Length; i++)
                {
                    beachDeck.AddCard(beachNumberSettings.MaxOut.NumbersToAdd[i]);
                }
                for (int i = 0; i < beachNumberSettings.MaxOut.NumbersToRemove.Length; i++)
                {
                    beachDeck.RemoveCard(beachNumberSettings.MaxOut.NumbersToAdd[i]);
                }
            }
            if (beachPriceLevel == minPriceLevel)
            {
                for (int i = 0; i < beachNumberSettings.BottomOut.NumbersToAdd.Length; i++)
                {
                    beachDeck.AddCard(beachNumberSettings.BottomOut.NumbersToAdd[i]);
                }
                for (int i = 0; i < beachNumberSettings.BottomOut.NumbersToRemove.Length; i++)
                {
                    beachDeck.RemoveCard(beachNumberSettings.BottomOut.NumbersToAdd[i]);
                }
            }
        }

        if (suburbPriceLevel == maxPriceLevel && savedSuburbCard > 0 || suburbPriceLevel == minPriceLevel && savedSuburbCard < 0)
        {
            savedSuburbCard = 0;
            if (suburbPriceLevel == maxPriceLevel)
            {
                for (int i = 0; i < suburbNumberSettings.MaxOut.NumbersToAdd.Length; i++)
                {
                    suburbDeck.AddCard(suburbNumberSettings.MaxOut.NumbersToAdd[i]);
                }
                for (int i = 0; i < suburbNumberSettings.MaxOut.NumbersToRemove.Length; i++)
                {
                    suburbDeck.RemoveCard(suburbNumberSettings.MaxOut.NumbersToAdd[i]);
                }
            }
            if (suburbPriceLevel == minPriceLevel)
            {
                for (int i = 0; i < suburbNumberSettings.BottomOut.NumbersToAdd.Length; i++)
                {
                    suburbDeck.AddCard(suburbNumberSettings.BottomOut.NumbersToAdd[i]);
                }
                for (int i = 0; i < suburbNumberSettings.BottomOut.NumbersToRemove.Length; i++)
                {
                    suburbDeck.RemoveCard(suburbNumberSettings.BottomOut.NumbersToAdd[i]);
                }
            }
        }

        if (cityPriceLevel == maxPriceLevel && savedCityCard > 0 || cityPriceLevel == minPriceLevel && savedCityCard < 0)
        {
            savedCityCard = 0;
            if (cityPriceLevel == maxPriceLevel)
            {
                for (int i = 0; i < cityNumberSettings.MaxOut.NumbersToAdd.Length; i++)
                {
                    cityDeck.AddCard(cityNumberSettings.MaxOut.NumbersToAdd[i]);
                }
                for (int i = 0; i < cityNumberSettings.MaxOut.NumbersToRemove.Length; i++)
                {
                    cityDeck.RemoveCard(cityNumberSettings.MaxOut.NumbersToAdd[i]);
                }
            }
            if (cityPriceLevel == minPriceLevel)
            {
                for (int i = 0; i < cityNumberSettings.BottomOut.NumbersToAdd.Length; i++)
                {
                    cityDeck.AddCard(cityNumberSettings.BottomOut.NumbersToAdd[i]);
                }
                for (int i = 0; i < cityNumberSettings.BottomOut.NumbersToRemove.Length; i++)
                {
                    cityDeck.RemoveCard(cityNumberSettings.BottomOut.NumbersToAdd[i]);
                }
            }
        }

        string beachDeckLog = "";
        for (int i = 0; i < beachDeck.currentDeck.Count; i++)
        {
            beachDeckLog += beachDeck.currentDeck[i] + " ";
        }
        string suburbDeckLog = "";
        for (int i = 0; i < suburbDeck.currentDeck.Count; i++)
        {
            suburbDeckLog += suburbDeck.currentDeck[i] + " ";
        }
        string cityDeckLog = "";
        for (int i = 0; i < cityDeck.currentDeck.Count; i++)
        {
            cityDeckLog += cityDeck.currentDeck[i] + " ";
        }

        Debug.Log("Beach Deck: " + beachDeckLog);
        Debug.Log("Suburb Deck: " + suburbDeckLog);
        Debug.Log("City Deck: " + cityDeckLog);

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

    public float GetMultiplier(Location key)
    {
        
        int index = (int)GetCurrentMarketForce(key) + 2; // Map key (-2 to 2) to array index (0 to 4)

        if (key >= 0 && (int)key < intToFloatMap.Length)
        {
            return intToFloatMap[index];
        }
        throw new IndexOutOfRangeException($"Key {key} is out of range.");
    }

    private void OnCardDrawn(int cardValue, DeckManager deck)
    {
        DeckDrawSettings NumberSettings = new DeckDrawSettings();
        if (deck == beachDeck)
        {
            NumberSettings = beachNumberSettings;
        }
        else if (deck == cityDeck) 
        {
            NumberSettings = cityNumberSettings;
        }
        else if (deck == suburbDeck) 
        {
            NumberSettings = suburbNumberSettings;
        }

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