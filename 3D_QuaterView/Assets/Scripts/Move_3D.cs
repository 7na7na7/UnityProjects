using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_3D : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] grenades;
    private Camera followCamera;
    public int ammo;
    public int coin;
    public int health;
    public int hasGrenades;
    
    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;
    public int maxHasGrenades;
    
    private Animator anim;
    private Rigidbody rigid;
    private Vector2 axis;
    private Vector3 moveVec;
    private Vector3 dodgeVec;
    private bool isFireReady = true;
    private bool attackDown;
    private bool walkDown;
    private bool jumpDown;
    private bool interactionDown;
    private bool swap1Down;
    private bool swap2Down;
    private bool swap3Down;
    private bool reloadDown;
    private bool isJump;
    private bool isDodge;
    private bool isSwap=false;
    private bool isReload = false;
    private bool isBorder = false;
    private float attackDelay;
    private GameObject nearObject;
    private Weapon equipWeapon;
    private int equipWeaponIndex=-1;
    void Start()
    {
        followCamera=Camera.main;
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }
    
    void Update()
    { 
        GetInput(); //키입력
        Move();  //이동
        SetAnim(); //애니메이션 설정
        Turn(); //이동방향으로 바라보기
        Jump(); //점프
        Dodge(); //구르기
        Interaction(); //템줍기
        Swap(); //무기 교체
        Attack(); //공격
        Reload(); //재쟝전
    }
    
    private void FixedUpdate()
    {
       FreezeRotation();
       StopToWall();
    }
    
    void GetInput()
    {
        axis.x=Input.GetAxisRaw("Horizontal");
        axis.y = Input.GetAxisRaw("Vertical");
        walkDown = Input.GetButton("Walk");
        jumpDown = Input.GetButtonDown("Jump");
        interactionDown = Input.GetButtonDown("Interaction");
        swap1Down=Input.GetButtonDown("Swap1");
        swap2Down=Input.GetButtonDown("Swap2");
        swap3Down=Input.GetButtonDown("Swap3");
        attackDown = Input.GetButton("Fire1");
        reloadDown = Input.GetButtonDown("Reload");
    }

    void Move()
    {
        if (isDodge)
            moveVec = dodgeVec;
        else if(isSwap||!isFireReady||isReload) 
            moveVec=Vector3.zero;
        else
            moveVec = new Vector3(axis.x, 0, axis.y).normalized; //정규화해서 대각선이동 속도일정
        
        if(!isBorder) 
            rigid.MovePosition(transform.position+moveVec*speed*Time.deltaTime*(walkDown ? 0.3f:1f)); //MovePosition이동
    }

    void SetAnim()
    {
        anim.SetBool("isRun",moveVec!=Vector3.zero); //이동중이면 isRun 참으로 전달
        anim.SetBool("isWalk",walkDown); //걷는지 안걷는지 전달
    }

    void Turn()
    {
        //키보드회전
        if (moveVec != Vector3.zero)
        {
            Vector3 relativePos = (transform.position + moveVec) - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime*10);   
        } 
        //마우스회전
        if (attackDown)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point-transform.position; //레이가 닿은 지점 - 플레이어위치
                nextVec.y = 0;
                transform.LookAt(transform.position+nextVec);
            }   
        }
    }

    void Reload()
    {
        if (equipWeapon == null)
            return;
        if(equipWeapon.type==Weapon.Type.Melee)
            return;
        if(ammo==0)
            return;
        if (reloadDown && !isJump && !isDodge && !isSwap && isFireReady)
        {
            anim.SetTrigger("doReload");
            isReload = true;
            Invoke("ReloadOut",2.5f);
        }
            
    }

    void ReloadOut()
    {
        int reAmmo = ammo < equipWeapon.maxAmmo ? ammo : equipWeapon.maxAmmo;
        equipWeapon.curAmmo = reAmmo;
        ammo -= reAmmo;
        equipWeapon.curAmmo = reAmmo;
        isReload = false;
    }
    private void Interaction()
    {
        if (interactionDown&&nearObject!=null&&!isJump&&!isDodge&&!isSwap)
        {
            if (nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;
                Destroy(nearObject);
            }
        }
    }

    void FreezeRotation()
    {
        rigid.angularVelocity=Vector3.zero; //회전속도 계속 0으로 만들어주기
    }

    void StopToWall()
    {
        Debug.DrawRay(transform.position,transform.forward*3,Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 3, LayerMask.GetMask("Wall"));
    }
    void Jump()
    {
        if (jumpDown&&!isJump&&moveVec==Vector3.zero&&!isDodge&&!isSwap)
        {
            isJump = true;
            rigid.AddForce(Vector3.up*jumpForce,ForceMode.Impulse);
            anim.SetBool("isJump",true);
            anim.SetTrigger("doJump"); 
        }
    }

    void Dodge()
    {
        if (jumpDown && !isJump&&moveVec!=Vector3.zero&&!isDodge&&!isSwap)
        {
            speed *= 2;
            anim.SetTrigger("doDodge");
            dodgeVec = moveVec;
            isDodge = true;
            Invoke("DodgeOut",0.5f);
        }
    }

    void Swap()
    {
        if ((swap1Down && (!hasWeapons[0] || equipWeaponIndex == 0)) ||
            (swap2Down && (!hasWeapons[1] || equipWeaponIndex == 1)) ||
            (swap3Down && (!hasWeapons[2] || equipWeaponIndex == 2)))
        {
            return;
        }

        if ((swap1Down || swap2Down || swap3Down)&&!isJump&&!isDodge&&!isSwap)
        {
            int weaponIndex = -1;
            if (swap1Down) weaponIndex = 0;
            if (swap2Down) weaponIndex = 1;
            if (swap3Down) weaponIndex = 2;
            if (equipWeapon != null)
                equipWeapon.gameObject.SetActive(false);
            anim.SetTrigger("doSwap");
            isSwap = true;
            equipWeaponIndex = weaponIndex;
            Invoke("SwapOut",0.5f);
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);   
        }
    }

    void SwapOut()
    {
        isSwap = false;
    }
    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    void Attack()
    {
        if (equipWeapon == null)
            return;
        attackDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < attackDelay;

        if (attackDown && isFireReady && !isDodge && !isSwap&&!isReload)
        {
            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type==Weapon.Type.Melee ? "doSwing":"doShot");
            attackDelay = 0;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Floor"))
        {
            isJump = false;
            anim.SetBool("isJump",false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = null;
        }
    }
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Ammo:
                    if (ammo >= maxAmmo)
                        return;
                    ammo += item.value;
                    if (ammo > maxAmmo)
                        ammo = maxAmmo;
                    break;
                case Item.Type.Coin:
                    if (coin >= maxCoin)
                        return;
                    coin += item.value;
                    if (coin > maxCoin)
                        coin = maxCoin;
                    break;
                case Item.Type.Heart:
                    if (health >= maxHealth)
                        return;
                    health += item.value;
                    if (health > maxHealth)
                        health = maxHealth;
                    break;
                case Item.Type.Grenade:
                    if (hasGrenades >= maxHasGrenades)
                        return;
                    grenades[hasGrenades].SetActive(true);
                    hasGrenades += item.value;
                    if (hasGrenades > maxHasGrenades)
                        hasGrenades = maxHasGrenades;
                    break;
            }
            Destroy(other.gameObject);
        }
    }
}
