using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
	public float MaxCursorDistance = 50;
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
		cursorMaterial = lookCursor.GetComponent<MeshRenderer>().materials[0];
		cursorBaseColor = cursorMaterial.color;
		cursorSelectedColor = cursorBaseColor;
		cursorSelectedColor.g = 1;
		cursorSelectedColor.b*=0.5f;
		cursorSelectedColor.r*=0.5f;
		cursorParticles = lookCursor.GetComponent<ParticleSystem>();
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
			return hit.collider.transform;
		}
		else
		{
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
				cursorMaterial.color = cursorSelectedColor;
				cursorMaterial.SetColor("_Emission", cursorSelectedColor);
				cursorParticles.Emit(1);
			}
			else if (interactibleObject)
			{
				////// popup for action? I.e. switch, door...
			}
			else
			{
				SelectedItem = null;
				cursorMaterial.color = cursorBaseColor;
				cursorMaterial.SetColor("_Emission", cursorBaseColor);
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
		if (lookCursor)
			lookCursor.gameObject.SetActive(true);
	}
	
	void OnDisable()
	{
		Screen.showCursor = true;
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

		if (item != null)
		{
			InvGameItem gi = new InvGameItem(itemID, item);
			gi.quality = (InvGameItem.Quality)Random.Range(0, qualityLevels);
			gi.itemLevel = NGUITools.RandomRange(item.minItemLevel, item.maxItemLevel);
			eq.Equip(gi);
		}
		else
		{
			Debug.LogWarning("Can't resolve the item ID of " + itemID);
		}
	}
}
