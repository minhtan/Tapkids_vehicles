using UnityEngine;
using System.Collections;
using UnityEngine.Purchasing;
using System;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class MyIAPManager : MonoBehaviour, IStoreListener{

//	[DllImport ("__Internal")]
//	private static extern void _CallIAP ();

	private IStoreController controller;
	private IExtensionProvider extensions;
	private string productName = "online_code";
	private string googleProductID = "com.Taplife.TapkidsVehicle.online_code_google";
	private string appleProductID = "com.Taplife.TapkidsVehicle.online_code_apple";
	public string googlePublicKey;

	void Start(){
		if (controller == null)
		{
			// Begin to configure our connection to Purchasing
			InitializePurchasing();
		}
	}

	public void InitializePurchasing() 
	{
		// If we have already connected to Purchasing ...
		if (IsInitialized())
		{
			// ... we are done here.
			return;
		}

		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
		IDs productIDs = new IDs () {
			{googleProductID, GooglePlay.Name},
			{appleProductID, AppleAppStore.Name}
		};
		builder.AddProduct(productName, ProductType.Consumable, productIDs);
		builder.Configure<IGooglePlayConfiguration>().SetPublicKey(googlePublicKey);
		UnityPurchasing.Initialize (this, builder);
	}

	public void OnInitialized (IStoreController controller, IExtensionProvider extensions)
	{
		this.controller = controller;
		this.extensions = extensions;
		Product pr = controller.products.WithStoreSpecificID (productName);
		string price = pr.metadata.localizedPrice + " " + pr.metadata.isoCurrencyCode;
	}

	public void OnInitializeFailed (InitializationFailureReason error)
	{
		Debug.Log ("Init IAP failed");
	}

	public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs args)
	{
		if (String.Equals(args.purchasedProduct.definition.id, productName, StringComparison.Ordinal))
		{
			Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.receipt));
			FindObjectOfType<Checkcode> ().OnCodeValid ();
			StartCoroutine(WebServiceUltility.OrderKey(args.purchasedProduct.transactionID, (isOwned) => {}));
		}
		return PurchaseProcessingResult.Complete;
	}

	public void OnPurchaseFailed (Product i, PurchaseFailureReason p)
	{
		Debug.Log ("Purchase failed");
		GUIController.Instance.OpenDialog ("Purchase failed").AddButton ("Ok", UIDialogButton.Anchor.CENTER, 0, -60);
	}

	private bool IsInitialized()
	{
		// Only say we are initialized if both the Purchasing references are set.
		return controller != null && extensions != null;
	}

	void BuyProductID(string productId)
	{
		// If the stores throw an unexpected exception, use try..catch to protect my logic here.
		try
		{
			// If Purchasing has been initialized ...
			if (IsInitialized())
			{
				// ... look up the Product reference with the general product identifier and the Purchasing system's products collection.
				Product product = controller.products.WithID(productId);

				// If the look up found a product for this device's store and that product is ready to be sold ... 
				if (product != null && product.availableToPurchase)
				{
					Debug.Log (string.Format("Purchasing product asychronously: '{0}'", product.definition.id));// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
					controller.InitiatePurchase(product);
				}
				// Otherwise ...
				else
				{
					// ... report the product look-up failure situation  
					Debug.Log ("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
//					CanvasError.instance.ShowError("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
				}
			}
			// Otherwise ...
			else
			{
				// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or retrying initiailization.
				Debug.Log("BuyProductID FAIL. Not initialized.");
//				CanvasError.instance.ShowError("BuyProductID FAIL. Not initialized.");
			}
		}
		// Complete the unexpected exception handling ...
		catch (Exception e)
		{
			// ... by reporting any unexpected exception for later diagnosis.
			Debug.Log ("BuyProductID: FAIL. Exception during purchase. " + e);
//			CanvasError.instance.ShowError ("BuyProductID: FAIL. Exception during purchase. " + e);
		}
	}

	//public method
	public void _BuyCodeOnline(){
		#if UNITY_ANDROID
			BuyProductID (productName);
		#endif
		#if UNITY_IOS
//		if (Application.platform != RuntimePlatform.OSXEditor)
//			_CallIAP();
			BuyProductID (productID);
		#endif
	}
}