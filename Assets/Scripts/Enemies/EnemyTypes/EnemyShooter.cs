using UnityEngine;

public class EnemyShooter : EnemyBase
{
    [Header("Enemy Shooter")]
    public BulletProperties bulletProperties;
    public SoundProperties soundProperties;

    protected void Fire(Transform direction)
    {
        // Bullet Properties
        GameObject bullet = Instantiate(bulletProperties.bulletPrefab, transform);
        BulletScript script = bullet.GetComponent<BulletScript>();
        bullet.transform.position = transform.position;
        bullet.transform.parent = null;

        script.speed = bulletProperties.speed;
        script.destroyOnTouch = bulletProperties.destroyOnTouch;
        script.deleteTimer = bulletProperties.deleteTimer;
        script.parryable = bulletProperties.parryable;
        script.deleteOnTouch = bulletProperties.destroyable;

        bullet.transform.LookAt(direction);

        // pega rotação atual
        Vector3 euler = bullet.transform.eulerAngles;

        // se passar muito do valor esperado, corrige
        if (Mathf.Abs(euler.y % 90) > 1f) 
        {
            euler.y -= 90; // ou += 90, depende do seu prefab
            bullet.transform.eulerAngles = euler;
        }

                

        // Bullet Sound
        if (soundProperties.fireSound != null)
            SoundManager.instance.Play(soundProperties.fireSound, soundProperties.volume);
    }

    [System.Serializable]
    public class BulletProperties
    {
        public GameObject bulletPrefab;
        public float speed = 1;
        public bool destroyOnTouch = true;
        public float deleteTimer = 0;
        public bool parryable = false;
        public bool destroyable = false;
    }

    [System.Serializable]
    public class SoundProperties
    {
        public AudioClip fireSound;
        public float volume = 1;
    }
}
