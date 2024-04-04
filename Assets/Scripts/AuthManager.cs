using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.Events;


public class AuthManager : MonoBehaviour
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

    private void Awake()
    {
        //StartCoroutine(CheckAndFixDependenciesCoroutine());


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
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });


    }

    private IEnumerator CheckAndFixDependenciesCoroutine()
    {
        var checkDependenciesTask = Firebase.FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => checkDependenciesTask.IsCompleted);

        var dependencyStatus = checkDependenciesTask.Result;
        if (dependencyStatus == Firebase.DependencyStatus.Available)
        {
            Debug.Log($"Firebase: {dependencyStatus} :)");
            onFirebaseInitialized.Invoke();
            InitializeFirebase();
        }
        else
        {
            Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            // Firebase Unity SDK is not safe to use here.
        }
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //establecemos el objeto de instancia de autenticacion
        auth = FirebaseAuth.DefaultInstance;
    }

    public void btnLogin()
    {
        if (auth != null)
        {
            StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
        }
        else
        {
            Debug.LogError("Firebase auth is not initialized.");
        }
        //StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
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
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login fallido";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Falta Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Falta Contrase�a";
                    break;
                case AuthError.WrongPassword:
                    message = "Contrase�a Incorrecta";
                    break;
                case AuthError.InvalidEmail:
                    message = "Email Inv�lido";
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
            //User is now logged in
            //Now get the result
            User = LoginTask.Result.User;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);

            //warningLoginText.text = ""; //limpiamos los errores previos
            //confirmLoginText.text = "Logged In";
            Debug.Log("exito en el inicio se sesion");
        }
    }

    private IEnumerator Registro(string _email, string _password)
    {
        if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //warningRegisterText.text = "Password Does Not Match!";
            Debug.Log("Las contrase�as no coinciden");
        }
        else
        {
            Task<AuthResult> RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //espera
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //si hay algun error
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Registro fallido";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Falta Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Falta contrase�a";
                        break;
                    case AuthError.WeakPassword:
                        message = "Contrase�a d�bil";
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
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
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
                    }
                }
            }
        }
    }

}
