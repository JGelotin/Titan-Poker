// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;

// public class RoundManager : MonoBehaviour
// {
//     public static RoundManager instance;

//     private static Sprite unflippedCard;
//     public SpriteRenderer playerCard1;
//     public SpriteRenderer playerCard2;
//     public SpriteRenderer opponentCard1;
//     public SpriteRenderer opponentCard2;
//     public SpriteRenderer communityCard1;
//     public SpriteRenderer communityCard2;
//     public SpriteRenderer communityCard3;
//     public SpriteRenderer communityCard4;
//     public SpriteRenderer communityCard5;

//     private static List<Card> communityCards;
//     private static Round round;
//     private static Blind blind;
//     private static int gamesPassed;

//     public static List<Card> CommunityCards { get { return communityCards; } }
//     public static Round Round { get { return round; } }
//     public static Blind Blind { get { return blind; } }

//     /*************************************************/
//     /*                UNITY FUNCTIONS                */
//     /*************************************************/
//     void Start()
//     {
//         MakeSingleton();
//         unflippedCard = Resources.Load<Sprite>("Sprites/Cards/back");
//         communityCards = new List<Card>();
//         round = Round.PREFLOP;
//         blind = new Blind();
//         gamesPassed = 0;

//         Debug.Log("ROUND MANAGER CALLED");
//     }
//     private void MakeSingleton()
//     {
//         if(instance != null)
//         {
//             Destroy(gameObject);
//         }
//         else
//         {
//             instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//     }
//     void Update() 
//     {
//         if(EntityManager.AreEntitiesReady)
//         {
            
//         }
//     }
// }
