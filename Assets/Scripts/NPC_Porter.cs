using UnityEngine;
using UnityEngine.AI;

public class NPC_Porter : MonoBehaviour
{
    private int _id;
    [SerializeField]private int _quantityProduct;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private NavMeshAgent _agent;
    public bool done;    
    public GameObject Bag;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.SetDestination(_endPosition);
    }

    public int GetPorterId()
    {
        return _id;
    }

    public int GetQuantityProduct()
    {
        return _quantityProduct;
    }

    public void NPC_Data(int id, int quantityProduct, Vector3 startPosition, Vector3 endPosition)
    {
        _id = id;
        _quantityProduct = quantityProduct;
        _startPosition = startPosition;
        _endPosition = endPosition;
    }

    public void GoBack()
    {
        _agent.SetDestination(_startPosition);
        Bag.SetActive(false);
    }
}
