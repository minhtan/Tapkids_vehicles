using UnityEngine;
using System.Collections.Generic;

public class GestureParticleDrawing : GestureDrawing
{
    public GameObject _particleEffect;

    private Camera mainCam;
    private List<GameObject> effectList;
    private Transform trans;
    private Vector2 previousPosition;
    private int sqrMinPixelMove;
    private bool canDraw = false;
    private int pointCount = 0;

    void Start()
    {
        trans = transform;
        mainCam = Camera.main;
        effectList = new List<GameObject>();

        // Used for .sqrMagnitude, which is faster than .magnitude
        sqrMinPixelMove = minPixelMove * minPixelMove;
    }

    protected override void StrokeStart(Lean.LeanFinger finger)
    {
        previousPosition = finger.ScreenPosition;
        canDraw = true;
        pointCount++;
    }

    protected override void StrokeDrag(Lean.LeanFinger finger)
    {
        if ((finger.ScreenPosition - previousPosition).sqrMagnitude > sqrMinPixelMove && canDraw)
        {
            trans.position = mainCam.ScreenPointToRay(finger.ScreenPosition).GetPoint(10);
            pointCount++;
            previousPosition = finger.ScreenPosition;

            if (pointCount >= maxPoints)
                canDraw = false;

            effectList.Add(Instantiate(_particleEffect, trans.position, Quaternion.identity) as GameObject);
        }
    }

	protected override void StrokeEnd (Lean.LeanFinger finger)
	{

	}

	public void ResetStroke()
    {
        for (int i = 0; i < effectList.Count; i++)
        {
            Destroy(effectList[i], 2f);
        }

        effectList.Clear();
        pointCount = 0;
        canDraw = false;
    }
}
