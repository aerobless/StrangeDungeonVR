using SixtyMeters.logic.item;
using UnityEngine;

namespace SixtyMeters.logic.player
{
    public class CoinAttractor : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnTriggerStay(Collider other)
        {
            Coin coin = other.GetComponent<Coin>();
            if (coin && coin.isCollectible)
            {
                other.transform.position =
                    Vector3.Lerp(other.transform.position, transform.position + new Vector3(0, 2, 0),
                        Time.deltaTime * 3);

                if (Vector3.Distance(transform.position, other.transform.position) < 0.3f)
                {
                    coin.Deposit();
                    //TODO: count player coins in level manager or somehwere
                }
            }
        }
    }
}