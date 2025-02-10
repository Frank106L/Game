using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PhysicsCheck : MonoBehaviour
{
    [Header("基本参数")]
    public Vector2 bottomOffset;

    public float checkRaduis;
    public LayerMask groundLayer;
    [Header("状态")]
    public bool isGround;
    private void Update() {
        Check();
    }

    public void Check(){
         //检测地面
        isGround = Physics2D.OverlapCircle((Vector2)transform.position+bottomOffset,checkRaduis,groundLayer);
    }
    //画线
     private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere((Vector2)transform.position+bottomOffset,checkRaduis);
        
    }
}
