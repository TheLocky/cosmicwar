using CWServer.DBConnection;
using CWServer.Models;
using Nancy;
using Nancy.Security;

namespace CWServer.NancyModules {
    public class LobbyModule : NancyModule {
        public LobbyModule() {
            this.RequiresAuthentication();

            Get["/lobby"] = args => {
                var userLobby = ((User) Context.CurrentUser).GetCurrentLobby();
                if (userLobby == null)
                    return View["Lobby/lobbyList.cshtml", new {LobbyList = LobbyDatabase.GetLobbies()}];
                return Response.AsRedirect($"/lobby/{userLobby.Id}");
            };

            Post["/lobby"] = args => {
                if (Request.Form.Create) {
                    var lobby = LobbyDatabase.CreateLobby((User) Context.CurrentUser);
                    return Response.AsRedirect($"/lobby/{lobby.Id}");
                }
                if (Request.Form.Join) {
                    var lobby = LobbyDatabase.GetLobby((int) Request.Form.Join);
                    if (lobby == null) return Response.AsRedirect("/lobby");
                    lobby.AddUser((User) Context.CurrentUser);
                    return Response.AsRedirect($"/lobby/{lobby.Id}");
                }
                return Response.AsRedirect("/lobby");
            };

            Get["/lobby/{id}"] = args => {
                var lobby = LobbyDatabase.GetLobby((int) args.id);
                if ((lobby == null) || !lobby.Players.Contains((User) Context.CurrentUser))
                    return Response.AsRedirect("/lobby");
                return View["Lobby/lobbyView.cshtml", new {Lobby = lobby}];
            };

            Post["/lobby/{id}"] = args => {
                var lobby = LobbyDatabase.GetLobby((int) args.id);
                if (lobby == null) return Response.AsRedirect("/lobby");
                if (Context.CurrentUser == lobby.Owner) {
                    if (Request.Form.Kick) {
                        var user = UserDatabase.GetUser((int) Request.Form.Kick);
                        if (user != null) lobby.RemoveUser(user);
                    }
                    if (Request.Form.Start) lobby.Start();
                    if (Request.Form.Stop) {
                        lobby.RemoveUser(lobby.Owner);
                        return Response.AsRedirect("/lobby");
                    }
                    return Response.AsRedirect($"/lobby/{args.id}");
                }
                if (Request.Form.Leave) lobby.RemoveUser((User) Context.CurrentUser);
                return Response.AsRedirect("/lobby");
            };
        }
    }
}
