using Stas.Utils;
using System.Diagnostics;
namespace Stas.GA.Updater;

public partial class ui {
    //https://github.com/secure-remote-password/srp.net
    public static void Login() {
        // a user enters his name and password
        var userName = "alice";
        var password = "password123";

        var client = new SrpClient();
        var salt = client.GenerateSalt();
        var privateKey = client.DerivePrivateKey(salt, userName, password);
        var verifier = client.DeriveVerifier(privateKey);
        var clientEphemeral = client.GenerateEphemeral();

    }
}
