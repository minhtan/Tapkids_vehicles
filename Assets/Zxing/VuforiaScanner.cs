//
// based upon the following solution, modified to work with ZXing.Net
// http://whydoidoit.com/2012/07/18/unity-vuforia-zxing-barcode-scanning-in-ar-games/
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

using ZXing;
using ZXing.Client.Result;
using Vuforia;

[AddComponentMenu("System/VuforiaScanner")]
public class VuforiaScanner : MonoBehaviour
{
	bool isFrameFormatSet;
	string tempText;
	private Image image;

	IBarcodeReader barcodeReader;

	public void OnInitialized ()
	{
		Debug.Log ("Init-ed");
	}

	void OnEnable(){
		barcodeReader = new BarcodeReader();
	}

	void Update(){
		try
		{
			if(!isFrameFormatSet) {
				isFrameFormatSet = CameraDevice.Instance.SetFrameFormat(Image.PIXEL_FORMAT.GRAYSCALE, true);
			}

			image = CameraDevice.Instance.GetCameraImage(Image.PIXEL_FORMAT.GRAYSCALE);
			var data = barcodeReader.Decode(image.Pixels, image.BufferWidth, image.BufferHeight, ZXing.RGBLuminanceSource.BitmapFormat.Gray8);
			if (data != null)
			{
				tempText = data.Text;
				Debug.Log(data);
			}
		}
		catch {
			// Fail detecting QR Code!
			Debug.Log("decode expection");
		}
		finally
		{
			if(!string.IsNullOrEmpty(tempText)) {
				Messenger.Broadcast<string> (EventManager.AR.QR_TRACKING.ToString(), tempText);
				tempText = null;
			}
		}
	}
		
}