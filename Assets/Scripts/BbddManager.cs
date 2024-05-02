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
                        SceneManager.LoadScene("Menu");
                    }
                }
            }
        }
    }

    //metodos bbdd-----------------
    //guardamos las partidas
    private IEnumerator guardarPartidaId(string idPartida) {
        Debug.Log(idPartida);
        Debug.Log(User.UserId);
        var DBTask = BBDDref.Child("users").Child(User.UserId).Child("partidas").SetValueAsync(idPartida);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) {
            Debug.LogWarning("error al guardar");
        } else {
            Debug.Log("guardado con exito");
        }
    }


    public void guardarPartidaEnBBDD(string idPartida) {
        if (BBDDref != null) StartCoroutine(guardarPartidaId(idPartida));
        else Debug.Log("la referencia es nula...");
    }

    public void guardarDatos() {
        //aqui pondremos los valores que vamos a guardar
    }

}
