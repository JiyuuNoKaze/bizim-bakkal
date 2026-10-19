"Bizim Bakkal" oyunu için bir müşterinin bakkala girmesi, rafları gezmesi, kasaya gitmesi ve çıkmasını sağlayan temel bir **NavMesh** tabanlı hareket sistemi hazırladım.

Bu kodu kullanmadan önce sahnenizde **NavMesh** (Navigation) sisteminin kurulu olduğundan ve müşteride bir **NavMesh Agent** bileşeni olduğundan emin olun.

### BakkalMusteriHareket.cs

```csharp
using UnityEngine;
using UnityEngine.AI; // NavMesh için gerekli
using System.Collections;
using System.Collections.Generic;

public class BakkalMusteriHareket : MonoBehaviour
{
    public enum MusteriDurumu { RafaGidiyor, AlisverisYapiyor, KasayaGidiyor, KasadaBekliyor, DisariCikiyor }
    
    [Header("Ayarlar")]
    public float alisverisSuresi = 3f; // Bir rafta ne kadar bekleyecek?
    public float kasaSuresi = 5f;      // Kasada ne kadar bekleyecek?
    
    [Header("Hedef Noktalar")]
    public List<Transform> raflar;     // Bakkaldaki rafların konumları
    public Transform kasaNoktasi;      // Kasa sırasına gireceği yer
    public Transform cikisNoktasi;     // Bakkal kapısı/dışarısı

    private NavMeshAgent agent;
    private MusteriDurumu mevcutDurum;
    private int gezilenRafSayisi = 0;
    private int maksimumRafSayisi;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        maksimumRafSayisi = Random.Range(1, raflar.Count + 1); // Rastgele kaç raf gezeceğini seç
        
        // İlk hedefe başla
        SonrakiRafaGit();
    }

    void Update()
    {
        // Hedefe ulaşıp ulaşmadığını kontrol et
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            DurumuYonet();
        }
    }

    void DurumuYonet()
    {
        switch (mevcutDurum)
        {
            case MusteriDurumu.RafaGidiyor:
                StartCoroutine(RafBeklemeSuresi());
                break;
                
            case MusteriDurumu.KasayaGidiyor:
                StartCoroutine(KasaBeklemeSuresi());
                break;

            case MusteriDurumu.DisariCikiyor:
                Destroy(gameObject); // Bakkaldan çıktı, objeyi yok et
                break;
        }
    }

    void SonrakiRafaGit()
    {
        if (gezilenRafSayisi < maksimumRafSayisi)
        {
            mevcutDurum = MusteriDurumu.RafaGidiyor;
            Transform hedefRaf = raflar[Random.Range(0, raflar.Count)];
            agent.SetDestination(hedefRaf.position);
            gezilenRafSayisi++;
        }
        else
        {
            KasayaGit();
        }
    }

    IEnumerator RafBeklemeSuresi()
    {
        mevcutDurum = MusteriDurumu.AlisverisYapiyor;
        yield return new WaitForSeconds(alisverisSuresi);
        SonrakiRafaGit();
    }

    void KasayaGit()
    {
        mevcutDurum = MusteriDurumu.KasayaGidiyor;
        agent.SetDestination(kasaNoktasi.position);
    }

    IEnumerator KasaBeklemeSuresi()
    {
        mevcutDurum = MusteriDurumu.KasadaBekliyor;
        // Burada bakkalın parayı alması gibi bir event tetiklenebilir
        yield return new WaitForSeconds(kasaSuresi);
        CikisaGit();
    }

    void CikisaGit()
    {
        mevcutDurum = MusteriDurumu.DisariCikiyor;
        agent.SetDestination(cikisNoktasi.position);
    }
}
```

### Nasıl Kullanılır?

1.  **NavMesh Oluşturun:** Unity'de `Window > AI > Navigation` kısmından bakkalın zeminini "Static" yapıp "Bake" edin.
2.  **Müşteri Objesi:** Müşteri karakterinize (bir Capsule veya Model) `NavMesh Agent` bileşeni ekleyin.
3.  **Scripti Ekleyin:** Hazırladığım bu scripti müşteri objesine sürükleyin.
4.  **Referansları Tanımlayın:**
    *   **Raflar:** Sahnedeki rafların önüne boş objeler (Empty Object) koyun ve bunları scriptteki "Raflar" listesine sürükleyin.
    *   **Kasa Noktası:** Kasanın önündeki boş bir objeyi buraya sürükleyin.
    *   **Çıkış Noktası:** Kapının dışındaki boş bir objeyi buraya sürükleyin.

### Geliştirme Önerileri:
*   **Animasyon:** `agent.velocity.magnitude` değerini kullanarak Animator parametrelerini (Yürüme/Bekleme) tetikleyebilirsiniz.
*   **Sıra Sistemi:** Eğer bakkalda çok müşteri varsa, `kasaNoktasi` yerine bir `Queue` (Kuyruk) sistemi yapabilirsiniz.
*   **Ürün Alma:** Müşteri rafa ulaştığında eline rastgele bir paket görseli eklettirebilirsiniz.