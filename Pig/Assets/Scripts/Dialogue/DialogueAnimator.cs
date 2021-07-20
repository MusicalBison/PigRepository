using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAnimator : MonoBehaviour
{
    public Animator startAnim;
    public Animator circleAnim;
    public Animator boltAnim;
    public Animator endAnim;
    public Animator waitAnim;
    public DialogueManager dm;

    Player player;

    bool isTachka = false;
    public bool isBoltPinguin = false;
    public bool isCirclePinguin = false;

    public SpriteRenderer tachkaSR;
    public Sprite tachka;
    public BoxCollider2D refrigeratorCollider;
    public GameObject rightCircle;
    public GameObject leftCircle;
    bool refrigeratorIsClose = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            /*
             * Нужно тут весь код переписать с этим плеером, 
             * иначе он очень громоздкиq
             */
            player = other.gameObject.GetComponent<Player>();
            if (!isTachka)
            {
                if (!other.gameObject.GetComponent<Player>().isCircle && !other.gameObject.GetComponent<Player>().isBolt)
                {
                    if (startAnim.gameObject.GetComponent<DialogueTrigger>().dialogue.isRead == false)
                    {
                        startAnim.SetBool("StartOpen", true);
                    }
                    else waitAnim.SetBool("StartOpen", true);
                }
                if (other.gameObject.GetComponent<Player>().isCircle && !other.gameObject.GetComponent<Player>().isBolt && !isCirclePinguin && !isBoltPinguin)
                {
                    if (circleAnim.gameObject.GetComponent<DialogueTrigger>().dialogue.isRead == false)
                    {
                        circleAnim.SetBool("StartOpen", true);
                    }
                    else waitAnim.SetBool("StartOpen", true);
                }
                if (!other.gameObject.GetComponent<Player>().isCircle && other.gameObject.GetComponent<Player>().isBolt && !isCirclePinguin && !isBoltPinguin)
                {
                    if (boltAnim.gameObject.GetComponent<DialogueTrigger>().dialogue.isRead == false)
                    {
                        boltAnim.SetBool("StartOpen", true);
                    }
                    else waitAnim.SetBool("StartOpen", true);
                }
                if ((((!other.gameObject.GetComponent<Player>().isCircle && other.gameObject.GetComponent<Player>().isBolt) || (other.gameObject.GetComponent<Player>().isCircle && !other.gameObject.GetComponent<Player>().isBolt)) && (isBoltPinguin || isCirclePinguin)) || (isBoltPinguin && isCirclePinguin))
                {
                    endAnim.SetBool("StartOpen", true);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (!isTachka)
            {
                if (!other.gameObject.GetComponent<Player>().isCircle && !other.gameObject.GetComponent<Player>().isBolt)
                {
                    if (startAnim.gameObject.GetComponent<DialogueTrigger>().dialogue.isRead == false) startAnim.SetBool("StartOpen", false);
                    else waitAnim.SetBool("StartOpen", false);
                }
                if (other.gameObject.GetComponent<Player>().isCircle && !other.gameObject.GetComponent<Player>().isBolt)
                {
                    if (circleAnim.gameObject.GetComponent<DialogueTrigger>().dialogue.isRead == false) circleAnim.SetBool("StartOpen", false);
                    else waitAnim.SetBool("StartOpen", false);
                }
                if (!other.gameObject.GetComponent<Player>().isCircle && other.gameObject.GetComponent<Player>().isBolt)
                {
                    if (boltAnim.gameObject.GetComponent<DialogueTrigger>().dialogue.isRead == false) boltAnim.SetBool("StartOpen", false);
                    else waitAnim.SetBool("StartOpen", false);
                }
                dm.EndDialogue();
            }
            else
            {
                EndPinguinDialogue();
                dm.EndDialogue();
            }
        }
    }

    public void TachkaRepair()
    {
        isTachka = true;
        //tachkaSR.sprite = tachka;
        //gameObject.GetComponent<CircleCollider2D>().enabled = false;
        tachkaSR.sprite = tachka;
        rightCircle.GetComponent<SpriteRenderer>().enabled = true;
        leftCircle.GetComponent<SpriteRenderer>().enabled = true;
        rightCircle.GetComponent<CircleCollider2D>().enabled = true;
        leftCircle.GetComponent<CircleCollider2D>().enabled = true;
        foreach (PolygonCollider2D collider in tachkaSR.GetComponents<PolygonCollider2D>())
        {
            if (collider.offset.x == 0) collider.enabled = true;
            if (collider.offset.x == -0.001f) collider.enabled = false;
        }


        //refrigeratorCollider.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        //tachkaSR.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void EndPinguinDialogue()
    {
        if (isBoltPinguin && isCirclePinguin)
        {
            if (refrigeratorIsClose == false)
            {
                refrigeratorIsClose = true;
                player.DirtingLeavesAndroid();
                refrigeratorCollider.enabled = false;

                refrigeratorCollider.GetComponent<Rigidbody2D>().mass = 25f;
                refrigeratorCollider.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                tachkaSR.GetComponent<Rigidbody2D>().mass = 25f;
                tachkaSR.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                tachkaSR.GetComponent<FixedJoint2D>().enabled = true;
                rightCircle.GetComponent<Rigidbody2D>().mass = 25f;
                leftCircle.GetComponent<Rigidbody2D>().mass = 25f;



            }
        }
    }

    public void GettingCircle()
    {
        if ((!isBoltPinguin && !isCirclePinguin) || (isBoltPinguin && !isCirclePinguin))
        {
            player.isCircle = false;
            isCirclePinguin = true;
            player.ClearInventory();
        }
    }

    public void GettingBolt()
    {
        if ((!isBoltPinguin && !isCirclePinguin) || (!isBoltPinguin && isCirclePinguin))
        {
            player.isBolt = false;
            isBoltPinguin = true;
            player.ClearInventory();
        }
    }
}
