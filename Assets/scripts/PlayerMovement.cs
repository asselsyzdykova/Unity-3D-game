using UnityEngine;
using System.IO;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("Jump Settings")]
    public float jumpForce = 5f;
    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;

    [Header("Camera")]
    public Camera playerCamera;

    private Rigidbody rb;
    private Animator anim;
    private bool isGrounded;

    [Header("Logging Settings")]
    private string logPath;
    private float logTimer = 0f;
    private int score = 0;
    private int zombieKills = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        logPath = Path.Combine(Application.dataPath + "/../", "Game_History_Log.txt");

        Debug.Log("Log file: " + logPath);
        LogEvent("--- NEW GAME SESSION STARTED ---");
        LogEvent("Player's record at start: " + PlayerPrefs.GetInt("HighScore", 0));
    }

    public void RegisterZombieKill()
    {
        zombieKills++;
        LogEvent($"Zombie killed! Total for the session: {zombieKills}");

        int savedRecord = PlayerPrefs.GetInt("ZombieRecord", 0);
        if (zombieKills > savedRecord)
        {
            PlayerPrefs.SetInt("ZombieRecord", zombieKills);
            PlayerPrefs.Save();
            LogEvent($"!!! NEW KILL RECORD: {zombieKills} !!!");
        }
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            anim.SetTrigger("Jump");
            LogEvent("Player jumped");
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = (forward * v + right * h).normalized;

        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.deltaTime);

        anim.SetFloat("Speed", moveDir.magnitude);
        logTimer += Time.deltaTime;
        if (logTimer >= 5f)
        {
            LogEvent($"Position: {transform.position} | Turn: {transform.eulerAngles.y}");
            logTimer = 0;
        }
    }

    public void LogEvent(string message)
    {
        string timestamp = System.DateTime.Now.ToString("HH:mm:ss");
        string entry = $"[{timestamp}] {message}\r\n";
        File.AppendAllText(logPath, entry);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
}
