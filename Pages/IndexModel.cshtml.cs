using DogNode.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DogNode.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProcessInput _inputProcessor;

        public IndexModel(IProcessInput inputProcessor) {
            _inputProcessor = inputProcessor;
            Result = new ProcessResult("");
            DefaultValue = "...";

            Random rand = new Random();
            int index = rand.Next(1,7);
            BackgroundImageUrl =$"/images/img{index}.jpg";
        }

        [BindProperty]
        public string InputText { get; set; }

        public string DefaultValue { get; set; }

        public ProcessResult Result { get; set; }

        public string BackgroundImageUrl { get; private set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string selectedOption)
        {
            if (string.IsNullOrEmpty(InputText))
            {
                // Handle empty input case
                ModelState.AddModelError(string.Empty, "Text input cannot be empty.");
                return Page();
            }

            NodeAction nodeAction = selectedOption switch
            {
                "sound" => NodeAction.SOUND,
                "buzz" => NodeAction.BUZZ,
                _ => throw new Exception("Invalid action type")
            };

            Result = await _inputProcessor.Process(InputText, nodeAction);
            DefaultValue = InputText;

            return Page();
        }

    }
}
