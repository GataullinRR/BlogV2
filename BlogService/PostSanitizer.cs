using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ganss.XSS;
using HtmlAgilityPack;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;

namespace BlogService.Controllers
{
    public class PostSanitizer : IPostSanitizer
    {
        public const int MAX_IMAGE_SIZE = 3 * 1024 * 1024;

        readonly IHtmlSanitizer _allowAllButNotExecutable;
        readonly IStorage _storage;
        readonly IUrlHelper _urlHelper;
        readonly IHttpContextAccessor _httpContextAccessor;

        public PostSanitizer(IStorage storage, IUrlHelper urlHelper)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _urlHelper = urlHelper ?? throw new ArgumentNullException(nameof(urlHelper));
            _allowAllButNotExecutable = new HtmlSanitizer(
                allowedTags: "h1 h2 h3 h4 h5 h6 code i b s img li ul ol link p em strong tr td table tbody a br span code pre sup sub blockquote caption".Split(" "),
                allowedSchemes: "http https data".Split(" "),
                allowedAttributes: "href src style class".Split(" "),
                uriAttributes: "href src".Split(" "),
                allowedCssProperties: "list-style-type padding-left text-decoration height width border border-collapse cellspacing cellpadding data-mce-style".Split(" "),
                allowedCssClasses: "language-csharp language-markup language-javascript language-css language-php language-ruby language-python language-java language-c language-cpp token operator punctuation keyword string number".Split(" "));
        }

        public async Task<string> SanitizeAsync(string body, bool escapeExecutable)
        {
            var intermediate = escapeExecutable
                ? _allowAllButNotExecutable.Sanitize(body)
                : body;
            var document = new HtmlDocument();
            document.LoadHtml(intermediate);
            var nodes = document.DocumentNode.SelectNodes("//img");
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    var srcAttr = node.Attributes.Single(a => a.Name == "src");
                    var src = srcAttr.Value;
                    //src = await TrySanitizeImage(src);
                    if (src == null)
                    {
                        node.Remove();
                    }
                    else
                    {
                        srcAttr.Value = src;

                        const string ATTRIBUTE = "class";
                        if (!node.Attributes.Any(a => a.Name == ATTRIBUTE))
                        {
                            node.Attributes.Add(ATTRIBUTE, "");
                        }

                        if (string.IsNullOrEmpty(node.Attributes[ATTRIBUTE].Value))
                        {
                            node.Attributes[ATTRIBUTE].Value = "rounded img-fluid";
                        }
                        else
                        {
                            node.Attributes[ATTRIBUTE].Value += " rounded img-fluid";
                        }
                    }
                }
            }
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            document.Save(writer);
            writer.Flush();
            ms.Position = 0;
            var sanitized = new StreamReader(ms).ReadToEnd();

            return sanitized;
        }

        private async Task<string> TrySanitizeImage(string imageSrc)
        {
            var formats = new (string Prefix, string Extension, ImageFormat Format)[]
            {
                ("data:image/jpeg;base64,", "jpg", ImageFormat.Jpeg),
                ("data:image/png;base64,", "png", ImageFormat.Png),
                ("data:image/gif;base64,", "gif", ImageFormat.Gif),
            };
            foreach (var format in formats)
            {
                if (imageSrc.StartsWith(format.Prefix))
                {
                    var length = imageSrc.Length - format.Prefix.Length / 4 * 3;
                    if (length < MAX_IMAGE_SIZE)
                    {
                        try
                        {
                            var image = new MemoryStream(Convert.FromBase64String(new string(imageSrc
                                .Skip(format.Prefix.Length)
                                .ToArray())));
                            var i = Image.FromStream(image);

                            if (format.Format.Equals(i.RawFormat))
                            {
                                image.Position = 0;
                                var localPath = await _storage.SaveImageAsync(image, format.Extension);

                                return AbsoluteContent(localPath);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }

            return null;
        }

        private string AbsoluteContent(string contentPath)
        {
            HttpRequest request = _urlHelper.ActionContext.HttpContext.Request;

            return new Uri(new Uri(request.Scheme + "://" + request.Host.Value), _urlHelper.Content(contentPath)).ToString();
        }

        public string IgnoreNonTextNodes(string richHtml)
        {
            if (string.IsNullOrEmpty(richHtml))
            {
                return string.Empty;
            }

            var document = new HtmlDocument();
            document.LoadHtml(richHtml);

            var acceptableTags = new String[] { "strong", "em", "u" };

            var nodes = new Queue<HtmlNode>(document.DocumentNode.SelectNodes("./*|./text()"));
            while (nodes.Count > 0)
            {
                var node = nodes.Dequeue();
                var parentNode = node.ParentNode;

                if (!acceptableTags.Contains(node.Name) && node.Name != "#text")
                {
                    var childNodes = node.SelectNodes("./*|./text()");

                    if (childNodes != null)
                    {
                        foreach (var child in childNodes)
                        {
                            nodes.Enqueue(child);
                            parentNode.InsertBefore(child, node);
                        }
                    }

                    parentNode.RemoveChild(node);

                }
            }

            return document.DocumentNode.InnerHtml;
        }
    }
}
