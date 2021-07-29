using PlantRater.Data;
using PlantRater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantRater.Services
{
    public class TreeService
    {
        private readonly Guid _userId;

        public TreeService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateTree(TreeCreate model)
        {
            var entity =
                new Tree()
                {
                    OwnerId = _userId,
                    Name = model.Name,
                    CreatedUtc = DateTimeOffset.Now
                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Trees.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<TreeListItem> GetTrees()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Trees
                    .Where(e => e.OwnerId == _userId)
                    .Select(
                        e =>
                        new TreeListItem
                        {
                            TreeId = e.TreeId,
                            Name = e.Name,
                            CreatedUtc = e.CreatedUtc
                        }
                        );
                return query.ToArray();
            }
        }

        public TreeDetail GetTreeById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Trees
                    .Single(e => e.TreeId == id && e.OwnerId == _userId);
                return
                    new TreeDetail
                    {
                        TreeId = entity.TreeId,
                        Name = entity.Name,
                        CreatedUtc = entity.CreatedUtc,
                        ModifiedUtc = entity.ModifiedUtc
                    };
            }
        }

        public bool UpdateTree(TreeEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Trees
                    .Single(e => e.TreeId == model.TreeId && e.OwnerId == _userId);

                entity.Name = model.Name;
                entity.ModifiedUtc = DateTimeOffset.UtcNow;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteTree(int treeId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Trees
                    .Single(e => e.TreeId == treeId && e.OwnerId == _userId);

                ctx.Trees.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
