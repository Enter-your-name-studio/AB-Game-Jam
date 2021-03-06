﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DemonDialogUI : MonoBehaviour {
	public DemonDialog demon;

	public Action<GameObject> OnCorrectGift;

	[SerializeField] CraftPanel craftPanel;
	[SerializeField] Inventory inventory;
	[SerializeField] PlayerHp hp;

	[SerializeField] CanvasGroup canvasGroup;
	[SerializeField] Image image;
	[SerializeField] TextMeshProUGUI dialogText;

	private void Awake() {
		canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
		canvasGroup.alpha = 0.0f;
	}

	public void ShowDialog() {
		Item.isCanDrag = false;
		herowalking.isCanMove = false;

		canvasGroup.interactable = canvasGroup.blocksRaycasts = true;
		LeanTween.value(gameObject, canvasGroup.alpha, 1.0f, 0.2f)
		.setOnUpdate((float a)=> { 
			canvasGroup.alpha = a;
		})
		.setOnComplete(()=> {
			Item.isCanDrag = true;
		});

		image.sprite = demon.sprite;
		dialogText.text = demon.dialogText;
		if(demon.walking != null)
			demon.walking.isMoving = false;
	}

	public void CloseDialog() {
		Item.isCanDrag = false;
		herowalking.isCanMove = true;
		canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
		LeanTween.value(gameObject, canvasGroup.alpha, 0.0f, 0.2f)
		.setOnUpdate((float a) => {
			canvasGroup.alpha = a;
		})
		.setOnComplete(()=> { 
			if(demon.walking)
				demon.walking.isMoving = true;
		});
	}

	public void CorrectGift(Item item, ItemType reward) {
		if (craftPanel.items.Contains(item))
			craftPanel.items.Remove(item);

		if (inventory.items.Contains(item))
			inventory.items.Remove(item);

		Item createdItem = craftPanel.CreateItem(reward);
		inventory.AddItem(createdItem);

		Destroy(item.gameObject);
		Destroy(createdItem.gameObject);

		dialogText.text = "Thank you, Santa!";
		demon.isGifted = true;
		demon.OnGifted?.Invoke();

		LeanTween.delayedCall(1.0f, ()=> {
			CloseDialog();
			OnCorrectGift?.Invoke(demon.gameObject);
			}
		);
	}

	public void WrongGift() {
		dialogText.text = "No, I don't want this. \n" + demon.dialogText;
		--hp.CurrHp;
	}
}
