using UnityEngine;
using System.Collections;

public class DrawTail : MonoBehaviour {

	public LineRenderer lineRenderer;
	
	private float tailLength = 20;
	public float TailLength {
		get {
			return tailLength;
		}
		set {
			if (tailLength != value)
			{
				tailLength = value;
				RecomputeVertexCount();
			}
		}
	}

	private float smoothFactor = 1.0F;
	public float SmoothFactor {
		get {
			return smoothFactor;
		}
		set {
			if (smoothFactor != value)
			{
				smoothFactor = value;
				RecomputeVertexCount();
			}
		}
	}

    public Vector3 offset = new Vector3(-0.5f, 0, 0);

 	// line segments per unit x 
	private int vertexCount;
	private Vector3[] positionHistory;

	void Start() {
		if (lineRenderer == null) 
		{
			CreateLineRenderer ();
		}
        RecomputeVertexCount();
	}

	void CreateLineRenderer ()
	{
        print("creating line renderer since none existed");
		lineRenderer = gameObject.AddComponent<LineRenderer> ();
		lineRenderer.material = new Material (Shader.Find ("Particles/Additive"));
		lineRenderer.SetWidth (1.0F, 0.1F);
	}

	void RecomputeVertexCount ()
	{
		vertexCount = (int)(tailLength * smoothFactor);
		lineRenderer.SetVertexCount (vertexCount);
		positionHistory = new Vector3[vertexCount];
        positionHistory [0] = offset;
	}

	void UpdateHistory()
	{
		var dt = Time.deltaTime;
		int pointIndex = vertexCount - 1 ;
		var dx = - 5 * dt / SmoothFactor;
		while (pointIndex > 0) {
			var nextPoint = positionHistory[pointIndex -1];
			nextPoint.x = nextPoint.x + dx;
			positionHistory[pointIndex] = nextPoint;
			--pointIndex;
		}
		positionHistory [0] = gameObject.transform.position + offset;
	}

	void UpdateTail ()
	{
		int pointIndex = vertexCount - 1;
		while (pointIndex >= 0) {
			lineRenderer.SetPosition (pointIndex, positionHistory[pointIndex]);
			--pointIndex;
		}
	}

	void FixedUpdate() 
	{
		UpdateHistory ();
		UpdateTail ();
	}
}
