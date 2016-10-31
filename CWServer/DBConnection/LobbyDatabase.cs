using System.Collections.Generic;
using System.Linq;
using CWServer.Models;
using Microsoft.Data.Entity;

namespace CWServer.DBConnection {
    public static class LobbyDatabase {
        public static Lobby CreateLobby(User owner, Lobby.State status = Lobby.State.Wait) {
            var lobby = new Lobby {Owner = owner, Status = status};
            lobby.Players.Add(owner);

            DB.Context.Lobbies.Add(lobby);
            DB.Context.SaveChanges();

            return lobby;
        }

        public static Lobby GetCurrentLobby(this User user) {
            var lobby = DB.Context.Lobbies.Include(l => l.Players).Where(l => l.Status != Lobby.State.Inactive).ToList();
            return lobby.SingleOrDefault(l => l.Players.Contains(user));
        }

        public static List<Lobby> GetLobbies() {
            return
                DB.Context.Lobbies.Include(l => l.Owner)
                    .Include(l => l.Players)
                    .Where(l => l.Status != Lobby.State.Inactive).ToList();
        }

        public static void AddUser(this Lobby lobby, User user) {
            if (lobby.Players.Contains(user)) return;
            lobby.Players.Add(user);
            DB.Context.Lobbies.Update(lobby);
            DB.Context.SaveChanges();
        }

        public static void RemoveUser(this Lobby lobby, User user) {
            if (!lobby.Players.Contains(user)) return;
            lobby.Players.Remove(user);
            if ((lobby.Players.Count == 0) || (lobby.Owner == user))
                lobby.Status = Lobby.State.Inactive;
            DB.Context.Lobbies.Update(lobby);
            DB.Context.SaveChanges();
        }

        public static void Start(this Lobby lobby) {
            lobby.Status = Lobby.State.Active;
            DB.Context.Lobbies.Update(lobby);
            DB.Context.SaveChanges();
        }

        public static Lobby GetLobby(int id) {
            return DB.Context.Lobbies.Include(l => l.Owner).Include(l => l.Players).SingleOrDefault(l => l.Id == id);
        }
    }
}
