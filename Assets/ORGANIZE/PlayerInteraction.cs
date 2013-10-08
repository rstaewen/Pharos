﻿using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
	public UIItemStorage storageScript;
	public float MaxCursorDistance = 10;
	public Transform cameraTransform;
	private Transform lookCursor;
	private LayerMask LSObjectsMask;
	private LayerMask TerrainMask;
	private LayerMask DefaultMask;
	private LayerMask WaterMask;
	private LayerMask PlayerMask;
	private LayerMask InteractibleMask;
	public Transform cursorOver;
	public EquippableItem SelectedItem;
	private Material cursorMaterial;
	private Color cursorBaseColor;
	private Color cursorSelectedColor;
	private Collider itemCollider;
	
	public Material selectedCursorMaterial;
	public Material activeCursorMaterial;
	public Material inactiveCursorMaterial;
	private MeshRenderer cursorRenderer;
	
	public Transform rightHandTransform;
	public Transform leftHandTransform;
	
	private Vector3 relativeLightPosition;
	private Transform playerLightTransform;
	private Light playerLightScript;
	private Quaternion lightRotationOffset;
	
	private ParticleSystem cursorParticles;
	
	// Use this for initialization
	void Start ()
	{
		lookCursor = transform.FindChild("Cursor");
		OnDisable();
		LSObjectsMask = 1<<9;
		TerrainMask = 1<<8;
		DefaultMask = 1;
		WaterMask = 1<<4;
		PlayerMask = 1<<11;
		InteractibleMask = 1<<12;
		enabled = false;
		cursorParticles = lookCursor.GetComponent<ParticleSystem>();
		cursorRenderer = lookCursor.GetComponent<MeshRenderer>();
	}
	public void SetCursor()
	{
		lookCursor.position = (cameraTransform.forward*MaxCursorDistance)+cameraTransform.position;
		cursorRenderer.material = inactiveCursorMaterial;
	}
	public void EquipItem(EquippableItem itemToEquip)
	{
		Debug.Log("Adding Item...");
		playerLightScript = itemToEquip.GetComponentInChildren<Light>();
		if (playerLightScript)
		{
			Debug.Log("Item was a light.");
			playerLightTransform = itemToEquip.transform;
			relativeLightPosition = playerLightTransform.localPosition;
			lightRotationOffset = playerLightTransform.localRotation;
			playerLightScript = playerLightTransform.GetComponentInChildren<Light>();
			itemCollider = itemToEquip.interactionZone;
		}
	}
	Transform getSelectedObject(Ray _ray)
	{
		RaycastHit hit = new RaycastHit();
		if (
			Physics.Raycast(_ray, out hit, MaxCursorDistance, InteractibleMask)||
			Physics.Raycast(_ray, out hit, MaxCursorDistance, DefaultMask) ||
			Physics.Raycast(_ray, out hit, MaxCursorDistance, LSObjectsMask) ||
			Physics.Raycast(_ray, out hit, MaxCursorDistance, TerrainMask) ||
			Physics.Raycast(_ray, out hit, MaxCursorDistance, WaterMask)
			)
		{
			lookCursor.position = hit.point;
			cursorRenderer.material = activeCursorMaterial;
			return hit.collider.transform;
		}
		else
		{
			cursorRenderer.material = inactiveCursorMaterial;
			lookCursor.position = _ray.direction.normalized*MaxCursorDistance+cameraTransform.position;
			return null;
		}
	}
	// Update is called once per frame
	void FixedUpdate ()
	{
		Ray _ray = new Ray(cameraTransform.position, cameraTransform.forward);
		cursorOver = getSelectedObject(_ray);
		if(cursorOver)
		{
			EquippableItem item = cursorOver.gameObject.GetComponent<EquippableItem>();
			ObjectController interactibleObject = cursorOver.gameObject.GetComponent<ObjectController>();
			if (item)
			{
				SelectedItem = item;
				cursorRenderer.material = selectedCursorMaterial;
				cursorParticles.Emit(1);
			}
			else if (interactibleObject)
			{
				cursorRenderer.material = selectedCursorMaterial;
				////// popup for action? I.e. switch, door...
			}
			else
			{
				SelectedItem = null;
			}
		}
		if (playerLightScript)
		{
			if (Input.GetButtonDown("Fire3"))
				playerLightScript.gameObject.SetActive(!playerLightScript.gameObject.activeSelf);
			else
				playerLightTransform.rotation = Quaternion.LookRotation(_ray.direction);
		}
		if (SelectedItem && Input.GetButtonDown("Fire1"))
		{
			cursorParticles.Emit(40);
			AddToInventory(SelectedItem.ItemID);
			Destroy(SelectedItem.gameObject);
		}
	}
	
	void OnEnable()
	{
		Screen.showCursor = false;
		Screen.lockCursor = true;
		if (lookCursor)
			lookCursor.gameObject.SetActive(true);
	}
	
	void OnDisable()
	{
		Screen.showCursor = true;
		Screen.lockCursor = false;
		SelectedItem = null;
		cursorOver = null;
		lookCursor.gameObject.SetActive(false);
		if (playerLightScript)
		{
			playerLightTransform.localRotation = lightRotationOffset;
		}
	}
	
	public void AddToInventory(int itemID)
	{
		InvEquipment eq = GetComponent<InvEquipment>();
		if (eq == null) eq = gameObject.AddComponent<InvEquipment>();

		int qualityLevels = (int)InvGameItem.Quality._LastDoNotUse;
		InvBaseItem item = InvDatabase.FindByID(itemID);
		
		Debug.Log("attempting to add: "+item.name);
		if (item != null)
		{
			InvGameItem gi = new InvGameItem(itemID, item);
			gi.quality = (InvGameItem.Quality)Random.Range(0, qualityLevels);
			gi.itemLevel = NGUITools.RandomRange(item.minItemLevel, item.maxItemLevel);
			InvGameItem ri = eq.Equip(gi);
			if (ri == ri)
			{
				if (storageScript.PlaceItemInNextAvailableSlot(gi))
				{
					Debug.Log(item.name + " added to inventory");
				}
				else
				{
					Debug.Log("out of room!");
				}
			}
			else
			{
				Debug.Log("equipped "+ri.name+ " maybe");
			}
		}
		else
		{
			Debug.Log("Can't resolve the item ID of " + itemID);
		}
	}
}
