using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Te : MonoBehaviour
{

    public GameObject cube;

    public float spawnX = 0;
    public float spawnY = 0;

    public float spawnScaleY = 0;

    [Space(10)]
    public int max;
    public List<GameObject> cubeList = new List<GameObject>();

    private float value;
    private int index;

    private GameObject temp;

    private float A_changeX;
    private float B_changeX;

    private float timer = 0.05f;

    private AudioSource audioSource;

    void Start() 
    {
        audioSource = GetComponent<AudioSource>();

        for (int i = 0; i < max; i ++)
        {
            GameObject Test = Instantiate(cube, new Vector3(spawnX, spawnY, 0), Quaternion.identity, transform);
            Test.transform.localScale += new Vector3(0,spawnScaleY,0);

            cubeList.Add(Test);

            spawnX += 1.25f;
            spawnY += 0.5f;

            spawnScaleY += 1;
        }

        for(int i = 0; i < max; i++)
        {
            int A = Random.Range(0,cubeList.Count);
            int B = Random.Range(0,cubeList.Count);

            A_changeX = cubeList[A].transform.position.x;
            B_changeX = cubeList[B].transform.position.x;

            SetTransform(cubeList[A], B_changeX);
            SetTransform(cubeList[B], A_changeX);

            int A_index = cubeList.IndexOf(cubeList[A]);
            int B_index = cubeList.IndexOf(cubeList[B]);

            GameObject A_temp = cubeList[A_index];
            GameObject B_temp = cubeList[B_index];

            cubeList[A_index] = B_temp;
            cubeList[B_index] = A_temp;
        }

        transform.position = new Vector3(-(max/2), 0, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine("Sort");
        }
    }

    void SetTransform(GameObject test, float pos)
    {
        test.transform.position = new Vector3(pos,test.transform.position.y,0);
    }

    IEnumerator Sort()
    {
        Debug.Log("정렬을 시작합니다!");

        for (int i = 0; i < cubeList.Count - 1; i++)
        {
            audioSource.Play();

            value = max;

            for (int j = i; j < cubeList.Count; j++)
            {
                cubeList[i].GetComponent<MeshRenderer>().material.color = Color.green;

                GameObject obj = cubeList[j];
                obj.GetComponent<MeshRenderer>().material.color = Color.red;
                yield return new WaitForSeconds(timer);
                obj.GetComponent<MeshRenderer>().material.color = Color.white;

                if (value > cubeList[j].transform.localScale.y)
                {
                    if (temp)
                        temp.GetComponent<MeshRenderer>().material.color = Color.white;

                    value = cubeList[j].transform.localScale.y;
                    index = j;

                    temp = cubeList[index];
                    temp.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                yield return new WaitForSeconds(timer);

                if(timer > 0)
                    timer -= 0.001f;
            }

            cubeList[i].GetComponent<MeshRenderer>().material.color = Color.white;

            audioSource.Stop();

            A_changeX = cubeList[i].transform.position.x;
            B_changeX = cubeList[index].transform.position.x;

            SetTransform(cubeList[i], B_changeX);
            SetTransform(cubeList[index], A_changeX);

            cubeList[index] = cubeList[i];
            cubeList[i] = temp;

            cubeList[i].GetComponent<MeshRenderer>().material.color = Color.white;
            cubeList[index].GetComponent<MeshRenderer>().material.color = Color.white;
        }

        audioSource.Play();

        for (int i = 0; i < cubeList.Count; i++)
        {
            cubeList[i].GetComponent<MeshRenderer>().material.color = Color.green;
            yield return new WaitForSeconds(timer);
        }

        audioSource.Stop();

        for (int i = 0; i < cubeList.Count; i++)
        {
            cubeList[i].GetComponent<MeshRenderer>().material.color = Color.white;
        }

        yield return null;
    }
}
