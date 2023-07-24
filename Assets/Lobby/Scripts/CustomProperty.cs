using Photon.Realtime;
using PhotonHastable = ExitGames.Client.Photon.Hashtable;

public static class CustomProperty
{
    public static bool GetReady(this Player player)
    {
        PhotonHastable property = player.CustomProperties;

        if (property.ContainsKey("Ready"))
            return (bool)property["Ready"];
        else
            return false;
    }

    public static void SetReady(this Player player, bool ready)
    {
        PhotonHastable property = player.CustomProperties;

        property["Ready"] = ready;
        player.SetCustomProperties(property);
    }
}
