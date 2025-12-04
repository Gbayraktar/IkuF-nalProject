using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    // Bu minik sýnýf, listede her bir eþyanýn nasýl görüneceðini ayarlar
    [System.Serializable]
    public class LootItem
    {
        public string itemName;       // Sadece hatýrlaman için isim (Örn: Altýn)
        public GameObject itemPrefab; // Düþecek eþyanýn prefabý
        [Range(0, 100)]
        public int dropChance;        // % Kaç þansla düþsün? (0 ile 100 arasý)
    }

    // Inspector'da göreceðin liste
    public List<LootItem> lootList = new List<LootItem>();

    // Bu fonksiyonu EnemyAI çaðýracak
    public void DropLoot()
    {
        // Listedeki her bir eþya için tek tek zar atýyoruz
        foreach (LootItem item in lootList)
        {
            // 1 ile 100 arasýnda rastgele sayý tut
            int randomNum = Random.Range(1, 101);

            // Eðer tutulan sayý, þans oranýndan küçük veya eþitse düþür
            // Örnek: Þans 30 ise, 1-30 arasý gelirse düþer.
            if (randomNum <= item.dropChance)
            {
                InstantiateLoot(item.itemPrefab);
            }
        }
    }

    void InstantiateLoot(GameObject lootPrefab)
    {
        if (lootPrefab != null)
        {
            // Düþmanla ayný yerde oluþtur (Quaternion.identity = düz dursun)
            GameObject droppedItem = Instantiate(lootPrefab, transform.position, Quaternion.identity);

            // --- GÜZELLÝK YAPALIM: FIRLATMA EFEKTÝ ---
            // Eþyalar üst üste binmesin diye hafifçe saða sola fýrlatalým
            Rigidbody2D rb = droppedItem.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Rastgele bir yön ve güç belirle
                Vector2 dropDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                rb.AddForce(dropDirection * 5f, ForceMode2D.Impulse);
            }
        }
    }
}
