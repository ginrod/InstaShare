using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;

namespace InstaShare.Client.Pages
{
    [Authorize] // ensure a signed-in user
    [AuthorizeForScopes(Scopes = new[] { "api://2eb56b0f-2d68-41d1-a8da-3714a7540e49/.default" })]
    public class FilesModel : PageModel
    {
        public string AccessToken { get; private set; } = "";

        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public FilesModel(ITokenAcquisition tokenAcquisition, ILogger<FilesModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _tokenAcquisition = tokenAcquisition;
            _configuration = configuration;
        }

        public async Task OnGet()
        {
            _logger.LogInformation("FilesModel OnGet called");

            if (User?.Identity is not null && User.Identity.IsAuthenticated) {
                AccessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(
                    [_configuration["AzureAdApi"] ?? ""]
                );
            }

            _logger.LogDebug("Access Token: {token}", AccessToken);

        }
    }
}
