using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CikguHub.Data;

namespace CikguHub.Api.Model
{
    public class ResourceFilterModel
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;

        public List<int> Categories { get; set; } = new List<int>();

        public string Search { get; set; }
    }

    public class ResourceModel
    {
        public int ResourceId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string OwnerImage { get; set; }

        public List<(int, string)> Categories { get; set; }

        public ResourceModel() { }
        public ResourceModel(Resource resource)
        {
            ResourceId = resource.ResourceId;
            Name = resource.Name;
            OwnerId = resource.OwnerId;
            OwnerName = resource.Owner.Name;
            OwnerImage = resource.Owner.ImageUrl;
        }

        public Resource UpdateResource(Resource resource)
        {
            resource.ResourceId = this.ResourceId;
            resource.OwnerId = this.OwnerId;
            resource.Owner.Name = this.OwnerName;
            resource.Owner.ImageUrl = this.OwnerImage;

            return resource;
        }
    }

    public static class ResourceFilters
    {
        public static IQueryable<Resource> ForTenant(this IQueryable<Resource> resource, Int32 tenantID)
        {
            return resource.Where(u => u.OwnerId == tenantID);
        }
    }
}
