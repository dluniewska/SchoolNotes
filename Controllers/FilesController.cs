﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using School.Authorization;
using School.Exceptions;
using School.Models;
using School.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace School.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class FilesController : ControllerBase
    {
        private readonly ApiContext _context;
        public static IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<FilesController> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public IFormFile FormFile { get; set; }

        public FilesController(ApiContext context, IWebHostEnvironment webHostEnvironment,
            ILogger<FilesController> logger, IAuthorizationService authorizationService,
            IUserContextService userContextService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        // GET: /api
        [HttpGet(Name = "showFiles")]
        //[ResponseCache(Duration = 1200)]
        public async Task<ActionResult<IEnumerable<FileData>>> GetFiles()
        {
            try
            {
                return await _context.Files.ToListAsync();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retriving data from database {e}");
            }
        }
        // GET: /api
        [HttpGet("{id:int}", Name = "getFile")]
        public async Task<ActionResult<FileData>> GetFile(int id)
        {
            try
            {
                var file = await _context.Files.FindAsync(id);

                if (file == null)
                {
                    throw new NotFoundException("File not found");
                }
                return file;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retriving data from database");
            }
        }

        [HttpGet("downloadFile/{id:int}", Name = "downloadFile")]
        //GET
        public async Task<ActionResult<FileData>> DownloadFile(int id)
        {
            var file = await _context.Files.FindAsync(id);

            if (file == null)
            {
                throw new NotFoundException("File not found");
            }
            if (file.FileUpload == null)
            {
                return NotFound();
            }
            byte[] byteArr = file.FileUpload;
            string mimeType = "multipart/form-data";
            return new FileContentResult(byteArr, mimeType)
            {
                FileDownloadName = $"{file.fileUploadName}"
            };
        }

        [HttpPost("createFile", Name = "createFile")]
        public async Task<ActionResult<FileData>> PostFile([FromForm] FileData file)
        {
            try
            {
                using (var target = new MemoryStream())
                {
                    FormFile.CopyTo(target);
                    file.FileUpload = target.ToArray();
                }
                file.CreatedOn = DateTime.Now;
                //var userId = _userContextService.GetUserId;
                //var user = await _context.Users.FindAsync(userId);
                //file.UploadedBy = user.Email;
                //file.UploadedBy = "user";
                var filename = FormFile.FileName;
                file.fileUploadName = filename;
                _context.Files.Add(file);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetFile), routeValues: new { id = file.ID }, value: file);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return BadRequest(e.ToString());
                throw;
            }
        }


        //PUT: /api/putfile
        [HttpPut("editFile/{id:int}", Name = "editFile")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditFile(int id, [FromForm] FileData file)
        {
            if (id != file.ID)
            {
                return BadRequest();
            }

            if (file.FileUpload != null)
            {
                using (var target = new MemoryStream())
                {
                    FormFile.CopyTo(target);
                    file.FileUpload = target.ToArray();
                }
            }
            file.CreatedOn = DateTime.Now;
            file.UploadedBy = "user";
            _context.Entry(file).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetFile), routeValues: new { id = file.ID }, value: file);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

        }

        //PUT
        [HttpPut("edit/{id:int}", Name = "edit")]
        public async Task<IActionResult> UpdateFile(int id, [FromForm] FileData file)
        {
            if (id != file.ID)
            {
                return BadRequest();
            }
            var fileToUpdate = await _context.Files.FindAsync(id);

            if (fileToUpdate == null)
            {
                return NotFound();
            }

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, fileToUpdate, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            fileToUpdate.Name = file.Name;
            fileToUpdate.Description = file.Description;
            fileToUpdate.Category = file.Category;
            fileToUpdate.CreatedOn = DateTime.Now;
            fileToUpdate.UploadedBy = "user";
            _context.Entry(fileToUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw new NotFoundException("File not found");
                }
            }
        }

        [HttpPatch("updateFile/{id:int}", Name = "patch")]
        public async Task<IActionResult> UpdateFilePatch(int id, [FromBody] JsonPatchDocument<FileData> fileUpdates)
        {
            if (fileUpdates == null)
            {
                return BadRequest();
            }
            var fileToUpdate = await _context.Files.FindAsync(id);
            if (fileToUpdate == null)
            {
                throw new NotFoundException("File not found");
            }
            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, fileToUpdate, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            fileUpdates.ApplyTo(fileToUpdate);
            fileToUpdate.CreatedOn = DateTime.Now;
            fileToUpdate.UploadedBy = "user";
            await _context.SaveChangesAsync();

            _context.Entry(fileToUpdate).State = EntityState.Modified;
            return NoContent();
        }


        // DELETE: api/Images/5
        [HttpDelete("delete/{id}", Name = "deleteFile")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            _logger.LogWarning($"File with id: {id}, DELETE action invoked");
            var file = await _context.Files.FindAsync(id);
            if (file == null)
            {
                throw new NotFoundException("File not found");
            }

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, file, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            _context.Files.Remove(file);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FileExists(int id)
        {
            return _context.Files.Any(e => e.ID == id);
        }
    }
}
