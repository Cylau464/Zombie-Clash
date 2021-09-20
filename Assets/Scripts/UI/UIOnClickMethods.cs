using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIOnClickMethods : MonoBehaviour
{
    public void ClickEffect()
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        GameObject upgradeEffect = Resources.Load<GameObject>("UI/Button Pressed Effect");
        GameObject inst = Instantiate(upgradeEffect, clickedButton.transform.position, Quaternion.identity, clickedButton.transform);
        //Texture id from shader DecolorizeSprite
        inst.GetComponent<Image>().material.SetTexture("Texture2D_F93BDDA5", clickedButton.GetComponent<Button>().image.sprite.texture);
        Destroy(inst, inst.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }
}