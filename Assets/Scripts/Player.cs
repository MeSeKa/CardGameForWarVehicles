using UnityEngine;
using System.Collections.Generic;
using System;

public class Player
{
	private bool isAI;
	private int playerID;
	private string playerName;
	private int score;

	private bool isPossibleDeckFull;
	List<Card> cards = new List<Card>();
	List<Card> possibleCards = new List<Card>();

	public Player()
	{ }
	public Player(int playerID, string playerName, int score, bool isAI = false, bool isPossibleDeckFull = false)
	{
		this.playerID = playerID;
		this.playerName = playerName;
		this.score = score;
		this.isAI = isAI;

		this.possibleCards = new List<Card>();
		this.cards = new List<Card>();
		this.isPossibleDeckFull = isPossibleDeckFull;
	}

	public void AddScore(int amount)
	{
		score += amount;
	}

	public int PlayerID { get => playerID; }
	public string PlayerName { get => playerName; set => playerName = value; }
	public int Score { get => score; }
	public List<Card> Cards { get => cards; }
	public bool IsAI { get => isAI; set => isAI = value; }
	public bool IsPossibleDeckFull { get => isPossibleDeckFull; private set => isPossibleDeckFull = value; }
	public List<Card> PossibleCards { get => possibleCards; }

	public void SetStartingCards(List<Card> cards)
	{
		this.cards = cards;
	}
	public void SetPossibleCards(List<Card> cards)
	{
		this.cards = cards;
	}

	public bool CanPlayThisCard(Card selectedCard)
	{
		if(!cards.Contains(selectedCard)) return false;

		//Sadece 2 olasılık olursa seçilen kart oynayabilir:
		//1.Seçilen kart savaşmamışsa
		//2.Seçilen kart savaşmış ama savaşmamış kartların hepsi zaten sahada

		//seçilen kart savaşmamış
		if (!selectedCard.HaveFought) return true;

		//savaşmamış kartlar içinde savaş alanında olmayan var
		foreach (var card in cards)
		{
			if (card == selectedCard) continue;
			if (!card.HaveFought && !card.AtBattleField)
			{
				Debug.Log(card.name +" kartını oynamadınız" + card.transform.name);
				Debug.Log("Hala savaşmamış kartlarınız varken bu kartı oynayamazsınız");
				return false;
			}
		}

		//savaşmamış kartların hepsi zaten sahada
		return true;
	}
	public void SetPossibleDeckFull(bool set)
	{
		isPossibleDeckFull = set;
	}
}
