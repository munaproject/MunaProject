using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Firebase.Database;
using System;


public class BbddManager : MonoBehaviour
{
    public UnityEvent onFirebaseInitialized;

    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;

    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    //public TMP_Text warningLoginText;
    //public TMP_Text confirmLoginText;

    //Register variables
    [Header("Registro")]
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    //public TMP_Text warningRegisterText;

    //control de la bbdd
    public DatabaseReference BBDDref;

    //guardamos el nombre de la partida que se esta jugando
    private string partidaJugandose;

    GameManager gameManager;

    private void Awake()
    {

        //Verifica que todas las dependencias necesarias para Firebase estan en el sistema
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //e inicializa si todas estan
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("no se resolvieron todas las dependencias: " + dependencyStatus);
            }
        });


    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        DontDestroyOnLoad(gameObject);        
    }
    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //establecemos el objeto de instancia de autenticacion
        auth = FirebaseAuth.DefaultInstance;
        BBDDref = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void btnLogin()
    {
        if (auth != null)
        {
            StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
        }
        else
        {
            Debug.LogError("Firebase auth no inicializado");
        }
    }

    public void btnRegistro()
    {
        StartCoroutine(Registro(emailRegisterField.text, passwordRegisterField.text));
    }

    private IEnumerator Login(string _email, string _password)
    {
        Task<AuthResult> LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //espera a que la tarea se complete
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //si hay algun error
            Debug.LogWarning(message: $"error al registrar {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login fallido";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Falta Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Falta Contraseña";
                    break;
                case AuthError.WrongPassword:
                    message = "Contraseña Incorrecta";
                    break;
                case AuthError.InvalidEmail:
                    message = "Email Inválido";
                    break;
                case AuthError.UserNotFound:
                    message = "El usuario no existe";
                    break;
            }

            Debug.Log(message);
            //warningLoginText.text = message;
        }
        else
        {
            //login exitoso
            //imprmimos traza
            User = LoginTask.Result.User;
            Debug.LogFormat("user logado con exito: {0} ({1})", User.DisplayName, User.Email);

            //warningLoginText.text = ""; //limpiamos los errores previos
            //confirmLoginText.text = "Logged In";
            Debug.Log("exito en el inicio de sesion");
            gameManager.IdUser = User.UserId;
            SceneManager.LoadScene("Menu");

        }
    }

    private IEnumerator Registro(string _email, string _password)
    {
        if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //warningRegisterText.text = "la contra no coincide";
            Debug.Log("Las contraseñas no coinciden");
        }
        else
        {
            Task<AuthResult> RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //espera
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //si hay algun error
                Debug.LogWarning(message: $"error al registrar tarea {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Registro fallido";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Falta Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Falta contraseña";
                        break;
                    case AuthError.WeakPassword:
                        message = "Contraseña débil";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email ya registrado";
                        break;
                }
                //warningRegisterText.text = message;
                Debug.Log(message);
            }
            else
            {
                User = RegisterTask.Result.User;

                if (User != null)
                {
                    UserProfile profile = new UserProfile();
                    Task ProfileTask = User.UpdateUserProfileAsync(profile);
                    //espera a que la tarea anterior termine
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"error al registrar tarea {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        //warningRegisterText.text = "Username Set Failed!";
                        Debug.Log("error registrando");
                    }
                    else
                    {
                        //Volvemos a la pantalla de login
                        //UIManager.instance.LoginScreen();
                        //warningRegisterText.text = "";
                        Debug.Log("exito en el registro");
                        //si el usuario se ha registrado con exito, cargamos la escena del menu directamente
                        gameManager.IdUser = User.UserId;
                        SceneManager.LoadScene("Menu");
                    }
                }
            }
        }
    }

    //metodos bbdd-----------------
    //guardamos las partidas
    private IEnumerator guardarPartidaId(string idPartida) {
        //var DBTask = BBDDref.Child("users").Child(User.UserId).Child("partidas").SetValueAsync(idPartida);

        //para que un usuario pueda tener varias partidas---
        //hacemos referencia al nodo de las partidas de x user
        var dataPartidasRef = BBDDref.Child("users").Child(User.UserId).Child("partidas"); 
        //añadimos un nuevo nodo
        //var newPartida = dataPartidasRef.Push().SetValueAsync(idPartida);
        partidaJugandose = idPartida;//al crearse, lo guardamos
        var newPartida = dataPartidasRef.Child(idPartida).SetValueAsync(""); //nodo vacio
 
        yield return new WaitUntil(predicate: () => newPartida.IsCompleted);

        if (newPartida.Exception != null) {
            Debug.LogWarning("error al guardar");
        } else {
            Debug.Log("guardado con exito");
        }
    }

    private IEnumerator guardarNombrePartida (string idPartida, string nombre) {
        var DBTask = BBDDref.Child("users").Child(User.UserId).Child("partidas").Child(idPartida).Child("nombre").SetValueAsync(nombre);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) {
            Debug.LogWarning("error al guardar el nombre");
        } else {
            Debug.Log("guardado con exito");
        }
    }

    private IEnumerator guardarEscena (string idPartida, string escena) {
        var DBTask = BBDDref.Child("users").Child(User.UserId).Child("partidas").Child(idPartida).Child("escena").SetValueAsync(escena);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) {
            Debug.LogWarning("error al guardar la escena");
        } else {
            Debug.Log("guardado con exito");
        }
    }

    private IEnumerator guardarPosicion (string idPartida, string pjPos, Vector3 pos) {
        var xDBTask = BBDDref.Child("users").Child(User.UserId).Child("partidas").Child(idPartida).Child(pjPos).Child("x").SetValueAsync((int)pos.x);
        var yDBTask = BBDDref.Child("users").Child(User.UserId).Child("partidas").Child(idPartida).Child(pjPos).Child("y").SetValueAsync((int)pos.y);
        yield return new WaitUntil(predicate: () => xDBTask.IsCompleted && yDBTask.IsCompleted);

        if (xDBTask.Exception != null || yDBTask.Exception != null) {
            Debug.LogWarning("error al guardar la posicion del personaje");
        } else {
            Debug.Log("guardado con exito");
        }
    }


    public void guardarPartidaEnBBDD(string idPartida, string nombre) {
        StartCoroutine(guardarPartidaId(idPartida));
        StartCoroutine(guardarNombrePartida(idPartida, nombre));
    }

    public void guardarDatos(string idPartida, string escena, Vector3 posLille, Vector3 posLiv) {
        StartCoroutine(guardarEscena(idPartida, escena));
        StartCoroutine(guardarPosicion(idPartida, "posLille", posLille));
        StartCoroutine(guardarPosicion(idPartida, "posLiv", posLiv));
    }

    public async Task<List<(string,string)>> cargarTodasPartidas() {
        // referencias de las partidas al usuario actual
        //hacemos una snapshot
        DataSnapshot partidasSnapshot = await BBDDref.Child("users").Child(User.UserId).Child("partidas").GetValueAsync();

        List<(string, string)> partidasIds = new List<(string, string)>();
        string partidaId;
        string nombrePartida;

        // comprobamos que tiene partidas
        if (partidasSnapshot.Exists)
        {
            foreach (DataSnapshot partidaSnapshot in partidasSnapshot.Children)
            {
                partidaId = partidaSnapshot.Key; //esto da el id (nombre del nodo)
                nombrePartida = partidaSnapshot.Child("nombre").Value.ToString();
                partidasIds.Add((partidaId, nombrePartida));//la ponemos en una lista
                Debug.Log("Partida ID con el snaptshot: " + partidaId);
            }
        }
        else
        {
            Debug.Log("No partidas found for the current user.");
        }

        return partidasIds;//devolvemos la lista
    }

    public async void eliminarPartida(string idPartida) {
        //hacemos una referencia al nodo
        var partidaRef = BBDDref.Child("users").Child(User.UserId).Child("partidas").Child(idPartida);
        //y borramos
        try {
            await partidaRef.RemoveValueAsync();
            Debug.Log("nodo borrado con exito");
        } catch(FirebaseException e) {
            Debug.Log("error al borrar el nodo");
        }
    }

    //contaremos las partidas creadas para generar un id unico
    public async Task<string> GenerarCode()
    {
        // tomamos una referencia de todos los usuarios
        DataSnapshot usersSnapshot = await BBDDref.Child("users").GetValueAsync();
        DataSnapshot partidasSnapshot;
        int totalPartidas = 0;
        int new_code = 4096; //convertiremos el numero a hexadecimal al retornarlo

        if (!usersSnapshot.Exists)
        {
            Debug.Log("no hay usuarios.");
            return new_code.ToString("X");
        }

        // por cada usuario, contamos las partidas
        foreach (DataSnapshot userSnapshot in usersSnapshot.Children)
        {
            // obtenemos sus partidas
            partidasSnapshot = userSnapshot.Child("partidas");

            if (!partidasSnapshot.Exists) //0 partidas
            {
                Debug.Log($"no hay partidas guardadas para este usuario: {userSnapshot.Key}");
                continue;
            }

            // contamos
            totalPartidas += (int)partidasSnapshot.ChildrenCount;
        }

        Debug.Log("nro partidas:" + totalPartidas);
        new_code += totalPartidas;
        Debug.Log("cod con hexadecimal: " + new_code.ToString("X"));
        return new_code.ToString("X");
    }

    public async Task cargarDatos(string idPartida) {
        DataSnapshot partidasSnapshot = await BBDDref.Child("users").Child(User.UserId).Child("partidas").Child(idPartida).GetValueAsync();
        
        if (partidasSnapshot.Exists)
        {
            gameManager.Escena = partidasSnapshot.Child("escena").Value.ToString();
            gameManager.PosLille_X = int.Parse(partidasSnapshot.Child("posLille").Child("x").Value.ToString());
            gameManager.PosLille_y = int.Parse(partidasSnapshot.Child("posLille").Child("y").Value.ToString());
            gameManager.PosLiv_x = int.Parse(partidasSnapshot.Child("posLiv").Child("x").Value.ToString());
            gameManager.PosLiv_y = int.Parse(partidasSnapshot.Child("posLiv").Child("y").Value.ToString());

        }
        else
        {
            Debug.LogWarning("No hay datos previos guardados");
            //poner datos por defecto
        }
    }

}
