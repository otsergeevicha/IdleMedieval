using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Warrior : MonoBehaviour
{
    public Transform endPos;
    public int TotalWarriors;
    public Transform TextCountTransform;
    private TextMeshPro _textCout;
    private NavMeshAgent _agent;

    private void Start()
    {
        _textCout = TextCountTransform.gameObject.GetComponent<TextMeshPro>();
        _agent = GetComponent<NavMeshAgent>();
        _textCout.text = TotalWarriors.ToString();
        _agent.SetDestination(endPos.position);
    }

    private void Update()
    {
        TextCountTransform.rotation = Quaternion.Euler(55, -transform.rotation.y+10, -10);
    }
}
