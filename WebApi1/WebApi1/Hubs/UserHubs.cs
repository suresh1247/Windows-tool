using Microsoft.AspNetCore.SignalR;

public class UserHubs:Hub
{
   public async Task JoinChat(UserConnection user){
    Clients.All.SendAsync("joinchat",user.Name+" Has joined the chat");
   }
}