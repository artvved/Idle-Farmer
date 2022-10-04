using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CoinsCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinsCountText;
        [SerializeField] private CoinsReceiver coinsReceiver;
        [SerializeField] private Camera camera;
        [SerializeField] private LayerMask layerMask;

        private Animator animator;

        private bool isEnabledToMove;
        private Coroutine waitForDisable;

        private int coinsCount;
        private float oneCoinValue;
        

        private void Start()
        {
            coinsCountText.text = PlayerPrefs.GetInt("Coins").ToString();
            coinsCount = PlayerPrefs.GetInt("Coins");
            animator = GetComponent<Animator>();
            coinsReceiver.CoinReceiveEvent+=AnimateOneCoinIncrement;
        }

        private void LateUpdate()
        {
            if (isEnabledToMove)
            {
                var ray = camera.ScreenPointToRay(transform.position);
                RaycastHit hit;


                if (Physics.Raycast(ray, out hit, 10000f, layerMask))
                {
                    MoveReceiver(hit.point);
                }
            }
        }

        public void EnableReceiver()
        {
            isEnabledToMove = true;
            coinsReceiver.gameObject.SetActive(true);
            waitForDisable = StartCoroutine(WaitForDisable());
        }

        private IEnumerator WaitForDisable()
        {
            yield return new WaitForSeconds(4f);
            isEnabledToMove = false;
            coinsReceiver.gameObject.SetActive(false);
        }

        private void MoveReceiver(Vector3 pos)
        {
            var tr = coinsReceiver.transform;
            tr.position = pos;
        }

        public void SetVisualCoinValue(float oneCoinValue)
        {
            this.oneCoinValue = oneCoinValue;
        }

        private void AnimateOneCoinIncrement()
        {
            animator.SetBool("Vibrate",true);
            coinsCount += (int)oneCoinValue;
            coinsCountText.text = coinsCount.ToString();
        }

        public void StopVibrating()
        {
            animator.SetBool("Vibrate",false);
        }
    }
}