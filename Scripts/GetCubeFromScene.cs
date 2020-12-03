using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCubeFromScene : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject theCubeFromTheScene;
    void Start()
    {
        theCubeFromTheScene = GameObject.Find("MyCanvasToModify(Clone)");
        if (theCubeFromTheScene != null) print("Se encontro el cubo en la escena!");
        else print("No se encontro el cubo en la escena!");
    }

    // Update is called once per frame
    void Update()
    {

    }
}