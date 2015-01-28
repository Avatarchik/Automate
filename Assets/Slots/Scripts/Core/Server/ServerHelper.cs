using Core.Server;
using Core.Server.Handlers;
using Core.Server.Request;
using Core.Server.Response;
using System;
using System.Linq;
using UnityEngine;

namespace UnitySlot {

    public static class ServerHelper {

        private static BaseHandler GetFreeGameHandler (FreeGameRequest spinRequest) {
            return new FreeGameHandler (spinRequest).AddOkListener ((response) => Loom.DispatchToMainThread (() => {
                GameState.FreeGame.Balance = response.Balance;
                GameState.FreeGame.IsDone = true;

            })).AddErrorListener ((exception) => {
                //TODO need open popup
            });
        }

        private static BaseHandler GetFreeGameFruitHandler (FreeGameRequest spinRequest) {
            return new FruitFreegameHandler (spinRequest).AddOkListener ((response) => Loom.DispatchToMainThread (() => {
                GameState.FreeGame.Balance = response.Balance;
                GameState.FreeGame.Lives = response.Lives;
                GameState.FreeGame.MidSymbols = response.MidSymbols;
                GameState.FreeGame.NewPosition = response.NewPosition;
                GameState.FreeGame.Score = response.Score;
                GameState.FreeGame.TotalWinScore += response.Score;
                GameState.FreeGame.IsDone = true;

            })).AddErrorListener ((exception) => {
                //TODO need open popup
            });
        }

        public static void Spin(Action<SpinResponse> callback, Action errorCallback) {
            var spinRequest = new SpinRequest ();
            var handler = new SpinHandler (spinRequest).AddOkListener ((response) => Loom.DispatchToMainThread (() => {

                GameState.CurrentGame.WinLines = response.WinLines;
                GameState.CurrentGame.WinLinesLength = response.WinLinesLength;
                GameState.CurrentGame.ReelSymbols = response.Symbols;
                GameState.CurrentGame.IsDone = true;
                GameState.CurrentGame.DealerDoubleCard = response.CardForDouble;

                if (response.WinLines != null && response.WinLines.Count > 0) {
                    for (var i = 0; i < response.WinLines.Count; i++) {
                        GameState.CurrentGame.LinesScore.Add(response.WinLines[i], response.Scores[i]);
                        GameState.CurrentGame.TotalWinScore += response.Scores[i];
                    }
                }

                if (GameState.CurrentGame.StartSuperGame) {
                    GameState.FreeGame.Clear();
                    GameState.FreeGame.Lives = GameState.CurrentGame.ReelSymbols.Count(i => i == 9) - 2;
                }

                User.Level = response.Level;
                User.Experience = response.Exp;
                User.MinExperience = response.MinExperience;
                User.MaxExperience = response.MaxExperience;
                User.CurrentStatus = response.Status;
                User.Coins = response.Balance;

                if (callback != null) {
                    callback(response);
                }


            }));
            handler.AddErrorListener ((exception) => {
                //TODO логика ошибок
//                GameState.CurrentGame.IsDone = true;

            });
            handler.DoRequest ();
        }

        public static void FreeSpin () {
            GameState.FreeGame.IsDone = false;

            var spinRequest = new FreeGameRequest ();

            if (GamePrefs.GetString (Constants.SettingsSelectedSlot) == "fruit") {
                GetFreeGameFruitHandler (spinRequest).DoRequest ();
            } else {
                GetFreeGameHandler (spinRequest).DoRequest ();

            }
        }

        public static void DoubleFiveCardsGameRequest (int cardnum,  Action callback) {

            var lastScore = GameState.CurrentGame.TotalWinScore;
            var oldCardIndex = GameState.CurrentGame.DealerDoubleCard;

            GameState.CurrentGame.Clear ();

            var doubleRequest = new CardsDoubleRequest (cardnum);
            var handler = new FiveCardsDoubleHandler (doubleRequest).AddOkListener ((response) => Loom.DispatchToMainThread (() => {

                var selectedCardValue = CardDescriptor.FromIndex (GameState.CurrentGame.DoubleCards [cardnum]).IntValue;
                var dealerCardValue = CardDescriptor.FromIndex (oldCardIndex).IntValue;
                var forward = selectedCardValue == dealerCardValue;

                GameState.CurrentGame.IsDoubleWin = response.IsWin;
                GameState.CurrentGame.IsDoubleForward = forward;
                GameState.CurrentGame.DoubleSelectedCardIndex = cardnum;

                if (response.IsWin) {
                    GameState.CurrentGame.TotalWinScore = lastScore * 2;
                    if (forward)
                        GameState.CurrentGame.TotalWinScore = lastScore;
                } else {
                    GameState.CurrentGame.TotalWinScore = 0;
                }

                if (callback != null) {
                    callback();
                }

            }));


            handler.AddErrorListener ((exception) => {
                //TODO логика ошибок
                GameState.CurrentGame.IsDone = true;
            });

            handler.DoRequest ();
        }

        public static void SafeBonusGameRequest (Action callback) {
            GameState.BonusGame.Clear ();
            var handler = new BonusSafeHandler (new BonusSafeRequest()).AddOkListener ((response) => Loom.DispatchToMainThread (() => {
                GameState.BonusGame.Coef = response.Coef;
                GameState.BonusGame.Lives = response.Lives;
                GameState.BonusGame.Score = response.Score;
                GameState.BonusGame.StartDoorGame = response.StartDoor == 1;
                GameState.BonusGame.Item = response.Item;
                GameState.BonusGame.Dice = response.Dice;
                GameState.BonusGame.TotalWinScore += response.Score;

                if (callback != null) {
                    callback();
                }
            }));

            handler.AddErrorListener ((exception) => {
                //TODO логика ошибок
                GameState.CurrentGame.IsDone = true;
            });

            handler.DoRequest ();
        }

        public static void DoorBonusGameRequest (Action callback) {

           
            GameState.BonusGame.Clear ();
            var handler = new BonusDoorHandler (new BonusDoorRequest()).AddOkListener ((response) => Loom.DispatchToMainThread (() => {
                GameState.BonusGame.Coef = response.Coef;
                GameState.BonusGame.Score = response.Score;
                GameState.BonusGame.TotalWinScore += response.Score;

                if (callback != null) {
                    callback();
                }
            }));

            handler.AddErrorListener ((exception) => {
                //TODO логика ошибок
                GameState.CurrentGame.IsDone = true;
            });
           
            handler.DoRequest ();
            Debug.LogError("DoorBonus");
        }
    }
}