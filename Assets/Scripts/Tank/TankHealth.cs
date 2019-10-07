using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float startingHealth = 100f;
    public Slider slider;
    public Image fillImage;
    public Color fullHPColor = Color.green;
    public Color zeroXPColor = Color.red;
    public GameObject explosionPrefab;
    
    
    private AudioSource  explosionAudio;
    private ParticleSystem explosionParticles;
    private float currentHealth;

    private bool Dead;


    private void Awake()
    {
        explosionParticles = Instantiate(explosionPrefab).GetComponent<ParticleSystem>();

        explosionAudio = explosionParticles.GetComponent<AudioSource>();
        explosionParticles.gameObject.SetActive(false);

        //currentHealth = startingHealth;
    }


    private void OnEnable()
    {
        currentHealth = startingHealth;
        Dead = false;
        SetHealthUI();
    }
    

    public void TakeDamage(float amount)
    {
        // Adjust the tank's current health, update the UI based on the new health and check
        //whether or not the tank is dead.
        currentHealth -= amount;

        SetHealthUI();

        if(currentHealth <=0 && !Dead)
        {
            OnDeath();
        } 
    }


    private void SetHealthUI()
    {
        // Adjust the value and colour of the slider.
        slider.value = currentHealth;
        fillImage.color = Color.Lerp(zeroXPColor, fullHPColor, currentHealth / startingHealth);

    }


    private void OnDeath()
    {
        // Play the effects for the death of the tank and deactivate it.
        Dead = true;
        explosionParticles.transform.position = transform.position;
        explosionParticles.gameObject.SetActive(true);

        explosionParticles.Play();
        explosionAudio.Play();

        gameObject.SetActive(false);
    }
}