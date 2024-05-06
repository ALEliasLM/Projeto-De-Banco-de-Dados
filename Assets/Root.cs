using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class Root : MonoBehaviour
{
    private const int Space = 150;
    public TextMeshProUGUI text, OrderText;
    public GameObject childPrefab;
    public bool isRoot = false;
    public List<Root> child;
    public string txt;
    public GameObject Pannel;
    public int OrderIndex = -1;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        text.gameObject.SetActive(false);
        OrderText.gameObject.SetActive(false);
        StartGraph.instance.StillCreating();
        child = new List<Root>();
        //Fazer rota pra pegar string dps
        if (isRoot)
        {
            //txt = "Pi Tb1.Nome, tb3.sal (((((Pi Pk, nome (Sigma tb1.id > 300 (Tb1))) |X| Tb1.pk = tb2.fk (Pi Pk,fk(Tb2))) |X| tb2.pk = tb3.fk (Pi Sal, fk ((Sigma tb3.sal <> 0 (Tb3)))))))";
            var rootDivision = ReadUntilFind(txt, '(');
            text.text = rootDivision[0];
            text.gameObject.SetActive(true);
            //foreach (var s in rootDivision[1].Split(" |X| ")) { print(s); };

            //Primeiro Child, direita, somente a ultima parte deve ir para a direita!


            //Segund Child Esquerda... Juntar as demais partes com o |X| para separar novamente no futuro
            //tambem tenho que retirar os parenteses a mais do começo e fim... 

            //tentar criar a linha
            var minSpace = Space * transform.lossyScale.x;

            var finalPos = transform.position + new Vector3(0, -minSpace, 0) + new Vector3(0, 0, 0);
            yield return StartCoroutine(DrawLine(transform.position, finalPos, -90));

            /* while ((line.GetPosition(1) - finalPos).magnitude > .5f)
             {
                 line.SetPosition(1, Vector3.Slerp(line.GetPosition(1), finalPos, Time.deltaTime * 3));
                 yield return null;
             }*/

            yield return null;
            var childInstance = Instantiate(childPrefab, transform.position + new Vector3(0, -minSpace, 0), Quaternion.identity, transform.parent);
            var rightDivision = rootDivision[1].Split(" |X| ");
            var childRoot = childInstance.GetComponent<Root>();
            child.Add(childRoot);
            StartCoroutine(childRoot.SubdivideString(rootDivision[1]));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator DrawLine(Vector3 pointA, Vector3 pointB, float Zangle)
    {

        var panelInstance = Instantiate(Pannel, pointA, Quaternion.Euler(0,0,Zangle),transform.parent).GetComponent<RectTransform>();
        panelInstance.SetAsFirstSibling();
        var Tx = transform.lossyScale.x;
        var expandSpeed = 3f;
        // Calcula o vetor direção do ponto A para o ponto B
        var direction = new Vector2(Vector3.Distance(pointB, pointA), 5f) / (Tx) ;

        panelInstance.sizeDelta = new Vector2(0, 5 * Tx);

        float t = 0;
        while (true)
        {
            t += Time.deltaTime;
            var perc = Mathf.Clamp((t / .75f),0f,1f);

            if(perc >= 1)
            {
                panelInstance.sizeDelta = direction;
                yield break;
            }

            panelInstance.sizeDelta = Vector2.Lerp(panelInstance.sizeDelta, direction, perc);
            yield return null;
        }
        
    }

    public string RemoveParenthesis()
    {
        int parenthesis = 0;
        for (int i = txt.Length - 1; i >= 0; i--)
        {
            if (txt[i] == ')') parenthesis++;
            else if (txt[i] == '(') parenthesis--;
            //else if (txt[i] == 'X') parenthesis = 0;
            if (parenthesis < 0)
            {
                print(txt);

                
                while (txt[++i] == '(');
                return txt[(i)..(txt.Length - 1)];
            }
        }
        if (parenthesis > 0)
        {
            //print("Tem mais parentese do que necessario, novo texto = " + txt[..(txt.Length - 1 - parenthesis)]);
            if (txt[0] == '(')
            {
                return txt[1..(txt.Length - 1 - parenthesis)];
            }
            return txt[..(txt.Length - 1 - parenthesis)];

        }

        else {
            //print("Deu igual, entao retorna");
            if (txt[0] == '(')
            {
                return txt[1..(txt.Length - 1)];
            }
            return txt; 
        }
    }

    public string[] ReadUntilFind(string text, char stopChar)
    {
        string[] arr = new string[2];
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == stopChar)
            {
                arr[0] = text[..i];
                arr[1] = text[(i)..];
                return arr;
            }
        }
        return null;
    }

    public IEnumerator SubdivideString(string s)
    {
        var minSpace = Space * transform.lossyScale.x;
        txt = s;
        yield return StartCoroutine(Start());
        txt = RemoveParenthesis();
        //print(txt);
        ///2 possibilidades
        ///1- Ou eh a branch Direita, logo não temos que subdividir por |X| pra separar
        ///2- eh a branch esquerda, vamos subdividir e reiniciar o processo

        if (txt.Split(" |X| ").Length > 1)
        {
            //Aqui temos o caso de ser a branch Esquerda
            //print("Esquerda");

            //Mandar para a direita
            var splitted = txt.Split(" |X| ");
            //rint(splitted[splitted.Length - 1] + " Foi para a direita");
            //Separar a primeira parte e mandar o resto pra direita
            txt = splitted[splitted.Length - 1];
            var division = RemoveParenthesis();
            var nodeText = ReadUntilFind(division, '(');
            text.gameObject.SetActive(true);
            text.text = "|X| "+nodeText[0];

            yield return StartCoroutine(DrawLine(transform.position, transform.position + new Vector3(minSpace, -minSpace, 0), -45));
            var rightchildInstance = Instantiate(childPrefab, transform.position + new Vector3(minSpace, -minSpace, 0), Quaternion.identity, transform.parent);
            rightchildInstance.name = "RightChild";
            yield return new WaitForSeconds(.3f);
            var rightchildRoot = rightchildInstance.GetComponent<Root>();
            child.Add(rightchildRoot);
            StartCoroutine(rightchildRoot.SubdivideString(nodeText[1]));


            //Mandar para a esquerda
            yield return StartCoroutine(DrawLine(transform.position, transform.position + new Vector3(-minSpace, -minSpace, 0), -135));
            var leftchildInstance = Instantiate(childPrefab, transform.position + new Vector3(-minSpace, -minSpace, 0), Quaternion.identity, transform.parent);
            yield return new WaitForSeconds(.3f);
            var newString = splitted[0];
            for(int i = 1; i < splitted.Length-1; i++)
            {
                newString += " |X| "+splitted[i];
            }
            print(newString + " Foi para esquerda");
            var leftchildRoot = leftchildInstance.GetComponent<Root>();
            leftchildInstance.name = "LeftChild";
            child.Add(leftchildRoot);
            StartCoroutine(leftchildRoot.SubdivideString(newString));
            
        }
        else if(txt.IndexOf('(') == -1 && txt.IndexOf(')') == -1)
        {
            //print("caiu: "+txt);
            text.gameObject.SetActive(true);
            text.text = txt;
        }
        else
        {
            //print("Direita");
            var division = ReadUntilFind(txt, '(');
            //foreach (var d in division) print("D = " + d);
            text.gameObject.SetActive(true);
            text.text = division[0];

            yield return StartCoroutine(DrawLine(transform.position, transform.position + new Vector3(0, -minSpace, 0), -90));
            var childInstance = Instantiate(childPrefab, transform.position + new Vector3(0, -minSpace, 0), Quaternion.identity, transform.parent);
         
            yield return new WaitForSeconds(.3f);
            var childRoot = childInstance.GetComponent<Root>();
            childRoot.name = "Child";
            child.Add(childRoot);
            StartCoroutine(childRoot.SubdivideString(division[1]));
        }
    }

    public int GetOrderIndex(int current)
    {
        if (child.Count == 0)
        {
            OrderIndex = current+1;
            OrderText.text = OrderIndex.ToString();
            OrderText.gameObject.SetActive(true);
            OrderManager.Instance.AddString(OrderIndex, txt);
            return current + 1;
        }
        else
        {
            int index = current;
            for (int i = child.Count - 1; i >= 0; i--)
            {
                index = child[i].GetOrderIndex(index);
            }
            OrderIndex = index + 1;
            OrderText.text = OrderIndex.ToString();
            OrderText.gameObject.SetActive(true);
            OrderManager.Instance.AddString(OrderIndex, txt);
            return index + 1;
        }
    }
}
