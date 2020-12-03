using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuInicial : MonoBehaviour
{
    [SerializeField] private GameObject menuPrincipal;
    [SerializeField] private GameObject menuIniciarSesion;
    [SerializeField] private GameObject menuRegistro;
    [SerializeField] private GameObject popUpIniciarSesion;

    private bool inicioSesionSatisfactorio = false;

    private string dbResponse = "";

    public void ActivarMenuPrincipal(bool cancel)
    {
        // Se activa menú principal desde la interfaz para login
        if (!cancel && !menuRegistro.activeSelf)
        {
            menuIniciarSesion.transform.Find("ErrorIcon").gameObject.SetActive(false);
            menuIniciarSesion.transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "";

            string nombreUsuario = menuIniciarSesion.transform.Find("NombreInputField").GetComponent<InputField>().text;
            string password = menuIniciarSesion.transform.Find("ContraInputField").GetComponent<InputField>().text;

            if (string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(password))
            {
                menuIniciarSesion.transform.Find("ErrorIcon").gameObject.SetActive(true);
                menuIniciarSesion.transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "Faltan completar campos obligatorios.";
                return;
            }

            LoginInfo loginInfo = new LoginInfo();
            loginInfo.actor = new LoginInfo.Login(nombreUsuario, password);
            loginInfo.sala = "5f9d4de60be3b62b2c70a5a1"; 
            string json = JsonUtility.ToJson(loginInfo);
            StartCoroutine(Post("https://diseno2020.herokuapp.com/api/login", json, "login"));

            // Se bloquea el botón de iniciar sesión para evitar múltiples couroutines
            menuIniciarSesion.transform.Find("BotonIniciarSesion").GetComponent<Button>().interactable = false;
        }
        // Se activa menú principal desde la interfaz para register
        else if(!cancel && !menuIniciarSesion.activeSelf)
        {
            string nombre = menuRegistro.transform.Find("NombreInputField").GetComponent<InputField>().text;
            string apellido = menuRegistro.transform.Find("ApellidoInputField").GetComponent<InputField>().text;
            string email = menuRegistro.transform.Find("EmailInputField").GetComponent<InputField>().text;
            string nombreUsuario = menuRegistro.transform.Find("NombreUsuarioInputField").GetComponent<InputField>().text;
            string password = menuRegistro.transform.Find("ContraInputField").GetComponent<InputField>().text;

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(password))
            {
                menuRegistro.transform.Find("ErrorIcon").gameObject.SetActive(true);
                menuRegistro.transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "Faltan completar campos obligatorios.";
                return;
            }

            RegisterInfo registerInfo = new RegisterInfo();
            registerInfo.actor = new RegisterInfo.Register(nombre, apellido, email, nombreUsuario, password);
            string json = JsonUtility.ToJson(registerInfo);
            StartCoroutine(Post("https://diseno2020.herokuapp.com/api/record", json, "register"));

            // Se bloquea el botón de iniciar sesión para evitar múltiples couroutines
            menuRegistro.transform.Find("BotonRegistrar").GetComponent<Button>().interactable = false;

            ActorInfo actorInfo = new ActorInfo("5f9d4de60be3b62b2c70a5a1", nombreUsuario, "Programador");
            string jsonActor = JsonUtility.ToJson(actorInfo);
            Debug.Log("Post: " + jsonActor);
            StartCoroutine(Post("https://diseno2020.herokuapp.com/api/sala/addActor", jsonActor, "addActor"));
        }

        if(cancel)
        {
            menuIniciarSesion.transform.Find("NombreInputField").GetComponent<InputField>().text = "";
            menuIniciarSesion.transform.Find("ContraInputField").GetComponent<InputField>().text = "";

            //Se activa menú principal
            menuIniciarSesion.SetActive(false);
            menuRegistro.SetActive(false);
            menuPrincipal.SetActive(true);
        }
    }

    public void ActivarMenuIniciarSesion()
    {
        menuPrincipal.SetActive(false);
        menuIniciarSesion.SetActive(true);

        menuIniciarSesion.transform.Find("ErrorIcon").gameObject.SetActive(false);
        menuIniciarSesion.transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "";
        return;
    }

    public void ActivarMenuRegistroUsuario()
    {
        menuIniciarSesion.SetActive(false);
        menuRegistro.SetActive(true);

        menuRegistro.transform.Find("ErrorIcon").gameObject.SetActive(false);
        menuRegistro.transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "";
        return;
    }

    public void Entrar()
    {
        if (inicioSesionSatisfactorio)
            SceneManager.LoadScene("MenuRoles");
        else
            popUpIniciarSesion.SetActive(true);
    }

    private void Start()
    {
        if(!string.IsNullOrEmpty(PlayerInfo.playerName))
            menuPrincipal.transform.Find("BotonCuentaJugador").GetComponentInChildren<TextMeshProUGUI>().text = PlayerInfo.playerName;
    }

    #region Clases para construir los json

    [System.Serializable]
    private class LoginInfo
    {
        [System.Serializable]
        public class Login
        {
            public string user;
            public string password;

            public Login(string user, string password)
            {
                this.user = user;
                this.password = password;
            }
        }

        public Login actor;
        public string sala;
    }

    [System.Serializable]
    private class RegisterInfo
    {
        [System.Serializable]
        public class Register
        {
            public string firstname;
            public string surname;
            public string email;
            public string nameid;
            public string password;

            public Register(string firstname, string surname, string email, string nameid, string password)
            {
                this.firstname = firstname;
                this.surname = surname;
                this.email = email;
                this.nameid = nameid;
                this.password = password;
            }
        }

        public Register actor;
    }

    [System.Serializable]
    private class ActorInfo
    {
        public string sala;
        public string nameid;
        public string rol;

        public ActorInfo(string sala, string nameid, string rol)
        {
            this.sala = sala;
            this.nameid = nameid;
            this.rol = rol;
        }
    }

    #endregion

    #region Método POST para API

    public IEnumerator Post(string url, string envio, string uso)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, envio))
        {
            www.SetRequestHeader("content-type", "application/json");
            www.uploadHandler.contentType = "application/json";
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(envio));
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                dbResponse = string.Format("{0}: {1}", www.url, www.error);
                Debug.Log(string.Format("{0}: {1}", www.url, www.error));

                if (uso.Equals("register"))
                {
                    if (!dbResponse.Contains("recorded"))
                        Debug.LogError("Error al registrar usuario");
                }

            }
            else
            {
                dbResponse = string.Format("Response: {0}", www.downloadHandler.text);
                Debug.Log(string.Format("Response: {0}", www.downloadHandler.text));
                Debug.Log(uso);
                if(uso.Equals("register"))
                {
                    menuRegistro.transform.Find("BotonRegistrar").GetComponent<Button>().interactable = true;
                    if (dbResponse.Contains("recorded"))
                    {
                        // Se setea el nombre de usuario automáticamente
                        menuIniciarSesion.transform.Find("NombreInputField").GetComponent<InputField>().text = 
                            menuRegistro.transform.Find("NombreUsuarioInputField").GetComponent<InputField>().text;
                        // Se vuelve al menú de iniciar sesión
                        menuIniciarSesion.SetActive(true);
                        menuRegistro.SetActive(false);
                        menuPrincipal.SetActive(false);
                    }
                    else
                    {
                        Debug.Log("error register");
                        menuRegistro.transform.Find("ErrorIcon").gameObject.SetActive(true);
                        string error = dbResponse.Replace("Response: {", "").Split(':')[1].Replace("\"", "").Replace("}", "");
                        menuRegistro.transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = error + ".";
                    }
                    menuRegistro.transform.Find("NombreInputField").GetComponent<InputField>().text = "";
                    menuRegistro.transform.Find("ApellidoInputField").GetComponent<InputField>().text = "";
                    menuRegistro.transform.Find("EmailInputField").GetComponent<InputField>().text = "";
                    menuRegistro.transform.Find("NombreUsuarioInputField").GetComponent<InputField>().text = "";
                    menuRegistro.transform.Find("ContraInputField").GetComponent<InputField>().text = "";

                }
                else if(uso.Equals("login"))
                {
                    menuIniciarSesion.transform.Find("BotonIniciarSesion").GetComponent<Button>().interactable = true;
                    if (!dbResponse.Contains("no coinciden"))
                    {
                        Debug.Log("login good");
                        string nombreUsuario = menuIniciarSesion.transform.Find("NombreInputField").GetComponent<InputField>().text;
                        PlayerInfo.playerName = nombreUsuario;
                        menuPrincipal.transform.Find("BotonCuentaJugador").GetComponentInChildren<TextMeshProUGUI>().text = nombreUsuario;
                        inicioSesionSatisfactorio = true;
                        // Se vuelve al menú principal
                        menuIniciarSesion.SetActive(false);
                        menuRegistro.SetActive(false);
                        menuPrincipal.SetActive(true);
                    }
                    else
                    {
                        Debug.Log("login error");
                        menuIniciarSesion.transform.Find("ErrorIcon").gameObject.SetActive(true);
                        menuIniciarSesion.transform.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = "Inicio de sesión no satisfactorio.";
                    }
                    menuIniciarSesion.transform.Find("NombreInputField").GetComponent<InputField>().text = "";
                    menuIniciarSesion.transform.Find("ContraInputField").GetComponent<InputField>().text = "";
                }
            }
        }
    }

    #endregion
}
