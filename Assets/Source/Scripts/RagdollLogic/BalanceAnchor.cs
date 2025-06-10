using UnityEngine;

namespace Source.Scripts.RagdollLogic
{
    public class BalanceAnchor : MonoBehaviour
    {
        [SerializeField] private float _breakForce = 500f;
        [SerializeField] private Rigidbody _mainBody;
    
        private FixedJoint _supportJoint;
    
        void Start()
        {
            GameObject balanceAnchor = new GameObject("BalanceAnchor");
            balanceAnchor.transform.position = transform.position;
            Rigidbody anchorRb = balanceAnchor.AddComponent<Rigidbody>();
            anchorRb.isKinematic = true;
        
            _supportJoint = _mainBody.gameObject.AddComponent<FixedJoint>();
            _supportJoint.connectedBody = anchorRb;
            _supportJoint.breakForce = _breakForce;
            _supportJoint.breakTorque = _breakForce;
        }
    }
}