using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
	public UIItemStorage storageScript;
	public float MaxCursorDistance = 10;
	public Transform cameraTransform;
	public Transform lookCursor;
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
	public Animator playerAnimator;
	public DetachableObject detachableInteractionXforms;
	public CharacterController charController;
	
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
	
	private enum HandEquipStates {none, left, right, both}
	private HandEquipStates handEquip;
	
	// Use this for initialization
	void Start ()
	{
		handEquip = HandEquipStates.none;
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
		SetArmLayerWeight(0f, 0f);
		charController = GetComponent<CharacterController>();
	}
	public void SetCursor()
	{
		lookCursor.position = (cameraTransform.forward*MaxCursorDistance)+cameraTransform.position;
		cursorRenderer.material = inactiveCursorMaterial;
	}
	public void EquipItem(EquippableItem itemToEquip, InvBaseItem.Slot slotPlaced)
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
		switch(handEquip)
		{
		case HandEquipStates.left:
			if(slotPlaced == InvBaseItem.Slot.RightHand)
				handEquip = HandEquipStates.both;
			break;
		case HandEquipStates.right:
			if(slotPlaced == InvBaseItem.Slot.LeftHand)
				handEquip = HandEquipStates.both;
			break;
		case HandEquipStates.none:
			if(slotPlaced == InvBaseItem.Slot.LeftHand)
				handEquip = HandEquipStates.left;
			if(slotPlaced == InvBaseItem.Slot.RightHand)
				handEquip = HandEquipStates.right;
			break;
		}
		SetArmLayerWeight(0f, 0f);
	}

	public void UnEquipItem (InvBaseItem.Slot slotRemoved)
	{
		switch(slotRemoved)
		{
		case InvBaseItem.Slot.LeftHand:
			if(handEquip == HandEquipStates.both)
				handEquip = HandEquipStates.right;
			if(handEquip == HandEquipStates.left)
				handEquip = HandEquipStates.none;
			break;
		case InvBaseItem.Slot.RightHand:
			if(handEquip == HandEquipStates.both)
				handEquip = HandEquipStates.left;
			if(handEquip == HandEquipStates.right)
				handEquip = HandEquipStates.none;
			break;
		}
		SetArmLayerWeight(0f, 0f);
	}
	
	public void SetArmLayerWeight (float speed, float itemWeight)
	{
		switch(handEquip)
		{
		case HandEquipStates.none:
			playerAnimator.SetLayerWeight(1, 0f);
			playerAnimator.SetLayerWeight(2, 0f);
			break;
		case HandEquipStates.left:
			playerAnimator.SetLayerWeight(1, 1f);
			playerAnimator.SetLayerWeight(2, 0f);
			break;
		case HandEquipStates.right:
			playerAnimator.SetLayerWeight(1, 0f);
			playerAnimator.SetLayerWeight(2, 1f);
			break;
		case HandEquipStates.both:
			playerAnimator.SetLayerWeight(1, 1f);
			playerAnimator.SetLayerWeight(2, 1f);
			break;
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
	private ObjectController interactibleObject;
	void FixedUpdate ()
	{
		if(charController.velocity.magnitude < 0.1f)
			detachableInteractionXforms.enabled = true;
		Ray _ray = new Ray(cameraTransform.position, cameraTransform.forward);
		cursorOver = getSelectedObject(_ray);
		interactibleObject = null;
		SelectedItem = null;
		if(cursorOver)
		{
			EquippableItem item = cursorOver.gameObject.GetComponent<EquippableItem>();
			interactibleObject = cursorOver.gameObject.GetComponent<ObjectController>();
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
			playerLightTransform.rotation = Quaternion.LookRotation(_ray.direction);
	}
	
	void Update()
	{
		if (playerLightScript && Input.GetButtonDown("Fire3"))
				playerLightScript.gameObject.SetActive(!playerLightScript.gameObject.activeSelf);
		if (SelectedItem && Input.GetButtonDown("Fire1"))
		{
			cursorParticles.Emit(40);
			AddToInventory(SelectedItem.ItemID);
			if(SelectedItem.GetComponent<FireEventOnPickup>())
				SelectedItem.GetComponent<FireEventOnPickup>().OnPickup();
			Destroy(SelectedItem.gameObject);
		}
		else if(interactibleObject && Input.GetButtonDown("Fire1"))
		{
			interactibleObject.OnClickAction1();
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
		detachableInteractionXforms.enabled = false;
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
			if (storageScript.PlaceItemInNextAvailableSlot(gi))
				Debug.Log(item.name + " added to inventory");
			else
				Debug.Log("out of room!");
		}
		else
		{
			Debug.Log("Can't resolve the item ID of " + itemID);
		}
	}
}
