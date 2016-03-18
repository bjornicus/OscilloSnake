using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public float forwardSpeed = 5;

	const float MinimumFrequency = 0.1F;
	const float MaximumFrequency = 2.5F;
	[Range(MinimumFrequency, MaximumFrequency)]
	public float frequency = 1.0F;
	
	const float MinimumAmplitude = 2;
	const float MaximumAmplitude = 10;
	[Range(MinimumAmplitude, MaximumAmplitude)]
	public float amplitude = 1.0F;

    public float MaximumAF = 5;

	private float springConstant = 0;

	Rigidbody2D physicsBody;

    public delegate void OscillationUpdateDelegate(float amplitude, float frequency);
    public static event OscillationUpdateDelegate OscillationUpdated;

    void OnOscilationUpdate()
    {
        if (OscillationUpdated != null)
        {
            OscillationUpdated(amplitude, frequency);
        }
    }

	// Use this for initialization
	void Start () 
	{
		physicsBody = GetComponent<Rigidbody2D> ();
        physicsBody.velocity = new Vector2(forwardSpeed,0);
		AdjustSpringConstant ();
        AdjustAmplitude();
        OnOscilationUpdate();
    }
	
    void Update()
    {    
        ReactToInput ();
    }

	void FixedUpdate () 
	{
		UpdatePosition ();
	}

    void OnCollisionExit2D(Collision2D coll) {
        AdjustAmplitude();
        OnOscilationUpdate();
    }

	void AdjustSpringConstant ()
	{
		springConstant = Mathf.Pow (2 * Mathf.PI * frequency, 2) * physicsBody.mass;
	}

    void AdjustAmplitude()
    {
        var mass = physicsBody.mass;
        var y = physicsBody.transform.position.y;
        var ySquared = Mathf.Pow(y, 2);
        var ampSquared = Mathf.Pow(amplitude, 2);
        var vSquared = (springConstant / mass) * (ampSquared - ySquared);
        vSquared = Mathf.Max(vSquared, 0);
        var yDirection = 1;
        if (physicsBody.velocity.y < 0)
        {
            yDirection = -1;
        }

        physicsBody.velocity = new Vector2(forwardSpeed , yDirection * Mathf.Sqrt(vSquared));
    }

	void UpdatePosition ()
	{
		Vector3 toCenter = new Vector2(0, -1 * gameObject.transform.position.y);
		physicsBody.AddForce (toCenter * springConstant);

        if (physicsBody.transform.position.y > amplitude)
        {
            physicsBody.velocity = new Vector2(forwardSpeed,0);
            var targetPosition = new Vector2(physicsBody.transform.position.x, 0);
            physicsBody.transform.position = Vector3.Lerp(physicsBody.transform.position, targetPosition, MaximumAF * Time.deltaTime);
        } 
	}

	void ReactToInput ()
	{
        if (Input.GetKey("escape"))
            Application.Quit();

        var dFreq = GetHorizontalAxis();
		if (dFreq != 0)
		{
			ChangeFrequency (dFreq/10);
		}
	
		var dAmp = GetVerticalAxis();
		if (dAmp != 0) 
		{
			ChangeAmplitude (dAmp/2);
		}

        if (dAmp != 0 || dFreq != 0)
        {
            OnOscilationUpdate();
        }
	}

    static float GetHorizontalAxis()
    {
        var dx = Input.GetAxis("Horizontal");
        dx += Input.GetAxis("Mouse X");
        return dx;
    }

    static float GetVerticalAxis()
    {
        var dy = Input.GetAxis("Vertical");
        dy += Input.GetAxis("Mouse Y");
        return dy;
    }

	void ChangeFrequency (float dFreq)
	{
		var oldFrequency = frequency;
		frequency += dFreq;

		frequency = Mathf.Min (frequency, MaximumFrequency);
		frequency = Mathf.Max (frequency, MinimumFrequency);

		if (oldFrequency != frequency)
		{
			AdjustSpringConstant();
            var maxAmplitudeAtThisFrequency = MaximumAF / frequency;
            amplitude = Mathf.Min(amplitude, maxAmplitudeAtThisFrequency);
            AdjustAmplitude();
		}
	}

	void ChangeAmplitude (float dAmp)
	{
        var oldAmplitude = amplitude;
		amplitude += dAmp;
        
        amplitude = Mathf.Min(amplitude, MaximumAmplitude);
        amplitude = Mathf.Max(amplitude, MinimumAmplitude);

        if (oldAmplitude != amplitude)
        {
            AdjustAmplitude();
            var maxFrequencyAtThisAmplitude = MaximumAF / amplitude;
            frequency = Mathf.Min(frequency, maxFrequencyAtThisAmplitude);
            AdjustSpringConstant();
        }
	}
}
