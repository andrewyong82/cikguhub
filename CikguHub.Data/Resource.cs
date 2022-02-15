using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CikguHub.Data
{
    public class Resource
    {
        public Resource() { }

        public int ResourceId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }

        public bool Deleted { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime Modified { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Below new for FastLaw
        /// </summary>
        public string StoragePath { get; set; }

        public string ContainerName { get; set; }

        public string BlobName { get; set; }

        /// <summary>
        /// File extension
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// MIME type of the file. This may actually be guessed if a file type was not provided by the user’s browser, so this is a best-effort value and not guaranteed to be accurate.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Size in bytes of the file
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// An optional URL to a visual thumbnail for the file.
        /// </summary>
        public string ImageUrl { get; set; }
    }
}
