using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public struct JSONReturn
{
    public Token[] tokens;
    public string algebra;
}

[Serializable]
public struct Token
{
    public string sqlWordType;
    public string content;
}

[Serializable]
public struct Error
{
    public string error;
}

public class StartGraph : MonoBehaviour
{
    public enum STATUS { Enable, Writing, Finished };
    public STATUS status;
    float listener = 0f;
    public TMP_InputField URLField, queryField;


    public GameObject graphPrefab;
    public GameObject StartNode;
    public TextMeshProUGUI Debugger;
    public static StartGraph instance;

    public string url = "";

    private void Start()
    {
        instance = this;
    }
    public void Update()
    {
        //string query = "Pi cliente.nome, pedido.idPedido, pedido.DataPedido, Status.descricao, pedido.ValorTotalPedido, produto.QuantEstoque ((((Pi Cliente_idCliente, ValorTotalPedido, DataPedido, idPedido, Pedido.status_idstatus (Sigma pedido.ValorTotalPedido = 0 (pedido))) |X| Status.idstatus = Pedido.status_idstatus (Pi idstatus, descricao (Sigma Status.descricao = 'Aberto' (Status)))) |X| pedido.idPedido = pedido_has_produto.Pedido_idPedido (pedido_has_produto)) |X| produto.idProduto = pedido_has_produto.Produto_idProduto (Pi idProduto, QuantEstoque (Sigma produto.QuantEstoque > 0 (produto))))";

        if (Input.GetKeyDown(KeyCode.P))
            Request(queryField.text, 
                (string s) => 
                { //Successs

                    var jsonObject = JsonUtility.FromJson<JSONReturn>(s);
                    print(jsonObject.algebra);
                    //print(jsonObject.tokens.Length);
                    var parentTr = transform.parent.GetComponent<RectTransform>();
                    //var i = Instantiate(graphPrefab, new Vector3(parentTr.position.x + parentTr.sizeDelta.x / 2, parentTr.position.y + parentTr.sizeDelta.y / 2, 0), Quaternion.identity, transform.parent);

                    var graph = StartNode.GetComponentInChildren<Root>();
                    graph.txt = jsonObject.algebra;
                    graph.isRoot = true;
                    graph.enabled = true;

                }, 
                (string s) => 
                { // Fail
                    print(s);
                    var jsonObject = JsonUtility.FromJson<Error>(s);
                    print(jsonObject.error);
                    Debugger.text = jsonObject.error;
                    Debugger.color = Color.red;
                });


        if (Input.GetKeyDown(KeyCode.T))
        {
            var graph = StartNode.GetComponent<Root>();
            graph.txt = "Pi cliente.nome, pedido.idPedido, pedido.DataPedido, pedido.ValorTotalPedido ((Pi cliente.TipoCliente_idTipoCliente, cliente.nome, cliente.idcliente (Sigma cliente.TipoCliente_idTipoCliente = 1 (Cliente))) |X| cliente.idcliente = pedido.Cliente_idCliente (Pi DataPedido, Cliente_idCliente, idPedido, ValorTotalPedido (Sigma pedido.ValorTotalPedido = 0 (pedido))))";
            graph.isRoot = true;
            graph.enabled = true;
        }

        if(status == STATUS.Writing)
        {
            listener -= Time.deltaTime;
            if(listener <= 0) 
            {
                status = STATUS.Finished;
                //Codar para terminar e validar
                StartNode.GetComponent<Root>().GetOrderIndex(0);
            }
        }

    }

    public async void Request(string query, Action<string> onSuccess, Action<string> onFail)
    {
        if (query == "") return;
        if(URLField.text == "")
        {
            Debugger.text = "URL Inválido.";
            Debugger.color = Color.red;
            return;
        }
        Debugger.text = query;
        Debugger.color = Color.white;

        var requestBody = new
        {
            sql = query
        };

        print(requestBody.sql);
        // Converte o objeto para JSON
        string json = $"{{ \"sql\": \"{query}\"}}";//JsonUtility.ToJson(requestBody);


        print(json);
        // Cria um objeto de requisição e define o tipo de conteúdo para JSON
        UnityWebRequest request = new UnityWebRequest(URLField.text, "POST");
        byte[] jsonToSend = new UTF8Encoding().GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Aguarde até a requisição estar completa e então:
        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            //Houve um erro na requisi��o
            Debug.LogError("Erro na requisi��o: " + request.error);
            onFail(request.downloadHandler.text);
        }
        else
        {
            // Requisi��o bem-sucedida, exibindo resposta
            Debug.Log("Resposta do servidor: " + request.downloadHandler.text);
            var resposta = request.downloadHandler.text;
            onSuccess(resposta);
        }
    }

    public void ChangeRoute(TMP_InputField field)
    {
        this.url = field.text;
    }

    public void StillCreating()
    {
        this.status = STATUS.Writing;
        listener = 2f;
    }
}
    

  

