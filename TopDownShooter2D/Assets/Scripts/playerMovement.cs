using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    //���ߵ�UI
    [SerializeField] private UI_Inventory uiInventory;
    //�ƶ�����
    private float walkingSpeed = 5, runningSpeed = 60, dodgingSpeed = 10, dodgeFrameCounter = 0;
    //��ǰHp,����ֵ
    private float hp, stamina;
    //���hp,����ֵ
    private static float maxStamina = 100000, maxHp = 100;
    private Animator playerAnim;
    //��ǰ�Ƿ����ڶ���
    public static bool isDodging = false;

    public Rigidbody2D rb;
    private float horiMove, vertiMove;
    private Inventory inventory;
    //����ģʽ��ʵ��
    public static playerMovement instance;


    void Awake()
    {
        //����ģʽ��һ��������ͬʱֻ����һ��playerObject�Ĵ��ڣ����Ҵ�ʼ���ն���ͬһ��ʵ��
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        //?
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        stamina = maxStamina;
        hp = maxHp;

        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
        
    }
    private void manageMovement()
    {
        //�����̽��յ��ƶ�����
        horiMove = Input.GetAxis("Horizontal");
        vertiMove = Input.GetAxis("Vertical");
        //��ǰ�ٶ�
        float curSpeed = walkingSpeed;
        //�������ڱ��ܣ�����٣���������
        if (Input.GetButton("Run") && stamina > 1)
        {
            curSpeed = runningSpeed;
            stamina -= Time.deltaTime;
        }
        else
        {
            if (stamina <= maxStamina - Time.deltaTime) stamina += Time.deltaTime;
        }
        isDodging = PlayerBehaviour.getIsDodging();
        if (isDodging)
        {
            dodgeFrameCounter += Time.deltaTime;
            curSpeed = dodgingSpeed;
            if (dodgeFrameCounter > 0.3)
            {
                isDodging = false;
                dodgeFrameCounter = 0;
            }
        }
        rb.velocity = new Vector2(curSpeed * horiMove, curSpeed * vertiMove);
    }
    // Update is called once per frame
    void Update()
    {
        manageMovement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
    }

}
