using UnityEngine;

public class ParticleRandomColor : MonoBehaviour
{
    ParticleSystem myParticleSystem;
    ParticleSystem.ColorOverLifetimeModule colorModule;
    Gradient ourGradient;
    Color particleColor, particleColor1, particleColor2, particleColor3, particleColor4, particleColor5, particleColor6, particleColor7, particleColor8, particleColor9;

    void Start()
    {
        particleColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        particleColor1 = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        particleColor2 = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        particleColor3 = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        particleColor4 = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        particleColor5 = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        particleColor6 = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        particleColor7 = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        particleColor8 = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        particleColor9 = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        // Get the system and the color module.
        myParticleSystem = gameObject.GetComponent<ParticleSystem>();
        colorModule = myParticleSystem.colorOverLifetime;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        ourGradient = new Gradient();
        ourGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(particleColor, 0.0f), new GradientColorKey(particleColor1, 0.143f), new GradientColorKey(particleColor2, 0.286f), new GradientColorKey(particleColor3, 0.429f), new GradientColorKey(particleColor4, 0.572f), new GradientColorKey(particleColor5, 0.715f), new GradientColorKey(particleColor6, 0.858f), new GradientColorKey(particleColor7, 1f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );

        // Apply the gradient.
        colorModule.color = ourGradient;

        // In 5 seconds we will modify the gradient.
        Invoke("ModifyGradient", 5.0f);
    }

    void ModifyGradient()
    {
        particleColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        particleColor1 = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        particleColor2 = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        particleColor3 = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        particleColor4 = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        particleColor5 = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        particleColor6 = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        particleColor7 = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);

        // Reduce the alpha
        float alpha = 1f;
        ourGradient.SetKeys(
             new GradientColorKey[] { new GradientColorKey(particleColor, 0.0f), new GradientColorKey(particleColor1, 0.143f), new GradientColorKey(particleColor2, 0.286f), new GradientColorKey(particleColor3, 0.429f), new GradientColorKey(particleColor4, 0.572f), new GradientColorKey(particleColor5, 0.715f), new GradientColorKey(particleColor6, 0.858f), new GradientColorKey(particleColor7, 1f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );

        // Apply the changed gradient.
        colorModule.color = ourGradient;
    }

    private void Update()
    {
        
        if (Input.GetKey(KeyCode.Space))
        {
            ModifyGradient();
        }
    }
}
