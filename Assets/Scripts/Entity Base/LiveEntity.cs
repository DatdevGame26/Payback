using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/* Đây là lớp thực thể sống, tức đối tượng sẽ có máu và có thể bị thương và bị tiêu diệt.
 * Các đối tượng như là người chơi hay kẻ thù đều là thực thể sống và kế thừa từ lớp này.
 */

public class LiveEntity : MonoBehaviour, IDamageable
{
    // Max health của đối tượng (máu tối đa), có thể chỉnh sửa từ Inspector
    [SerializeField] protected int maxHealth = 100;

    // Biến lưu trữ máu hiện tại của đối tượng
    protected int currentHealth;

    // Biến kiểm tra xem đối tượng đã chết chưa
    protected bool isDead;

    protected virtual void Awake()
    {
        // Khởi tạo currentHealth bằng maxHealth khi đối tượng được khởi tạo
        currentHealth = maxHealth;
    }

    protected virtual void Start()
    {
    }

    // Phương thức Update gọi mỗi frame
    protected virtual void Update()
    {
    }

    // Phương thức nhận sát thương, được gọi khi đối tượng bị tấn công
    public virtual void Damage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0 && !isDead)
        {
            // Thực hiện một số hành động trước khi chết (như hoạt ảnh, âm thanh)
            doSomethingThenDie();
        }
    }

    // Phương thức thực hiện các hành động trước khi chết (như hoạt ảnh, âm thanh, hiệu ứng)
    public virtual void doSomethingThenDie()
    {
        // Đánh dấu đối tượng là đã chết
        isDead = true;

        // Ví dụ: có thể thêm các hành động như phát hiệu ứng chết, âm thanh, v.v. ở đây
    }

    // Phương thức Die sẽ phá hủy đối tượng
    public void Die()
    {
        // Phá hủy đối tượng khỏi cảnh
        Destroy(gameObject);
    }

    // Phương thức trả về max health của đối tượng
    public int getMaxHealth()
    {
        return maxHealth;
    }

    // Phương thức trả về máu hiện tại của đối tượng
    public int getCurrentHealth()
    {
        return currentHealth;
    }

    // Phương thức kiểm tra xem đối tượng đã chết chưa
    public bool IsDead()
    {
        return isDead;
    }
}