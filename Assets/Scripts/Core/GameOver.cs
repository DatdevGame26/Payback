using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//  Lớp này gọi ở cảnh cuối (tức Game Over)
public class GameOver : MonoBehaviour
{
    // Tham chiếu đến Animator, dùng để chơi các hoạt ảnh Game Over
    [SerializeField] Animator animator;

    // Lưu trữ kết quả của trò chơi (thắng hay thua)
    string gameResult;

    void Start()
    {
        // Lấy kết quả của trò chơi từ PlayerPrefs. Nếu không có, mặc định là "Win"
        gameResult = PlayerPrefs.GetString("Game Result", "Win");

        // Gọi phương thức để khởi tạo màn hình Game Over
        InitGameOver();
    }


    // Kiểm tra kết quả trò chơi
    void InitGameOver()
    {
        if (gameResult == "Win")
        {
            // Nếu người chơi thắng, phát hoạt ảnh "game_over_win"
            animator.Play("game_over_win");

            // Phát âm thanh "game_over_win" (nhạc khi thắng)
            AudioManager.Instance.PlaySound("game_over_win", gameObject, false);
        }
        else
        {
            // Nếu người chơi thua, phát hoạt ảnh "game_over_lose"
            animator.Play("game_over_lose");

            // Phát âm thanh "game_over_lose" (nhạc khi thua)
            AudioManager.Instance.PlaySound("game_over_lose", gameObject, false);
        }

        // Mở khóa con trỏ chuột (để người chơi có thể sử dụng chuột trong màn hình Game Over)
        Cursor.lockState = CursorLockMode.None;
    }
}
