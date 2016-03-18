using UnityEngine;
using System.Collections;

public class DrawPathPrediction : MonoBehaviour {

    private LineRenderer line;
    public int vertexCount = 50; 
    public float projectionDistance = 30;

    private float amplitude = 1;
    private float frequency = 1;
    private float phase = 0;
   
    public Rigidbody2D target;

    public void Start()
    {
        line = GetComponent<LineRenderer>();
        line.SetVertexCount(vertexCount);

        PlayerControl.OscillationUpdated += UpdatePrediction;
    }

    void UpdatePrediction(float amp, float freq)
    {
        amplitude = amp;
        frequency = freq;
    }

    void UpdatePhase()
    {
        var yPos = target.transform.position.y;
        var yVel = target.velocity.y;
        phase = Mathf.Asin(yPos / amplitude);
        // since asin returns a value between pi/2 and -pi/2, this angle is correct when the y value is increasing (yVel > 0)
        // but is the wrong point on the unit circle when the y value is decreasing so needs to be mirrored across the y axis in that case
        if (yVel < 0)
        {
            phase = Mathf.PI - phase;
        }
    }

	// Update is called once per frame
	public void Update () {
        UpdatePhase();
        int i = 0;
        var vertexesPerUnitX = projectionDistance/vertexCount;
        while (i < vertexCount)
        {
            var xi = target.transform.position.x + i*vertexesPerUnitX; 
            // but also
            // xi = x0 + velscity.x * dt
            // so 
            // xo + velocity.x *dt = x0 + i*vertexesPerUnitX
            // therefore
            var dt = i*vertexesPerUnitX/target.velocity.x;

            var y = amplitude * Mathf.Sin(2 * Mathf.PI * frequency * (dt) + phase);

            var pos = new Vector2(xi, y);
            line.SetPosition(i, pos);
            i++;
        }
	}
}
