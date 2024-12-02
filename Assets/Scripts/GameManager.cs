using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.GPUSort;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
	public Transform PopUpText;
	[SerializeField] FinishScreen finishScreen;
	bool canFinishTheTurn = false;
	float animationDuration = .75f;
	float jumpPower = 2f;
	[SerializeField] GameObject cardPrefab;
	[SerializeField] Transform canvas;
	[SerializeField] PlayerHolder playerHolder;
	[SerializeField] PlayerHolder aiHolder;

	List<PlayerHolder> holders;

	int startingCardCountForEachPlayer = 6;
	int scoreNeededForNewCards = 20;
	int maximumTurnCount = 5;

	public static GameManager instance;

	[SerializeField] Transform allCardsSpawnPoint;

	private List<Card> startingPossibleCards;
	private List<Card> cardsToBeAddedWhenLevelReached;

	[SerializeField] BattleField[] playerBattleFields;
	[SerializeField] BattleField[] aiBattleFields;

	int turnCount = 0;
	bool willEndNextTurn = false;

	public Transform Canvas { get => canvas; set => canvas = value; }
	public PlayerHolder PlayerHolder { get => playerHolder; set => playerHolder = value; }

	private void Awake()
	{
		instance = this;
		holders = new List<PlayerHolder>();
		holders.Add(playerHolder);
		holders.Add(aiHolder);
	}

	private void Start()
	{
		StartCoroutine(StartingTheGame());
	}
	IEnumerator StartingTheGame()
	{
		CloseMouseDragCards();
		CreatePlayers();
		CreatePossibleCards();
		yield return new WaitForSeconds(animationDuration);
		GivePlayersStartingCards();
		yield return new WaitForSeconds(animationDuration);
		ReOpenMouseControls();
	}
	private void CloseMouseDragCards()
	{
		Draggable.SetCanDrag(false);
	}

	private void ReOpenMouseControls()
	{
		Draggable.SetCanDrag(true);
	}
	void CreatePlayers()
	{
		Player ai = new Player(1, "Bilgisayar", 0, true);
		aiHolder.SetPlayer(ai);

		Player player = new Player(0, "PlayerName", 0);
		playerHolder.SetPlayer(player);
	}

	private void CreatePossibleCards()
	{
		startingPossibleCards = new List<Card>();
		startingPossibleCards.Add(CreateCard(new Ucak()));
		startingPossibleCards.Add(CreateCard(new Obus()));
		startingPossibleCards.Add(CreateCard(new Firkateyn()));

		cardsToBeAddedWhenLevelReached = new List<Card>();
		cardsToBeAddedWhenLevelReached.Add(CreateCard(new Siha()));
		cardsToBeAddedWhenLevelReached.Add(CreateCard(new Sida()));
		cardsToBeAddedWhenLevelReached.Add(CreateCard(new KFS()));

		foreach (var holder in holders)
		{
			List<Card> startingPossibleCardsForEachPlayer = AddCardsToPossibleDeck(startingPossibleCards, holder);
			holder.Player.SetPossibleCards(startingPossibleCardsForEachPlayer);
		}
	}

	Card CreateCard(Vehicle vehicle, Transform end = null, Transform start = null)
	{
		if (start == null) start = allCardsSpawnPoint;
		if (end == null) end = allCardsSpawnPoint;
		Card obj = Instantiate(cardPrefab, start).GetComponent<Card>();

		obj.transform.DOJump(end.position, jumpPower, 1, animationDuration)
			.OnComplete(() => { obj.transform.SetParent(end); });

		obj.SetVehicle(vehicle);
		obj.ShowCard();

		return obj;
	}

	private void GivePlayersStartingCards()
	{
		foreach (var holder in holders)
		{
			DrawStartingCardsForPlayer(holder);
		}

	}
	private void DrawStartingCardsForPlayer(PlayerHolder holder)
	{
		for (int i = 0; i < startingCardCountForEachPlayer; i++)
		{
			DrawRandomCardFromPossibleDeckAndAddItToHand(holder);
		}
	}
	void DrawRandomCardFromPossibleDeckAndAddItToHand(PlayerHolder holder)
	{
		var obj = DrawRandomCardFromDeck(holder.Player.PossibleCards, holder.HandTransformParent);
		holder.Player.Cards.Add(obj);
	}
	Card DrawRandomCardFromDeck(List<Card> deck, Transform endPositionParent)
	{
		int cardIndex = (int)Random.Range(0, deck.Count);
		Card card = deck[cardIndex];
		return CopyCard(card, endPositionParent);
	}

	Card CopyCard(Card card, Transform endPositionParent)
	{
		var obj = CreateCard((Vehicle)card.Vehicle.Clone(), endPositionParent, card.transform);
		obj.transform.localScale = Vector3.one;
		obj.transform.DOJump(endPositionParent.transform.position, jumpPower, 1, animationDuration)
			.OnComplete(() => { obj.transform.SetParent(endPositionParent); });

		return obj;
	}

	public void ClearThePlayerTable()
	{
		CloseMouseDragCards();
		foreach (var card in playerHolder.Player.Cards)
		{
			card.transform.DOMove(playerHolder.HandTransformParent.position, animationDuration)
				.OnComplete(() =>
				{
					card.transform.SetParent(playerHolder.HandTransformParent);
					ReOpenMouseControls();
				});
			card.SetAtBattleField(false);
		}

		foreach (var item in playerBattleFields)
		{
			item.Card = null;
		}
	}

	public void ClearTheAITable()
	{
		foreach (var card in aiHolder.Player.Cards)
		{
			card.transform.DOMove(aiHolder.HandTransformParent.position, animationDuration)
				.OnComplete(() => { card.transform.SetParent(aiHolder.HandTransformParent); });
			card.SetAtBattleField(false);
		}

		foreach (var item in aiBattleFields)
		{
			item.Card = null;
		}
	}

	public void FinishTurn()
	{
		if (!IsPlayersAllBattleFieldsFull())
		{
			print("Her bir savaş alanı için kart seçtiğinizden emin olunuz");
			return;
		}
		StartCoroutine(PlayTurn());
		turnCount++;
	}

	IEnumerator PlayTurn()
	{
		CloseMouseDragCards();

		ChooseCardsForAI();
		yield return new WaitForSeconds(animationDuration);

		for (int i = 0; i < aiBattleFields.Length; i++)
		{
			yield return new WaitForSeconds(animationDuration);
			Fight(i);
		}

		yield return new WaitForSeconds(animationDuration);
		DestroyDeadCards();
		yield return new WaitForSeconds(animationDuration);


		yield return new WaitForSeconds(animationDuration);
		CheckIfAnyPlayerReachedNewCardLevel();
		yield return new WaitForSeconds(animationDuration);

		CheckIfGameFinished();
		yield return new WaitForSeconds(animationDuration);


		yield return new WaitForSeconds(animationDuration);
		DrawOneMoreCardsForEachPlayer();
		yield return new WaitForSeconds(animationDuration);

		ClearTheAITable();
		yield return new WaitForSeconds(animationDuration);
		ClearThePlayerTable();

		ReOpenMouseControls();
	}

	private void CheckIfGameFinished()
	{
		if (willEndNextTurn) FinishTheGame();

		if (turnCount >= maximumTurnCount) FinishTheGame();

		foreach (var holder in holders)
		{
			int cardCountAtHand = holder.Player.Cards.Count;

			switch (cardCountAtHand)
			{
				case 0: FinishTheGame(); break;
				case 1:
					willEndNextTurn = true;
					DrawRandomCardFromPossibleDeckAndAddItToHand(holder);
					break;
				default: break;

			}
		}
	}

	private void FinishTheGame()
	{
		finishScreen.gameObject.SetActive(true);
		string finishText = "Game finished.\n";

		if (playerHolder.Player.Score > aiHolder.Player.Score)
		{
			finishText += playerHolder.Player.PlayerName + " has won this game.\n";
			finishText += "Score:" + playerHolder.Player.Score;
		}
		else if (playerHolder.Player.Score < aiHolder.Player.Score)
		{
			finishText += aiHolder.Player.PlayerName + " has won this game.\n";
			finishText += "Score:" + aiHolder.Player.Score;
		}
		else
		{
			finishText += "The scores are equal. It's a draw.\n";
			finishText += "Score:" + playerHolder.Player.Score;
		}

		finishScreen.SetFinishText(finishText);
	}

	private void DrawOneMoreCardsForEachPlayer()
	{
		foreach (var holder in holders)
		{
			DrawRandomCardFromPossibleDeckAndAddItToHand(holder);
		}
	}

	private void CheckIfAnyPlayerReachedNewCardLevel()
	{

		foreach (var holder in holders)
		{
			if (!holder.Player.IsPossibleDeckFull)
			{
				if (holder.Player.Score >= scoreNeededForNewCards)
				{
					AddCardsToPossibleDeck(cardsToBeAddedWhenLevelReached, holder);
					holder.Player.SetPossibleDeckFull(true);
				}
			}
		}

	}

	private List<Card> AddCardsToPossibleDeck(List<Card> cardsToBeAdded, PlayerHolder holder)
	{
		foreach (var card in cardsToBeAdded)
		{
			Card newCard = CopyCard(card, holder.PossibleCardsTranformParent);
			holder.Player.PossibleCards.Add(newCard);
		}

		return holder.Player.Cards;
	}

	private void DestroyDeadCards()
	{
		foreach (var battleField in aiBattleFields)
		{
			if (battleField.Card.Vehicle.IsDead())
			{
				aiHolder.Player.Cards.Remove(battleField.Card);
				int level = battleField.Card.Vehicle.Level;
				if (level < 10) level = 10;
				playerHolder.Player.AddScore(level);
				playerHolder.UpdateScore();
				DamagePopup.Create(playerHolder.transform.position, "+" + level + " XP", false);

				battleField.Card.transform.DOPunchScale(Vector3.one, animationDuration)
					.OnComplete(() =>
					{
						Destroy(battleField.Card.gameObject);
						battleField.Card = null;
					});

			}
		}

		foreach (var battleField in playerBattleFields)
		{
			if (battleField.Card.Vehicle.IsDead())
			{
				playerHolder.Player.Cards.Remove(battleField.Card);
				int level = battleField.Card.Vehicle.Level;
				if (level < 10) level = 10;
				aiHolder.Player.AddScore(level);
				aiHolder.UpdateScore();
				DamagePopup.Create(aiHolder.transform.position, "+" + level + " XP", false);

				battleField.Card.transform.DOPunchScale(Vector3.one, animationDuration)
					.OnComplete(() =>
					{
						Destroy(battleField.Card.gameObject);
						battleField.Card = null;
					});
			}
		}
	}

	private void Fight(int battleFieldIndex)
	{

		Card playerCard = playerBattleFields[battleFieldIndex].Card;
		Card aiCard = aiBattleFields[battleFieldIndex].Card;

		float damageAmount = playerCard.Vehicle.GiveDamage(aiCard.Vehicle);
		bool isCrit = damageAmount > playerCard.Vehicle.attack;//if advantage added
		DamagePopup.Create(aiBattleFields[battleFieldIndex].transform.position, "-" + damageAmount, isCrit);

		damageAmount = aiCard.Vehicle.GiveDamage(playerCard.Vehicle);
		isCrit = damageAmount > aiCard.Vehicle.attack;
		DamagePopup.Create(playerBattleFields[battleFieldIndex].transform.position, "-" + damageAmount, isCrit);

		playerCard.SetFought(true);
		playerCard.ShowCard();
		aiCard.SetFought(true);
		aiCard.ShowCard();
	}

	private bool IsPlayersAllBattleFieldsFull()
	{
		foreach (var item in playerBattleFields)
		{
			if (item.Card == null) return false;
		}
		return true;
	}

	void ChooseCardsForAI()
	{
		List<Card> selectableCards = new List<Card>();
		for (int i = 0; i < aiHolder.Player.Cards.Count; i++)
		{
			selectableCards.Add(aiHolder.Player.Cards[i]);
		}

		foreach (var battleField in aiBattleFields)
		{
			Card card = selectableCards[Random.Range(0, selectableCards.Count)];
			selectableCards.Remove(card);

			battleField.Card = card;
			card.transform.SetParent(battleField.transform);
			//Visualization
			card.transform.DOJump(battleField.transform.position, jumpPower, 1, animationDuration);
			((RectTransform)card.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Card.normalCardSize.y);
			((RectTransform)card.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Card.normalCardSize.x);
		}
	}
}
