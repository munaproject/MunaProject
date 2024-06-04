using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class RoomManager : MonoBehaviourPunCallbacks
{

    public TMP_InputField crearField;
    public TMP_InputField unirseField;
    public GameObject objJugar;
    public GameObject objTexto;
    public GameObject objTextoCode;
    private Button btnJugar;
    private bool esMaster;

    [Header("Ventanas")]
    public GameObject panelElegir;
    public GameObject panelCrear;
    public GameObject panelUnirse;
    public GameObject panelEspera;
    public GameObject panelCargarPartida;
    public GameObject opJuego;

    [Header("Sig Escena")]
    public string escena;

    //
    private BbddManager bbdd;
    //guardamos el id de la ultima room creada
    //si se le da clic a jugar, se almacenara en la bbdd
    private string idPartida;
    private string nombrePartida;

    private bool esNuevaPartida;

    GameManager gameManager;
    PhotonView view;

    void Start() {
        btnJugar = objJugar.GetComponent<Button>();

        //como el objeto no se destruye entre escenas, 
        //hay que buscarlo por tipo
        bbdd = FindObjectOfType<BbddManager>();
        esNuevaPartida = false;
        view = GetComponent<PhotonView>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update() {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2) {
            btnJugar.interactable =true;
        } else {
            btnJugar.interactable = false;
        }
    }

    public void verVentanaCrear() {
        panelElegir.SetActive(false);
        panelCrear.SetActive(true);
        esNuevaPartida = true;
    }

    public void verVentanaUnirse() {
        panelElegir.SetActive(false);
        panelUnirse.SetActive(true);
    }

    public void verVentanaEspera() {
        panelCrear.SetActive(false);
        panelUnirse.SetActive(false);
        panelCargarPartida.SetActive(false);
        panelEspera.SetActive(true);
    }

    public void verVentanaContinuar() {
        panelElegir.SetActive(false);
        panelCargarPartida.SetActive(true);
        esNuevaPartida = false;
    }

    public void volverPanelUnirse() {
        panelEspera.SetActive(false);
        panelUnirse.SetActive(true);
    }

    public void volverPanelCrear() {
        panelEspera.SetActive(false);
        panelCrear.SetActive(true);
    }

    public void salirEspera() {
        PhotonNetwork.LeaveRoom();
        if (PhotonNetwork.IsMasterClient) {
            volverPanelElegir();
        } else {
            volverPanelUnirse();
        }
    }

    public void volverPanelElegir() {
        panelCrear.SetActive(false);
        panelUnirse.SetActive(false);
        panelCargarPartida.SetActive(false);
        panelEspera.SetActive(false);
        panelElegir.SetActive(true);
    }

    public void volverMenu() {
        //PhotonNetwork.LeaveRoom();
        //PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
    }

    public void cargarPartida(string idRoom) {
        RoomOptions options = new RoomOptions { MaxPlayers = 2 };
        idPartida = idRoom;
        PhotonNetwork.CreateRoom(idRoom, options);
    }
    
    public async void crearRoom() {
        //Creamos una 'room'
        esMaster=true;
        RoomOptions options = new RoomOptions { MaxPlayers = 2 };
        string code = await bbdd.GenerarCode();
        idPartida = code; //almacenamos el valor en local
        nombrePartida = crearField.text;
        Debug.Log(code);
        Debug.Log(nombrePartida);
        PhotonNetwork.CreateRoom(code, options);
        
        crearField.text = "";
    }

    public void unirseRoom() {
        //Nos unimos a una 'room'
        esMaster=false;
        PhotonNetwork.JoinRoom(unirseField.text);
        unirseField.text = "";
    }

    public async void jugar() {
        Debug.Log("clic jugar");
        //Empezamos el juego
        esMaster=false;

        //antes de cambiar la escena, guardamos la partida en la bbdd
        //solo si la partida no existia antes
        if (esNuevaPartida) {
            bbdd.guardarPartidaEnBBDD(idPartida, nombrePartida);
            gameManager.Escena = escena;
            view.RPC("activarOpJuegos", RpcTarget.All);
            PhotonNetwork.LoadLevel(gameManager.Escena);
        }
        else {
            view.RPC("cargarDatos", RpcTarget.Others, idPartida, gameManager.IdUser);
            bbdd.OnDataLoaded += HandleDataLoaded;
            await bbdd.cargarDatos(idPartida, gameManager.IdUser);
        }
        
    }

    private void HandleDataLoaded() {
        view.RPC("activarOpJuegos", RpcTarget.All);
        if (PhotonNetwork.IsMasterClient) {
            bbdd.OnDataLoaded -= HandleDataLoaded;
            PhotonNetwork.LoadLevel(gameManager.Escena);
        }
        
    }

    [PunRPC]
    public void cargarDatos(string idPartidaMain, string idUser) {
        Debug.Log("llamando con photon");
        StartCoroutine(CargarDatosCoroutine(idPartidaMain, idUser));
    }

    [PunRPC]
    void activarOpJuegos() {
        opJuego.SetActive(true);
    }

    private IEnumerator CargarDatosCoroutine(string idPartidaMain, string idUser) {
        var task = bbdd.cargarDatos(idPartidaMain, idUser);
        while (!task.IsCompleted) {
            yield return null;
        }

        if (task.Exception != null) {
            Debug.LogError("Error cargando datos: " + task.Exception);
        } else {
            Debug.Log("task cargar datos null");
        }
    }

    public override void OnJoinedRoom()
    {
        gameManager.IdPartida = idPartida;

        Debug.Log("se unio a la room");
        verVentanaEspera();
        PlayerPrefs.SetInt("esMaster", true ? 1 : 0); 
        if (PhotonNetwork.IsMasterClient) {
            objJugar.SetActive(true);
            objTexto.SetActive(false);
            objTextoCode.SetActive(true);
            objTextoCode.GetComponent<TextMeshProUGUI>().text = "Código:\n" + idPartida;
        } else {
            objJugar.SetActive(false);
            objTexto.SetActive(true);
            objTextoCode.SetActive(false);
        }
    }

    public override void OnDisconnected(DisconnectCause cause) {
        Debug.Log("Motivo de desconexion: " + cause);

        switch (cause)
        {
            case DisconnectCause.DisconnectByClientLogic:
                Debug.Log("desconectado por cliente");
                break;

            default:
                Debug.Log("Desoncexion por un error (ver causas)...");
                break;
        }

        SceneManager.LoadScene("Menu");
    }
}
