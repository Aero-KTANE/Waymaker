using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class waymak3r : MonoBehaviour
{

    public Material[] materials;
    public KMBombInfo bombInfo;
    public KMAudio audio;
    public KMSelectable[] buttons;
    public Transform cubeTransform;
    public MeshFilter[] spheres;
    public MeshRenderer[] shafts;
    public int fadeSpeed;
    public Material black;
    public Material white;
    public GameObject player;

    bool stop;
    int cubeState = 0;

    int[] over;
    int[] under;

    static int moduleIdCounter = 1;
    int moduleId;
    private bool ModuleSolved;
    private float degreesPerSecond = 1;

    // Use this for initialization
    void Start()
    {
        foreach (MeshRenderer n in shafts)
        {
            n.material = black;
        }
        CalculateMaze();
        StartCoroutine(StateCheck());
    }

    private void CalculateMaze()
    {
        for (int i = 0; i < Rnd.Range(10, 20); i++)
        {
            int n = Rnd.Range(0, 53);
            if (shafts.Length == 0)
                break;
            shafts[n].material = white;
        }
    }

    void Awake()
    {
        moduleId = moduleIdCounter++;

        foreach (KMSelectable button in buttons)
        {
            KMSelectable pressedButton = button;
            button.OnInteract += delegate () { ButtonPress(pressedButton); return false; };
            button.OnHighlight += delegate () { ButtonHighlight(pressedButton);  };
            button.OnHighlightEnded += delegate () { ButtonUnhighlight(pressedButton);  };
        }
    }

    void ButtonUnhighlight(KMSelectable pressedButton)
    {
        StartCoroutine(FadeOut(pressedButton));
    }

    IEnumerator FadeIn(KMSelectable pressedButton)
    {
        if(stop == true) { yield break; }
        stop = true;
        for (int i = 0; i < 255; i++)
        {
            pressedButton.GetComponent<MeshRenderer>().material.color = new Color32((byte)i, (byte)i, (byte)i, (byte)i);
        }
        stop = false;
        yield break;
    }

    void ButtonHighlight(KMSelectable pressedButton)
    {
        StartCoroutine(FadeIn(pressedButton));
    }

    IEnumerator FadeOut(KMSelectable pressedButton)
    {
        if (stop == true) { yield break; }
        stop = true;
        for (int i = 255; i > 0; i--)
        {
            pressedButton.GetComponent<MeshRenderer>().material.color = new Color32((byte)i, (byte)i, (byte)i, (byte)i);
        }
        stop = false;
        yield break;
    }

    void ButtonPress(KMSelectable pressedButton)
    {
        if (ModuleSolved == false)
        {
            pressedButton.AddInteractionPunch();
            GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
            if (pressedButton == buttons[0])
            {
                cubeState--;
                if (cubeState < 0)
                {
                    cubeState = cubeState + 4;
                }
                StartCoroutine(RotateLeft());
                StartCoroutine(StateCheck());
            }
            else if (pressedButton == buttons[1])
            {
                cubeState++;
                if (cubeState > 3)
                {
                    cubeState = cubeState - 4;
                }
                StartCoroutine(RotateRight());
                StartCoroutine(StateCheck());
            }
            for (int i = 0; i < 6; i++)
            {
                if (pressedButton == buttons[i+2])
                {
                    switch (i+2)
                    {
                        case 2:
                            StartCoroutine(MoveUp());
                            break;
                        case 3:
                            StartCoroutine(MoveUpLeft());
                            break;
                        case 4:
                            StartCoroutine(MoveUpRight());
                            break;
                        case 5:
                            StartCoroutine(MoveDown());
                            break;
                        case 6:
                            StartCoroutine(MoveDownLeft());
                            break;
                        case 7:
                            StartCoroutine(MoveDownRight());
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    private IEnumerator MoveDownRight()
    {
        Vector3 pos = player.transform.localPosition;
        Debug.Log("Schmovin");
        for (int i = 0; i < 10; i++)
        {
            pos = player.transform.localPosition;
            pos = new Vector3(pos.x - 0.1f, pos.y, pos.z);
            player.transform.localPosition = pos;
            yield return new WaitForSeconds(0.001f);
        }
        foreach (MeshFilter sphere in spheres)
        {
            while (pos.x == sphere.transform.position.x + 1 && pos.y == sphere.transform.position.y + 1 && pos.z == sphere.transform.position.z - 1)
            {
                Debug.Log("Moved");
                pos = player.transform.localPosition;
                pos = new Vector3(pos.x - 1, pos.y - 1, pos.z + 1);
                player.transform.localPosition = pos;
            }
        }
    }

    private IEnumerator MoveDownLeft()
    {
        Vector3 pos = player.transform.localPosition;
        Debug.Log("Schmovin");
        for (int i = 0; i < 10; i++)
        {
            pos = player.transform.localPosition;
            pos = new Vector3(pos.x, pos.y - 0.1f, pos.z);
            player.transform.localPosition = pos;
            yield return new WaitForSeconds(0.001f);
        }
        foreach (MeshFilter sphere in spheres)
        {
            while (pos.x == sphere.transform.position.x + 1 && pos.y == sphere.transform.position.y + 1 && pos.z == sphere.transform.position.z - 1)
            {
                Debug.Log("Moved");
                pos = player.transform.localPosition;
                pos = new Vector3(pos.x - 1, pos.y - 1, pos.z + 1);
                player.transform.localPosition = pos;
            }
        }
    }

    private IEnumerator MoveUpRight()
    {
        Vector3 pos = player.transform.localPosition;
        Debug.Log("Schmovin");
        for (int i = 0; i < 10; i++)
        {
            pos = player.transform.localPosition;
            pos = new Vector3(pos.x, pos.y + 0.1f, pos.z);
            player.transform.localPosition = pos;
            yield return new WaitForSeconds(0.001f);
        }
        foreach (MeshFilter sphere in spheres)
        {
            while (pos.x == sphere.transform.position.x + 1 && pos.y == sphere.transform.position.y + 1 && pos.z == sphere.transform.position.z - 1)
            {
                Debug.Log("Moved");
                pos = player.transform.localPosition;
                pos = new Vector3(pos.x - 1, pos.y - 1, pos.z + 1);
                player.transform.localPosition = pos;
            }
        }
    }

    private IEnumerator MoveDown()
    {
        Vector3 pos = player.transform.localPosition;
        Debug.Log("Schmovin");
        for (int i = 0; i < 10; i++)
        {
            pos = player.transform.localPosition;
            pos = new Vector3(pos.x, pos.y, pos.z - 0.1f);
            player.transform.localPosition = pos;
            yield return new WaitForSeconds(0.001f);
        }
        foreach (MeshFilter sphere in spheres)
        {
            while (pos.x == sphere.transform.position.x + 1 && pos.y == sphere.transform.position.y + 1 && pos.z == sphere.transform.position.z - 1)
            {
                Debug.Log("Moved");
                pos = player.transform.localPosition;
                pos = new Vector3(pos.x - 1, pos.y - 1, pos.z + 1);
                player.transform.localPosition = pos;
            }
        }
    }

    private IEnumerator MoveUpLeft()
    {
        Vector3 pos = player.transform.localPosition;
        Debug.Log("Schmovin");
        for (int i = 0; i < 10; i++)
        {
            pos = player.transform.localPosition;
            pos = new Vector3(pos.x + 0.1f, pos.y, pos.z);
            player.transform.localPosition = pos;
            yield return new WaitForSeconds(0.001f);
        }
        foreach (MeshFilter sphere in spheres)
        {
            while (pos.x == sphere.transform.position.x + 1 && pos.y == sphere.transform.position.y + 1 && pos.z == sphere.transform.position.z - 1)
            {
                Debug.Log("Moved");
                pos = player.transform.localPosition;
                pos = new Vector3(pos.x - 1, pos.y - 1, pos.z + 1);
                player.transform.localPosition = pos;
            }
        }
    }

    private IEnumerator MoveUp()
    {
        Vector3 pos = player.transform.localPosition;
        Debug.Log("Schmovin");
        for (int i = 0; i < 10; i++)
        {
            pos = player.transform.localPosition;
            pos = new Vector3(pos.x, pos.y, pos.z + 0.1f);
            player.transform.localPosition = pos;
            yield return new WaitForSeconds(0.001f);
        }
        foreach (MeshFilter sphere in spheres)
        {
            while (pos.x == sphere.transform.position.x + 1 && pos.y == sphere.transform.position.y + 1 && pos.z == sphere.transform.position.z - 1)
            {
                Debug.Log("Moved");
                pos = player.transform.localPosition;
                pos = new Vector3(pos.x - 1, pos.y - 1, pos.z + 1);
                player.transform.localPosition = pos;
            }
        }
    }

    IEnumerator RotateRight()
    {
        for (int i = 0; i < 90; i++)
        {
            cubeTransform.Rotate(0, 0, degreesPerSecond);
            player.transform.Rotate(0, 0, (degreesPerSecond*-1));
            yield return new WaitForSeconds(0.001f);
        }
    }

    IEnumerator RotateLeft()
    {
        for (int i = 0; i < 90; i++)
        {
            cubeTransform.Rotate(0, 0, (degreesPerSecond * -1));
            player.transform.Rotate(0, 0, degreesPerSecond);
            yield return new WaitForSeconds(0.001f);
        }
    }

    IEnumerator StateCheck()
    {
        switch (cubeState)
        {
            case 0:
                under = new int[]
                {
                    0, 1, 3, 5, 9, 11, 12, 14
                };
                over = new int[]
                {
                    4, 6, 7, 18, 19, 24, 25  
                };
                break;
            case 1:
                under = new int[]
                {
                    3, 5, 6, 8, 9, 11, 15, 17
                };
                over = new int[]
                {
                    0, 2, 4, 18, 19, 21, 22
                };
                break;
            case 2:
                under = new int[]
                {
                    3, 5, 6, 8, 18, 20, 24, 26
                };
                over = new int[]
                {
                    0, 2, 4, 9, 10, 12, 13
                };
                break;
            case 3:
                under = new int[]
                {
                    0, 1, 3, 5, 18, 20, 21, 23
                };
                over = new int[]
                {
                    4, 6, 7, 9, 10, 15, 16
                };
                break;
            default:
                yield break;

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}