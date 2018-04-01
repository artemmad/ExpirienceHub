using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using VRTeleportator.Models;


namespace VRTeleportator.Controllers
{
    [Produces("application/json")]
    [Route("api/File")]
    public class FileController : Controller
    {
        private readonly IHostingEnvironment environment;
        private readonly ApplicationContext context;

        public FileController(IHostingEnvironment environment, ApplicationContext context)
        {
            this.environment = environment;
            this.context = context;
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadFile(IFormFile uploadedFile)
        {
            string path = Path.Combine("File", uploadedFile.FileName);

            using (var fileStream = new FileStream(Path.Combine(environment.WebRootPath, path), FileMode.Create))
            {
                await uploadedFile.CopyToAsync(fileStream);
            }

            FileModel file = new FileModel
            {
                FileName = uploadedFile.FileName,
                FilePath = path
            };

            await context.Files.AddAsync(file);
            await context.SaveChangesAsync();

            ZipFile.ExtractToDirectory(Path.Combine(environment.WebRootPath, file.FilePath),
                Path.Combine(environment.WebRootPath, "Extracts"));
            return Ok();
        }

        [HttpGet]
        [Route("getinfo")]

        public IActionResult GetNumber()
        {
            return Json(new DirectoryInfo(Path.Combine(environment.WebRootPath, "Extracts")).GetFiles().Length.ToString());
        }

    }
}