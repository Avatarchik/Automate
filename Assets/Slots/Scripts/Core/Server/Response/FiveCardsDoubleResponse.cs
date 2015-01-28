using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnitySlot;

namespace Core.Server.Response {
    public sealed class FiveCardsDoubleResponse : RedBlackDoubleResponse {

        public FiveCardsDoubleResponse (JObject o) : base(o) {
            var cards = JsonUtil.JarrayToList<int> (o, "cards");
            var dealerCard = (int)JsonUtil.GetInt(o, "nextDealerCard");
            var resultCards = new int[5];
            if (SessionData.Instance.IsFun) {
                resultCards[0] = dealerCard;
                var index = 1;
                foreach (int card in cards) {
                    resultCards[index++] = card;
                }
            } else {
                resultCards = cards.ToArray();
            }


            GameState.CurrentGame.DoubleCards = resultCards;
            GameState.CurrentGame.IsDone = true;
            GameState.CurrentGame.DealerDoubleCard = resultCards[0];
        }
    }
}
