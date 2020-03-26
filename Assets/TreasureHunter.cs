using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AttachmentRule{KeepRelative,KeepWorld,SnapToTarget};

public class TreasureHunter : MonoBehaviour
{
    public Vector3 waist;
    public GameObject head;
    public GameObject center;
    public GameObject leftPointerObject;
    public GameObject rightPointerObject;
    GameObject thingOnGun;
    public LayerMask collectiblesMask;
    Vector3 previousPointerPos;
    public TreasureHunterInventory inventory;
    public TextMesh numitems;
    public TextMesh score;
    public TextMesh displayscore;
    public int count = 0;
    CollectibleTreasure grabbed;
    bool rotate;

    private float presize = 1f;
    void Start()
    {
        for(int i=0;i<inventory.treasures.Count;i++){
            inventory.amount.Add(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(rotate){
            head.transform.Rotate(Random.Range(-10,10),Random.Range(-10,10),Random.Range(-10,10),Space.Self);
        }
        if(!rotate){
            head.transform.rotation = new Quaternion(0,0,0,0);
        }
        if(Input.GetKeyDown("9")){ //code for how to raycast was based off of Nick Rewkowski's VrPawn teleport code
            RaycastHit outHit;
            if(Physics.Raycast(rightPointerObject.transform.position,rightPointerObject.transform.forward,out outHit, 100.0f)){
                if(outHit.collider.gameObject.GetComponent("CollectibleTreasure")){
                    print("hit");
                    GameObject item = outHit.collider.gameObject;
                    /*string objectname = outHit.collider.gameObject.GetComponent<CollectibleTreasure>().name;
                    bool exist = false;
                    for(int i=0;i<inventory.treasures.Count;i++){
                        if(inventory.treasures.ElementAt(i).name==objectname){
                            inventory.amount[i]++;
                            exist = true;
                        }
                    }
                    if(exist==false){
                        inventory.treasures.Add(outHit.collider.gameObject.GetComponent<CollectibleTreasure>());
                        inventory.amount.Add(1);
                    }*/
                    /*if(objectname==1){
                        numberofEach[0]++;
                    } else if(objectvalue==5){
                        numberofEach[1]++;
                    } else if(objectvalue==10){
                        numberofEach[2]++;
                    }*/
                    Destroy(outHit.collider.gameObject);
                    int sum = 0;
                    int totalitems = 0;
                    int spheres = 0;
                    int cylinders = 0;
                    int cubes = 0;
                    int capsules = 0;
                    for(int j=0;j<inventory.treasures.Count;j++){
                        if(inventory.treasures[j].value==1){
                            cylinders = inventory.amount[j];
                        }
                        if(inventory.treasures[j].value==10){
                            cubes = inventory.amount[j];
                        }
                        if(inventory.treasures[j].value==20){
                            capsules = inventory.amount[j];
                        }
                        if(inventory.treasures[j].value==5){
                            spheres = inventory.amount[j];
                        }
                        sum += inventory.treasures[j].value*inventory.amount[j];  
                        totalitems += inventory.amount[j];

                    }
                    displayscore.text = totalitems+" items \n Spheres (5 ea):"+spheres+" \n Cubes (10 ea): "+cubes+"\n Capsules (20 ea): "+capsules+"\n Cylinders (1 ea): "+cylinders+"\n Total score: "+sum;
                }
            }
            print("You hit the collector button");
        }

        if(OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger)){
            score.text="hit button";
            //head.transform.position = new Vector3(0,10,0);
            forceGrab(true);
        }

        if(OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger)){
            score.text="let go button";
            letGo();
            //head.transform.position = new Vector3(0,-1,0);
        }

        if(OVRInput.GetDown(OVRInput.RawButton.RHandTrigger)){
            score.text="hit button2";
            //head.transform.position = new Vector3(0,10,0);
            forceGrab(false);
        }

        if(OVRInput.GetUp(OVRInput.RawButton.RHandTrigger)){
            score.text="let go button2";
            letGo();
            //head.transform.position = new Vector3(0,-1,0);
        }

        if(OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger)){
            score.text="hit button3";
            //head.transform.position = new Vector3(0,10,0);
            Collider[] handgrip = Physics.OverlapSphere(leftPointerObject.transform.position,0.1f,collectiblesMask);
            attachGameObjectToAChildGameObject(handgrip[0].gameObject,leftPointerObject.gameObject,AttachmentRule.KeepWorld,AttachmentRule.KeepWorld,AttachmentRule.KeepWorld,true);
            grabbed=handgrip[0].gameObject.GetComponent<CollectibleTreasure>();
            if(grabbed.name=="CubeCollectible"){
                rotate=true;
            } else if(handgrip.Length>0){
                rotate=false;
            }
            Vector3 scaleChange = new Vector3(-0.01f, -0.01f, -0.01f);
            presize = grabbed.transform.localScale.y;
        }

