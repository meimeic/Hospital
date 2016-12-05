using System.ComponentModel.DataAnnotations;

namespace WebFramework.Models.AccountViewModels
{
    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }

        [Display(Name = "记住此浏览器?")]
        public bool RememberBrowser { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
    }
}
