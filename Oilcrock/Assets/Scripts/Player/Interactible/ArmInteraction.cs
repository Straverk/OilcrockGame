using GameSettings;
using UnityEngine;
using DG.Tweening;

namespace Player.Modules.Interactible
{
    public class ArmInteraction : MonoBehaviour
    {
        //[SerializeField] private CharacterJoint _armJoint;
        [SerializeField] private Transform _armTransform;

        private void OnValidate()
        {
            //_armJoint ??= GetComponent<CharacterJoint>();
            _armTransform ??= transform;
        }

        private PlayerConfig _playerConfig;

        private ITakeable _itemTakeable;
        private Component _itemComponent;

        public void Init(PlayerConfig playerCharacteristics) =>
            _playerConfig = playerCharacteristics;

        public void DropItem()
        {
            if (_itemTakeable == null)
                return;


            //_armJoint.connectedBody = null;
            _itemComponent.transform.SetParent(null, true);

            var rb = _itemComponent.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(_armTransform.parent.forward * _playerConfig.DropItemForce * rb.mass);

            _itemComponent = null;
            _itemTakeable = null;

            _armTransform.gameObject.SetActive(false);
        }

        public void TakeItem(Component itemComponent, ITakeable takeItem)
        {
            if (_itemComponent != null)
            {
                if (takeItem == _itemTakeable)
                    return;

                DropItem();
            }

            _itemTakeable = takeItem;
            _itemComponent = itemComponent;

            itemComponent.transform.SetParent(_armTransform, true);
            _armTransform.gameObject.SetActive(true);

            itemComponent.transform.DOLocalMove(Vector3.zero, _playerConfig.TakeTime);
            itemComponent.transform.DOLocalRotate(Vector3.zero, _playerConfig.TakeTime);
            //.OnComplete(() => _armJoint.connectedBody = itemComponent.GetComponent<Rigidbody>());

            itemComponent.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
