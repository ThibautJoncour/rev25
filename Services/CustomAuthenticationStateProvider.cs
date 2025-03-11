using System.Security.Claims; //Gestion des identités et rôles des utilisateurs.
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization; ////Fournit un mécanisme d'authentification pour Blazor. """"AuthenticationState"""
using Microsoft.JSInterop; // CALL JS

//📌 Caractéristiques de LocalStorage
//✔ Stockage persistant → Les données restent même après un rechargement de la page ou la fermeture du navigateur.
public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _jsRuntime; // LocalStorage stock les data dans le navigateur en JS
    private ClaimsPrincipal _currentUser = new(new ClaimsIdentity()); // stock l'user actuellement authentifié
    private bool _isInitialized = false; //check l'user est initalisé

    private readonly Dictionary<string, string> _users = new()
    {
        { "TJONCOUR", "Joncour" },
        { "TGOMEZ", "TomasGV" },
        { "JREADY", "ready" }
    };


    private readonly Dictionary<string, List<string>> _userEntities = new()
    {
        { "TJONCOUR", new List<string>{"CFS"} },
        { "JREADY", new List<string>{"SYGMA UK"} },
    };



    public CustomAuthenticationStateProvider(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (!_isInitialized) //si false
        {
            await LoadUserFromStorage();  // Charger l'utilisateur après le premier rendu // Lance la methode du dessous
        }

        return new AuthenticationState(_currentUser);
    }

    private async Task LoadUserFromStorage()
    {
        try
        {
            var username = await _jsRuntime.InvokeAsync<string>("blazorLocalStorage.getItem", "username");

            if (!string.IsNullOrEmpty(username) && _users.ContainsKey(username))
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
            };

                if (_userEntities.TryGetValue(username, out var entities))
                {
                    foreach (var entity in entities)
                    {
                        Console.WriteLine($"Restauration du claim Entity: {entity} pour {username}");
                        claims.Add(new Claim("Entity", entity));
                    }
                }
                else
                {
                    Console.WriteLine($"Aucune entité restaurée pour {username}");
                }

                var identity = new ClaimsIdentity(claims, "apiauth");
                _currentUser = new ClaimsPrincipal(identity);
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
            }
        }
        catch
        {
            // Erreur silencieuse si LocalStorage est vide
        }

        _isInitialized = true;
    }


    public async Task<bool> Login(string username, string password)
    {
        if (_users.TryGetValue(username, out var storedPassword) && storedPassword == password)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username)
        };

            // 🔹 Vérifier si des entités sont bien ajoutées
            if (_userEntities.TryGetValue(username, out var entities))
            {
                foreach (var entity in entities)
                {
                    Console.WriteLine($"Ajout du claim Entity: {entity} pour {username}");
                    claims.Add(new Claim("Entity", entity));
                }
            }
            else
            {
                Console.WriteLine($"Aucune entité trouvée pour {username}");
            }

            var identity = new ClaimsIdentity(claims, "apiauth");
            _currentUser = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));

            // Stocker l'utilisateur dans LocalStorage
            await _jsRuntime.InvokeVoidAsync("blazorLocalStorage.setItem", "username", username);
            return true;
        }

        return false;
    }

    public async Task Logout()
    {
        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));

        // Supprimer l'utilisateur du LocalStorage après le rendu
        await _jsRuntime.InvokeVoidAsync("blazorLocalStorage.removeItem", "username");
    }
}
