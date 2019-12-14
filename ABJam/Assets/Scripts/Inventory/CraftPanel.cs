﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CraftPanel : MonoBehaviour, IDropHandler {
	[NonSerialized] public List<Item> items;

	[SerializeField] ItemType[] types;
	[SerializeField] Sprite[] images;

	[Header("Refs")]
	[SerializeField] TextMeshProUGUI craftText;

	private void Awake() {
		items = new List<Item>();
	}

	public void TryCraft() {
		Item crafted = Craft();

		if (crafted != null) {
			craftText.text = "Crafted!";
		}
		else {
			craftText.text = "Can't craft";
		}

		LeanTween.value(craftText.gameObject, craftText.alpha, 1.0f, 1.0f)
		.setOnUpdate((float a)=> {
			craftText.alpha = a;
		})
		.setOnComplete(() => {
			LeanTween.value(craftText.gameObject, craftText.alpha, 0.0f, 1.0f)
			.setOnUpdate((float a) => {
				craftText.alpha = a;
			})
			.setDelay(0.5f);
		});
	}

	public Item Craft() {
		Item crafted = null;

		if (CheckItem(ItemType.Sweater, ItemType.Paints)) { 
			crafted = CreateItem(ItemType.SatanSweater);
		}
		else if (CheckItem(ItemType.Bible, ItemType.Paints)) {
			crafted = CreateItem(ItemType.SatanBible);
		}
		else if (CheckItem(ItemType.Doll, ItemType.Paints)) {
			crafted = CreateItem(ItemType.DeamonDoll);
		}
		else if (CheckItem(ItemType.Candy, ItemType.Knife)) {
			crafted = CreateItem(ItemType.Impaler);
		}
		else if (CheckItem(ItemType.Doll, ItemType.Knife)) {
			crafted = CreateItem(ItemType.HeadlessDoll);
		}
		else if (CheckItem(ItemType.Soap, ItemType.Garland)) {
			crafted = CreateItem(ItemType.Noose);
		}

		else if (CheckItem(ItemType.Bones, ItemType.Sulfur, ItemType.Horns)) {
			crafted = CreateItem(ItemType.Scooter);
		}
		else if (CheckItem(ItemType.Pizza, ItemType.HumanHand, ItemType.PentagramPostcard)) {
			crafted = CreateItem(ItemType.Friend);
		}

		if (crafted != null) {
			foreach (var item in items) 
				Destroy(item.gameObject);
			items.Clear();

			items.Add(crafted);
		}

		return crafted;
	}

	public bool CheckItem(ItemType i1, ItemType i2) {
		if (items.Count != 2)
			return false;

		foreach (var item in items)
			if (
				item.type != i1 &&
				item.type != i2 
				)
				return false;

		return true;
	}

	public bool CheckItem(ItemType i1, ItemType i2, ItemType i3) {
		if (items.Count != 3)
			return false;

		foreach (var item in items) 
			if(
				item.type != i1 &&
				item.type != i2 &&
				item.type != i3
				)
				return false;

		return true;
	}

	public Item CreateItem(ItemType itemType) {
		GameObject itemgo = Item.CreateDragItem(items[0].transform.position);
		Item item = itemgo.GetComponent<Item>();
		Item.SetDragItem(item, items[0]);

		item.type = itemType;

		item.SetImage(images[Array.IndexOf(types, itemType)]);
		item.SetCountForce(1);
		item.ActivateRaycast();
		return item;
	}

	public void OnDrop(PointerEventData eventData) {
		Item.isDragCatch = true;
		if(!items.Contains(eventData.selectedObject.GetComponent<Item>()))
			items.Add(eventData.selectedObject.GetComponent<Item>());
	}
}
