using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public static class HistoryManager
    {
        private const string FilePath = "data/game_history.json";

        public static List<GameRecord> LoadHistory()
        {
            if (!File.Exists(FilePath))
            {
                File.WriteAllText(FilePath, "[]");
                return new List<GameRecord>();
            }

            try
            {
                string json = File.ReadAllText(FilePath);
                return JsonConvert.DeserializeObject<List<GameRecord>>(json)
                       ?? new List<GameRecord>();
            }
            catch
            {
                return new List<GameRecord>();
            }
        }

        public static void SaveHistory(List<GameRecord> history)
        {
            string json = JsonConvert.SerializeObject(history, Formatting.Indented);

            File.WriteAllText(FilePath, json);
        }

        public static void SaveGame(Game finishedGame)
        {
            GameRecord record = new GameRecord
            {
                Date = DateTime.Now,
                Winner = finishedGame.Winner,
                Reason = finishedGame.GameOverReason
            };

            foreach (Move move in finishedGame.MoveHistory)
            {
                record.Moves.Add(new MoveRecord
                {
                    From = move.FromPos,
                    To = move.ToPos
                });
            }

            List<GameRecord> history = LoadHistory();
            history.Add(record);

            string json = JsonConvert.SerializeObject(history, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }
    }
}