        if(OVRInput.GetUp(OVRInput.RawButton.LIndexTrigger)){
            score.text="let go button3";
            letGo();
            //head.transform.position = new Vector3(0,-1,0);
            
        }

        if(OVRInput.GetDown(OVRInput.RawButton.LHandTrigger)){
            score.text="Shreya Gullapalli and Ammar Puri";
        }

        if(Input.GetKeyDown("1")){
            print("You hit the score button");
            //score.text = "Score = "+sum + " Ammar Puri & Shreya Gullapalli \n";
            //numitems.text = "Total Items = "+totalitems+"\n";
            
        }
        /*if(Input.GetKeyDown("1")){
            if(inventory.treasures.Contains(PointerArray[0])==false){
                inventory.treasures.Add(PointerArray[0]);
            }
            print("You hit 1");
        }

        if(Input.GetKeyDown("2")){
            if(inventory.treasures.Contains(PointerArray[1])==false){
                inventory.treasures.Add(PointerArray[1]);
            }
            print("You hit 2");
        }

        if(Input.GetKeyDown("3")){
            if(inventory.treasures.Contains(PointerArray[2])==false){
                inventory.treasures.Add(PointerArray[2]);
            }
            print("You hit 3");
        }

        if(Input.GetKeyDown("4")){
            score.text="Score: " + countScore()+ "\n"+"# of obj:" + inventory.treasures.Count;
            print("You hit 4");
        }

        if(inventory.treasures.Count==3){
            win.text="You Win! - Ammar Puri";
        }*/
    }

    /*public int countScore(){
        count=0;
        for(int i=0;i<inventory.treasures.Count;i++){
            count+=inventory.treasures[i].value;
        }
        return count;
    }*/

    void forceGrab(bool pressedA){
        RaycastHit outHit;
        numitems.text="test";
        //notice I'm using the layer mask again
        if (Physics.Raycast(rightPointerObject.transform.position, rightPointerObject.transform.up, out outHit, 100.0f,collectiblesMask))
        {
            numitems.text="hit";
            AttachmentRule howToAttach=pressedA?AttachmentRule.KeepWorld:AttachmentRule.SnapToTarget;
            attachGameObjectToAChildGameObject(outHit.collider.gameObject,rightPointerObject.gameObject,howToAttach,howToAttach,AttachmentRule.KeepWorld,true);
            grabbed=outHit.collider.gameObject.GetComponent<CollectibleTreasure>();
            if(grabbed.name=="CubeCollectible"){
                rotate=true;
            }else{
                rotate=false;
            }

            //
            
            Vector3 scaleChange = new Vector3(-0.01f, -0.01f, -0.01f);
            presize = grabbed.transform.localScale.y;
            grabbed.transform.localScale = new Vector3(2,2,2);
            //

        }
    }
    void letGo(){
        if (grabbed){
            score.text = "first";
            waist= center.transform.position - new Vector3(0,1,0);
            Collider[] overlappingThingsWithLeftHand=Physics.OverlapSphere(leftPointerObject.transform.position,0.01f,collectiblesMask);
            Collider[] waisttrue=Physics.OverlapSphere(waist,.5f,collectiblesMask);
            score.text= "second";
            if(waisttrue.Length>0){
                score.text="third";
                string objectname = grabbed.gameObject.GetComponent<CollectibleTreasure>().name;
                bool exist = false;
                score.text = "fourth";
                for(int i=0;i<inventory.treasures.Count;i++){
                    if(inventory.treasures.ElementAt(i).name==objectname){
                        inventory.amount[i]++;
                        exist = true;
                    }
                }
                if(exist==false){
                    inventory.treasures.Add(grabbed.gameObject.GetComponent<CollectibleTreasure>());
                    inventory.amount.Add(1);
                }
                Destroy(grabbed.gameObject);
                int sum = 0;
                    int totalitems = 0;
                    int spheres = 0;
                    int cylinders = 0;
                    int cubes = 0;
                    int capsules = 0;
                    for(int j=0;j<inventory.treasures.Count;j++){
                        if(inventory.treasures[j].value==1){
                            cylinders = inventory.amount[j];
                        }
                        if(inventory.treasures[j].value==10){
                            cubes = inventory.amount[j];
                        }
                        if(inventory.treasures[j].value==20){
                            capsules = inventory.amount[j];
                        }
                        if(inventory.treasures[j].value==5){
                            spheres = inventory.amount[j];
                        }
                        sum += inventory.treasures[j].value*inventory.amount[j];  
                        totalitems += inventory.amount[j];

                    }
                    displayscore.text = totalitems+" items \n Spheres (5 ea):"+spheres+" \n Cubes (10 ea): "+cubes+"\n Capsules (20 ea): "+capsules+"\n Cylinders (1 ea): "+cylinders+"\n Total score: "+sum;
            }
            if (overlappingThingsWithLeftHand.Length>0){
                if (thingOnGun){
                    detachGameObject(thingOnGun,AttachmentRule.KeepWorld,AttachmentRule.KeepWorld,AttachmentRule.KeepWorld);
                    simulatePhysics(thingOnGun,Vector3.zero,true);
                    
                }
                attachGameObjectToAChildGameObject(overlappingThingsWithLeftHand[0].gameObject,leftPointerObject,AttachmentRule.SnapToTarget,AttachmentRule.SnapToTarget,AttachmentRule.KeepWorld,true);
                thingOnGun=overlappingThingsWithLeftHand[0].gameObject;
                grabbed=null;
            }else{
                Vector3 scaleChange = new Vector3(-0.01f, -0.01f, -0.01f);
                while(grabbed.transform.localScale.y<presize){
                    grabbed.transform.localScale -= scaleChange;
                }
                detachGameObject(grabbed.gameObject,AttachmentRule.KeepWorld,AttachmentRule.KeepWorld,AttachmentRule.KeepWorld);
                simulatePhysics(grabbed.gameObject,Vector3.zero,true);
                grabbed=null;
            }
            numitems.text="let go";
        }
    }
    public void attachGameObjectToAChildGameObject(GameObject GOToAttach, GameObject newParent, AttachmentRule locationRule, AttachmentRule rotationRule, AttachmentRule scaleRule, bool weld){
        GOToAttach.transform.parent=newParent.transform;
        handleAttachmentRules(GOToAttach,locationRule,rotationRule,scaleRule);
        if (weld){
            simulatePhysics(GOToAttach,Vector3.zero,false);
        }
    }

    public static void detachGameObject(GameObject GOToDetach, AttachmentRule locationRule, AttachmentRule rotationRule, AttachmentRule scaleRule){
        //making the parent null sets its parent to the world origin (meaning relative & global transforms become the same)
        GOToDetach.transform.parent=null;
        handleAttachmentRules(GOToDetach,locationRule,rotationRule,scaleRule);
    }

    public static void handleAttachmentRules(GameObject GOToHandle, AttachmentRule locationRule, AttachmentRule rotationRule, AttachmentRule scaleRule){
        GOToHandle.transform.localPosition=
        (locationRule==AttachmentRule.KeepRelative)?GOToHandle.transform.position:
        //technically don't need to change anything but I wanted to compress into ternary
        (locationRule==AttachmentRule.KeepWorld)?GOToHandle.transform.localPosition:
        new Vector3(0,0,0);

        //localRotation in Unity is actually a Quaternion, so we need to specifically ask for Euler angles
        GOToHandle.transform.localEulerAngles=
        (rotationRule==AttachmentRule.KeepRelative)?GOToHandle.transform.eulerAngles:
        //technically don't need to change anything but I wanted to compress into ternary
        (rotationRule==AttachmentRule.KeepWorld)?GOToHandle.transform.localEulerAngles:
        new Vector3(0,0,0);

        GOToHandle.transform.localScale=
        (scaleRule==AttachmentRule.KeepRelative)?GOToHandle.transform.lossyScale:
        //technically don't need to change anything but I wanted to compress into ternary
        (scaleRule==AttachmentRule.KeepWorld)?GOToHandle.transform.localScale:
        new Vector3(1,1,1);
    }
    public void simulatePhysics(GameObject target,Vector3 oldParentVelocity,bool simulate){
        Rigidbody rb=target.GetComponent<Rigidbody>();
        if (rb){
            if (!simulate){
                Destroy(rb);
            } 
        } else{
            if (simulate){
                //there's actually a problem here relative to the UE4 version since Unity doesn't have this simple "simulate physics" option
                //The object will NOT preserve momentum when you throw it like in UE4.
                //need to set its velocity itself.... even if you switch the kinematic/gravity settings around instead of deleting/adding rb
                Rigidbody newRB=target.AddComponent<Rigidbody>();
                newRB.velocity=oldParentVelocity;
            }
        }
    }
}
